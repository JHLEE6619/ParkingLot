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
    /// PrePayment.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PrePayment : Page
    {
        public PrePayment()
        {
            InitializeComponent();
        }

        private void Btn_input_VehicleNum_Click(object sender, RoutedEventArgs e)
        {
            Btn_Payment.Visibility = Visibility.Visible;
        }

        private void Btn_Payment_Click(object sender, RoutedEventArgs e)
        {
            Payment payment = new Payment();
            this.NavigationService.Navigate(payment);
        }
    }
}
