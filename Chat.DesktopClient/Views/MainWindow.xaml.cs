namespace Chat.DesktopClient.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public object List { get; internal set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }

    public class GetMVVM
    {
        public void GetMsg(string message)
        {

            ((MainWindow)System.Windows.Application.Current.MainWindow).ListMsg.Items.Add(message);

        }
    }
}
