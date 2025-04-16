using System.Collections.ObjectModel;
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

namespace WpfTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// 
public class Test
{
    public int A { get; set; }
    public string B { get; set; }
    public string C { get; set; }
}

public partial class MainWindow : Window
{
    public static ObservableCollection<Brush> Color { get; set; } = [];
    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        Color.Add(Brushes.Aqua);
        Color.Add(Brushes.Aqua);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Color.RemoveAt(0);
        Color.Insert(0, Brushes.Red);
        Color.RemoveAt(1);
        Color.Insert(1, Brushes.Green);
    }
}