using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace csm.Client.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string gameName = GameNameBox.Text;
                string tag = TagBox.Text;

                
                // use apiservice here 
            }
        }

        private void Tierlist_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string gameName = GameNameBox.Text;
                string tag = TagBox.Text;

                //NavigationService?.Navigate(new Profile(gamename, tag));
                // use apiservice here 
            }
        }
    }
}
