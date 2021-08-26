using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SaintSender.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool isClosed { get; private set; } = false;
        public string emailAddress => TextboxEmail.Text;
        public string password => Passwordbox.Password;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        private bool CredentialsNotFilled => TextboxEmail == null || Passwordbox == null ? true : TextboxEmail.Text.Equals("Email address") && Passwordbox.Password.Equals("Password");

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CredentialsNotFilled)
            {
                isClosed = true;
            }
            DialogResult = true;
        }

        private void TextboxEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextboxEmail.Text == "Email address")
                TextboxEmail.Text = null;
            TextboxEmail.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void TextboxEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextboxEmail.Text == "")
                TextboxEmail.Text = "Email address";
            TextboxEmail.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        private void PasswordboxWatermark_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordboxWatermark.Visibility = System.Windows.Visibility.Collapsed;
            Passwordbox.Visibility = System.Windows.Visibility.Visible;
            Passwordbox.Focus();
        }

        private void Passwordbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Passwordbox.Password))
            {
                Passwordbox.Visibility = System.Windows.Visibility.Collapsed;
                PasswordboxWatermark.Visibility = System.Windows.Visibility.Visible;
                Passwordbox.Password = "Password";
                Passwordbox.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
            Passwordbox.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        private void Passwordbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Passwordbox.Password == "Password")
                Passwordbox.Password = null;
            Passwordbox.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            UpdateLoginButton();
        }

        private void TextboxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLoginButton();
        }

        private void Passwordbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateLoginButton();
        }

        private void UpdateLoginButton()
        { if (LoginButton != null)
                LoginButton.Text = CredentialsNotFilled ? "Close" : "Login";
        }
    }
}
