using SaintSender.Core.Models;
using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;
using System;
using System.Runtime.InteropServices;
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
            ButtonPreviousPage.IsEnabled = EmailDisplayList.CanNavigatePrevious;
            ButtonNextPage.IsEnabled = EmailDisplayList.CanNavigateNext;
            LabelPagination.Content = EmailDisplayList.PaginationText;
            ButtonPreviousPage.Refresh();
            ButtonNextPage.Refresh();
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
    }
}
