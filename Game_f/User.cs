using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_f
{
    public class User
    {
        private readonly IUserInput _input;
        private readonly IOutput _output;

        public User(IUserInput input, IOutput output)
        {
            _input = input;
            _output = output;
        }

        public int MakeGuess()
        {
            _output.Print("Введиет ваше предположение: ");
            return _input.GetInput();
        }
    }
}
