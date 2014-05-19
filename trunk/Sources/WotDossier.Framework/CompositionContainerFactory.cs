using SimpleInjector;

namespace WotDossier.Framework
{
    public class CompositionContainerFactory
    {
        //private CompositionContainer _container;
        private static readonly object _syncObject = new object();
        private static volatile CompositionContainerFactory _instance;
        private Container _simpleContainer;

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

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            SimpleContainer.Register<TService, TImplementation>();
        }

        public void RegisterSingle<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            SimpleContainer.RegisterSingle<TService, TImplementation>();
        }

        //private CompositionContainer Container
        //{
        //    get
        //    {
        //        if (_container == null)
        //        {
        //            var directoryCatalog =
        //                new DirectoryCatalog(
        //                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        //            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(Assembly.GetEntryAssembly());
        //            _container = new CompositionContainer(new AggregateCatalog(directoryCatalog, assemblyCatalog));
        //        }
        //        return _container;
        //    }
        //}

        public Container SimpleContainer
        {
            get
            {
                if (_simpleContainer == null)
                {
                    _simpleContainer = new Container();
                }
                return _simpleContainer;
            }
        }

        //public T GetExport<T>(string contractName)
        //{
        //    var export = Container.GetExport<T>(contractName);
        //    if (export != null) return export.Value;
        //    return default(T);
        //}

        public T GetExport<T>() where T: class 
        {
            T instance = SimpleContainer.GetInstance<T>();
            return instance;

            //var export = Container.GetExport<T>();
            //if (export != null) return export.Value;
            //return default(T);
        }

        public void Dispose()
        {
            //Container.Dispose();
        }
    }
}
