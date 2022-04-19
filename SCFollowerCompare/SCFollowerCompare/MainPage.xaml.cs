using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

// TODO: OBSERVABLECOLLECTION DURCH LISTE REPLACEN
// TODO: INVALID HTTP ADRESSE ERRORMESSAGE


namespace SCFollowerCompare
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ObservableCollection<Artist> _artistList)
        {
            Init(_artistList);
            InitializeComponent();
        }


        private void Init(ObservableCollection<Artist> _artistList)
        {
            if (_artistList != null && _artistList.Count > 0)
            {
                artistList = new ObservableCollection<Artist>();
                artistList = _artistList;
                ProcessWholeList();
            }
            else
            {
                artistList = new ObservableCollection<Artist>();
            }

            CleanUpArtistList(artistList);
        }

        private const string prevString = "followers_count\":";
        private const string endString = ",\"followings_count\"";

        private ObservableCollection<Artist> artistList;
        public ObservableCollection<Artist> ArtistList
        {
            get => artistList;
            set
            {
                artistList = value;
                //this.RaisePropertyChanged();
            }
        }

        private void ProcessWholeList()
        {
            HtmlAccess accessHelper = new HtmlAccess(prevString, endString);

            foreach (var artist in artistList)
            {
                if (artist.Url != "" && artist.Url != null && artist.Url.Contains("https://soundcloud.com/"))
                {
                    artist.Followers = Convert.ToInt32(accessHelper.getAndPrintHtmlData(artist.Url));
                }
            }
        }


        private ObservableCollection<Artist> CleanUpArtistList(ObservableCollection<Artist> artistList)
        {
            // Clean up list by removing all empty entries
            for (int i = 0; i < artistList.Count; i++)
            {
                if (artistList[i].Url.Equals(null) || artistList[i].Url.Equals(""))
                    artistList.Remove(artistList[i]);
            }

            return artistList;
        }


        private void Add_Artist(object sender, EventArgs e)
        {
            CleanUpArtistList(artistList);
            artistList.Add(new Artist("", ""));
            Navigation.PushAsync(new EditArtistPage(artistList, artistList.Count - 1));
        }


        private void Refresh_List(object sender, EventArgs e)
        {
            ProcessWholeList();
        }

        private void Edit_ArtistInfo(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Artist art = new Artist("", "");
            int index = -1;


            foreach (var artist in ArtistList)
            {
                if (artist.Name.Equals(btn.CommandParameter.ToString()))
                {
                    art.Name = artist.Name;
                    art.Url = artist.Url;
                    index = artistList.IndexOf(artist);
                }
            }

            Navigation.PushAsync(new EditArtistPage(artistList, index));
        }



        private void Delete_Artist(object sender, EventArgs e)
        {
            var btn = (Button)sender;

            //Removes list entry where the name matches the string of the command parameter of the clicked button (which is the name)
            for (int i = ArtistList.Count - 1; i >= 0; i--)
            {
                if (ArtistList[i].Name.Equals(btn.CommandParameter.ToString()))
                {
                    ArtistList.Remove(ArtistList[i]);
                }
            }

            serializeArtistList(artistList);
        }


        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    if (!string.IsNullOrEmpty(propertyName))
        //    {
        //        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}


        private async void CPage_Disappearing(object sender, EventArgs e)
        {
            //if (artistList != null)
            //{
            //    string fileName = @"C:/Users/kotec/Desktop/TestOrdner/loadData.xml";
            //    XmlManager.XmlDataWriter(artistList, fileName);
            //}
            //if (artistList != null)
            //{
            //    FileService fs = new FileService();
            //    await Task.Run(() => fs.WriteToJsonFile(artistList));
            //}
            serializeArtistList(artistList);
        }

        private async void serializeArtistList(ObservableCollection<Artist> aList)
        {
            if (aList != null)
            {
                FileService fs = new FileService();
                await Task.Run(() => fs.WriteToJsonFile(aList));
            }
        }
    }
}
