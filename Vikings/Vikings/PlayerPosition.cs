using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vikings
{
    public class PlayerPosition : Player, INotifyPropertyChanged
    {
        private int inning = 1;
        public int Inning
        {
            get
            {
                return inning;
            }
            set
            {
                inning = value;
                OnPropertyChanged();
            }
        }


        public override PlayerPosition CopyFrom()
        {
            return this as PlayerPosition;
        }

        public override PlayerPosition ClonePosition()
        {
            var clone = new PlayerPosition();
            clone.PlayerNumber = PlayerNumber;
            clone.PlayerName = PlayerName;
            clone.Position = Position;
            clone.Inning = Inning;
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
        public string InningName
        {
           get{ return String.Format("Inning: {0}", (Inning)); }
        }

    }
}
