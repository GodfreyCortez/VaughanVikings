using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vikings 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerAssignPage : ContentPage
    {
        private SQLiteAsyncConnection connection;
        private ObservableCollection<PlayerPosition> Players;
        public PlayerAssignPage(PlayerPosition player, ObservableCollection<PlayerPosition> players)
        {
            
            InitializeComponent();

            BindingContext = player;

            Players = players;

            connection = DependencyService.Get<ISQLiteDb>().GetConnection();
               
        }

        protected override void OnAppearing() 
        {
            switchPitch.OnChanged += OnChangedSwitch;
            switchCatcher.OnChanged += OnChangedSwitch;
            switchSS.OnChanged += OnChangedSwitch;
            switchBase1.OnChanged += OnChangedSwitch;
            switchBase2.OnChanged += OnChangedSwitch;
            switchBase3.OnChanged += OnChangedSwitch;
            switchLF.OnChanged += OnChangedSwitch;
            switchCF.OnChanged += OnChangedSwitch;
            switchRF.OnChanged += OnChangedSwitch;
            switchOff.OnChanged += OnChangedSwitch;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            switchPitch.OnChanged -= OnChangedSwitch;
            switchCatcher.OnChanged -= OnChangedSwitch;
            switchSS.OnChanged -= OnChangedSwitch;
            switchBase1.OnChanged -= OnChangedSwitch;
            switchBase2.OnChanged -= OnChangedSwitch;
            switchBase3.OnChanged -= OnChangedSwitch;
            switchLF.OnChanged -= OnChangedSwitch;
            switchCF.OnChanged -= OnChangedSwitch;
            switchRF.OnChanged -= OnChangedSwitch;
            switchOff.OnChanged -= OnChangedSwitch;
            base.OnDisappearing();
        }

        //This method will check if any other toggles and prevent the user from ticking two toggles at once
        private bool CheckToggles(SwitchCell switcher) 
        {
            var player = BindingContext as PlayerPosition;

            //Here we will compare if the switch is equal to one of the other switches and if not
            //check if it is currently on. If both are true, it means that there are two switches
            //toggled on so we must return true
            if (!switcher.Equals(switchPitch) && player.Pitcher)
                return true;

            else if (!switcher.Equals(switchCatcher) && player.Catcher)
                return true;

            else if (!switcher.Equals(switchSS) && player.SS)
                return true;

            else if (!switcher.Equals(switchBase1) && player.Base1)
                return true;

            else if (!switcher.Equals(switchBase2) && player.Base2)
                return true;

            else if (!switcher.Equals(switchBase3) && player.Base3)
                return true;

            else if (!switcher.Equals(switchLF) && player.LF)
                return true;

            else if (!switcher.Equals(switchCF) && player.CF)
                return true;

            else if (!switcher.Equals(switchRF) && player.RF)
                return true;

            else if (!switcher.Equals(switchOff) && player.Off)
                return true;
            
            return false;
        }

        private bool CheckPlayers(int PlayerIndex) //method to compare the to the other players
        {
            var player = BindingContext as PlayerPosition;

            //compare if the toggles are on and if so, check if that toggle in the other player is already on
            if (player.Pitcher == Players[PlayerIndex].Pitcher && player.Pitcher)
                return true;

            else if (player.Catcher == Players[PlayerIndex].Catcher && player.Catcher)
                return true;

            else if (player.SS == Players[PlayerIndex].SS && player.SS)
                return true;

            else if (player.Base1 == Players[PlayerIndex].Base1 && player.Base1)
                return true;

            else if (player.Base2 == Players[PlayerIndex].Base2 && player.Base2)
                return true;

            else if (player.Base3 == Players[PlayerIndex].Base3 && player.Base3)
                return true;

            else if (player.LF == Players[PlayerIndex].LF && player.LF)
                return true;

            else if (player.CF == Players[PlayerIndex].CF && player.CF)
                return true;

            else if (player.RF == Players[PlayerIndex].RF && player.RF)
                return true;

            return false;
        }

        async void OnChangedSwitch (object sender, ToggledEventArgs e)
        {
            var player = BindingContext as PlayerPosition;
            var switchObject = sender as SwitchCell;
            int positionCounter = 0;
            int PlayersCounter = Players.Count;
            //before doing anything check if other toggles are already on
            if (CheckToggles(switchObject))
            {
                await DisplayAlert("Warning", "Cannot assign one player to two positions!", "Okay");
                switchObject.OnChanged -= OnChangedSwitch;
                switchObject.On = false;                    
                switchObject.OnChanged += OnChangedSwitch;
                
                return; //turn the object that ws switched on off and return the method
            }

            for(int i = 0; i < PlayersCounter; i++)//next check if any other players were already assigned the respective position
            {
                if (player.PlayerName == Players[i].PlayerName) //skip over the same player
                    continue;

                if(CheckPlayers(i))
                {
                    await DisplayAlert("Warning", String.Format("{0} is already playing this position!", Players[i].PlayerName), "Okay");

                    switchObject.OnChanged -= OnChangedSwitch;
                    switchObject.On = false;
                    switchObject.OnChanged += OnChangedSwitch;

                    return; //turn the object that ws switched on off and return the method
                }
            }

            //Find out which object or switch was pressed and then assign the respective counter
            if (switchObject.Equals(switchPitch))
                positionCounter = player.PositionPitcher;
            
            else if(switchObject.Equals(switchCatcher))
                positionCounter = player.PositionCatcher;

            else if(switchObject.Equals(switchSS))            
                positionCounter = player.PositionSS;
            
            else if(switchObject.Equals(switchBase1))   
                positionCounter = player.PositionBase1;

            else if (switchObject.Equals(switchBase2))
                positionCounter = player.PositionBase2;

            else if (switchObject.Equals(switchBase3))
                positionCounter = player.PositionBase3;

            else if (switchObject.Equals(switchLF))
                positionCounter = player.PositionLF;

            else if (switchObject.Equals(switchCF))
                positionCounter = player.PositionCF;

            else if (switchObject.Equals(switchRF))
                positionCounter = player.PositionRF;

            if (!e.Value)
            {
                player.Position = null;

                if (switchObject.Equals(switchPitch))
                    player.PositionPitcher = (positionCounter - 1);

                else if (switchObject.Equals(switchCatcher))
                    player.PositionCatcher = (positionCounter - 1);

                else if (switchObject.Equals(switchSS))
                    player.PositionSS = (positionCounter - 1);

                else if (switchObject.Equals(switchBase1))
                    player.PositionBase1 = (positionCounter - 1);

                else if (switchObject.Equals(switchBase2))
                    player.PositionBase2 = (positionCounter - 1);

                else if (switchObject.Equals(switchBase3))
                    player.PositionBase3 = (positionCounter - 1);

                else if (switchObject.Equals(switchLF))
                    player.PositionLF = (positionCounter - 1);

                else if (switchObject.Equals(switchCF))
                    player.PositionCF = (positionCounter - 1);

                else if (switchObject.Equals(switchRF))
                    player.PositionRF = (positionCounter - 1);

                return;
            }

            if (positionCounter >= 2) //if the position counter is at 2 the player has already played this position twice
            {
                await DisplayAlert("Warning", String.Format("{0} has played this position twice!", player.PlayerName), "Okay");

                switchObject.OnChanged -= OnChangedSwitch;
                switchObject.On = false;
                switchObject.OnChanged += OnChangedSwitch;
                return;
            }

            if(e.Value) //logic for adding the positions
            {
                //assign the proper positions according to the toggle
                if (await DisplayAlert("Confirm", " Are you sure you want to assign this position?", "Yes", "No"))
                {
                    if (switchObject.Equals(switchPitch)) 
                    {
                        player.Position = "Pitcher";
                        player.PositionPitcher = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchCatcher))
                    {
                        player.Position = "Catcher";
                        player.PositionCatcher = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchSS))
                    {
                        player.Position = "Short Stop";
                        player.PositionSS = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchBase1))
                    {
                        player.Position = "1st Base";
                        player.PositionBase1 = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchBase2))
                    {
                        player.Position = "2nd Base";
                        player.PositionBase2 = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchBase3))
                    {
                        player.Position = "3rd Base";
                        player.PositionBase3 = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchLF))
                    {
                        player.Position = "Left Field";
                        player.PositionLF = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchCF))
                    {
                        player.Position = "Center Field";
                        player.PositionCF = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchRF))
                    {
                        player.Position = "Right Field";
                        player.PositionRF = (positionCounter + 1);
                    }
                    else if (switchObject.Equals(switchOff))
                    {
                        player.Position = "Off";
                    }
                    await Navigation.PopAsync();
                    return;
                }
                else
                {
                    switchObject.OnChanged -= OnChangedSwitch;
                    switchObject.On = false;
                    switchObject.OnChanged += OnChangedSwitch;
                }

                return; //end method
            }

        }
     
    }
}