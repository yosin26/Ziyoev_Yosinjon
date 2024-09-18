using Game_f;
using Microsoft.Extensions.DependencyInjection;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Настраиваем контейнер зависимостей
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IUserInput, ConsoleInput>()
            .AddSingleton<IOutput, ConsoleOutput>()
            .AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>()
            .AddSingleton<User>()  // Пользователь
            .AddTransient<Game>()  // Игра создается как новый экземпляр каждый раз
            .BuildServiceProvider();

        // Получаем экземпляры для ввода и вывода
        // Проверка загрузки
        var input = serviceProvider.GetService<IUserInput>();
        var output = serviceProvider.GetService<IOutput>();

        // Запрашиваем у пользователя минимальное и максимальное число диапазона
        output.Print("Введите минимальное число диапазона: ");
        int minNumber = input.GetInput();

        output.Print("Введите максимальное число диапазона: ");
        int maxNumber = input.GetInput();

        // Настраиваем новые параметры игры
        var gameSettings = new GameSettings(minNumber, maxNumber);

        // Создаем экземпляр игры с новыми настройками
        var game = new Game(
            serviceProvider.GetService<User>(),
            output,
            serviceProvider.GetService<IRandomNumberGenerator>(),
            gameSettings
        );

        // Запускаем игру
        game.Play();
    }
}
