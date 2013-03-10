using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using WotDossier.Framework.Applications.Services;
using Microsoft.Win32;

namespace WotDossier.Framework.Presentation.Services
{
    /// <summary>
    /// This is the default implementation of the <see cref="IFileDialogService"/>. It shows a open or save file dialog box.
    /// </summary>
    /// <remarks>
    /// If the default implementation of this service doesn't serve your need then you can provide your own implementation.
    /// </remarks>
    [Export(typeof(IFileDialogService))]
    public class FileDialogService : IFileDialogService
    {
        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file type.</param>
        /// <param name="defaultFileName">Default filename.</param>
        /// <param name="defaultDirectory">Default directory.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public FileDialogResult ShowOpenFileDialog(IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName, string defaultDirectory = null)
        {
            if (fileTypes == null) { throw new ArgumentNullException("fileTypes"); }
            if (!fileTypes.Any()) { throw new ArgumentException("The fileTypes collection must contain at least one item."); }

            OpenFileDialog dialog = new OpenFileDialog();
            SetInitialDirectory(defaultDirectory, dialog);

            return ShowFileDialog(dialog, fileTypes, defaultFileType, defaultFileName);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file type.</param>
        /// <param name="defaultFileName">Default filename.</param>
        /// <param name="defaultDirectory">Default directory.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public FileDialogResult ShowSaveFileDialog(IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName, string defaultDirectory = null)
        {
            if (fileTypes == null) { throw new ArgumentNullException("fileTypes"); }
            if (!fileTypes.Any()) { throw new ArgumentException("The fileTypes collection must contain at least one item."); }

            SaveFileDialog dialog = new SaveFileDialog();
            SetInitialDirectory(defaultDirectory, dialog);

            return ShowFileDialog(dialog, fileTypes, defaultFileType, defaultFileName);
        }

        private static FileDialogResult ShowFileDialog(FileDialog dialog, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            int filterIndex = fileTypes.ToList().IndexOf(defaultFileType);
            if (filterIndex >= 0) { dialog.FilterIndex = filterIndex + 1; }
            if (!string.IsNullOrEmpty(defaultFileName)) { dialog.FileName = defaultFileName; }

            dialog.Filter = CreateFilter(fileTypes);
            if (dialog.ShowDialog() == true)
            {
                if (dialog.FilterIndex - 1 < fileTypes.Count())
                {
                    defaultFileType = fileTypes.ElementAt(dialog.FilterIndex - 1);
                }
                else
                {
                    defaultFileType = null;
                }
                return new FileDialogResult(dialog.FileName, defaultFileType);
            }
            else
            {
                return new FileDialogResult();
            }
        }

        private static string CreateFilter(IEnumerable<FileType> fileTypes)
        {
            string filter = "";
            foreach (FileType fileType in fileTypes)
            {
                if (!String.IsNullOrEmpty(filter)) { filter += "|"; }
                filter += fileType.Description + "|*" + fileType.FileExtension;
            }
            return filter;
        }

        private void SetInitialDirectory(string defaultDirectory, FileDialog dialog)
        {
            if (!string.IsNullOrEmpty(defaultDirectory))
            {
                dialog.InitialDirectory = defaultDirectory;
            }
        }

    }
}
