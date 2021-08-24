using NUnit.Framework;
using SaintSender.Core.Models;
using System;
using System.Collections.Generic;
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
        public void CanRetrieveDataFromIso_SignleLine()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            storageAccess.WriteUserData("TestStore.txt", "Hello Isolated Storage");
            
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Open, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    Assert.AreEqual(reader.ReadToEnd().Trim(), "Hello Isolated Storage");
                }
            }
        }

        [Test]
        public void CanRetrieveDataFromIso_MultipleLine()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            List<string> testList = new List<string>() { "Hello", "Isolated", "Storage" };
            storageAccess.WriteUserData("TestStore.txt", testList);

            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Open, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    Assert.AreEqual(storageAccess.ReadData("TestStore.txt"), testList);
                }
            }
            testList = null;
        }





        [TearDown]
        public void TearDown()
        {
            //Remove TestStore.txt
        }
    }
}
