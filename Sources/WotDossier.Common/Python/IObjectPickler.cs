using System.IO;

namespace WotDossier.Common.Python
{

    /// <summary>
    /// Interface for object Picklers used by the pickler, to pickle custom classes. 
    /// </summary>
    public interface IObjectPickler
    {
        /**
         * Pickle an object.
         */
        void pickle(object o, Stream outs, Pickler currentPickler);
    }

}
