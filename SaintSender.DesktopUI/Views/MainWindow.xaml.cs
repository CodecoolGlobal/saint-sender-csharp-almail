using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;
using System.Windows;
using System.Windows.Forms;


namespace SaintSender.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;

        public MainWindow()
        {
            // set DataContext to the ViewModel object
            _vm = new MainWindowViewModel();
           
            DataContext = _vm;
            InitializeComponent();

            LoginWindow loginWindow = new LoginWindow();
            bool? loginResult = loginWindow.ShowDialog();
            if (loginResult.HasValue || loginResult.Value) {
                _vm.LogIn(loginWindow.emailAddress, loginWindow.password);
            }
        }
    }
}
