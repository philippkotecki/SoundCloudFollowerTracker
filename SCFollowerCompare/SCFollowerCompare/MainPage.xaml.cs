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
    //[System.Xml.Serialization.XmlInclude(typeof(Artist))]
    //[System.Xml.Serialization.XmlInclude(typeof(ObservableCollection<Artist>))]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
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
                this.RaisePropertyChanged();
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
                //artist.Followers = Convert.ToInt32(getAndPrintHtmlData(artist.Name, artist.Url));

            }
        }


        //private string getAndPrintHtmlData(string artist, string url)
        //{
        //    HttpClient client = new HttpClient();
        //    var responseBody = client.GetStringAsync(url).Result;

        //    var htmlDocument = new HtmlDocument();
        //    htmlDocument.LoadHtml(responseBody);

        //    return findFollowersCount(responseBody);
        //}

        //private string findFollowersCount(string responseBody)
        //{
        //    // Find the area in the html document that holds the follower count of an artist
            
        //    string followerString = "followers_count\":";                                // string taken from HTML code
        //    var index = responseBody.IndexOf(followerString) + followerString.Count();   // skip that string

        //    var responseBodyCharArr = responseBody.ToCharArray();
        //    List<char> followerCount = new List<char>();

        //    // Add all chars (numbers) to an char array. Stop at "," where the follower count ends

        //    for (int i = index; !responseBodyCharArr[i].Equals(','); i++)
        //    {
        //        followerCount.Add(responseBodyCharArr[i]);
        //    }

        //    string result = new string(followerCount.ToArray());
        //    return result;
        //}

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
            //string artistName = "";


            foreach (var artist in ArtistList)
            {
                if (artist.Name.Equals(btn.CommandParameter.ToString()))
                {
                    art.Name = artist.Name;
                    art.Url = artist.Url;
                    index = artistList.IndexOf(artist);
                }
                //if (artist.Name.Equals(btn.CommandParameter.ToString()))
                //{
                //    artistName = artist.Name;
                //    index = ArtistList.IndexOf(artist);
                //}
            }

            //if (art.Name != null && art.Name != "")
            //    Application.Current.Properties["ArtistName"] = art.Name;

            //if (art.Url != null && art.Url != "")
            //    Application.Current.Properties["ArtistUrl"] = art.Url;


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

            //this.CPage = new MainPage(artistList);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (artistList != null)
        //    {
        //        FileService fs = new FileService();
        //        //await Task.Run(() => fs.WriteToJsonFile(artistList));
        //        await fs.WriteToJsonFile(artistList);
        //    }
        //}

        //private async void CPage_Appearing(object sender, EventArgs e)
        //{
        //    ////if (File.Exists("loadData.xml"))
        //    ////{
        //    ////    string fileName = "loadData.xml";
        //    ////    XmlManager.XmlDataReader(fileName);
        //    ////}
        //    FileService fs = new FileService();
        //    //await Task.Run(() => fs.ReadFromJsonFile());
        //    artistList = await fs.ReadFromJsonFile();
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
