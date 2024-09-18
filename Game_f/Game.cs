using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_f
{
    public class Game
    {
        private readonly User _user;
        private readonly IOutput _output;
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly GameSettings _settings;
        private int _targetNumber;

        public Game(User user, IOutput output, IRandomNumberGenerator randomNumberGenerator, GameSettings settings)
        {
            _user = user;
            _output = output;
            _randomNumberGenerator = randomNumberGenerator;
            _settings = settings;
           

        }

        private void InitializeGame()
        {
            _targetNumber = _randomNumberGenerator.Generate(_settings.MinNumber, _settings.MaxNumber);
        }

        private bool AskToContinue()
        {
            _output.Print("Хотите сыграть еще раз? (да/нет): ");
            string answer = Console.ReadLine().ToLower();
            return answer == "да" || answer == "yes";
        }

        public void Play()
        {
            do
            {
                InitializeGame();
                bool isGuessed = false;

                _output.Print($"Угадай число от {_settings.MinNumber} до {_settings.MaxNumber}");
                while (!isGuessed)
                {
                    int guess = _user.MakeGuess();
                    if (guess < _targetNumber)
                    {
                        _output.Print("Загаданное число больше.");

                    }
                    else if (guess > _targetNumber)
                    {
                        _output.Print("Загаданное число меньше");
                    }
                    else
                    {
                        _output.Print("Поздравляем, вы угадали!");
                        isGuessed = true;
                    }
                }
                
            }while (AskToContinue()) ;
            _output.Print("Спасибо за игру! До свидания.");
        }
    }
}
