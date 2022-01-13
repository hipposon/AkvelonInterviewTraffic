using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var strArr = new string[] { "1:[5]", "4:[5]", "3:[5]", "5:[1,4,3,2]", "2:[5,15,7]", "7:[2,8]", "8:[7,38]", "15:[2]", "38:[8]"};
            //var strArr = new string[] { "1:[5]", "2:[5]", "3:[5]", "4:[5]", "5:[1,2,3,4]" };
            //var strArr = new string[] { "1:[5]", "2:[5,18]", "3:[5,12]", "4:[5]", "5:[1,2,3,4]", "18:[2]", "12:[3]" };
            Console.WriteLine(CityTraffic(strArr));
        }

        private static Dictionary<int, int[]> _towns;
        static public string CityTraffic(string[] strArr)
        {
            SetTowns(strArr);

            return string.Join(",", GetMaxTraffics().Select(tt => string.Join(":", tt.Key, tt.Value))); ;
        }

        private static void SetTowns(string[] strArr)
        {
            _towns = new Dictionary<int, int[]>();
            foreach (var town in strArr)
            {
                var pair = town.Split(':');
                if (!int.TryParse(pair[0], out int population))
                    return ;
                _towns.Add(population, pair[1].Replace("[", null).Replace("]", null).Split(',').Select(int.Parse).ToArray());
            }
        }

        private static Dictionary<int, int> GetMaxTraffics()
        {
            var townTraffics = new Dictionary<int, int>();

            foreach (var town in _towns.OrderBy(t => t.Key))
            {
                townTraffics.Add(town.Key, GetMaxTraffic(town.Key, town.Value));
            }
            return townTraffics;
        }

        private static int GetMaxTraffic(int town, int[] routes)
        {
            var result = 0;
            foreach (var route in routes)
            {
                var path = GetPathPopulation(route, town);
                if (path > result)
                    result = path;
            }
            return result;
        }

        private static int GetPathPopulation(int town, int prevTown)
        {
            var result = town;
            foreach (var item in _towns.GetValueOrDefault(town))
            {
                if (item == prevTown)
                    continue;
                result += GetPathPopulation(item, town);
            }
            return result;
        }
    }
}
