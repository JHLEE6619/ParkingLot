using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client3.View
{
    /// <summary>
    /// Main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Btn_prepayment_Click(object sender, RoutedEventArgs e)
        {
            PrePayment page = new();
            this.NavigationService.Navigate(page);
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            Registration page = new();
            this.NavigationService.Navigate(page);
        }

        private void Btn_period_extension_Click(object sender, RoutedEventArgs e)
        {
            Period_Extension page = new();
            this.NavigationService.Navigate(page);
        }
    }
}
