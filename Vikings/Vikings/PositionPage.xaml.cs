using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vikings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionPage : ContentPage, INotifyPropertyChanged
    {
        private SQLiteAsyncConnection connection;
        private ObservableCollection<Player> copyFrom;
        private ObservableCollection<PlayerPosition> positionsCollection;
        private ObservableCollection<PlayerPosition> playersToDisplay;

        int index = 0;
        int id = 1;
        public int ID { get { return id; } set { id = value; OnPropertyChanged(); } }
        private bool isDataLoaded = false;
        private bool fromPrevious = false;
        private bool endCopying = false; 

        public PositionPage()
        {
            InitializeComponent();

            connection = DependencyService.Get<ISQLiteDb>().GetConnection();

        }
        protected override void OnDisappearing()
        {
            Title.Remove(0);
            base.OnDisappearing();
        }
        protected async override void OnAppearing() //when we initialize we must create a sort of filter
        {
            Title = $"Inning {ID}";
            if (isDataLoaded)
                return;

            isDataLoaded = true;

            await LoadData();

            base.OnAppearing();
        }



        async Task LoadData()
        {
            await connection.CreateTableAsync<PlayerPosition>();
            var copier = await connection.Table<Player>().ToListAsync();
            copyFrom = new ObservableCollection<Player>(copier);
            var temp = await connection.Table<PlayerPosition>().ToListAsync();

            positionsCollection = new ObservableCollection<PlayerPosition>(temp);

            positionsCollection.Clear();
            foreach (var player in copyFrom) //clone the players from the my team collection
            {
                var newPlayer = player.ClonePosition();
                positionsCollection.Add(newPlayer);

            }


            playersToDisplay = new ObservableCollection<PlayerPosition>();

            foreach (var player in positionsCollection)//now find the instance of the players we want to display
            {
                var newplayer = player.CopyFrom();
                if (newplayer.Inning == ID)
                    playersToDisplay.Add(newplayer);
            }
            
            positionList.ItemsSource = playersToDisplay;
        }

        async void OnNext(object sender, EventArgs e)
        {
            
            if (await DisplayAlert("Confirm", "Are the positions for this inning okay?", "Yes", "No"))
            {
                if (!fromPrevious)
                {
                    int counter = positionsCollection.Count;
                    while (index < counter)
                    {

                        var playerClone = positionsCollection.ElementAt(index).ClonePosition();
                        if (playerClone.Inning == ID && !endCopying)
                        {
                            playerClone.Position = null;
                            playerClone.Inning = (ID + 1);

                            positionsCollection.Add(playerClone);

                        }
                        index++;
                    }
                    index = counter;
                }
                ID++;//increment to the next inning
                Title = $"Inning {ID}";

                playersToDisplay.Clear();//clear the current list displaying to screen
                foreach (var player in positionsCollection)//now find the instance of the players we want to display
                {
                    var newPlayer = player.CopyFrom();

                    if (newPlayer.Inning == ID)
                        playersToDisplay.Add(newPlayer);
                }
                
            }
            else
                return;

            fromPrevious = false;
			if (ID == 5)
			{
				endCopying = true;
				next.Text = "Save";
				next.Clicked -= OnNext;//change event handlers and the button
				next.Clicked += OnSave;

			}
        }

        async void OnPrev(object sender, EventArgs e)
        {
            
            if (ID == 1)//if we're on the previous innng do nothing  
                return;


            if (await DisplayAlert("Confirm", "Do you want to go back to the previous inning?", "Yes", "No"))
            {
                fromPrevious = true;
                ID--; //decrement to the previous inning
                Title = $"Inning {ID}";
                playersToDisplay.Clear();
                foreach (var player in positionsCollection)//now find the instance of the players we want to display
                {
                    var newPlayer = player.CopyFrom();

                    if (newPlayer.Inning == ID)
                        playersToDisplay.Add(newPlayer);
                }

                if (ID == 4)
                {
                    next.Text = "Next";
                    next.Clicked -= OnSave;
                    next.Clicked += OnNext;
                }

            }
            else
                return;
        }

        async void OnSave(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirm", "Are you sure you want to save? (Positions can no longer be changed)", "Yes", "No"))
            {
                
                ID = 1;
                var today = DateTime.Today.ToString("D") +" "+ DateTime.Now.ToString("h:mm tt");
                var NewGame = new Game();
                NewGame.Date = today;
                RecapPage.AddGame(NewGame);

                foreach (var player in positionsCollection)
                {
                    var newPlayer = player.CopyFrom();
                    newPlayer.Date = today;
                }
                
                next.Text = "Next";
                Title = $"Inning {ID}";
                await connection.InsertAllAsync(positionsCollection);
                await DisplayAlert("Saved!", "Check out previous games in the recaps page", "Okay");
                await LoadData();
            }
            else
                return;
        }

        async void OnPositionSelected (object sender, SelectedItemChangedEventArgs e)
        {
            if (positionList.SelectedItem == null)
                return;

            var selectedPlayer = e.SelectedItem as PlayerPosition;

            positionList.SelectedItem = null;

            var page = new PlayerAssignPage(selectedPlayer, playersToDisplay);
            
            await Navigation.PushAsync(page);
        }
    }
}