﻿using SaintSender.Core.Interfaces;
using SaintSender.Core.Models;
using SaintSender.Core.Services;

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
        private IMailerClient _mailService = new OnlineMailerService();

        /// <summary>
        /// Gets or sets value of Greeting.
        /// </summary>
        public string Greeting
        {
            get => _greeting;
            set => SetProperty(ref _greeting, value);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        public void LogIn(string email, string password)
        {
            _mailService.LogInUser(email, password);
        }

        public void RefreshMails()
        {
            _mailService.LoadMails();
        }

        public EmailMessage[] GetEmailList()
        {
            return _mailService.Emails.ToArray();
        }
    }
}
