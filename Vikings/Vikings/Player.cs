using SQLite;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Vikings
{
    
    //This class is to serve as the class to define each individual player
    public class Player : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }//ID in database

        private string _playerName;
        [MaxLength(255)]
        public string PlayerName//playerName property
        {
            get { return _playerName; }
            set
            {
                if (_playerName == value)
                    return;

                _playerName = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(PositionFullName));
            }
        }

        private string date;
        [MaxLength(255)]
        public string Date
        {
            get { return date; }
            set
            {
                if (date == value)
                    return;

                date = value;
                OnPropertyChanged();
            }
        }

        private string playerNumber;
        [MaxLength(255)]      
        public string PlayerNumber
        {
            get { return playerNumber; }
            set
            {
                if (playerNumber == value)
                    return;

                playerNumber = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(PositionFullName));
            }
        }

        private string position;
        [MaxLength(255)]
        public string Position
        {
            get { return position; }
            set
            {
                if (position == value)
                    return;

                position = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(PositionFullName));
            }
        }
        public string PositionFullName
        {
            get { return $"{PlayerName} #{PlayerNumber} {Position}"; }
        }

        public string FullName //we will displayer the players name + their number here
        {
            get { return $"{PlayerName} {PlayerNumber}"; }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private int positionPitcher;
        public int PositionPitcher
        {
            get
            {
                return positionPitcher;
            }
            set
            {
                positionPitcher = value;
                OnPropertyChanged();
            }
        }

        private int positionCatcher;
        public int PositionCatcher
        {
            get
            {
                return positionCatcher;
            }

            set
            {
                positionCatcher = value;
                OnPropertyChanged();
            }
        }
        private int positionSS;
        public int PositionSS
        {
            get
            {
                return positionSS;
            }
            set
            {
                positionSS = value;
                OnPropertyChanged();
            }
        }

        private int positionBase1;
        public int PositionBase1
        {
            get
            {
                return positionBase1;
            }
            set
            {
                positionBase1 = value;
                OnPropertyChanged();
            }
        }

        private int positionBase2;
        public int PositionBase2
        {
            get
            {
                return positionBase2;
            }
            set
            {
                positionBase2 = value;
                OnPropertyChanged();
            }
        }

        private int positionBase3;
        public int PositionBase3
        {
            get
            {
                return positionBase3;
            }
            set
            {
                positionBase3 = value;
                OnPropertyChanged();
            }
        }

        private int positionLF;
        public int PositionLF
        {
            get
            {
                return positionLF;
            }
            set
            {
                positionLF = value;
                OnPropertyChanged();
            }
        }

        private int positionCF;
        public int PositionCF
        {
            get
            {
                return positionCF;
            }
            set
            {
                positionCF = value;
                OnPropertyChanged();
            }
        }

        private int positionRF;
        public int PositionRF
        {
            get
            {
                return positionRF;
            }
            set
            {
                positionRF = value;
                OnPropertyChanged();
            }
        }

        private bool pitcher;
        public bool Pitcher
        {
            get
            {
                return pitcher;
            }
            set
            {
                pitcher = value;
                OnPropertyChanged();
            }
        }

        private bool catcher;
        public bool Catcher
        {
            get
            {
                return catcher;
            }
            set
            {
                catcher = value;
                OnPropertyChanged();
            }
        }

        private bool sS;
        public bool SS
        {
            get
            {
                return sS;
            }
            set
            {
                sS = value;
                OnPropertyChanged();
            }
        }

        private bool base1;
        public bool Base1
        {
            get
            {
                return base1;
            }
            set
            {
                base1 = value;
                OnPropertyChanged();
            }
        }

        private bool base2;
        public bool Base2
        {
            get
            {
                return base2;
            }
            set
            {
                base2 = value;
                OnPropertyChanged();
            }
        }

        private bool base3;
        public bool Base3
        {
            get
            {
                return base3;
            }
            set
            {
                base3 = value;
                OnPropertyChanged();
            }
        }

        private bool lF;
        public bool LF
        {
            get
            {
                return lF;
            }
            set
            {
                lF = value;
                OnPropertyChanged();
            }
        }

        private bool cF;
        public bool CF
        {
            get
            {
                return cF;
            }
            set
            {
                cF = value;
                OnPropertyChanged();
            }
        }

        private bool rF;
        public bool RF
        {
            get
            {
                return rF;
            }
            set
            {
                rF = value;
                OnPropertyChanged();
            }
        }

        private bool off;
        public bool Off
        {
            get
            {
                return off;
            }
            set
            {
                off = value;
                OnPropertyChanged();
            }
        }

        public Player Clone()
        {
            var clone = JsonConvert.DeserializeObject<Player>(JsonConvert.SerializeObject(this));

            return clone;
        }

        public virtual PlayerPosition CopyFrom()//returns current object casted to Player
        {
            return this as PlayerPosition;
        }

        public virtual PlayerPosition ClonePosition()
        {
            var clone = new PlayerPosition();
            clone.PlayerNumber = PlayerNumber;
            clone.PlayerName = PlayerName;
            clone.Position = Position;
            clone.PositionPitcher = PositionPitcher;
            clone.PositionCatcher = PositionCatcher;
            clone.PositionSS = PositionSS;
            clone.PositionBase1 = PositionBase1;
            clone.PositionBase2 = PositionBase2;
            clone.PositionBase3 = PositionBase3;
            clone.PositionLF = PositionLF;
            clone.PositionCF = PositionCF;
            clone.PositionRF = PositionRF;
            
            return clone;
        }

        public void ClearSets()//method to clear the boolean values of all position properties
        {
            Pitcher = false;
            Catcher = false;
            SS = false;
            Base1 = false;
            Base2 = false;
            Base3 = false;
            LF = false;
            CF = false;
            RF = false;
        }

        public void ClearPositions() //method to clear the counters of each of the players
        {
            PositionPitcher = 0;
            PositionCatcher = 0;
            PositionSS = 0;
            PositionBase1 = 0;
            PositionBase2 = 0;
            PositionBase3 = 0;
            PositionLF = 0;
            PositionCF = 0;
            PositionRF = 0;

            ClearSets();
        }
    }
}
