using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vikings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerDetailPage : ContentPage
    {
        public event EventHandler<Player> PlayerAdded;
        public event EventHandler<Player> PlayerUpdated;

        public SQLiteAsyncConnection _connection;

        public PlayerDetailPage(Player player)
        {

            if (player == null)
                throw new ArgumentNullException(nameof(player));

            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();

            BindingContext = new Player
            {
                ID = player.ID,
                PlayerName = player.PlayerName,
                PlayerNumber = player.PlayerNumber
            };
        }

        async void OnSave(object sender, System.EventArgs e)
        {
            var player = BindingContext as Player;

            if (String.IsNullOrWhiteSpace(player.PlayerName))
            {
                await DisplayAlert("Hey!", "Please enter a name", "Okay");
                return;
            }

            if (String.IsNullOrWhiteSpace(player.PlayerNumber))
            {
                await DisplayAlert("Hey!", "Please enter the player's number", "Okay");
                return;
            }

            if (player.ID == 0)
            {
                await _connection.InsertAsync(player);

                PlayerAdded?.Invoke(this, player);
            }
            else
            {
                await _connection.UpdateAsync(player);
                
                PlayerUpdated?.Invoke(this, player);
            }

            await Navigation.PopAsync();
        }
    }
}