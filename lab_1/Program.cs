using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using static LAB_1.Program;

namespace LAB_1
{



    internal class Program
    {

        static void Main(string[] args)
        {
            bool flag = true, flag_1, flag_2;
            int choice;

            var factories = GetFactories(@"D:\Metanit\Labi po C#\lab_1\factorys_data.txt");

            var units = GetUnits(@"D:\Metanit\Labi po C#\lab_1\units_data.txt");

            var tanks = GetTanks(@"D:\Metanit\Labi po C#\lab_1\tanks_data.txt");

            while (flag)
            {
                Console.WriteLine(" Меню программы:");
                Console.WriteLine("\t1 - Вывод все объекты таблицы");
                Console.WriteLine("\t2 - Вывод всех резервуаров с именами цеха и завода, где они числятся");
                Console.WriteLine("\t3 - Вывод общей суммы загрузки всех резервуаров");
                Console.WriteLine("\t4 - Найти объект в таблице по имени");
                Console.WriteLine("\t0 - Выход");

                while (true)
                {
                    Console.Write("\n\nВаш выбор > ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 0:
                                flag = false;
                                break;

                            case 1:
                                flag_1 = true;
                                Console.WriteLine("\n Выберите таблицу:");
                                Console.WriteLine("\t1 - Заводы");
                                Console.WriteLine("\t2 - Установки");
                                Console.WriteLine("\t3 - Резервуары");
                                Console.WriteLine("\t0 - Выход");

                                while (flag_1)
                                {
                                    Console.Write("\n\n Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice))
                                    {
                                        switch (choice)
                                        {
                                            case 0:
                                                flag_1 = false;
                                                break;

                                            case 1:
                                                if (factories.Count != 0)
                                                {
                                                    factories.ForEach(factory => factory.Print());
                                                    Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_1 = false;
                                                }


                                                else
                                                    Console.WriteLine("\nСписок заводов пуст!");

                                                break;

                                            case 2:
                                                if (units.Count != 0)
                                                {
                                                    units.ForEach(item => item.Print());
                                                    Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_1 = false;
                                                }


                                                else
                                                    Console.WriteLine("\nСписок установок пуст!");

                                                break;

                                            case 3:
                                                if (tanks.Count != 0)
                                                {
                                                    tanks.ForEach(tank => tank.Print());
                                                    Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_1 = false;
                                                }

                                                else
                                                    Console.WriteLine("\nСписок резервуаров пуст!");

                                                break;

                                            default:
                                                Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                    }


                                }

                                break;

                            case 2:
                                if (factories.Count != 0)
                                {
                                    if (units.Count != 0)
                                    {
                                        if (tanks.Count != 0)
                                        {
                                            Console.WriteLine("\n");
                                            PrintAllTanks(factories, units, tanks);
                                        }

                                        else
                                            Console.WriteLine("\nСписок резервуаров пуст!");
                                    }

                                    else
                                        Console.WriteLine("\nСписок установок пуст!");
                                }

                                else
                                    Console.WriteLine("\nСписок заводов пуст!");

                                Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                Console.ReadKey();
                                Console.Clear();
                                break;

                            case 3:
                                if (tanks.Count != 0)
                                    Console.WriteLine($"\n\nОбщая сумма загрузки всех резервуаров: {GetTotalVolume(tanks)}");

                                else
                                    Console.WriteLine("Список резервуаров пуст!");

                                Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                Console.ReadKey();
                                Console.Clear();
                                break;

                            case 4:
                                flag_2 = true;
                                while (flag_2)
                                {
                                    Console.WriteLine("\n[*] Выберите таблицу:");
                                    Console.WriteLine("\t[1] Заводы");
                                    Console.WriteLine("\t[2] Установки");
                                    Console.WriteLine("\t[3] Резервуары");
                                    Console.WriteLine("\t[0] Выход");

                                    Console.Write("\n\n[*] Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice))
                                    {
                                        switch (choice)
                                        {
                                            case 0:
                                                flag_2 = false;
                                                break;

                                            case 1:
                                                if (factories.Count != 0)
                                                {
                                                    Console.Write("\n[*] Введите имя завода для поиска >");

                                                    string? name = Console.ReadLine();
                                                    if (name != null)
                                                    {
                                                        var result = SearchFactoryByName(factories, name);
                                                        if (result != null)
                                                        {
                                                            result.Print();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                    }

                                                    Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_2 = false;
                                                }

                                                else
                                                    Console.WriteLine("\nСписок заводов пуст!");

                                                break;

                                            case 2:
                                                if (units.Count != 0)
                                                {
                                                    Console.Write("\n[*] Введите имя установки для поиска >");

                                                    string? name = Console.ReadLine();
                                                    if (name != null)
                                                    {
                                                        var result = SearchUnitByName(units, name);
                                                        if (result != null)
                                                        {
                                                            result.Print();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                    }

                                                    Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_2 = false;
                                                }

                                                else
                                                    Console.WriteLine("\nСписок установок пуст!");

                                                break;

                                            case 3:
                                                if (tanks.Count != 0)
                                                {
                                                    Console.Write("\n[*] Введите имя резервуара для поиска >");

                                                    string? name = Console.ReadLine();
                                                    if (name != null)
                                                    {
                                                        var result = SearchTankByName(tanks, name);
                                                        if (result != null)
                                                        {
                                                            result.Print();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }
                                                    }

                                                    else
                                                    {
                                                        Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                    }

                                                    Console.WriteLine("\nНажмите любую клавишу, для возврата в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_2 = false;
                                                }


                                                else
                                                    Console.WriteLine("\nСписок резервуаров пуст!");

                                                break;

                                            default:
                                                Console.WriteLine("\nНеверный выбор! Повторите попытку");
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        Console.WriteLine("\nеверный выбор! Повторите попытку");
                                    }
                                }

                                break;

                            default:
                                Console.WriteLine("\nНеверный выбор! Повторите попытку\n");
                                break;
                        }

                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nНеверный выбор! Повторите попытку");
                    }
                }
            }
        }

        public class Factory
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public Factory(uint id, string name, string description)
            {
                Id = id;
                Name = name;
                Description = description;
            }

            public void Print()
            {
                Console.WriteLine($" Завод #{Id}");
                Console.WriteLine($"   Название: {Name}");
                Console.WriteLine($"   Описание: {Description}\n");
            }
        }

        public class Unit
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public uint FactotyId { get; set; }

            public Unit(uint id, string name, string description, uint factotyId)
            {
                Id = id;
                Name = name;
                Description = description;
                FactotyId = factotyId;
            }

            public void Print()
            {
                Console.WriteLine($" Установка #{Id}");
                Console.WriteLine($"   Название: {Name}");
                Console.WriteLine($"   Описание: {Description}");
                Console.WriteLine($"   Завод #{FactotyId}\n");
            }
        }




        public class Tank
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double Volume { get; set; }
            public double MaxVolume { get; set; }
            public uint UnitId { get; set; }

            public Tank(uint id, string name, string description, double volume, double maxVolume, uint unitId)
            {
                Id = id;
                Name = name;
                Description = description;
                Volume = volume;
                MaxVolume = maxVolume;
                UnitId = unitId;
            }

            public void Print()
            {
                Console.WriteLine($" Резервуар #{Id}");
                Console.WriteLine($"  Название: {Name}");
                Console.WriteLine($"  Описание: {Description}");
                Console.WriteLine($"  Объём: {Volume}");
                Console.WriteLine($"  Максимальный объём: {MaxVolume}");
                Console.WriteLine($"  Установка #{UnitId}\n");
            }
        }

        public static List<Factory> GetFactories(string path, string delimiter = ";")
        {
            var factories = new List<Factory>();
            uint id;
            int lineCount = 0;

            if (File.Exists(path))
            {
                var Lines = File.ReadAllLines(path);

                foreach (var Line in Lines)
                {
                    lineCount++;
                    if (!string.IsNullOrEmpty(Line))
                    {
                        var Data = Line.Split(delimiter);

                        if (Data.Length == 3)
                        {
                            if (!uint.TryParse(Data[0], out id)) id = 0;

                            if (id != 0 && !string.IsNullOrEmpty(Data[1]) && !string.IsNullOrEmpty(Data[2]))
                            {
                                Factory Object = new Factory(id, Data[1], Data[2]);
                                factories.Add(Object);
                            }
                            else
                            {
                                Console.WriteLine($"Некорректный формат данных Строка #{lineCount}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Некорректный формат данных Строка #{lineCount}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
            }

            return factories;
        }

        public static List<Unit> GetUnits(string path, string delimiter = ";")
        {
            var units = new List<Unit>();
            uint id, factoryId;
            int lineCount = 0;

            if (File.Exists(path))
            {
                var Lines = File.ReadAllLines(path);

                foreach (var Line in Lines)
                {
                    lineCount++;
                    if (!string.IsNullOrEmpty(Line))
                    {
                        var Data = Line.Split(delimiter);

                        if (Data.Length == 4)
                        {
                            if (!uint.TryParse(Data[0], out id)) id = 0;
                            if (!uint.TryParse(Data[3], out factoryId)) factoryId = 0;

                            if (id != 0 && factoryId != 0 && !string.IsNullOrEmpty(Data[1]) && !string.IsNullOrEmpty(Data[2]))
                            {
                                Unit Object = new Unit(id, Data[1], Data[2], factoryId);
                                units.Add(Object);
                            }
                            else
                            {
                                Console.WriteLine($"Некорректный формат данных Строка #{lineCount}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Некорректный формат данных Строка #{lineCount}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
            }

            return units;
        }

        public static List<Tank> GetTanks(string path, string delimiter = ";")
        {
            var tanks = new List<Tank>();
            uint id, volume, maxVolume, unitId;
            int lineCount = 0;

            if (File.Exists(path))
            {
                var Lines = File.ReadAllLines(path);

                foreach (var Line in Lines)
                {
                    lineCount++;
                    if (!string.IsNullOrEmpty(Line))
                    {
                        var Data = Line.Split(delimiter);

                        if (Data.Length == 6)
                        {
                            if (!uint.TryParse(Data[0], out id)) id = 0;
                            if (!uint.TryParse(Data[3], out volume)) volume = 0;
                            if (!uint.TryParse(Data[4], out maxVolume)) maxVolume = 0;
                            if (!uint.TryParse(Data[5], out unitId)) unitId = 0;

                            if (id != 0 && unitId != 0 && !string.IsNullOrEmpty(Data[1]) && !string.IsNullOrEmpty(Data[2]))
                            {
                                if (volume >= 0 || maxVolume >= 0)
                                {
                                    if (volume <= maxVolume)
                                    {
                                        Tank Object = new Tank(id, Data[1], Data[2], volume, maxVolume, unitId);
                                        tanks.Add(Object);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Объём резервуара не может быть больше максимального объёма Строка #{lineCount}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Объём резервуара не может быть отрицательным Строка #{lineCount}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Некорректный формат данных Строка #{lineCount}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Некорректный формат данных Строка #{lineCount}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
            }

            return tanks;
        }

        public static Factory? SearchFactoryByName(List<Factory> factories, string name)
        {
            foreach (var item in factories)
            {
                if (item.Name == name)
                    return item;
            }

            return null;
        }

        public static Unit? SearchUnitByName(List<Unit> units, string name)
        {
            foreach (var item in units)
            {
                if (item.Name == name)
                    return item;
            }

            return null;
        }

        public static Tank? SearchTankByName(List<Tank> tanks, string name)
        {
            foreach (var item in tanks)
            {
                if (item.Name == name)
                    return item;
            }

            return null;
        }

        public static Unit? FindUnit(List<Unit> units, List<Tank> tanks, string tankName)
        {
            foreach (var tank in tanks)
            {
                if (tank.Name == tankName)
                {
                    foreach (var unit in units)
                    {
                        if (tank.UnitId == unit.Id)
                            return unit;
                    }
                }
            }

            return null;
        }

        public static Factory? FindFactory(List<Factory> factories, Unit unit)
        {
            foreach (var factory in factories)
            {
                if (unit.Id == factory.Id)
                    return factory;
            }

            return null;
        }

        public static double GetTotalVolume(List<Tank> tanks)
        {
            return tanks.Sum(item => item.Volume);
        }

        public static void PrintAllTanks(List<Factory> factories, List<Unit> units, List<Tank> tanks)
        {
            foreach (var tank in tanks)
            {
                foreach (var item in units)
                {
                    if (tank.UnitId == item.Id)
                    {
                        foreach (var factory in factories)
                        {
                            if (item.FactotyId == factory.Id)
                            {
                                Console.WriteLine($"Резервуар #{tank.Id}");
                                Console.WriteLine($" Название: {tank.Name}");
                                Console.WriteLine($" Описание: {tank.Description}");
                                Console.WriteLine($" Объём: {tank.Volume}");
                                Console.WriteLine($" Максимальный объём: {tank.MaxVolume}");
                                Console.WriteLine($" Название завода: {factory.Description}");
                                Console.WriteLine($" Имя цеха: {factory.Name}\n");
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        
    }
}