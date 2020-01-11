using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Resources;

namespace AdventOfCode.Tasks.Y2018
{
    public class Day5Task1 : ITask
    {
        public int Year => 2018;
        
        public int Day => 5;

        public int TaskNumber => 1;

        public object Execute()
        {
            var result = Day5Helper.ReactPolymer(Manifest.Day5);

            return result.Length;
        }
    }

    public class Day5Task2 : ITask
    {
        public int Year => 2018;
        
        public int Day => 5;

        public int TaskNumber => 2;

        public object Execute()
        {
            var letters = Manifest.Day5.ToLower().Distinct();

            var results = new Dictionary<char, int>();

            foreach (char letter in letters)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Removing Unit {letter}/{char.ToUpper(letter)}");

                var result = Day5Helper.ReactPolymer(
                    Manifest.Day5
                        .Replace(letter.ToString(), string.Empty)
                        .Replace(char.ToUpper(letter).ToString(), string.Empty));

                results.Add(letter, result.Length);
            }

            return results.Values.OrderBy(x => x).First();
        }
    }

    internal sealed class Day5Helper
    {
        public static string ReactPolymer(string input)
        {
            var polymer = input;

            int count = 0;
            for (int i = 0; i < polymer.Length; i++)
            {
                var nextIndex = i + 1;
                if (nextIndex >= polymer.Length)
                {
                    //End Of File Reached
                    break;
                }

                var thisPoly = polymer[i];
                var nextPoly = polymer[nextIndex];

                if (char.ToUpper(thisPoly) == char.ToUpper(nextPoly)
                    && ((char.IsUpper(thisPoly) && char.IsLower(nextPoly))
                        || (char.IsUpper(nextPoly) && char.IsLower(thisPoly))))
                {
                    polymer = polymer.Remove(i, 2);
                    i = i - 2;
                    if (i < 0) i = -1;
                    count++;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Found {count} reaction(s)");
            Console.ResetColor();

            return polymer;
        }
    }
}
