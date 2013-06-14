using System.Windows;
using System.Windows.Controls;

namespace WotDossier.Resources
{
    public class EpicMedalTeplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/> based on custom logic.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
        /// </returns>
        /// <param name="item">The data object for which to select the template.</param><param name="container">The data-bound object.</param>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {


            return base.SelectTemplate(item, container);
        }

        public DataTemplate BoelterColumnHeaderTemplate { get; set; }
        public DataTemplate RadleyWaltersColumnHeaderTemplate { get; set; }
        public DataTemplate LafayettePoolColumnHeaderTemplate { get; set; }
        public DataTemplate OrlikColumnHeaderTemplate { get; set; }
        public DataTemplate OskinColumnHeaderTemplate { get; set; }

        public DataTemplate LehvaslaihoColumnHeaderTemplate { get; set; }

        public DataTemplate NikolasColumnHeaderTemplate { get; set; }

        public DataTemplate HalonenColumnHeaderTemplate { get; set; }

        public DataTemplate BurdaColumnHeaderTemplate { get; set; }

        public DataTemplate PascucciColumnHeaderTemplate { get; set; }

        public DataTemplate DumitruColumnHeaderTemplate { get; set; }

        public DataTemplate TamadaYoshioColumnHeaderTemplate { get; set; }

        public DataTemplate BillotteColumnHeaderTemplate { get; set; }

        public DataTemplate BrunoPietroColumnHeaderTemplate { get; set; }

        public DataTemplate TarczayColumnHeaderTemplate { get; set; }

        public DataTemplate KolobanovColumnHeaderTemplate { get; set; }

        public DataTemplate FadinColumnHeaderTemplate { get; set; }
    }
}
