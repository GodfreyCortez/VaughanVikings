using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vikings
{
    public class Game //This is the class which will help to filter out the players when displaying specific games
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        private string date;
        [MaxLength(255)]
        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                if (value == null)
                    return;

                date = value;
            }
        }

    }
}
