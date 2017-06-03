using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace Vikings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTeamPage : ContentPage
    {
        private static ObservableCollection<Player> playersCollection;
        private SQLiteAsyncConnection _connection;
        private bool isDataLoaded = false;

        public static ObservableCollection<Player> PlayersCollection { get => playersCollection; set => playersCollection = value; }

        public MyTeamPage()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection(); //get the connection object(will act as a gateway to database)
        }

        protected override async void OnAppearing()
        {
            if (isDataLoaded)
                return;

            isDataLoaded = true;

            await LoadData();

            base.OnAppearing();
        }

        private async Task LoadData()
        {           
            await _connection.CreateTableAsync<Player>();

            var players = await _connection.Table<Player>().ToListAsync();

           
            PlayersCollection = new ObservableCollection<Player>(players);
            playerList.ItemsSource = PlayersCollection;
        }

        async void OnAdd(object sender, System.EventArgs e)
        {
            var page = new PlayerDetailPage(new Player());
            
            page.PlayerAdded += (source, player) =>
            {
                PlayersCollection.Add(player);
            };

            await Navigation.PushAsync(page);

        }

        async void OnPlayerSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (playerList.SelectedItem == null)//if the selected player has nothing in it there is nothing to do so return
                return;

            var selectedPlayer = e.SelectedItem as Player;

            playerList.SelectedItem = null;

            var page = new PlayerDetailPage(selectedPlayer);
            page.PlayerUpdated += (source, player) =>
            {
                selectedPlayer.ID = player.ID;
                selectedPlayer.PlayerName = player.PlayerName;
                selectedPlayer.PlayerNumber = player.PlayerNumber;
            };

            await Navigation.PushAsync(page);
        }

        async void OnDelete(object sender, System.EventArgs e)
        {
            var delPlayer = (sender as MenuItem).CommandParameter as Player;

            if (await DisplayAlert("Warning", $"Are you sure you want to delete {delPlayer.PlayerName}?", "Yes", "No"))
            {
                PlayersCollection.Remove(delPlayer);

                await _connection.DeleteAsync(delPlayer);
            }
        }

    }
}