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
    /// Registration.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Combox_select(object sender, SelectionChangedEventArgs e)
        {
            Btn_Payment.Visibility = Visibility.Visible;
        }

        private void Btn_Payment_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("정기 차량 등록이 완료되었습니다.", "등록 완료");
            this.NavigationService.GoBack();
        }
    }
}
