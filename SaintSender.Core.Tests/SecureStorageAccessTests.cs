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

        IsolatedStorageFile isoStore;
        SecureStorageAccess storageAccess;
        [SetUp]
        public void SetUp()
        {
            isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            storageAccess = new SecureStorageAccess();
        }

        [Test]
        public void CanRetrieveDataFromIso_SignleLine()
        {
            storageAccess.WriteData("TestStore", "Hello Isolated Storage");
            
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Open, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    Assert.AreEqual(storageAccess.ReadData("TestStore")[0], "Hello Isolated Storage");
                }
                
            }
            
        }

        [Test]
        public void CanRetrieveDataFromIso_MultipleLine()
        {
            List<string> testList = new List<string>() { "Hello", "Isolated", "Storage" };
            storageAccess.WriteData("TestStore", testList);

            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Create, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    Assert.AreEqual(storageAccess.ReadData("TestStore"), testList);
                }
            }
            testList = null;
        }

        [Test]
        public void CanRetrieveUserDataForSpecificUser()
        {
            storageAccess.WriteData("UserData", new List<string>() { "test@gmail.com", "HASHPWD" });
            Dictionary<string,string> returnDict =  storageAccess.GetUserLoginData("test@gmail.com");
            Assert.AreEqual(returnDict, new Dictionary<string, string>() { { "test@gmail.com", "HASHPWD" } });

        }

        [Test]
        public void AddingAlreadyExistingUser_ThrowsError()
        {
            try
            {
                storageAccess.SaveUser("test@gmail.com", "HASHPWD");
                storageAccess.SaveUser("test@gmail.com", "HASHPWD");
            }
            catch(Exception)
            {
                Assert.Pass();
            }
        }



        [TearDown]
        public void TearDown()
        {
            storageAccess = null;
            isoStore = null;
        }
    }
}
