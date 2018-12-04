using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Resources;

namespace AdventOfCode.Tasks
{
    public class Day3Task1 : ITask
    {
        public int Day => 3;
        public int TaskNumber => 1;
        public object Execute()
        {
            var conflictedPoints = new List<Point>();
            var claims = new List<Claim>();

            foreach (var line in Manifest.Day3.Split("\r\n"))
            {
                claims.Add(Claim.Parse(line));
            }

            var allPoints = claims.SelectMany(c => c.Points)
                .GroupBy(p => p.X)
                .OrderBy(g => g.Key);

            foreach (var xGrouping in allPoints)
            {
                List<int> Ys = new List<int>();
                foreach (var p in xGrouping)
                {
                    if (Ys.Contains(p.Y) && !conflictedPoints.Contains(p))
                    {
                        conflictedPoints.Add(p);
                    }
                    else
                    {
                        Ys.Add(p.Y);
                    }
                }
            }

            return conflictedPoints.Distinct().Count();
        }       
    }

    public class Day3Task2 : ITask
    {
        public int Day => 3;
        public int TaskNumber => 2;
        public object Execute()
        {
            var claims = new List<Claim>();

            foreach (var line in Manifest.Day3.Split("\r\n"))
            {
                claims.Add(Claim.Parse(line));
            }

            foreach (var claim in claims)
            {
                bool clash = false;
                foreach (var innerClaim in claims)
                {
                    if (claim.ClaimId != innerClaim.ClaimId)
                    {
                        if (innerClaim.Points.Any(x => claim.Points.Any(p => p.X == x.X && p.Y == x.Y)))
                        {
                            clash = true;
                            break;
                        }
                    }
                }

                if (!clash)
                {
                    return claim.ClaimId;
                }
            }

            return "ERROR";
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
