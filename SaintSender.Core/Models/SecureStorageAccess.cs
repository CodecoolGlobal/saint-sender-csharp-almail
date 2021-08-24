using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace SaintSender.Core.Models
{
    public class SecureStorageAccess
    {
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

        //Read from secure storage

        public void ReadUserData(string userName)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Open, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    Console.WriteLine("Reading contents:");
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }

        public SecureStorageAccess()
        {
            Seed();
        }



        //Write to secure storage (only seed)
        #region Seed
        private void Seed()
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Create, isoStore)) 
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                writer.WriteLine("Hello Isolated Storage");
                }
            };
        }
        #endregion
    }
}

