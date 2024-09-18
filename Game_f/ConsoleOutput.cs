using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_f
{
    public class ConsoleOutput : IOutput
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
