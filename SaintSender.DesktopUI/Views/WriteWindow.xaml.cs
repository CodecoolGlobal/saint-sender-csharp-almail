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
    /// Interaction logic for WriteWindow.xaml
    /// </summary>
    public partial class WriteWindow : Window
    {
        public WriteWindow()
        {
            InitializeComponent();
            TextareaMessage.Document.Blocks.Clear();
        }

        public string Receiver => TextBoxTo.Text;
        public string Subject => TextBoxSubject.Text;
        public string Body =>new TextRange(TextareaMessage.Document.ContentStart, TextareaMessage.Document.ContentEnd).Text;

        private void ButtonSend_OnClick(object sender, EventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, EventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
