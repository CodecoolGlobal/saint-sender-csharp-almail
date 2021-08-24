using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace SaintSender.Core.Models
{
    public class SecureStorageAccess
    {
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

        //Read from secure storage

        public List<string> ReadData(string fileName)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    List<string> stringList = new List<string>();
                    while (!reader.EndOfStream) { 
                        stringList.Add(reader.ReadLine());
                    }
                    return stringList;

                }
            }
        }



        //Write to secure storage (only seed)
        public void WriteUserData(string fileToWrite, List<string> data)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileToWrite, FileMode.Create, isoStore)) 
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    foreach (string item in data)
                    {
                        writer.WriteLine(item);
                    }
                }
            };
        }

        public void WriteUserData(string fileToWrite, string data)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileToWrite, FileMode.Create, isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.WriteLine(data);
                }
            };
        }
    }
}

