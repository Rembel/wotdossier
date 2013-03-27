using System;
using System.Reflection;

namespace WotDossier.Framework.Presentation.Services
{
    /// <summary>
    /// This class provides information about the running application.
    /// </summary>
    public static class ApplicationInfo
    {
        private static string _productName;
        private static bool _productNameCached;
        private static string _version;
        private static bool _versionCached;
        private static string _company;
        private static bool _companyCached;
        private static string _copyright;
        private static bool _copyrightCached;


        /// <summary>
        /// Gets the full product name of the application.
        /// </summary>
        public static string FullProductName
        {
            get { return string.Format("{0} {1}", ProductName, Version); }
        }

        /// <summary>
        /// Gets the product name of the application.
        /// </summary>
        public static string ProductName
        {
            get
            {
                if (!_productNameCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyProductAttribute attribute = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
                            entryAssembly, typeof(AssemblyProductAttribute)));
                        _productName = (attribute != null) ? attribute.Product : "";
                    }
                    else
                    {
                        _productName = "";
                    }
                    _productNameCached = true;
                }
                return _productName;
            }
        }

        /// <summary>
        /// Gets the version number of the application.
        /// </summary>
        public static string Version
        {
            get
            {
                if (!_versionCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        _version = entryAssembly.GetName().Version.ToString();
                    }
                    else
                    {
                        _version = "";
                    }
                    _versionCached = true;
                }
                return _version;
            }
        }

        /// <summary>
        /// Gets the company of the application.
        /// </summary>
        public static string Company
        {
            get
            {
                if (!_companyCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyCompanyAttribute attribute = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(
                            entryAssembly, typeof(AssemblyCompanyAttribute)));
                        _company = (attribute != null) ? attribute.Company : "";
                    }
                    else
                    {
                        _company = "";
                    }
                    _companyCached = true;
                }
                return _company;
            }
        }

        /// <summary>
        /// Gets the copyright information of the application.
        /// </summary>
        public static string Copyright
        {
            get
            {
                if (!_copyrightCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyCopyrightAttribute attribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                            entryAssembly, typeof(AssemblyCopyrightAttribute));
                        _copyright = attribute != null ? attribute.Copyright : "";
                    }
                    else
                    {
                        _copyright = "";
                    }
                    _copyrightCached = true;
                }
                return _copyright;
            }
        }

        /// <summary>
        /// Loads the command line arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void LoadLineArguments(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                //foreach (string arg in args)
                //{
                //    if (arg.ToUpper().Equals(LINE_ARG_ERB))
                //    {
                //        _exportResultsButton = true;
                //    }

                //    if (arg.ToUpper().Equals(LINE_ARG_TL))
                //    {
                //        _testDataLog = true;
                //    }

                //    if (arg.ToUpper().Equals(LINE_ARG_DCB))
                //    {
                //        _disabledCalculationBuffer = true;
                //    }
                //}
            }
        }
    }
}
