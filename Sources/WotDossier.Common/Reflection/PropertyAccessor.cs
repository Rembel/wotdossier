using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace WotDossier.Common.Reflection
{
    /// <summary>
    /// The PropertyAccessor class provides fast dynamic access
    /// to a property of a specified target class.
    /// </summary>
    public class PropertyAccessor : IPropertyAccessor
    {
        private readonly bool mCanRead;
        private readonly bool mCanWrite;
        private readonly string mProperty;
        private readonly Type mPropertyType;
        private readonly Type mTargetType;
        private IPropertyAccessor mEmittedPropertyAccessor;
        private Hashtable mTypeHash;

        /// <summary>
        /// Creates a new property accessor.
        /// </summary>
        /// <param name="targetType">Target object type.</param>
        /// <param name="property">Property name.</param>
        public PropertyAccessor(Type targetType, string property)
        {
            mTargetType = targetType;
            mProperty = property;

            PropertyInfo propertyInfo =
                targetType.GetProperty(property);

            //
            // Make sure the property exists
            //
            if (propertyInfo == null)
            {
                throw new Exception(
                    string.Format("Property \"{0}\" does not exist for type "
                                  + "{1}.", property, targetType));
            }
            mCanRead = propertyInfo.CanRead;
            mCanWrite = propertyInfo.CanWrite;
            mPropertyType = propertyInfo.PropertyType;
        }

        /// <summary>
        /// Whether or not the Property supports read access.
        /// </summary>
        public bool CanRead
        {
            get { return mCanRead; }
        }

        /// <summary>
        /// Whether or not the Property supports write access.
        /// </summary>
        public bool CanWrite
        {
            get { return mCanWrite; }
        }

        /// <summary>
        /// The Type of object this property accessor was
        /// created for.
        /// </summary>
        public Type TargetType
        {
            get { return mTargetType; }
        }

        /// <summary>
        /// The Type of the Property being accessed.
        /// </summary>
        public Type PropertyType
        {
            get { return mPropertyType; }
        }

        #region IPropertyAccessor Members

        /// <summary>
        /// Gets the property value from the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Property value.</returns>
        public object Get(object target)
        {
            if (mCanRead)
            {
                if (mEmittedPropertyAccessor == null)
                {
                    Init();
                }

                return mEmittedPropertyAccessor.Get(target);
            }
            throw new Exception(
                string.Format("Property \"{0}\" does not have a get method.",
                              mProperty));
        }

        /// <summary>
        /// Sets the property for the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="value">Value to set.</param>
        public void Set(object target, object value)
        {
            if (mCanWrite)
            {
                if (mEmittedPropertyAccessor == null)
                {
                    Init();
                }

                //
                // Set the property value
                //
                mEmittedPropertyAccessor.Set(target, value);
            }
            else
            {
                throw new Exception(
                    string.Format("Property \"{0}\" does not have a set method.",
                                  mProperty));
            }
        }

        #endregion

        /// <summary>
        /// This method generates creates a new assembly containing
        /// the Type that will provide dynamic access.
        /// </summary>
        private void Init()
        {
            InitTypes();

            // Create the assembly and an instance of the 
            // property accessor class.
            Assembly assembly = EmitAssembly();

            mEmittedPropertyAccessor =
                assembly.CreateInstance("Property") as IPropertyAccessor;

            if (mEmittedPropertyAccessor == null)
            {
                throw new Exception("Unable to create property accessor.");
            }
        }

        /// <summary>
        /// Thanks to Ben Ratzlaff for this snippet of code
        /// http://www.codeproject.com/cs/miscctrl/CustomPropGrid.asp
        /// 
        /// "Initialize a private hashtable with type-opCode pairs 
        /// so i dont have to write a long if/else statement when outputting msil"
        /// </summary>
        private void InitTypes()
        {
            mTypeHash = new Hashtable();
            mTypeHash[typeof(sbyte)] = OpCodes.Ldind_I1;
            mTypeHash[typeof(byte)] = OpCodes.Ldind_U1;
            mTypeHash[typeof(char)] = OpCodes.Ldind_U2;
            mTypeHash[typeof(short)] = OpCodes.Ldind_I2;
            mTypeHash[typeof(ushort)] = OpCodes.Ldind_U2;
            mTypeHash[typeof(int)] = OpCodes.Ldind_I4;
            mTypeHash[typeof(uint)] = OpCodes.Ldind_U4;
            mTypeHash[typeof(long)] = OpCodes.Ldind_I8;
            mTypeHash[typeof(ulong)] = OpCodes.Ldind_I8;
            mTypeHash[typeof(bool)] = OpCodes.Ldind_I1;
            mTypeHash[typeof(double)] = OpCodes.Ldind_R8;
            mTypeHash[typeof(float)] = OpCodes.Ldind_R4;
        }

        /// <summary>
        /// Create an assembly that will provide the get and set methods.
        /// </summary>
        private Assembly EmitAssembly()
        {
            //
            // Create an assembly name
            //
            var assemblyName = new AssemblyName { Name = "PropertyAccessorAssembly" };

            //
            // Create a new assembly with one module
            //
            AssemblyBuilder newAssembly = Thread.GetDomain().DefineDynamicAssembly(assemblyName,
                                                                                   AssemblyBuilderAccess.Run);
            ModuleBuilder newModule = newAssembly.DefineDynamicModule("Module");

            //		
            //  Define a public class named "Property" in the assembly.
            //			
            TypeBuilder myType =
                newModule.DefineType("Property", TypeAttributes.Public);

            //
            // Mark the class as implementing IPropertyAccessor. 
            //
            myType.AddInterfaceImplementation(typeof(IPropertyAccessor));

            //            // Add a constructor
            //            ConstructorBuilder constructor =
            //                myType.DefineDefaultConstructor(MethodAttributes.Public);

            //
            // Define a method for the get operation. 
            //
            var getParamTypes = new[] { typeof(object) };
            Type getReturnType = typeof(object);
            MethodBuilder getMethod =
                myType.DefineMethod("Get",
                                    MethodAttributes.Public | MethodAttributes.Virtual,
                                    getReturnType,
                                    getParamTypes);

            //
            // From the method, get an ILGenerator. This is used to
            // emit the IL that we want.
            //
            ILGenerator getIL = getMethod.GetILGenerator();


            //
            // Emit the IL. 
            //
            MethodInfo targetGetMethod = mTargetType.GetMethod("get_" + mProperty);

            if (targetGetMethod != null)
            {
                getIL.DeclareLocal(typeof(object));
                getIL.Emit(OpCodes.Ldarg_1); //Load the first argument 
                //(target object)

                getIL.Emit(OpCodes.Castclass, mTargetType); //Cast to the source type

                getIL.EmitCall(OpCodes.Call, targetGetMethod, null); //Get the property value

                if (targetGetMethod.ReturnType.IsValueType)
                {
                    getIL.Emit(OpCodes.Box, targetGetMethod.ReturnType); //Box if necessary
                }
                getIL.Emit(OpCodes.Stloc_0); //Store it

                getIL.Emit(OpCodes.Ldloc_0);
            }
            else
            {
                getIL.ThrowException(typeof(MissingMethodException));
            }

            getIL.Emit(OpCodes.Ret);


            //
            // Define a method for the set operation.
            //
            var setParamTypes = new[] { typeof(object), typeof(object) };
            MethodBuilder setMethod =
                myType.DefineMethod("Set",
                                    MethodAttributes.Public | MethodAttributes.Virtual,
                                    null,
                                    setParamTypes);

            //
            // From the method, get an ILGenerator. This is used to
            // emit the IL that we want.
            //
            ILGenerator setIL = setMethod.GetILGenerator();
            //
            // Emit the IL. 
            //

            MethodInfo targetSetMethod = mTargetType.GetMethod("set_" + mProperty);
            if (targetSetMethod != null)
            {
                Type paramType = targetSetMethod.GetParameters()[0].ParameterType;

                setIL.DeclareLocal(paramType);
                setIL.Emit(OpCodes.Ldarg_1); //Load the first argument 
                //(target object)

                setIL.Emit(OpCodes.Castclass, mTargetType); //Cast to the source type

                setIL.Emit(OpCodes.Ldarg_2); //Load the second argument 
                //(value object)

                if (paramType.IsValueType)
                {
                    setIL.Emit(OpCodes.Unbox, paramType); //Unbox it 	
                    if (mTypeHash[paramType] != null) //and load
                    {
                        var load = (OpCode)mTypeHash[paramType];
                        setIL.Emit(load);
                    }
                    else
                    {
                        setIL.Emit(OpCodes.Ldobj, paramType);
                    }
                }
                else
                {
                    setIL.Emit(OpCodes.Castclass, paramType); //Cast class
                }

                setIL.EmitCall(OpCodes.Callvirt,
                               targetSetMethod, null); //Set the property value
            }
            else
            {
                setIL.ThrowException(typeof(MissingMethodException));
            }

            setIL.Emit(OpCodes.Ret);

            //
            // Load the type
            //
            myType.CreateType();

            return newAssembly;
        }
    }
}