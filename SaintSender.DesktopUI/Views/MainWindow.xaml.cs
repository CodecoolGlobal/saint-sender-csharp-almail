using SaintSender.Core.Models;
using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace SaintSender.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;
        public string searchText => TextboxSearch.Text;

        public MainWindow()
        {
            // set DataContext to the ViewModel object
            _vm = new MainWindowViewModel();

            DataContext = _vm;
            InitializeComponent();

            Login();
            UpdatePagination();
        }

        private void Login()
        {
            EmailDisplayList.Visibility = Visibility.Hidden;

            LoginWindow loginWindow = new LoginWindow();
            bool? loginResult = loginWindow.ShowDialog();
            if (loginResult.HasValue || loginResult.Value)
            {
                if (loginWindow.isClosed)
                {
                    System.Windows.Application.Current.Shutdown();
                    return;
                }
                if (_vm.LogIn(loginWindow.emailAddress, loginWindow.password))
                {
                    EmailDisplayList.UpdateEmailList(_vm.GetEmailList());
                    EmailDisplayList.Visibility = Visibility.Visible;
                }
                else
                    Login();
            } 
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
            // TODO: quit application if X clicked in login form
            /*else
                Application.Exit();*/
        }

        private void UpdatePagination()
        {
            if (ButtonPreviousPage != null)
            {
                ButtonPreviousPage.IsEnabled = EmailDisplayList.CanNavigatePrevious;
                ButtonPreviousPage.Refresh();
            }

            if (ButtonNextPage != null)
            {
                ButtonNextPage.IsEnabled = EmailDisplayList.CanNavigateNext;
                ButtonNextPage.Refresh();
            }

            if (LabelPagination != null)
                LabelPagination.Content = EmailDisplayList.PaginationText;
        }

        private void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            _vm.RefreshMails();
            EmailDisplayList.UpdateEmailList(_vm.GetEmailList());
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            _vm.LogOut();
            Login();
        }

        private void ButtonWrite_Click(object sender, RoutedEventArgs e)
        {
            WriteWindow writeWindow = new WriteWindow();
            bool? result = writeWindow.ShowDialog();

            if (result.HasValue && result.Value)
                _vm.SendMail(writeWindow.Receiver, writeWindow.Subject, writeWindow.Body);
        }
        
        private void ButtonPreviousPage_OnClick(object sender, EventArgs e)
        {
            EmailDisplayList.NavigatePagePrevious();
            UpdatePagination();
        }

        private void ButtonNextPage_OnClick(object sender, EventArgs e)
        {
            EmailDisplayList.NavigatePageNext();
            UpdatePagination();
        }
        
        private void TextboxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextboxSearch.Text == "")
                TextboxSearch.Text = "Search...";
            TextboxSearch.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        private void TextboxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextboxSearch.Text == "Search...")
                TextboxSearch.Text = null;
            TextboxSearch.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void ButtonReceived_Click(object sender, RoutedEventArgs e)
        {
            EmailDisplayList.FilterEmails(UserControls.MailFilter.Received);
            UpdatePagination();
        }

        private void ButtonSent_Click(object sender, RoutedEventArgs e)
        {
            EmailDisplayList.FilterEmails(UserControls.MailFilter.Sent);
            UpdatePagination();
        }

        private void ButtonAll_Click(object sender, RoutedEventArgs e)
        {
            EmailDisplayList.FilterEmails(UserControls.MailFilter.All);
            UpdatePagination();
        }

        private void TextboxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            EmailDisplayList.SearchText(TextboxSearch.Text == "Search..." ? "" : TextboxSearch.Text);
            UpdatePagination();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmailDisplayList.EmailReadStatusChanged += EmailDisplayList_EmailReadStatusChanged;
        }

        private void EmailDisplayList_EmailReadStatusChanged(object sender, UserControls.EmailReadStatusEventArgs e)
        {
            _vm.UpdateEmailReadStatus(e.EmailIndex, e.EmailStatus);
        }
    }
}
