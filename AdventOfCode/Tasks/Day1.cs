using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Resources;

namespace AdventOfCode.Tasks
{
    public class Day1Task1 : ITask
    {
        public int Day => 1;
        public int TaskNumber => 1;
        public object Execute()
        {
            int frequency = 0;
            var numerals = Manifest.Day1.Split('\n');
            foreach (var n in numerals)
            {
                frequency += int.Parse(n);
            }

            return frequency;
        }
    }

    public class Day1Task2 : ITask
    {
        public int Day => 1;
        public int TaskNumber => 2;
        public object Execute()
        {
            int frequency = 0;

            List<int> results = new List<int>();
            results.Add(0);

            var values = Manifest.Day1.Split('\n');

            int[] numerals = values.Select(int.Parse).ToArray();
            int position = 0;

            while (true)
            {
                frequency += numerals[position++];

                if (position == numerals.Length)
                {
                    position = 0;
                }

                if (results.Contains(frequency))
                {
                    //we've seen this before.
                    return frequency;
                }

                results.Add(frequency);
            }
        }
    }
}
