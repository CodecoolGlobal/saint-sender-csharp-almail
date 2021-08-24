using NUnit.Framework;
using SaintSender.Core.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;

namespace SaintSender.Core.Tests
{
    [TestFixture]
    class SecureStorageAccessTests
    {

        SecureStorageAccess storageAccess;
        [SetUp]
        public void SetUp()
        {
            storageAccess = new SecureStorageAccess();
        }

        [Test]
        public void CanRetrieveDataFromIso()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Open, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    Assert.AreEqual(reader.ReadToEnd(), "Hello Isolated Storage");
                    Console.WriteLine(reader.ReadToEnd());
                }
            }

        }





        [TearDown]
        public void TearDown()
        {

        }


    }
}
