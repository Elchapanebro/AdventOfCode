using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public static class HiddenTask
    {
        #region Maze Metics

        private class MazeObject
        {
            public Point Coordinates { get; }

            public bool IsWall => _object != ' ';

            private readonly char _object;

            private readonly bool _isEndOfLine;

            public MazeObject(int x, int y, char obj, bool isEndOfLine)
            {
                _object = obj;
                Coordinates = new Point(x, y);
                _isEndOfLine = isEndOfLine;
            }

            public void Draw(Point currentPosition)
            {
                Console.ForegroundColor = IsWall ? ConsoleColor.DarkGray : ConsoleColor.Yellow;
                Console.BackgroundColor = IsWall ? ConsoleColor.DarkGray : ConsoleColor.Black;

                Console.Write(currentPosition == Coordinates ? '*' : _object);

                if (_isEndOfLine)
                {
                    Console.Write("\r\n");
                }
            }
        }

        private const string Maze = @"
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa   a
8   8               8               8           8                   8   8
8   8   aaaaaaaaa   8   aaaaa   aaaa8aaaa   aaaa8   aaaaa   aaaaa   8   8
8               8       8   8           8           8   8   8       8   8
8aaaaaaaa   a   8aaaaaaa8   8aaaaaaaa   8aaaa   a   8   8   8aaaaaaa8   8
8       8   8               8           8   8   8   8   8           8   8
8   a   8aaa8aaaaaaaa   a   8   aaaaaaaa8   8aaa8   8   8aaaaaaaa   8   8
8   8               8   8   8       8           8           8       8   8
8   8aaaaaaaaaaaa   8aaa8   8aaaa   8   aaaaa   8aaaaaaaa   8   aaaa8   8
8           8       8   8       8   8       8           8   8           8
8   aaaaa   8aaaa   8   8aaaa   8   8aaaaaaa8   a   a   8   8aaaaaaaaaaa8
8       8       8   8   8       8       8       8   8   8       8       8
8aaaaaaa8aaaa   8   8   8   aaaa8aaaa   8   aaaa8   8   8aaaa   8aaaa   8
8           8   8           8       8   8       8   8       8           8
8   aaaaa   8   8aaaaaaaa   8aaaa   8   8aaaa   8aaa8   aaaa8aaaaaaaa   8
8   8       8           8           8       8   8   8               8   8
8   8   aaaa8aaaa   a   8aaaa   aaaa8aaaa   8   8   8aaaaaaaaaaaa   8   8
8   8           8   8   8   8   8           8               8   8       8
8   8aaaaaaaa   8   8   8   8aaa8   8aaaaaaa8   aaaaaaaaa   8   8aaaaaaa8
8   8       8   8   8           8           8   8       8               8
8   8   aaaa8   8aaa8   aaaaa   8aaaaaaaa   8aaa8   a   8aaaaaaaa   a   8
8   8                   8           8               8               8   8
8 * 8aaaaaaaaaaaaaaaaaaa8aaaaaaaaaaa8aaaaaaaaaaaaaaa8aaaaaaaaaaaaaaa8aaa8";

        private static List<MazeObject> Objects = new List<MazeObject>();

        private static Point CurrentPosition = new Point(2,2);
        
        #endregion

        public static void Run()
        {
            bool moved = false;

            while (true)
            {
                if (moved)
                {
                    DrawScene();
                }

                var newPoint = CurrentPosition;
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        newPoint = new Point(CurrentPosition.X - 1, CurrentPosition.Y);
                        break;
                    case ConsoleKey.DownArrow:
                        newPoint = new Point(CurrentPosition.X + 1, CurrentPosition.Y);
                        break;
                    case ConsoleKey.RightArrow:
                        newPoint = new Point(CurrentPosition.X, CurrentPosition.Y + 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        newPoint = new Point(CurrentPosition.X, CurrentPosition.Y - 1);
                        break;
                }

                if (IsNoCollision(newPoint))
                {
                    CurrentPosition = newPoint;
                    moved = true;
                }
                else
                {
                    moved = false;
                }
            }
        }

        public static void Init()
        {
            var mazeRows = Maze.Split("\r\n");
            for (int i = 0; i < mazeRows.Length; i++)
            {
                for (int j = 0; j < mazeRows[i].Length; j++)
                {
                    if (mazeRows[i][j] == '*')
                    {
                        Objects.Add(new MazeObject(i + 1, j + 1, ' ', false));
                        CurrentPosition = new Point(i + 1, j + 1);
                    }
                    else
                    {
                        Objects.Add(new MazeObject(i + 1, j + 1, mazeRows[i][j], j + 1 == mazeRows[i].Length));
                    }
                }
            }
        }

        private static bool IsNoCollision(Point newPosition)
        {
            var newPos = Objects.SingleOrDefault(x => x.Coordinates == newPosition);
            return newPos != null && !newPos.IsWall;
        }

        private static void DrawScene()
        {
            Console.Clear();

            foreach (var obj in Objects)
            {
                obj.Draw(CurrentPosition);
            }

            Console.ResetColor();
        }
    }
}
