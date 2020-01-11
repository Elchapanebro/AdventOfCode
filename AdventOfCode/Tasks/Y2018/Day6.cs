using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Resources;

namespace AdventOfCode.Tasks.Y2018
{
    public class Day6Task1 : ITask
    {
        public int Year => 2018;
        
        public int Day => 6;

        public int TaskNumber => 1;

        public object Execute()
        {
            var coordinates = Manifest.Day6.Split("\r\n").Select(x => x.Split(',')).Select(p => new Point(int.Parse(p[0].Trim()), int.Parse(p[1].Trim()))).ToList();

            var gridPoints = Day6Helper.GenerateGrid(coordinates, false);
            var expandedGridPoints = Day6Helper.GenerateGrid(coordinates, true);

            var resultDict = new Dictionary<Point, int>();

            foreach (var coordinate in coordinates)
            {
                var normal = gridPoints.Count(x => !x.IsClashed && x.ClosestPoint == coordinate);
                var expanded = expandedGridPoints.Count(x => !x.IsClashed && x.ClosestPoint == coordinate);

                if (normal == expanded)
                {
                    resultDict.Add(coordinate, normal);
                }
            }

            return resultDict.Values.Max();
        }
    }

    public class Day6Task2 : ITask
    {
        public int Year => 2018;
        
        public int Day => 6;

        public int TaskNumber => 2;

        public object Execute()
        {
            var coordinates = Manifest.Day6.Split("\r\n").Select(x => x.Split(',')).Select(p => new Point(int.Parse(p[0].Trim()), int.Parse(p[1].Trim()))).ToList();

            return Day6Helper.GenerateGrid(coordinates, false).Count(x => x.TotalDistance < 10000);
        }
    }

    internal static class Day6Helper
    {
        public static HashSet<GridPoint> GenerateGrid(List<Point> coordinates, bool largerGrid)
        {
            var gridSize = new Point(coordinates.Max(p => p.X), coordinates.Max(p => p.Y));

            var grid = new HashSet<GridPoint>();

            for (int x = 0 - (largerGrid ? 1 : 0); x < gridSize.X + (largerGrid ? 1 : 0); x++)
            {
                for (int y = 0 - (largerGrid ? 1 : 0); y < gridSize.Y + (largerGrid ? 1 : 0); y++)
                {
                    var p = new Point(x, y);
                    (Point closestPoint, int distance) lowest = (Point.Empty, int.MaxValue);
                    bool clashed = false;
                    var totalDistance = 0;
                    foreach (var coordinate in coordinates)
                    {
                        var d = CalculateDistance(p, coordinate);
                        totalDistance = totalDistance + d;

                        if (d == lowest.distance)
                        {
                            clashed = true;
                        }
                        else if (d < lowest.distance)
                        {
                            lowest.closestPoint = coordinate;
                            clashed = false;
                        }
                    }

                    grid.Add(new GridPoint(p, lowest.closestPoint, totalDistance, clashed));
                }
            }

            return grid;
        }

        private static int CalculateDistance(Point control, Point comparison)
        {
            return Math.Abs(control.X - comparison.X) + Math.Abs(control.Y - comparison.Y);
        }

        internal sealed class GridPoint
        {
            public Point Coordinates { get; }

            public Point ClosestPoint { get; }

            public bool IsClashed { get; }

            public int TotalDistance { get; }

            public GridPoint(Point coord, Point closest, int totalDistance, bool clashed)
            {
                Coordinates = coord;
                ClosestPoint = closest;
                TotalDistance = totalDistance;
                IsClashed = clashed;
            }

            public override string ToString()
            {
                return $"[{Coordinates.X},{Coordinates.Y}] {(IsClashed ? "CLASHED" : string.Empty)} c: [{ClosestPoint.X},{ClosestPoint.Y}] d: {TotalDistance}";
            }
        }
    }
}
