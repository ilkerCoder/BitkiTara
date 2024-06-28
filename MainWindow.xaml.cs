using MyWpfApp.classes;

using System.Windows;

using System.Windows.Input;
using System.Windows.Media;

using MyWpfApp.View.UserControls;
using MyWpfApp.View.UserControls;
using System.Windows.Controls;

namespace MyWpfApp
{
    public partial class MainWindow : Window
    {



        public MainWindow()
        {

            InitializeComponent();
              //Load sample data

          


        }


        private void IslemKayitlari_Click(object sender, RoutedEventArgs e)
        {
            UcCagir.Uc_Cagir(Content_icerik ,new IslemKayitlari());
            HomeBorder.Background = new SolidColorBrush(Colors.Transparent);

            

        }





        private void HomeImage_Click(object sender, MouseButtonEventArgs e)
        {
            HomeBorder.Background = new SolidColorBrush(Colors.Gray);

            

            UcCagir.Uc_Cagir(Content_icerik, new MainContent());
        }
    }

    }


