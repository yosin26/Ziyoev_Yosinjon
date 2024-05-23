using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace LAB_1
{
    internal class Program
    {
        public static string BASE_DIR = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        #region Models
        public class Factory
        {
            public uint Id { get; }
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
            public uint Id { get; }
            public string Name { get; set; }
            public string Description { get; set; }
            public uint FactoryId { get; set; }

            public Unit(uint id, string name, string description, uint factoryId)
            {
                Id = id;
                Name = name;
                Description = description;
                FactoryId = factoryId;
            }

            public void Print()
            {
                Console.WriteLine($" Установка #{Id}");
                Console.WriteLine($"   Название: {Name}");
                Console.WriteLine($"   Описание: {Description}");
                Console.WriteLine($"   Завод #{FactoryId}\n");
            }
        }

        public class Tank
        {
            public uint Id { get; }
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
                Console.WriteLine($"   Название: {Name}");
                Console.WriteLine($"   Описание: {Description}");
                Console.WriteLine($"   Объём: {Volume}");
                Console.WriteLine($"   Максимальный объём: {MaxVolume}");
                Console.WriteLine($"    Установка #{UnitId}\n");
            }
        }

        public struct TankInfo
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double Volume { get; set; }
            public double MaxVolume { get; set; }
            public string FactoryName { get; set; }
            public string WorkshopName { get; set; }

            public TankInfo(uint id, string name, string description, double volume, double maxVolume, string factoryName, string workshopName)
            {
                Id = id;
                Name = name;
                Description = description;
                Volume = volume;
                MaxVolume = maxVolume;
                FactoryName = factoryName;
                WorkshopName = workshopName;
            }

            public void Print()
            {
                Console.WriteLine($" Резервуар #{Id}");
                Console.WriteLine($"   Название: {Name}");
                Console.WriteLine($"   Описание: {Description}");
                Console.WriteLine($"   Объём: {Volume}");
                Console.WriteLine($"   Максимальный объём: {MaxVolume}");
                Console.WriteLine($"   Название завода: {FactoryName}");
                Console.WriteLine($"   Имя цеха: {WorkshopName}\n");
            }
        }

        private static uint GetNextUnusedId(HashSet<uint> ids)
        {
            uint nextId = 1;
            while (ids.Contains(nextId))
                nextId++;

            return nextId;
        }

        public static List<Factory> GetFactoriesFromFile(string path, string delimiter = ";")
        {
            List<Factory> factories = new();
            HashSet<uint> Ids = new();
            uint lineCount = 1, id;

            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
                return factories;
            }

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var data = line.Split(delimiter);

                if (data.Length != 3)
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                    continue;
                }

                if (!uint.TryParse(data[0], out id))
                    id = 0;

                if (id != 0 && !string.IsNullOrEmpty(data[1]) && !string.IsNullOrEmpty(data[2]))
                {
                    if (factories.Any(factory => factory.Id == id))
                        id = GetNextUnusedId(Ids);

                    Ids.Add(id);
                    lineCount++;
                    var factory = new Factory(id, data[1], data[2]);
                    factories.Add(factory);
                }
                else
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                }
            }

            return factories.OrderBy(factory => factory.Id).ToList();
        }

        public static List<Unit> GetUnitsFromFile(string path, string delimiter = ";")
        {
            List<Unit> units = new List<Unit>() { };
            HashSet<uint> Ids = new HashSet<uint>() { };
            uint lineCount = 1, id, factoryId;

            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
                return units;
            }

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var data = line.Split(delimiter);

                if (data.Length != 4)
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                    continue;
                }

                if (!uint.TryParse(data[0], out id) ||
                   !uint.TryParse(data[3], out factoryId))
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                    continue;
                }

                if (id == 0 || factoryId == 0 || string.IsNullOrEmpty(data[1]) || string.IsNullOrEmpty(data[2]))
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                    continue;
                }

                if (units.Any(unit => unit.Id == id))
                    id = GetNextUnusedId(Ids);

                Ids.Add(id);
                lineCount++;
                var unit = new Unit(id, data[1], data[2], factoryId);
                units.Add(unit);
            }

            return units.OrderBy(unit => unit.Id).ToList();
        }

        public static List<Tank> GetTanksFromFile(string path, string delimiter = ";")
        {
            List<Tank> tanks = new List<Tank>();
            HashSet<uint> Ids = new HashSet<uint>() { };
            uint id, volume, maxVolume, unitId, lineCount = 1;

            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
                return tanks;
            }

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var data = line.Split(delimiter);

                if (data.Length != 6)
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                    continue;
                }

                if (!uint.TryParse(data[0], out id) ||
                   !uint.TryParse(data[3], out volume) ||
                   !uint.TryParse(data[4], out maxVolume) ||
                   !uint.TryParse(data[5], out unitId))
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                    continue;
                }

                if (id == 0 || unitId == 0 || string.IsNullOrEmpty(data[1]) || string.IsNullOrEmpty(data[2]))
                {
                    Console.WriteLine($" Некорректный формат данных Строка #{lineCount}");
                    continue;
                }

                if ((volume >= 0 || maxVolume >= 0) && volume <= maxVolume)
                {
                    if (tanks.Any(tank => tank.Id == id))
                        id = GetNextUnusedId(Ids);

                    Ids.Add(id);
                    lineCount++;
                    var tank = new Tank(id, data[1], data[2], volume, maxVolume, unitId);
                    tanks.Add(tank);
                }
                else
                {
                    Console.WriteLine($" Объём резервуара не может быть отрицательным и/или больше максимального объёма Строка #{lineCount}");
                }
            }

            return tanks.OrderBy(factory => factory.Id).ToList();
        }

        public static List<Factory>? GetFactoriesFromJson(string path)
        {
            try
            {
                return !File.Exists(path) ? null : JsonSerializer.Deserialize<List<Factory>>(File.ReadAllText(path));
            }
            catch { return null; }
        }

        public static List<Unit>? GetUnitsFromJson(string path)
        {
            try
            {
                return !File.Exists(path) ? null : JsonSerializer.Deserialize<List<Unit>>(File.ReadAllText(path));
            }
            catch { return null; }
        }

        public static List<Tank>? GetTanksFromJson(string path)
        {
            try
            {
                return !File.Exists(path) ? null : JsonSerializer.Deserialize<List<Tank>>(File.ReadAllText(path));
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }
        }

        public static Factory? SearchFactoryByName(List<Factory> factories, string name)
        {
            return factories.FirstOrDefault(factory => factory.Name == name);
        }

        public static Unit? SearchUnitByName(List<Unit> units, string name)
        {
            return units.FirstOrDefault(unit => unit.Name == name);
        }

        public static Tank? SearchTankByName(List<Tank> tanks, string name)
        {
            return tanks.FirstOrDefault(tank => tank.Name == name);
        }

        public static Unit? FindUnit(List<Unit> units, List<Tank> tanks, string tankName)
        {
            var tank = tanks.FirstOrDefault(tank => tank.Name == tankName);
            return tank == null ? null : units.FirstOrDefault(unit => unit.Id == tank.UnitId);
        }

        public static Factory? FindFactory(List<Factory> factories, Unit unit)
        {
            return factories.FirstOrDefault(factory => factory.Id == unit.Id);
        }

        public static double GetTotalVolume(List<Tank> tanks)
        {
            return tanks.Sum(item => item.Volume);
        }

        public static void PrintAllTanks(List<Factory> factories, List<Unit> units, List<Tank> tanks)
        {
            bool flag = true;
            string? choice;

            foreach (var tank in tanks)
            {
                var unit = units.FirstOrDefault(unit => unit.Id == tank.UnitId);
                if (unit == null)
                    continue;

                var factory = factories.FirstOrDefault(f => f.Id == unit.FactoryId);
                if (factory == null)
                    continue;

                Console.WriteLine($" Резервуар #{tank.Id}");
                Console.WriteLine($"   Название: {tank.Name}");
                Console.WriteLine($"   Описание: {tank.Description}");
                Console.WriteLine($"   Объём: {tank.Volume}");
                Console.WriteLine($"   Максимальный объём: {tank.MaxVolume}");
                Console.WriteLine($"   Название завода: {factory.Description}");
                Console.WriteLine($"   Имя цеха: {factory.Name}\n");
            }

            while (flag)
            {
                Console.Write("\n Сохранить результат в JSON-файл? [Д(Y) / н(n)] > ");
                choice = Console.ReadLine();

                if (choice != null)
                    if ("ДдYy".Contains(choice))
                    {
                        if (SaveTanksInfoFromJson(factories, units, tanks))
                        {
                            Console.WriteLine($"\n Результат сохранен в JSON-файл по пути: {Path.Combine(BASE_DIR, "Output\\TanksInfoOutput.json")}");
                            flag = false;
                        }

                    }

                    else
                    {
                        Console.WriteLine("\n Не удалось сохранить информацию в JSON-файл!");
                        flag = false;
                    }
            }
        }

        public static bool SaveTanksInfoFromJson(List<Factory> factories, List<Unit> units, List<Tank> tanks)
        {
            List<TankInfo> TanksInfo = new List<TankInfo>() { };
            TankInfo tankInfo;

            foreach (var tank in tanks)
            {
                var unit = units.FirstOrDefault(unit => unit.Id == tank.UnitId);
                if (unit == null)
                    continue;

                var factory = factories.FirstOrDefault(f => f.Id == unit.FactoryId);
                if (factory == null)
                    continue;

                tankInfo = new TankInfo(tank.Id, tank.Name, tank.Description, tank.Volume, tank.MaxVolume, factory.Description, factory.Name);
                TanksInfo.Add(tankInfo);
                tankInfo.Print();
            }

            if (tanks.Count > 0)
            {
                try
                {
                    if (!Directory.Exists(Path.Combine(BASE_DIR, "Output")))
                        Directory.CreateDirectory(Path.Combine(BASE_DIR, "Output"));

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };

                    File.WriteAllText(Path.Combine(BASE_DIR, "Output\\TanksInfoOutput.json"), JsonSerializer.Serialize(TanksInfo, options));

                    return true;
                }

                catch { return false; }
            }

            else
                return false;
        }

        #endregion

        static void Main(string[] args)
        {
            bool flag = true, flag_1, flag_2;
            List<Factory>? factories = new List<Factory>() { };
            List<Unit>? units = new List<Unit>() { };
            List<Tank>? tanks = new List<Tank>() { };
            int choice;

            while (flag)
            {
                Console.WriteLine("[*] Выберете способ импорта данных:");
                Console.WriteLine("  > [1] Text File");
                Console.WriteLine("  > [2] JSON File");

                Console.Write("\n Ваш выбор > ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            factories = GetFactoriesFromFile(Path.Combine(BASE_DIR, "Files\\factories_data.txt"));
                            units = GetUnitsFromFile(Path.Combine(BASE_DIR, "Files\\units_data.txt"));
                            tanks = GetTanksFromFile(Path.Combine(BASE_DIR, "Files\\tanks_data.txt"));
                            flag = false;
                            break;

                        case 2:
                            factories = GetFactoriesFromJson(Path.Combine(BASE_DIR, "Json\\factories.json"));
                            units = GetUnitsFromJson(Path.Combine(BASE_DIR, "Json\\units.json"));
                            tanks = GetTanksFromJson(Path.Combine(BASE_DIR, "Json\\tanks.json"));
                            flag = false;
                            break;

                        default:
                            Console.WriteLine("\n Неверный выбор! Повторите попытку\n");
                            break;
                    }
                }
            }

            Console.Clear();
            flag = true;

            while (flag)
            {
                Console.WriteLine(" Меню программы:");
                Console.WriteLine("  > 5 - Вывод все объекты таблицы");
                Console.WriteLine("  > 4 - Вывод всех резервуаров с именами цеха и завода, где они числятся");
                Console.WriteLine("  > 3 - Вывод общей сумму загрузки всех резервуаров");
                Console.WriteLine("  > 2 - Найти объект в таблице по имени");
                Console.WriteLine("  > 1 - Выход");

                while (true)
                {
                    Console.Write("\n Ваш выбор > ");
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
                                Console.WriteLine("  > 1 - Заводы");
                                Console.WriteLine("  > 2 - Установки");
                                Console.WriteLine("  > 3 - Резервуары");
                                Console.WriteLine("  > 0 - Выход");

                                while (flag_1)
                                {
                                    Console.Write("\n Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice))
                                    {
                                        switch (choice)
                                        {
                                            case 0:
                                                flag_1 = false;
                                                break;

                                            case 1:
                                                if (factories != null)
                                                {
                                                    if (factories.Count != 0)
                                                    {
                                                        factories.ForEach(factory => factory.Print());
                                                        flag_1 = false;
                                                    }
                                                }

                                                else
                                                    Console.WriteLine("\n Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 2:
                                                if (units != null)
                                                {
                                                    if (units.Count != 0)
                                                    {
                                                        units.ForEach(item => item.Print());
                                                        flag_1 = false;
                                                    }
                                                }

                                                else
                                                    Console.WriteLine("\n Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 3:
                                                if (tanks != null)
                                                {
                                                    if (tanks.Count != 0)
                                                    {
                                                        tanks.ForEach(tank => tank.Print());
                                                        flag_1 = false;
                                                    }
                                                }

                                                else
                                                    Console.WriteLine("\n Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

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
                                if (factories != null && units != null && tanks != null)
                                {
                                    if (factories.Count != 0 || units.Count != 0 || tanks.Count != 0)
                                    {
                                        Console.WriteLine("\n");
                                        PrintAllTanks(factories, units, tanks);
                                    }
                                }

                                else
                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные о заводах и/или установках и/или резервуарах");

                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                Console.ReadKey();
                                Console.Clear();
                                break;

                            case 3:
                                if (tanks != null)
                                {
                                    if (tanks.Count != 0)
                                        Console.WriteLine($"\n\nОбщая сумма загрузки всех резервуаров: {GetTotalVolume(tanks)}");

                                    else
                                        Console.WriteLine("Список резервуаров пуст!");

                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                    Console.ReadKey();
                                    Console.Clear();
                                }

                                else
                                    Console.WriteLine("\n Невозможно выполнить действие так, как отсутствуют данные об резервуарах");


                                break;

                            case 4:
                                flag_2 = true;
                                while (flag_2)
                                {
                                    Console.WriteLine("\n Выберите таблицу:");
                                    Console.WriteLine("  > 1 - Заводы");
                                    Console.WriteLine("  > 2 - Установки");
                                    Console.WriteLine("  > 3 - Резервуары");
                                    Console.WriteLine("  > 0 - Выход");

                                    Console.Write("\n Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice))
                                    {
                                        switch (choice)
                                        {
                                            case 0:
                                                flag_2 = false;
                                                break;

                                            case 1:
                                                if (factories != null)
                                                {
                                                    if (factories.Count != 0)
                                                    {
                                                        Console.Write("\n Введите имя завода для поиска >");

                                                        string? name = Console.ReadLine();
                                                        if (!string.IsNullOrEmpty(name))
                                                        {
                                                            var result = SearchFactoryByName(factories, name);
                                                            if (result != null)
                                                                result.Print();

                                                            else
                                                                Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }

                                                        else
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");

                                                        flag_2 = false;
                                                    }

                                                    else
                                                        Console.WriteLine("\nСписок заводов пуст!");
                                                }

                                                else
                                                    Console.WriteLine("\n Невозможно выполнить действие так, как отсутствуют данные о заводах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 2:
                                                if (units != null)
                                                {
                                                    if (units.Count != 0)
                                                    {
                                                        Console.Write("\n Введите имя установки для поиска > ");

                                                        string? name = Console.ReadLine();
                                                        if (!string.IsNullOrEmpty(name))
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
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");

                                                        flag_2 = false;
                                                    }

                                                    else
                                                        Console.WriteLine("\nСписок установок пуст!");
                                                }

                                                else
                                                    Console.WriteLine("\n Невозможно выполнить действие так, как отсутствуют данные об установках");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 3:
                                                if (tanks != null)
                                                {
                                                    if (tanks.Count != 0)
                                                    {
                                                        Console.Write("\n Введите имя резервуара для поиска >");

                                                        string? name = Console.ReadLine();
                                                        if (!string.IsNullOrEmpty(name))
                                                        {
                                                            var result = SearchTankByName(tanks, name);
                                                            if (result != null)
                                                                result.Print();

                                                            else
                                                                Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }

                                                        else
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");

                                                        flag_2 = false;
                                                    }


                                                    else
                                                        Console.WriteLine("\nСписок резервуаров пуст!");
                                                }

                                                else
                                                    Console.WriteLine("\n Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            default:
                                                Console.WriteLine("\n Неверный выбор! Повторите попытку");
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        Console.WriteLine("\n Неверный выбор! Повторите попытку");
                                    }
                                }

                                break;

                            default:
                                Console.WriteLine("\n Неверный выбор! Повторите попытку\n");
                                break;
                        }

                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n Неверный выбор! Повторите попытку");
                    }
                }
            }
        }
    }
}