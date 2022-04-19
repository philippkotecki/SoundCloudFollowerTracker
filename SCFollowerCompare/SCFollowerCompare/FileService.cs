using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SCFollowerCompare
{
    public class FileService
    {
        IFileReadWrite fileReadWrite;

        public FileService()
        {
            fileReadWrite = Xamarin.Forms.DependencyService.Get<IFileReadWrite>();
        }

        public async Task<bool> WriteToJsonFile(ObservableCollection<Artist> artistList)
        {
            bool result = true;

            try
            {
                string serialized = JsonConvert.SerializeObject(artistList);
                await fileReadWrite.WriteToFile(serialized);
            }

            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<ObservableCollection<Artist>> ReadFromJsonFile()
        {
            ObservableCollection<Artist> deserialized = new ObservableCollection<Artist>();

            try
            {
                deserialized = JsonConvert.DeserializeObject<ObservableCollection<Artist>>(await fileReadWrite.ReadFromFile());

                if (deserialized == null)
                    deserialized = new ObservableCollection<Artist>();
            }

            catch (Exception ex)
            {
                deserialized = new ObservableCollection<Artist>();
            }

            return deserialized;
        }
    }
}
