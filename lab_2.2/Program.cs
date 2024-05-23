using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;

namespace LAB2_TASK2
{
    internal class Program
    {
        class Deal
        {
            public int Sum { get; set; }
            public required string Id { get; set; }
            public DateTime Date { get; set; }

            public void Print()
            {
                Console.WriteLine($"{Id} - {Date} - {Sum}\n");
            }
        }

        static List<Deal>? ImportDataFromJSON(string path)
        {
            try
            {
                return !File.Exists(path) ? null : JsonSerializer.Deserialize<List<Deal>>(File.ReadAllText(path));
            }
            catch { return null; }
        }

        static IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals)
        {
            return (
                from deal in deals
                where deal.Sum >= 100
                orderby deal.Date
                select deal
            ).Take(5).OrderByDescending(deal => deal.Sum).Select(d => d.Id).ToList();
        }

        record SumByMonth(DateTime Month, int Sum);

        static IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
        {
            return (
                from deal in deals
                group deal by new DateTime(deal.Date.Year, deal.Date.Month, 1) into groupedDeals
                select new SumByMonth(groupedDeals.Key, groupedDeals.Sum(deal => deal.Sum))
            ).ToList();
        }

        static void Main(string[] args)
        {
            string path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "JSON_sample_1.json");
            List<Deal>? deals = ImportDataFromJSON(path);

            if (deals != null)
            {
                Console.WriteLine($"Кол-во найденных сделок: {deals.Count()}");

                if (deals.Count > 0)
                {
                    Console.WriteLine($"Идентификаторы сделок: {string.Join(", ", GetNumbersOfDeals(deals))}\n");

                    IList<SumByMonth> sumsByMonth = GetSumsByMonth(deals);
                    foreach (var sumByMonth in sumsByMonth)
                        Console.WriteLine($"Месяц: {sumByMonth.Month.ToShortDateString()}, Сумма: {sumByMonth.Sum}");
                }
            }

            else { Console.WriteLine("Не удалось найти или прочитать файл!"); }
        }
    }
}