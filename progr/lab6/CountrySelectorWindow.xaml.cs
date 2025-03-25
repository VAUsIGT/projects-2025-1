using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Lab6
{
    public partial class CountrySelectorWindow : Window
    {
        public Country SelectedCountry { get; private set; }

        public CountrySelectorWindow()
        {
            InitializeComponent();
            lstCountries.ItemsSource = new List<Country>
            {
                new Country { Name = "Россия", Code = "+7", FlagPath = "Resourses/russia.png" },
                new Country { Name = "Беларусь", Code = "+375", FlagPath = "Resourses/belarus.png" },
                new Country { Name = "Казахстан", Code = "+77", FlagPath = "Resourses/kazakhstan.png" },
                new Country { Name = "США", Code = "+1", FlagPath = "Resourses/america.png" },
                new Country { Name = "Китай", Code = "+86", FlagPath = "Resourses/china.png" },
                new Country { Name = "Индия", Code = "+91", FlagPath = "Resourses/india.png" },
                new Country { Name = "Япония", Code = "+81", FlagPath = "Resourses/japan.png" },
                new Country { Name = "Германия", Code = "+49", FlagPath = "Resourses/germany.png" }
            };
        }

        private void lstCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCountry = (Country)lstCountries.SelectedItem;
            DialogResult = true;
        }
    }

    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string FlagPath { get; set; }
    }
}