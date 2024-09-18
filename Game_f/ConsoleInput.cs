using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_f
{
    public class ConsoleInput : IUserInput
    {
        public int GetInput()
        {
            return int.Parse(Console.ReadLine());
        }
    }
}
