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
using csm.Client.ViewModel;

namespace csm.Client.Views
{
    /// <summary>
    /// Interaction logic for Tierlists.xaml
    /// </summary>
    public partial class Tierlists : Page
    {
        public Tierlists()
        {
            InitializeComponent();

            var vm = new TierlistViewModel();

            //MessageBox.Show($"Champions: {vm.Champions.Count}");

            DataContext = vm;


        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new Build());
        }
    }
}
