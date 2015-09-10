using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using WotDossier.Applications.ViewModel;
using WotDossier.Dal;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications
{
    [Export(typeof(ReplaysManagerController))]
    public class ReplaysManagerController : Controller
    {
        private ReplayManagerShellViewModel _shellViewModel;
        private readonly DelegateCommand _exitCommand;

        #region Commands


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController"/> class.
        /// </summary>
        [ImportingConstructor]
        public ReplaysManagerController([Import(typeof(ReplayManagerShellViewModel))]ReplayManagerShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
            _exitCommand = new DelegateCommand(Close);
        }

        private void InitShellViewModel(ReplayManagerShellViewModel shellViewModel)
        {
            //shellViewModel.ExitCommand = _exitCommand;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            InitReplaysCatalog();

            InitShellViewModel(_shellViewModel);
            _shellViewModel.Show();
        }

        private void Close()
        {
            _shellViewModel.Close();
        }

        /// <summary>
        /// Initializes the replays catalog.
        /// </summary>
        public void InitReplaysCatalog()
        {
            string currentDirectory = Folder.AssemblyDirectory();
            string path = Path.Combine(currentDirectory, @"Data\ReplaysCatalog.xml");
            if (!File.Exists(path))
            {
                var resourceName = Assembly.GetEntryAssembly().GetName().Name + @".Data.ReplaysCatalog.xml";
                byte[] embeddedResource = GetEmbeddedResource(resourceName, Assembly.GetEntryAssembly());
                using (FileStream fileStream = File.OpenWrite(path))
                {
                    fileStream.Write(embeddedResource, 0, embeddedResource.Length);
                    fileStream.Flush();
                }
            }
        }

        /// <summary>
        /// Gets the embedded resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static byte[] GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                int length = Convert.ToInt32(resourceStream.Length); // get strem length
                byte[] byteArr = new byte[length]; // create a byte array
                resourceStream.Read(byteArr, 0, length);

                return byteArr;
            }
        }
    }
}
