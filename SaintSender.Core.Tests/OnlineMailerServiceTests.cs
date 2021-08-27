using System;
using System.Collections.Generic;
using NUnit.Framework;
using SaintSender.Core.Models;
using SaintSender.Core.Services;

namespace SaintSender.Core.Tests
{
    [TestFixture]
    class OnlineMailerServiceTests
    {
        [Test]
        public void EmailSendingTest()
        {
            OnlineMailerService mailer = new OnlineMailerService();
            mailer.LogInUser("almail.alma.mama@gmail.com", "whyTho123");
            EmailMessage mail = new EmailMessage("almail.alma.mama@gmail.com", "lehel.markon@gmail.com", "Subject", "BodyElement");
            mailer.SendMail(mail);
        }

    }
}
