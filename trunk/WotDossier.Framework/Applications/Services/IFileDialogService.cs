﻿using System;
using System.Collections.Generic;

namespace WotDossier.Framework.Applications.Services
{
    /// <summary>
    /// This service allows a user to specify a filename to open or save a file.
    /// </summary>
    /// <remarks>
    /// This interface is designed for simplicity. If you have to accomplish more advanced
    /// scenarios then we recommend implementing your own specific message service.
    /// </remarks>
    public interface IFileDialogService
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
        FileDialogResult ShowOpenFileDialog(IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName, string defaultDirectory = null);

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
        FileDialogResult ShowSaveFileDialog(IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName, string defaultDirectory = null);
    }
}
