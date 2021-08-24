using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace SaintSender.Core.Models
{
    public class SecureStorageAccess
    {
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

        public void SaveUser(string email, string hashedPassword)
        {
            if (isoStore.FileExists("UserData.txt"))
            {
                if (!ReadData("UserData").Contains(email))
                {
                    WriteData("UserData", new List<string> { email, hashedPassword }, FileMode.Append);
                } else
                {
                    throw new Exception($"The e-mail address {email} is already taken.");
                }
            } else
            {
                WriteData("UserData", new List<string> { email, hashedPassword }, FileMode.CreateNew);
            }
        }

        public Dictionary<string,string> GetUserLoginData(string email)
        {
            if (isoStore.FileExists("UserData.txt"))
            {
                List<string> userDataList = ReadData("UserData");
                try
                {
                    int index = userDataList.IndexOf(email);
                    return new Dictionary<string, string>() { { email, userDataList[index+1] } };
                }
                catch (Exception)
                {

                    throw new Exception("The user does not exist.");
                }
                
            } else
            {
                throw new Exception("The user does not exist.");
            }
        }

        //Read from secure storage

        public List<string> ReadData(string fileName)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName + ".txt", FileMode.Open, isoStore))
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

        

        //Write to secure storage
        public void WriteData(string fileToWrite, List<string> data, FileMode writeMethod = FileMode.Create)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileToWrite + ".txt", writeMethod, isoStore)) 
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

        public void WriteData(string fileToWrite, string data, FileMode writeMethod = FileMode.Create)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileToWrite + ".txt", writeMethod, isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.WriteLine(data);
                }
            };
        }
    }
}

