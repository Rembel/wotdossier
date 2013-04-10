using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Common.Logging;

namespace WotDossier.Test
{
    public class CompositionContainerFactory
    {
        protected static readonly ILog _log = LogManager.GetLogger("CompositionContainerFactory");
        private CompositionContainer _container;
        private static readonly object _syncObject = new object();
        private static volatile CompositionContainerFactory _instance = new CompositionContainerFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static CompositionContainerFactory()
        {
        }

        public static CompositionContainerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new CompositionContainerFactory();
                        }
                    }
                }
                return _instance;
            }
        }

        public CompositionContainer Container
        {
            get
            {
                if (_container == null)
                {
                    var directoryCatalog = new DirectoryCatalog(Environment.CurrentDirectory);
                    _container = new CompositionContainer(new AggregateCatalog(directoryCatalog));
                }
                return _container;
            }
        }
    }
}
