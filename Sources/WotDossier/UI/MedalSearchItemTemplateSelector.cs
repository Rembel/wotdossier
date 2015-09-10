using System.Windows;
using System.Windows.Controls;
using WotDossier.Applications.ViewModel;

namespace WotDossier.UI
{
    public class MedalSearchItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MedalGroupTemplate { get; set; }
        public DataTemplate MedalSearchItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CheckListItem<int>)
            {
                return MedalSearchItemTemplate;
            }
            return MedalGroupTemplate;
        }
    }
}