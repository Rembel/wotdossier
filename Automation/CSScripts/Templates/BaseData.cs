using CodeSmith.Engine;
using SchemaExplorer;
using System;
using System.Windows.Forms.Design;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Templates.Data
{
	/// <summary>
	/// Common code-behind class used to simplify SQL Server based CodeSmith templates
	/// </summary>
	[DefaultProperty("ChooseSourceDatabase")]
	public partial class BaseDataCodeTemplate : CodeTemplate
	{
	
		public string ToPrivate(string value)
		{
			return string.Concat("_",char.ToLower(value[0]) + value.Substring(1));	
		}
		private static Regex cleanRegEx = new Regex(@"\s+|_|-|\.", RegexOptions.Compiled);
		private static Regex cleanID = new Regex(@"(_ID|_id|_Id|\.ID|\.id|\.Id|ID|Id)", RegexOptions.Compiled);
		private static Regex sqlCharacters = new Regex(@"[\s|~|-|!|{|%|}|\^|'|&|.|\(|\\|\)|`]", RegexOptions.Compiled);

		private static Regex sqlReserved =
			new Regex(
				@"^(ADD|EXCEPT|PERCENT|ALL|EXEC|PLAN|ALTER|EXECUTE|PRECISION|AND|EXISTS|PRIMARY|ANY|EXIT|PRINT|AS|FETCH|PROC|ASC|FILE|PROCEDURE|AUTHORIZATION|FILLFACTOR|PUBLIC|BACKUP|FOR|RAISERROR|BEGIN|FOREIGN|READ|BETWEEN|FREETEXT|READTEXT|BREAK|FREETEXTTABLE|RECONFIGURE|BROWSE|FROM|REFERENCES|BULK|FULL|REPLICATION|BY|FUNCTION|RESTORE|CASCADE|GOTO|RESTRICT|CASE|GRANT|RETURN|CHECK|GROUP|REVOKE|CHECKPOINT|HAVING|RIGHT|CLOSE|HOLDLOCK|ROLLBACK|CLUSTERED|IDENTITY|ROWCOUNT|COALESCE|IDENTITY_INSERT|ROWGUIDCOL|COLLATE|IDENTITYCOL|RULE|COLUMN|IF|SAVE|COMMIT|IN|SCHEMA|COMPUTE|INDEX|SELECT|CONSTRAINT|INNER|SESSION_USER|CONTAINS|INSERT|SET|CONTAINSTABLE|INTERSECT|SETUSER|CONTINUE|INTO|SHUTDOWN|CONVERT|IS|SOME|CREATE|JOIN|STATISTICS|CROSS|KEY|SYSTEM_USER|CURRENT|KILL|TABLE|CURRENT_DATE|LEFT|TEXTSIZE|CURRENT_TIME|LIKE|THEN|CURRENT_TIMESTAMP|LINENO|TO|CURRENT_USER|LOAD|TOP|CURSOR|NATIONAL||TRAN|DATABASE|NOCHECK|TRANSACTION|DBCC|NONCLUSTERED|TRIGGER|DEALLOCATE|NOT|TRUNCATE|DECLARE|NULL|TSEQUAL|DEFAULT|NULLIF|UNION|DELETE|OF|UNIQUE|DENY|OFF|UPDATE|DESC|OFFSETS|UPDATETEXT|DISK|ON|USE|DISTINCT|OPEN|USER|DISTRIBUTED|OPENDATASOURCE|VALUES|DOUBLE|OPENQUERY|VARYING|DROP|OPENROWSET|VIEW|DUMMY|OPENXML|WAITFOR|DUMP|OPTION|WHEN|ELSE|OR|WHERE|END|ORDER|WHILE|ERRLVL|OUTER|WITH|ESCAPE|OVER|WRITETEXT)$",
				RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static Regex sqlFuture =
			new Regex(
				@"^(ABSOLUTE|FOUND|PRESERVE|ACTION|FREE|PRIOR|ADMIN|GENERAL|PRIVILEGES|AFTER|GET|READS|AGGREGATE|GLOBAL|REAL|ALIAS|GO|RECURSIVE|ALLOCATE|GROUPING|REF|ARE|HOST|REFERENCING|ARRAY|HOUR|RELATIVE|ASSERTION|IGNORE|RESULT|AT|IMMEDIATE|RETURNS|BEFORE|INDICATOR|ROLE|BINARY|INITIALIZE|ROLLUP|BIT|INITIALLY|ROUTINE|BLOB|INOUT|ROW|BOOLEAN|INPUT|ROWS|BOTH|INT|SAVEPOINT|BREADTH|INTEGER|SCROLL|CALL|INTERVAL|SCOPE|CASCADED|ISOLATION|SEARCH|CAST|ITERATE|SECOND|CATALOG|LANGUAGE|SECTION|CHAR|LARGE|SEQUENCE|CHARACTER|LAST|SESSION|CLASS|LATERAL|SETS|CLOB|LEADING|SIZE|COLLATION|LESS|SMALLINT|COMPLETION|LEVEL|SPACE|CONNECT|LIMIT|SPECIFIC|CONNECTION|LOCAL|SPECIFICTYPE|CONSTRAINTS|LOCALTIME|SQL|CONSTRUCTOR|LOCALTIMESTAMP|SQLEXCEPTION|CORRESPONDING|LOCATOR|SQLSTATE|CUBE|MAP|SQLWARNING|CURRENT_PATH|MATCH|START|CURRENT_ROLE|MINUTE|STATE|CYCLE|MODIFIES|STATEMENT|DATA|MODIFY|STATIC|DATE|MODULE|STRUCTURE|DAY|MONTH|TEMPORARY|DEC|NAMES|TERMINATE|DECIMAL|NATURAL|THAN|DEFERRABLE|NCHAR|TIME|DEFERRED|NCLOB|TIMESTAMP|DEPTH|NEW|TIMEZONE_HOUR|DEREF|NEXT|TIMEZONE_MINUTE|DESCRIBE|NO|TRAILING|DESCRIPTOR|NONE|TRANSLATION|DESTROY|NUMERIC|TREAT|DESTRUCTOR|OBJECT|TRUE|DETERMINISTIC|OLD|UNDER|DICTIONARY|ONLY|UNKNOWN|DIAGNOSTICS|OPERATION|UNNEST|DISCONNECT|ORDINALITY|USAGE|DOMAIN|OUT|USING|DYNAMIC|OUTPUT|VALUE|EACH|PAD|VARCHAR|END-EXEC|PARAMETER|VARIABLE|EQUALS|PARAMETERS|WHENEVER|EVERY|PARTIAL|WITHOUT|EXCEPTION|PATH|WORK|EXTERNAL|POSTFIX|WRITE|FALSE|PREFIX|YEAR|FIRST|PREORDER|ZONE|FLOAT|PREPARE)$",
				RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public static string CleanName(string name)
		{
			return cleanRegEx.Replace(name, "");
		}

		public static bool IsManyToManyTable(TableSchema table)
		{
			if (table.Columns.Count == 2 && table.PrimaryKey != null && table.PrimaryKey.MemberColumns.Count == 2 &&
			    table.ForeignKeys.Count == 2)
				return true;
			return false;
		}

		public static bool IsOneToOneTable(TableKeySchema tableKey)
		{
			if (tableKey.ForeignKeyMemberColumns.Count > 1) return false;
			if (tableKey.ForeignKeyMemberColumns[0].IsPrimaryKeyMember && tableKey.PrimaryKeyMemberColumns[0].IsPrimaryKeyMember)
				return true;
			return false;
		}

		public static bool IsSubClassTable(TableKeySchema tableKey)
		{
			if (tableKey.ForeignKeyMemberColumns.Count > 1) return false;
			if (tableKey.ForeignKeyMemberColumns[0].IsPrimaryKeyMember && tableKey.PrimaryKeyMemberColumns[0].IsPrimaryKeyMember)
				return true;
			return false;
		}

		public static bool IsSubClassTable(TableSchema sourceTable)
		{
			if (sourceTable.PrimaryKey == null) return false;
			if (sourceTable.PrimaryKey.MemberColumns.Count > 1) return false;
			if (sourceTable.PrimaryKey.MemberColumns[0].IsForeignKeyMember) return true;

			return false;
		}
		
		public string CSharpType(string name,string notNull)
		{
			switch (name)
			{
				case "String":
					return "string";
				case "Byte":
					name = "byte";
					break;
				case "Decimal":
					name =  "decimal";
					break;
				case "DateTime":
					name= "DateTime";
					break;
				case "Double":
					name= "double";
					break;
				case "Guid":
					name= "Guid";
					break;
				case "Int16":
					name = "short";
					break;
				case "Int32":
					name = "int";
					break;
				case "Int64":
					name = "long";
					break;
				case "Object":
					return "object";
				case "SByte":
					name = "sbyte";
					break;
				case "Single":
					name= "Single";
					break;
				case "UInt16":
					name = "ushort";
					break;
				case "UInt32":
					name = "uint";
					break;
				case "UInt64":
					name = "ulong";
					break;
			}
			
			return notNull!="false" ? name : name+"?";
		}
		public string CSharpType(string name)
		{
			return CSharpType(name,"true");
		}
		
		public string CSharpType(ColumnSchema column)
		{	
			if (column.Name.EndsWith("TypeCode")) return column.Name;

			switch (column.DataType)
			{
				case DbType.AnsiString:
					return "string";
				case DbType.AnsiStringFixedLength:
					return "string";
				case DbType.Binary:
					return "byte[]";
				case DbType.Boolean:
					return "bool";
				case DbType.Byte:
					return "byte";
				case DbType.Currency:
					return "decimal";
				case DbType.Date:
					return "DateTime";
				case DbType.DateTime:
					return "DateTime";
				case DbType.Decimal:
					return "decimal";
				case DbType.Double:
					return "double";
				case DbType.Guid:
					return "Guid";
				case DbType.Int16:
					return "short";
				case DbType.Int32:
					return "int";
				case DbType.Int64:
					return "long";
				case DbType.Object:
					return "object";
				case DbType.SByte:
					return "sbyte";
				case DbType.Single:
					return "float";
				case DbType.String:
					return "string";
				case DbType.StringFixedLength:
					return "string";
				case DbType.Time:
					return "TimeSpan";
				case DbType.UInt16:
					return "ushort";
				case DbType.UInt32:
					return "uint";
				case DbType.UInt64:
					return "ulong";
				case DbType.VarNumeric:
					return "decimal";
				default:
					{
						return "__UNKNOWN__" + column.NativeType;
					}
			}
		}

		public string NHibernateType(ColumnSchema column)
		{	
			if (column.Name.EndsWith("TypeCode")) return column.Name;

			switch (column.DataType)
			{
				case DbType.AnsiString:
					return "String";
				case DbType.AnsiStringFixedLength:
					return "String";
				case DbType.Binary:
					return "Byte[]";
				case DbType.Boolean:
					return "Boolean";
				case DbType.Byte:
					return "Byte";
				case DbType.Currency:
					return "Decimal";
				case DbType.Date:
					return "DateTime";
				case DbType.DateTime:
					return "DateTime";
				case DbType.Decimal:
					return "Decimal";
				case DbType.Double:
					return "Double";
				case DbType.Guid:
					return "Guid";
				case DbType.Int16:
					return "Int16";
				case DbType.Int32:
					return "Int32";
				case DbType.Int64:
					return "Int64";
				case DbType.Object:
					return "BinaryBlob";
				case DbType.SByte:
					return "Byte";
				case DbType.Single:
					return "Single";
				case DbType.String:
					return "String";
				case DbType.StringFixedLength:
					return "String";
				case DbType.Time:
					return "DateTime";
				case DbType.UInt16:
					return "Int16";
				case DbType.UInt32:
					return "Int32";
				case DbType.UInt64:
					return "Int64";
				case DbType.VarNumeric:
					return "Decimal";
				default:
					{
						return "__UNKNOWN__" + column.NativeType;
					}
			}
		}

		public string SqlIdentifier(string sqlIdentifier)
		{
			if (sqlCharacters.IsMatch(sqlIdentifier) || sqlReserved.IsMatch(sqlIdentifier) || sqlFuture.IsMatch(sqlIdentifier))
				return String.Format("`{0}`", sqlIdentifier);
			else
				return sqlIdentifier;
		}

		public string TableClass(TableSchema table)
		{
			string className = table.Name;
			//if (className.StartsWith(RemoveTablePrefix))
			//    className = className.Substring(RemoveTablePrefix.Length);
			return String.Format("{0}", StringUtil.ToSingular(StringUtil.ToPascalCase(className)));
		}

		public string TableClassFull(TableSchema table)
		{
			//return String.Format("{0}.{1}, {2}", Namespace, TableClass(table), Assembly);
			return String.Format("{0}.{1}, {2}", "Entities", TableClass(table), "Entities");
		}

		public string TableCollection(TableSchema table)
		{
			string className = table.Name;
			//if (className.StartsWith(RemoveTablePrefix))
			//    className = className.Substring(RemoveTablePrefix.Length);
			return String.Format("{0}",StringUtil.ToPlural(StringUtil.ToPascalCase(className)));
		}

		public string ClassName(TableSchema table)
		{
			return TableClass(table);
		}

		public string ClassNameAtt(TableSchema table)
		{
			return String.Format(" name=\"{0}\"", TableClassFull(table));
		}

		public string ClassTable(TableSchema table)
		{
			return table.Name;
		}

		public string ClassTableAtt(TableSchema table)
		{
			return String.Format(" table=\"{0}\"", SqlIdentifier(table.Name));
		}

		public string IdMemberName(TableSchema table)
		{
			return "_id";
		}

		public string IdName(TableSchema table)
		{
			return "Id";
		}

		public string IdNameAtt(TableSchema table)
		{
			return String.Format(" name=\"{0}\"", IdName(table));
		}

		public string IdMemberType(TableSchema table)
		{
			return MemberType(table.PrimaryKey.MemberColumns[0]);
		}

		public string IdType(TableSchema table)
		{
			return PropertyType(table.PrimaryKey.MemberColumns[0]);
		}

		public string IdTypeAtt(TableSchema table)
		{
			return String.Format(" type=\"{0}\"", PropertyType(table.PrimaryKey.MemberColumns[0]));
		}

		public string IdUnsavedValueAtt(TableSchema table)
		{
			ColumnSchema column = table.PrimaryKey.MemberColumns[0];
			if (column.Size == 0)
				return String.Format(" unsaved-value=\"{0}\"", 0);
			else
				return String.Format(" unsaved-value=\"{0}\"", "null");
		}

		public string PropertyName(ColumnSchema column)
		{
			return StringUtil.ToPascalCase(column.Name);
		}

		public string MemberName(ColumnSchema column)
		{
			return "_" + StringUtil.ToCamelCase(column.Name);
		}

		public string ParameterName(ColumnSchema column)
		{
			return StringUtil.ToCamelCase(column.Name);
		}

		public string PropertyNameAtt(ColumnSchema column)
		{
			return String.Format(" name=\"{0}\"", PropertyName(column));
		}

		public string PropertyType(ColumnSchema column)
		{
			return NHibernateType(column);
		}

		public string MemberType(ColumnSchema column)
		{
			return CSharpType(column);
		}

		public string PropertyTypeAtt(ColumnSchema column)
		{
			return String.Format(" type=\"{0}\"", PropertyType(column));
		}

		public string ColumnName(ColumnSchema column)
		{
			return column.Name;
		}

		public string ColumnNameAtt(ColumnSchema column)
		{
			return String.Format(" name=\"{0}\"", SqlIdentifier(ColumnName(column)));
		}

		public string ColumnLength(ColumnSchema column)
		{
			if (column.Size > 0)
				return column.Size.ToString();
			else
				return String.Empty;
		}

		public string ColumnLengthAtt(ColumnSchema column)
		{
			if (column.Size > 0)
				return String.Format(" length=\"{0}\"", column.Size);
			else
				return String.Empty;
		}

		public string ColumnSqlTypeAtt(ColumnSchema column)
		{
			return String.Format(" sql-type=\"{0}\"", column.NativeType);
		}

		public string ColumnNotNullAtt(ColumnSchema column)
		{
			return String.Format(" not-null=\"{0}\"", (!column.AllowDBNull).ToString().ToLower());
		}

		public string ColumnUniqueAtt(ColumnSchema column)
		{
			if (column.IsUnique)
				return String.Format(" unique=\"{0}\"", column.IsUnique.ToString().ToLower());
			else
				return String.Empty;
		}

		public string ColumnIndexAtt(TableSchema table, ColumnSchema column)
		{
			foreach (IndexSchema index in table.Indexes)
			{
				if (index.MemberColumns.Contains(column))
				{
					return String.Format(" index=\"{0}\"", index.Name);
				}
			}
			return String.Empty;
		}

		public string ManyToOneName(TableKeySchema foreignKey)
		{
			string className = TableClass(foreignKey.PrimaryKeyTable);

			string thiskey = foreignKey.ForeignKeyMemberColumns[0].Name;
			string primarykey = foreignKey.PrimaryKeyMemberColumns[0].Name;

			string differentiator = thiskey.Replace(primarykey, "").Replace("ID", "");

			string returnName = (differentiator == "" ? className : differentiator);

			return returnName;
		}

		public string ManyToOneMemberName(TableKeySchema foreignKey)
		{
			return "_" + StringUtil.ToCamelCase(ManyToOneName(foreignKey));
		}

		public string ManyToOneParameterName(TableKeySchema foreignKey)
		{
			return StringUtil.ToCamelCase(ManyToOneName(foreignKey));
		}

		public string ManyToOneNameAtt(TableKeySchema foreignKey)
		{
			return String.Format(" name=\"{0}\"", ManyToOneName(foreignKey));
		}

		public string ManyToOneClass(TableKeySchema foreignKey)
		{
			string className = TableClass(foreignKey.PrimaryKeyTable);

			return className;
		}

		public string ManyToOneClassAtt(TableKeySchema foreignKey)
		{
			string className = TableClassFull(foreignKey.PrimaryKeyTable);

			return String.Format(" class=\"{0}\"", className);
		}

		public string OneToOneName(TableKeySchema primaryKey)
		{
			string className = TableClass(primaryKey.ForeignKeyTable);

			string thiskey = primaryKey.PrimaryKeyMemberColumns[0].Name;
			string primarykey = primaryKey.ForeignKeyMemberColumns[0].Name;

			string differentiator = thiskey.Replace(primarykey, "");

			return className + differentiator;
		}

		public string OneToOneMemberName(TableKeySchema primaryKey)
		{
			return "_" + StringUtil.ToCamelCase(OneToOneName(primaryKey));
		}

		public string OneToOneNameAtt(TableKeySchema primaryKey)
		{
			return String.Format(" name=\"{0}\"", OneToOneName(primaryKey));
		}

		public string OneToOneClass(TableKeySchema primaryKey)
		{
			string className = TableClass(primaryKey.ForeignKeyTable);

			return className;
		}

		public string OneToOneClassAtt(TableKeySchema primaryKey)
		{
			string className = TableClassFull(primaryKey.ForeignKeyTable);

			return String.Format(" class=\"{0}\"", className);
		}

		public string JoinedSubclassName(TableKeySchema primaryKey)
		{
			string className = TableClass(primaryKey.ForeignKeyTable);

			string thiskey = primaryKey.PrimaryKeyMemberColumns[0].Name;
			string primarykey = primaryKey.ForeignKeyMemberColumns[0].Name;

			string differentiator = thiskey.Replace(primarykey, "");

			return className + differentiator;
		}

		public string JoinedSubclassNameAtt(TableKeySchema primaryKey)
		{
			string className = TableClassFull(primaryKey.ForeignKeyTable);
			return String.Format(" name=\"{0}\"", className);
		}

		public string JoinedSubclassTable(TableKeySchema primaryKey)
		{
			return primaryKey.ForeignKeyTable.Name;
		}

		public string JoinedSubclassTableAtt(TableKeySchema primaryKey)
		{
			return String.Format(" table=\"{0}\"", SqlIdentifier(primaryKey.ForeignKeyTable.Name));
		}

		public string CollectionName(TableKeySchema primaryKey)
		{
			//	string className = TableCollection(primaryKey.ForeignKeyTable);
			string className = primaryKey.ForeignKeyTable.Name;
			string thiskey = primaryKey.PrimaryKeyMemberColumns[0].Name;
			string primarykey = primaryKey.ForeignKeyMemberColumns[0].Name;

			string differentiator = primarykey.Replace(thiskey, "").Replace("ID", "");

			return StringUtil.ToPlural(differentiator + className);
		}

		public string CollectionMemberName(TableKeySchema primaryKey)
		{
			return "_" + StringUtil.ToCamelCase(CollectionName(primaryKey));
		}

		public string CollectionNameAtt(TableKeySchema primaryKey)
		{
			string className = TableCollection(primaryKey.ForeignKeyTable);

			string thiskey = primaryKey.PrimaryKeyMemberColumns[0].Name;
			string primarykey = primaryKey.ForeignKeyMemberColumns[0].Name;

			string differentiator = primarykey.Replace(thiskey, "");

			return String.Format(" name=\"{0}\"", CollectionName(primaryKey));
		}

		public string CollectionType(TableKeySchema primaryKey)
		{
			return "IList";
		}

		public string NewCollectionType(TableKeySchema primaryKey)
		{
			return "new ArrayList()";
		}

		public string CollectionKeyColumnAtt(TableKeySchema primaryKey)
		{
			ColumnSchema column = primaryKey.PrimaryKeyMemberColumns[0];
			return String.Format(" column=\"{0}\"", SqlIdentifier(column.Name));
		}

		public string CollectionSelfKeyColumnAtt(TableKeySchema primaryKey)
		{
			ColumnSchema column = primaryKey.ForeignKeyMemberColumns[0];
			return String.Format(" column=\"{0}\"", SqlIdentifier(column.Name));
		}

		public string CollectionOneToManyClass(TableKeySchema primaryKey)
		{
			return TableClass(primaryKey.ForeignKeyTable);
		}

		public string CollectionOneToManyClassAtt(TableKeySchema primaryKey)
		{
			return String.Format(" class=\"{0}\"", TableClassFull(primaryKey.ForeignKeyTable));
		}

		public string CollectionManyToManyName(TableKeySchema primaryKey)
		{
			//	string className = String.Empty;

			//	foreach(TableKeySchema tableKey in primaryKey.ForeignKeyTable.ForeignKeys)
			//	{
			//		className = TableCollection(tableKey.ForeignKeyTable);
			//		if (tableKey.PrimaryKeyTable != SourceTable)
			//		{
			//			className = TableCollection(tableKey.PrimaryKeyTable);
			//		}
			//	}

			//	string thiskey = primaryKey.PrimaryKeyMemberColumns[0].Name;
			string primarykey = primaryKey.ForeignKeyMemberColumns[0].Name;

			//	string differentiator = primarykey.Replace(thiskey, "");

			string otherkey = String.Empty;
			foreach (ColumnSchema column in primaryKey.ForeignKeyTable.PrimaryKey.MemberColumns)
			{
				if (column.Name != primarykey)
				{
					otherkey = column.Name;
				}
			}

			string returnName = StringUtil.ToPlural(primarykey.Replace("ID", "") + otherkey.Replace("ID", ""));

			return returnName;
		}

		public string CollectionManyToManyMemberName(TableKeySchema primaryKey)
		{
			return "_" + StringUtil.ToCamelCase(CollectionManyToManyName(primaryKey));
		}

		public string CollectionManyToManyNameAtt(TableKeySchema primaryKey)
		{
			return String.Format(" name=\"{0}\"", CollectionManyToManyName(primaryKey));
		}

		public string CollectionManyToManyClass(TableKeySchema primaryKey)
		{
			return TableClass(primaryKey.ForeignKeyTable);
		}

		public string CollectionManyToManyClassAtt(TableKeySchema primaryKey)
		{
			return String.Format(" class=\"{0}\"", TableClassFull(primaryKey.PrimaryKeyTable));
		}

		public string CollectionTableAtt(TableKeySchema primaryKey)
		{
			return String.Format(" table=\"{0}\"", SqlIdentifier(primaryKey.ForeignKeyTable.Name));
		}
		
		#region Properties name helper functions 
		public string PropPropertyName(PropertySchema property)
		{
			return StringUtil.ToPascalCase(property.Name);
		}
		
		public string RefPropertyName(ReferenceSchema reference)
		{
			return StringUtil.ToPascalCase(reference.ColumnName);
		}
		
		public string RefObjectPropertyName(ReferenceSchema reference)
		{
			return RefPropertyName(reference) + "Object";
		}
		
		public string CollPropertyName(CollectionSchema coll)
		{
			return StringUtil.ToPlural(CollTypeName(coll));
		}		
		#endregion
		
		#region Field name helper functions

		public string PropFieldName(PropertySchema property)
		{
			return StringUtil.ToPascalCase(ToPrivate(property.Name));
		}

		public string RefFieldName(ReferenceSchema reference)
		{
			return StringUtil.ToPascalCase(ToPrivate(reference.ColumnName));
		}

		public string RefObjectFieldName(ReferenceSchema reference)
		{
			return RefPropertyName(reference) + "Object";
		}
		
		public string CollFieldName(CollectionSchema coll)
		{
			return StringUtil.ToPlural(ToPrivate(CollTypeName(coll))); 
		}
		
		#endregion
		
		#region Type name helper functions

		public string CollTypeName(CollectionSchema coll)
		{
			return String.Concat(coll.ItemType,"Entity"); 
		}
		
		public string PropTypeName(PropertySchema property)
		{
			return CSharpType(property.Type,property.NotNull);
		}
		
		public string RefTypeName(ReferenceSchema reference)
		{
			return String.Concat(reference.Class.Name,"Entity");
		}
		
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public class NamespaceSchema
		{
			#region Static members

			public static NamespaceSchema Current
			{
				get
				{
					if (_Current == null)
					{
						_Current = new NamespaceSchema();
					}
					return _Current;
				}
			}

			public static NamespaceSchema _Current;

			public static void SetCurrent(string name, string assembly)
			{
				Current.Name = name;
				Current.Assembly = assembly;
			}

			#endregion Static members

			public Dictionary<string, ClassSchema> ClassSchemas
			{
				get
				{
					if (_ClassSchemas == null)
					{
						_ClassSchemas = new Dictionary<string, ClassSchema>();
					}
					return _ClassSchemas;
				}
			}

			private Dictionary<string, ClassSchema> _ClassSchemas;


			public ClassSchema GetClassSchema(TableSchema tableSchema)
			{
				if (!ClassSchemas.ContainsKey(tableSchema.Name))
				{
					ClassSchemas.Add(tableSchema.Name, new ClassSchema(tableSchema, this));
				}
				return ClassSchemas[tableSchema.Name];
			}


			public string Name
			{
				get { return _Name; }
				set { _Name = value; }
			}

			private string _Name;


			public string Assembly
			{
				get { return _Assembly; }
				set { _Assembly = value; }
			}

			private string _Assembly;
		}

		/// <summary>
		/// 
		/// </summary>
		public class ClassSchema
		{
			private TableSchema _tableSchema;
			private NamespaceSchema _NamespaceSchema;
			private ClassSchema _ParentClass;

			private string _Name;
			private string _ParentName;
			private bool _IsSubClass;

			private string _TableName;

			internal ClassSchema(TableSchema tableSchema, NamespaceSchema namespaceSchema)
			{
				if (tableSchema == null)
				{
					throw new ArgumentNullException("tableSchema");
				}
				if (namespaceSchema == null)
				{
					throw new ArgumentNullException("namespaceSchema");
				}

				_tableSchema = tableSchema;
				_NamespaceSchema = namespaceSchema;

				_TableName = tableSchema.Name;
				_Name = _TableName;

				_IsSubClass = BaseDataCodeTemplate.IsSubClassTable(tableSchema);
				if (_IsSubClass)
				{
					foreach (TableKeySchema foreignKey in tableSchema.ForeignKeys)
					{
						if (foreignKey.ForeignKeyMemberColumns.Contains(tableSchema.PrimaryKey.MemberColumns[0]))
						{
							_ParentClass = NamespaceSchema.GetClassSchema(foreignKey.PrimaryKeyTable);
							break;
						}
					}
				}

				_Id = new IdSchema(tableSchema.PrimaryKey, this);

				foreach (ColumnSchema columnSchema in _tableSchema.NonKeyColumns)
				{
					if (columnSchema.Name.Equals("Version"))
					{
						_Version = new PropertySchema(columnSchema, this);
						continue;
					}
					Properties.Add(new PropertySchema(columnSchema, this));
				}
			}

			public string TableName
			{
				get { return _TableName; }
			}

			public string Name
			{
				get { return _Name; }
			}

			public string FullName
			{
				get { return String.Format("{0}.{1}", Namespace, Name); }
			}

			
			
			
			public string HbmName
			{
				get { return String.Format("{0}.{1}, {2}", Namespace, Name, Assembly); }
			}

			public string ParentName
			{
				get { return _ParentClass == null ? string.Empty : _ParentClass.Name; }
			}

			public ClassSchema ParentClass
			{
				get { return _ParentClass; }
			}
			
			public bool IsSubClass
			{
				get { return _IsSubClass; }
			}

			public NamespaceSchema NamespaceSchema
			{
				get { return _NamespaceSchema; }
			}

			public string Namespace
			{
				get { return NamespaceSchema.Name; }
			}

			public string Assembly
			{
				get { return NamespaceSchema.Assembly; }
			}

			public bool HasId
			{
				get { return !_IsSubClass; }
			}

			public IdSchema Id
			{
				get { return _Id; }
			}

			private IdSchema _Id;

			public bool HasVersion
			{
				get { return null != _Version; }
			}

			public PropertySchema Version
			{
				get { return _Version; }
			}

			private PropertySchema _Version;
			

			/// <summary>
			/// 
			/// </summary>
			public PropertySchema.List Properties
			{
				get
				{
					if (_Properties == null)
					{
						_Properties = new PropertySchema.List();
					}
					return _Properties;
				}
			}

			private PropertySchema.List _Properties;

			/// <summary>
			/// 
			/// </summary>
			public ReferenceSchema.List References
			{
				get
				{
					if (_References == null)
					{
						_References = new ReferenceSchema.List();
						foreach (TableKeySchema fk in _tableSchema.ForeignKeys)
						{
							if (fk.ForeignKeyMemberColumns.Count > 1) continue;
							if (BaseDataCodeTemplate.IsSubClassTable(fk))
							{
								_ParentName = fk.PrimaryKeyTable.Name; //todo
								continue;
							}
							_References.Add(new ReferenceSchema(fk, this));
						}
					}
					return _References;
				}
			}

			private ReferenceSchema.List _References;

			/// <summary>
			/// 
			/// </summary>
			public CollectionSchema.List Collections
			{
				get
				{
					if (_Collections == null)
					{
						_Collections = new CollectionSchema.List();
						foreach (TableKeySchema pk in _tableSchema.PrimaryKeys)
						{
							if (pk.PrimaryKeyMemberColumns.Count > 1) continue;
							if (IsSubClassTable(pk))
							{
								continue;
							}
							_Collections.Add(new CollectionSchema(pk, this));
						}
					}
					return _Collections;
				}
			}

			private CollectionSchema.List _Collections;

			/// <summary>
			/// 
			/// </summary>
			public ClassSchema.List SubClasses
			{
				get
				{
					if (_SubClasses == null)
					{
						_SubClasses = new ClassSchema.List();
						foreach (TableKeySchema pk in _tableSchema.PrimaryKeys)
						{
							if (pk.PrimaryKeyMemberColumns.Count > 1) continue;
							if (IsSubClassTable(pk))
							{
								_SubClasses.Add(NamespaceSchema.GetClassSchema(pk.ForeignKeyTable));
							}
						}
					}
					return _SubClasses;
				}
			}

			private ClassSchema.List _SubClasses;

			public class List : List<ClassSchema>
			{
			}
		}

		public static bool IsIdentityColumn(ColumnSchema columnSchema)
		{
			bool isIdentityColumn = false;
			const string IsIdentityKey = "CS_IsIdentity";
			if(columnSchema.ExtendedProperties.Contains(IsIdentityKey))
			{
				isIdentityColumn = Convert.ToBoolean(columnSchema.ExtendedProperties[IsIdentityKey].Value);
			}
			return isIdentityColumn;
		}

		/// <summary>
		/// 
		/// </summary>
		public class IdSchema
		{
			private ClassSchema _ParentClass;
			private string _Name;
			private string _FieldName;
			private string _ParamName;
			private string _ColumnName;
			private string _Type;
			private string _Length;
			private string _UnsavedValue;
			private string _UnsavedValueCode;
			private string _Generator;

			public IdSchema(PrimaryKeySchema primaryKeySchema, ClassSchema classSchema)
			{
				if (primaryKeySchema == null)
				{
					throw new ArgumentNullException("primaryKeySchema");
				}
				if (classSchema == null)
				{
					throw new ArgumentNullException("classSchema");
				}
				_ParentClass = classSchema;

				ColumnSchema pkColumnSchema = primaryKeySchema.MemberColumns[0];
				
				_Name = "Id"; //always force id
				_FieldName = "_" + _Name; //HACK
				_ParamName = "a" + _Name; //HACK
				_ColumnName = pkColumnSchema.Name;
				_Type = pkColumnSchema.SystemType.Name;
				_UnsavedValue = primaryKeySchema.MemberColumns.Count > 1 ? "null" : "0";//will be overriden below

				switch (_Type)
				{
					case "Int32":
						_Generator = IsIdentityColumn(pkColumnSchema) ? "native" : "increment";
						_UnsavedValue = "0";
						_UnsavedValueCode = "0";
						break;
					case "Guid":
						_Generator = "guid.comb";
						_UnsavedValue = Guid.Empty.ToString();
						_UnsavedValueCode = "Guid.Empty";
						break;
					default:
						_Generator = "assigned";
						_UnsavedValue = "null";
						_UnsavedValueCode = "null";
						break;
				}
			}

			public string Name
			{
				get { return _Name; }
			}

			public string FieldName
			{
				get { return _FieldName; }
			}

			public string ParamName
			{
				get { return _ParamName; }
			}

			public string ColumnName
			{
				get { return _ColumnName; }
			}

			public string Type
			{
				get { return _Type; }
			}

			public string Length
			{
				get { return _Length; }
			}

			public string UnsavedValue
			{
				get { return _UnsavedValue; }
			}
			
			public string UnsavedValueCode
			{
				get { return _UnsavedValueCode; }
			}

			public string Generator
			{
				get { return _Generator; }
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public class PropertySchema
		{
			private ClassSchema _ParentClass;
			private string _Name;
			private string _Description;
			private string _FieldName;
			private string _ParamName;
			private string _Type;
			private int _Length;
			private string _ColumnName;
			private string _NotNull;

			internal PropertySchema(ColumnSchema columnSchema, ClassSchema classSchema)
			{
				if (columnSchema == null)
				{
					throw new ArgumentNullException("columnSchema");
				}
				if (classSchema == null)
				{
					throw new ArgumentNullException("classSchema");
				}
				_ParentClass = classSchema;

				_Name = columnSchema.Name;
				_FieldName = "_" + _Name; //HACK
				_ParamName = "a" + _Name; //HACK
				_Description = columnSchema.Description;
				_Type = columnSchema.SystemType.Name;
				_Length = columnSchema.Size;
				_ColumnName = columnSchema.Name;
				_NotNull = columnSchema.AllowDBNull ? "false" : "true";
			}

			public string Name
			{
				get { return _Name; }
			}

			public string Description
			{
				get { return _Description; }
			}

			public string FieldName
			{
				get { return _FieldName; }
			}

			public string ParamName
			{
				get { return _ParamName; }
			}

			public string Type
			{
				get { return _Type; }
			}

			public int Length
			{
				get { return _Length; }
			}

			public string ColumnName
			{
				get { return _ColumnName; }
			}

			public string NotNull
			{
				get { return _NotNull; }
			}
			
			public string TestValue
			{
				get 
				{
					string testValue;
					switch (_Type)
					{
						case "String":
							testValue = string.Format("Test {0} {1}", _ParentClass.Name, Name);
							testValue = string.Format("\"{0}\"", Length > 0 && testValue.Length > Length ? testValue.Substring(0, Length) : testValue);
							break;
						case "Int16":
							testValue = "1";
							break;
						case "Int32":
							testValue = "1";
							break;
						case "Guid":
							testValue = "new Guid()";
							break;
						case "DateTime":
							testValue = "DateTime.Now";
							break;
						case "Boolean":
							testValue = "false";
							break;
						case "Double":
							testValue = "3.14";
							break;
						case "Single":
							testValue = "3.14F";
							break;
						case "Decimal":
							testValue = "3.14M";
							break;
						case "Byte[]":
							testValue = "new Byte[1]";
							break;
						case "Byte":
							testValue = "1";
							break;
						default:
							testValue = _Type;
							break;
					}

					return testValue; 
				}
			}

			public string TestValue2
			{
				get
				{
					//TODO
					switch (_Type)
					{
						case "String":
							return TestValue[0] + "*" + TestValue.Substring(2);
					}
					return TestValue;
				}
			}


			public class List : List<PropertySchema>
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public class ReferenceSchema
		{
			private string _Name;
			private string _Description;
			private string _FieldName;
			private string _ParamName;
			private string _Type;
			private string _ClassName;
			private string _Length;
			private string _ColumnName;
			private string _NotNull;

			private ClassSchema _ParentClass;
			private ClassSchema _Class;

			public ReferenceSchema(TableKeySchema tableKeySchema, ClassSchema classSchema)
			{
				if (tableKeySchema == null)
				{
					throw new ArgumentNullException("tableKeySchema");
				}
				if (classSchema == null)
				{
					throw new ArgumentNullException("classSchema");
				}
				_ParentClass = classSchema;
				_Class = _ParentClass.NamespaceSchema.GetClassSchema(tableKeySchema.PrimaryKeyTable);
				_Name = GetReferenceName(tableKeySchema);
				_FieldName = "_" + _Name; //HACK
				_ParamName = "a" + _Name; //HACK
				_Type = _Class.FullName;
				_ClassName = _Class.Name;

				ColumnSchema columnSchema = tableKeySchema.ForeignKeyMemberColumns[0];
				_ColumnName = columnSchema.Name;
				_NotNull = columnSchema.AllowDBNull ? "false" : "true";
				_Description = columnSchema.Description;
			}


			public string Name
			{
				get { return _Name; }
			}

			public string Description
			{
				get { return _Description; }
			}

			public string FieldName
			{
				get { return _FieldName; }
			}

			public string ParamName
			{
				get { return _ParamName; }
			}

			public string Type
			{
				get { return _Type; }
			}

			public string ClassName
			{
				get { return _ClassName; }
			}
			
			public string Length
			{
				get { return _Length; }
			}

			public string ColumnName
			{
				get { return _ColumnName; }
			}

			public string NotNull
			{
				get { return _NotNull; }
			}

			public ClassSchema Class
			{
				get { return _Class; }
			}

			public class List : List<ReferenceSchema>
			{
			}
		}

		public static string GetReferenceName(TableKeySchema tableKeySchema)
		{
			string fcName = tableKeySchema.ForeignKeyMemberColumns[0].Name;
			string pcName = tableKeySchema.PrimaryKeyMemberColumns[0].Name;
			string ptName = tableKeySchema.PrimaryKeyTable.Name;
			if (fcName.Contains(pcName))
			{
				return fcName.Replace(pcName, ptName);
			}
			if (fcName.ToLower().EndsWith("id"))
			{
				return fcName.Substring(0, fcName.Length - 2);
			}
			return fcName;
		}

		public static string GetCollectionName(TableKeySchema tableKeySchema)
		{
			string fcName = tableKeySchema.ForeignKeyMemberColumns[0].Name;
			string pcName = tableKeySchema.PrimaryKeyMemberColumns[0].Name;
			string ptName = tableKeySchema.PrimaryKeyTable.Name;
			string ftName = tableKeySchema.ForeignKeyTable.Name;
			if (fcName.Contains(pcName))
			{
				return fcName.Replace(pcName, ftName);
			}
			if (fcName.ToLower().EndsWith("id"))
			{
				return fcName.Substring(0, fcName.Length - 2);
			}
			return fcName;
		}

		public class CollectionSchema
		{
			private string _Name;
			private string _FieldName;
			private string _ParamName;
			private string _Type;
			private string _TypeImpl;
			private string _ItemName;
			private string _ItemFieldName;
			private string _ItemParamName;
			private string _ItemType;
			private string _ItemBackReferenceName;

			private string _ColumnName;
			private string _NotNull;

			private ClassSchema _Class;
			private ClassSchema _ParentClass;

			public CollectionSchema(TableKeySchema tableKeySchema, ClassSchema classSchema)
			{
				if (tableKeySchema == null)
				{
					throw new ArgumentNullException("tableKeySchema");
				}
				if (classSchema == null)
				{
					throw new ArgumentNullException("classSchema");
				}
				_ParentClass = classSchema;
				_Class = _ParentClass.NamespaceSchema.GetClassSchema(tableKeySchema.ForeignKeyTable);

				_ItemType = _Class.Name;
				_ItemName = GetCollectionName(tableKeySchema);
				_ItemFieldName = "_" + _ItemName; //HACK
				_ItemParamName = "a" + _ItemName; //HACK
				_ItemBackReferenceName = GetReferenceName(tableKeySchema);

				_Name = String.Format("{0}List", _ItemName);
				_Type = String.Format("IList<{0}>", _ItemType);
				_TypeImpl = String.Format("List<{0}>", _ItemType);
				_FieldName = "_" + _Name;
				_ParamName = "a" + _Name;

				_ColumnName = tableKeySchema.ForeignKeyMemberColumns[0].Name;
			}


			public string Name
			{
				get { return _Name; }
			}

			public string FieldName
			{
				get { return _FieldName; }
			}

			public string ParamName
			{
				get { return _ParamName; }
			}

			public string Type
			{
				get { return _Type; }
			}

			public string TypeImpl
			{
				get { return _TypeImpl; }
			}

			public string ItemName
			{
				get { return _ItemName; }
			}

			public string ItemFieldName
			{
				get { return _ItemFieldName; }
			}

			public string ItemParamName
			{
				get { return _ItemParamName; }
			}

			public string ItemType
			{
				get { return _ItemType; }
			}

			public string ItemBackReferenceName
			{
				get { return _ItemBackReferenceName; }
			}

			public string ColumnName
			{
				get { return _ColumnName; }
			}

			public string NotNull
			{
				get { return _NotNull; }
			}

			public ClassSchema Class
			{
				get { return _Class; }
			}

			public class List : List<CollectionSchema>
			{
			}
		}
	}
}