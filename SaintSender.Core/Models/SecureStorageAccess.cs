using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace SaintSender.Core.Models
{
    class SecureStorageAccess
    {
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

        //Read from secure storage

        private void Test()
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

        //Write to secure storage (only seed)
        private void Seed()
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.CreateNew, isoStore)) 
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                { 
                writer.WriteLine("Hello Isolated Storage");
                Console.WriteLine("You have written to the file.");
                }
            };
        }
    }
}

