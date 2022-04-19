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
            //artistList = new ObservableCollection<Artist>();
            //MainPage = new NavigationPage(new MainPage(artistList));
            loadData();

            //artistList = new ObservableCollection<Artist>();
            //MainPage = new NavigationPage(new MainPage(artistList));
        }

        // [{"Name":"Impulsive Behavior","Url":"https://soundcloud.com/impulsive_behavior"}]



        private async void loadData()
        {
            //artistList = new ObservableCollection<Artist>();
            artistList = await loadArtistList(artistList);
            MainPage = new NavigationPage(new MainPage(artistList));
        }

        protected async override void OnStart()
        {
            ////if (File.Exists("loadData.xml"))
            ////{
            ////    string fileName = "loadData.xml";
            ////    XmlManager.XmlDataReader(fileName);
            ////}
            ////FileService fs = new FileService();
            ////await Task.Run(() => fs.WriteToJsonFile(artistList));
            //artistList = new ObservableCollection<Artist>();
            //FileService fs = new FileService();
            //artistList = await fs.ReadFromJsonFile();
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

        //private async Task loadArtistList(ObservableCollection<Artist> aList)
        //{
        //    FileService fs = new FileService();
        //    artistList = await fs.ReadFromJsonFile();
        //}

        //private int index;
        //private string artistName;
        //private string artistUrl;
        private ObservableCollection<Artist> artistList;
    }
}
