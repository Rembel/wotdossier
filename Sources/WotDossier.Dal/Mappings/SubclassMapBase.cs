using System;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils.Reflection;
using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
    public class SubclassMapBase<T> : SubclassMap<T>
          where T : EntityBase
    {
        public SubclassMapBase()
        {
            KeyColumn(ReflectionHelper.GetMember<T>(v=>v.Id).Name);
            Table(typeof(T).Name.Replace("Entity", String.Empty));
        }

        protected string Column(Expression<Func<T,object>> expression)
        {
            return ReflectionHelper.GetMember(expression).Name;
        }

        protected string Column<TC>(Expression<Func<TC, object>> expression)
        {
            return ReflectionHelper.GetMember(expression).Name;
        }
    }
}
