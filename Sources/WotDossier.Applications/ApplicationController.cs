using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using WotDossier.Applications.ViewModel;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications
{
    [Export(typeof(ApplicationController))]
    public class ApplicationController : Controller
    {
        private ShellViewModel _shellViewModel;
        private readonly DelegateCommand _exitCommand;

        #region Commands


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController"/> class.
        /// </summary>
        [ImportingConstructor]
        public ApplicationController([Import(typeof(ShellViewModel))]ShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
            _exitCommand = new DelegateCommand(Close);
        }

        private void InitShellViewModel(ShellViewModel shellViewModel)
        {
            //shellViewModel.ExitCommand = _exitCommand;
        }

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

        public void InitReplaysCatalog()
        {
            string currentDirectory = Folder.AssemblyDirectory();
            string path = Path.Combine(currentDirectory, @"Data\ReplaysCatalog.xml");
            if (!File.Exists(path))
            {
                byte[] embeddedResource = GetEmbeddedResource(@"WotDossier.Data.ReplaysCatalog.xml", Assembly.GetEntryAssembly());
                using (FileStream fileStream = File.OpenWrite(path))
                {
                    fileStream.Write(embeddedResource, 0, embeddedResource.Length);
                    fileStream.Flush();
                }
            }
        }

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
