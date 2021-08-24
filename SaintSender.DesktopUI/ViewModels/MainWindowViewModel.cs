using SaintSender.Core.Interfaces;
using SaintSender.Core.Services;
using System.ComponentModel;

namespace SaintSender.DesktopUI.ViewModels
{
    /// <summary>
    /// ViewModel for Main window. Contains all shown information
    /// and necessary service classes to make view functional.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private string _name;
        private string _greeting;
        private readonly IGreetService _greetService;

        /// <summary>
        /// Gets or sets value of Greeting.
        /// </summary>
        public string Greeting
        {
            get { return _greeting; }
            set
            {
                SetProperty<string>(ref _greeting, value);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty<string>(ref _name, value);
            }
        }

        public MainWindowViewModel()
        {
            Name = string.Empty;
            _greetService = new GreetService();
        }

        /// <summary>
        /// Call a vendor service and apply its value into <see cref="Greeting"/> property.
        /// </summary>
        public void Greet()
        {
            Greeting = _greetService.Greet(Name);
        }
    }
}
