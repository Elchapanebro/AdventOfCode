using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Resources;

namespace AdventOfCode.Tasks
{
    public class Day2Task1 : ITask
    {
        public int Day => 2;
        public int TaskNumber => 1;
        public object Execute()
        {
            var boxIds = Manifest.Day2.Split("\r\n");

            int Twosies = 0;
            int Threesies = 0;
            var letterDict = new Dictionary<char, int>();

            foreach (var boxId in boxIds)
            {
                foreach (var letter in boxId)
                {
                    if (letterDict.ContainsKey(letter))
                    {
                        letterDict[letter]++;
                    }
                    else
                    {
                        letterDict.Add(letter, 1);
                    }
                }

                if (letterDict.Any(x => x.Value == 2)) Twosies++;
                if (letterDict.Any(x => x.Value == 3)) Threesies++;

                letterDict.Clear();
            }

            return Twosies * Threesies;
        }
    }

    public class Day2Task2 : ITask
    {
        public int Day => 2;
        public int TaskNumber => 2;
        public object Execute()
        {
            var boxIds = Manifest.Day2.Split("\r\n").ToList();

            var letterDict = new Dictionary<char, int>();

            foreach (var boxId in Manifest.Day2.Split("\r\n"))
            {
                foreach (var innerBoxId in boxIds)
                {
                    if (innerBoxId != boxId)
                    {
                        bool strikeOne = false;
                        int position = 0;
                        for (int i = 0; i < innerBoxId.Length; i++)
                        {
                            if (innerBoxId[i] != boxId[i])
                            {
                                if (strikeOne)
                                {
                                    break;
                                }
                                else
                                {
                                    strikeOne = true;
                                    position = i;
                                }
                            }

                            if ((i+1) == innerBoxId.Length)
                            {
                                return boxId.Substring(0, position) + boxId.Substring(position + 1);
                            }
                        }
                    }
                }

                letterDict.Clear();
            }

            return "ERROR";
        }
    }
}
