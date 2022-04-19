using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SCFollowerCompare
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            loadData();
        }




        private async void loadData()
        {
            artistList = await loadArtistList(artistList);
            MainPage = new NavigationPage(new MainPage(artistList));
        }

        protected override void OnStart()
        {
        } 

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async Task<ObservableCollection<Artist>> loadArtistList(ObservableCollection<Artist> aList)
        {
            aList = new ObservableCollection<Artist>();
            FileService fs = new FileService();
            aList = await fs.ReadFromJsonFile();
            return aList;
        }

        private ObservableCollection<Artist> artistList;
    }
}
