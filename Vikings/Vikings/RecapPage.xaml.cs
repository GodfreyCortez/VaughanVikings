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
    public partial class RecapPage : ContentPage
    {
        private bool isDataLoaded;
        
        private static ObservableCollection<Game> Games; 
        private static SQLiteAsyncConnection connection;


        public RecapPage()
        {
            InitializeComponent();

            connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected async override void OnAppearing() //when we initialize we must create a sort of filter
        {
            if (isDataLoaded)
                return;

            isDataLoaded = true;

            await LoadData();

            base.OnAppearing();
        }

        private async Task LoadData()
        {
            await connection.CreateTableAsync<Game>();

            if (Games == null) //check if any of our collections are null
                Games = new ObservableCollection<Game>(await connection.Table<Game>().ToListAsync());

            GamesList.ItemsSource = Games;
        }

        public static async void AddGame(Game today)
        {
            connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            await connection.CreateTableAsync<Game>();

            await connection.InsertAsync(today);
        }

        async void OnDelete(object sender, System.EventArgs e)
        {
            var delGame = (sender as MenuItem).CommandParameter as Game;

            if (await DisplayAlert("Warning", $"Are you sure you want to delete {delGame.Date}?", "Yes", "No"))
            {
                Games.Remove(delGame);

                await connection.DeleteAsync(delGame);
            }
        }

        private async void GamesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;


            var game = e.SelectedItem as Game;
            var page = new RecapDetailsPage(game);

            GamesList.SelectedItem = null; //deselect the item by setting the property to null

            page.Title = (e.SelectedItem as Game).Date; //set the title
            await Navigation.PushAsync(page);
        }
    }
}