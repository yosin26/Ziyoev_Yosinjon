using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Game_f
{
    public class GameSettings
    {
        public int MinNumber{ get; set; }
        public int MaxNumber{ get; set; }

        public GameSettings(int minNumber, int maxNumber)
        { 
            MinNumber = minNumber;
            MaxNumber = maxNumber; 
        }
    }
}
