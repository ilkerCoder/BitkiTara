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

namespace MyWpfApp.View.UserControls
{
    /// <summary>
    /// TopControls.xaml etkileşim mantığı
    /// </summary>
    public partial class TopControls : UserControl
    {
        public TopControls()
        {
            InitializeComponent();
        }
        private void btnKapat_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }
    }
}
