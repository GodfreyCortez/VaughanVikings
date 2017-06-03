using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vikings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecapDetailsPage : ContentPage
    {
        private Game Today;
        private SQLiteAsyncConnection connection;
        private ObservableCollection<PlayerPosition> AllPlayers;
        private ObservableCollection<PlayerPosition> PlayersToDisplay;

        public RecapDetailsPage(Game SelectedGame)
        {
            InitializeComponent();

            connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            Today = SelectedGame;
        }

        protected async override void OnAppearing()
        {
            await connection.CreateTableAsync<PlayerPosition>();

            AllPlayers = new ObservableCollection<PlayerPosition>(await connection.Table<PlayerPosition>().ToListAsync());
            PlayersToDisplay = new ObservableCollection<PlayerPosition>();
            foreach(var player in AllPlayers)
            {
                if (player.Date == Today.Date)
                    PlayersToDisplay.Add(player);
            }

            PlayerList.ItemsSource = PlayersToDisplay;

            base.OnAppearing();
        }
    }
}