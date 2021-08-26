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
using System.Windows.Shapes;

namespace SaintSender.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public string emailAddress => TextboxEmail.Text;
        public string password => Passwordbox.Password;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
