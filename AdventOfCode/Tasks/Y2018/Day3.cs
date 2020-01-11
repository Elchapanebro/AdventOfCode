using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Resources;

namespace AdventOfCode.Tasks.Y2018
{
    public class Day3Task1 : ITask
    {

        
        public int Year => 2018; 
        
        public int Day => 3;
        public int TaskNumber => 1;
        public object Execute()
        {
            var claims = new HashSet<Claim>();

            foreach (var line in Manifest.Day3.Split("\r\n"))
            {
                claims.Add(Claim.Parse(line));
            }

            return claims.SelectMany(x => x.Points).GroupBy(g => (g.X, g.Y)).Count(x => x.Count() > 1);
        }       
    }

    public class Day3Task2 : ITask
    {

        
        public int Year => 2018; 
        
        public int Day => 3;
        public int TaskNumber => 2;
        public object Execute()
        {
            var claims = new HashSet<Claim>();

            foreach (var line in Manifest.Day3.Split("\r\n"))
            {
                claims.Add(Claim.Parse(line));
            }

            var singleUnclaimedPoints = claims.SelectMany(x => x.Points).GroupBy(g => (g.X, g.Y)).Where(x => x.Count() == 1).Select(x => new Point(x.Key.Item1, x.Key.Item2)).ToHashSet();

            return claims.Single(c => c.Points.All(p => singleUnclaimedPoints.Contains(p))).ClaimId;
        }
    }

    internal sealed class Claim
    {
        public int ClaimId { get; }

        public List<Point> Points { get; }

        public Claim(int claimId, int width, int height, int x, int y)
        {
            ClaimId = claimId;
            Points = new List<Point>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Points.Add(new Point(x + j, y + i));
                }
            }
        }

        public static Claim Parse(string line)
        {
            var match = Regex.Match(line, @"^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$");
            if (match.Success)
            {
                return new Claim(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[4].Value),
                    int.Parse(match.Groups[5].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value));
            }
            else return null;
        }
    }
}
