using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SCFollowerCompare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditArtistPage : ContentPage
    {
        public EditArtistPage(ObservableCollection<Artist> mainArtistList, int mainIndex)
        {
            InitializeComponent();

            Init(mainArtistList, mainIndex);
        }

        private void Init(ObservableCollection<Artist> mainArtistList, int mainIndex)
        {
            artistList = mainArtistList;
            index = mainIndex;
            //ArtistNameField.Text = artistList[index].Name;
            ArtistUrlField.Text = artistList[index].Url;
            dataHasChanged = false;
        }

        private const string prevString = "<title>Stream ";
        private const string endString = " music | Listen to songs,";

        private ObservableCollection<Artist> artistList;
        private int index;
        private bool dataHasChanged;

        private void Enter_Data(object sender, EventArgs e)
        {
            bool isInListAlready = false;

            // Check if the URL is already in the list
            //foreach (var artist in artistList)
            //{
            //    if (artist.Url.Equals(ArtistUrlField.Text)) 
            //    {
            //        isInListAlready = true;
            //        break;
            //    }
            //}
            if (artistList[index].Url.Equals(ArtistUrlField.Text) || ArtistUrlField.Text == "")
                Navigation.PushAsync(new MainPage(artistList));

            if (!ArtistUrlField.Text.Contains("https://soundcloud.com/"))
            {
                this.ErrorMessage.Text = "This is not a valid SoundCloud URL";
                this.ErrorMessage.IsVisible = true;
                goto Jump;
            }

            // Check if the URL is already in the list
            for (int i = 0; i < artistList.Count; i++)
            {
                if (artistList[i].Url.Equals(ArtistUrlField.Text) && !artistList[i].Equals(index))
                {
                    isInListAlready = true;
                    this.ErrorMessage.Text = "This URL is being observed by you already. Enter another URL.";
                    this.ErrorMessage.IsVisible = true;
                    break;
                }
            }



            if (!isInListAlready)
            {
                //if (ArtistNameField.Text != null &&
                //    ArtistNameField.Text != "" &&
                //    ArtistNameField.Text != artistList[index].Name)
                //{
                //    artistList[index].Name = ArtistNameField.Text;
                //    dataHasChanged = true;
                //}

                if (ArtistUrlField.Text != null &&
                    ArtistUrlField.Text != "" &&
                    ArtistUrlField.Text != artistList[index].Url)
                {
                    artistList[index].Url = ArtistUrlField.Text;
                    dataHasChanged = true;
                }


                if (dataHasChanged == true)
                {
                    HtmlAccess accessHelper = new HtmlAccess(prevString, endString);
                    Artist newArtist = new Artist(accessHelper.getAndPrintHtmlData(ArtistUrlField.Text), ArtistUrlField.Text);
                    artistList[index] = newArtist;
                    //artistList.Add(newArtist);
                }

                // Clean up list by removing all empty entries
                for (int i = 0; i < artistList.Count; i++)
                {
                    if (artistList[i].Url.Equals(null) || artistList[i].Url.Equals(""))
                        artistList.Remove(artistList[i]);
                }

                serializeArtistList(artistList);

                Navigation.PushAsync(new MainPage(artistList));
            }
        Jump:


            // Dummy method to get the goto jump statement working
            serializeArtistList(artistList);
            Console.WriteLine();
        }

        private async void serializeArtistList(ObservableCollection<Artist> aList)
        {
            if (aList != null)
            {
                FileService fs = new FileService();
                await Task.Run(() => fs.WriteToJsonFile(aList));
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    if (!string.IsNullOrEmpty(propertyName))
        //    {
        //        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
    }
}