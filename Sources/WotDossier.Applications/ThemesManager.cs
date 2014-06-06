using System;
using System.Collections.Generic;
using System.Windows;
using WotDossier.Domain;

namespace WotDossier.Applications
{
    public class ThemesManager
    {
        private static readonly Dictionary<DossierTheme, Uri> _themes = new Dictionary<DossierTheme, Uri>
        {
            {DossierTheme.Silver, new Uri("Styles/silver/ImplicitStyles.xaml", UriKind.RelativeOrAbsolute)},
            {DossierTheme.Black, new Uri("Styles/Black/ImplicitStyles.xaml", UriKind.RelativeOrAbsolute)},
        };

        public static void ApplyTheme(DossierTheme theme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = _themes[theme] });
        }
    }
}