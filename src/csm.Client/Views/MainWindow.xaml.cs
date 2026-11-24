using csm.Client.Views;
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

namespace csm.Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainFrame.Navigate(new HomePage());
    }

    private async void Search_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            string gameName = GameNameBox.Text;
            string tag = TagBox.Text;

            //MainFrame.Navigate(new Profile());
            // use apiservice here 
        }
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new HomePage());
    }

    private void Tierlist_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Tierlists());
        
    }

    private void Leaderboard_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Leaderboard());
    }

    private void MainFrame_Navigated(object sender, NavigationEventArgs e)
    {
        search.Visibility = e.Content is (csm.Client.Views.Leaderboard or Tierlists) ? Visibility.Visible : Visibility.Collapsed;
    }
}