using System;

namespace WotDossier.Common.Python.Objects
{

    /// <summary>
    /// This object constructor uses reflection to create instances of any given class. 
    /// </summary>
    public class AnyClassConstructor : IObjectConstructor
    {

        private Type type;

        public AnyClassConstructor(Type type)
        {
            this.type = type;
        }

        public object construct(object[] args)
        {
            try
            {
                return Activator.CreateInstance(type, args);
            }
            catch (Exception x)
            {
                throw new PickleException("problem constructing object", x);
            }
        }
    }

}