using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace SCFollowerCompare.UWP
{
    public class FileReadWrite : IFileReadWrite
    {
        public async Task<string> ReadFromFile()
        {
            string result = string.Empty;

            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.GetFileAsync("ArtistList.json");
                result = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            }
            catch(Exception ex)
            {
                throw new Exception("File Reading Error Occured", ex.InnerException);
            }

            return result;
        }

        public async Task<bool> WriteToFile(string text)
        {
            bool result = true;
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.CreateFileAsync("ArtistList.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, text);
            }
            catch(Exception ex)
            {
                throw new Exception("File writing Error Occured", ex.InnerException);
            }

            return result;
        }
    }
}
