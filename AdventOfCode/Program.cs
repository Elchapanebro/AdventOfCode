using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    internal sealed class Program
    {
        private const double TargetNfr = 7.5;

        private static readonly List<ITask> Tasks = new List<ITask>();

        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static int _selectedYear = 2018;

        private static int _selectedDay = 1;

        private static int _selectedTask = 1;

        private static int _minDays;

        private static int _maxDays;

        private static int _minYears;

        private static int _maxYears;

        static void Main(string[] args)
        {
            Console.Title = "Advent of Code";

            Load();

            SelectYear();
        }

        static void StartTaskWithInstrumentation(ITask task)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Executing Year {task.Year} Day {task.Day}, Task {task.TaskNumber}...");

                Stopwatch.Start();

                var result = task.Execute();

                Stopwatch.Stop();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(
                    $"\r\nResult:\r\n{result}\r\n\r\nElapsed Time: {Stopwatch.Elapsed.TotalSeconds} seconds");

                if (Stopwatch.Elapsed.TotalSeconds > TargetNfr)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"\r\nTask took longer than set threshold of {TargetNfr}");
                }

                Clipboard.CopyToClipboard(result.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Results have been copied to the clipboard.");
                Console.WriteLine("Press enter to return to the menu.");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\r\nAn error occured during the running of Year {task.Year} Day {task.Day}, Task {task.TaskNumber}:\r\n\r\n{e}");
            }
            finally
            {
                Console.ResetColor();
                Stopwatch.Reset();
                Console.ReadLine();
            }
        }

        static void DrawYearMenu()
        {
            DrawHeader("year");

            foreach (var year in Tasks
                .Select(x => x.Year)
                .Distinct()
                .OrderBy(x => x))
            {
                if (_selectedYear == year)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.WriteLine($"[{(_selectedYear == year ? "*" : " ")}] - Year {year}");

                Console.ResetColor();
            }

            DrawFooter();
        }

        static void DrawDayMenu()
        {
            DrawHeader("day");

            foreach (var task in Tasks
                .Where(x => x.Year == _selectedYear)
                .GroupBy(x => x.Day)
                .Select(x => x.First())
                .OrderBy(x => x.Day))
            {
                if (_selectedDay == task.Day)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.WriteLine($"[{(_selectedDay == task.Day ? "*" : " ")}] - Day {task.Day}");

                Console.ResetColor();
            }

            DrawFooter();
        }

        static void DrawTaskMenu()
        {
            DrawHeader("task");

            foreach (var task in Tasks
                .Where(x => x.Year == _selectedYear && x.Day == _selectedDay))
            {
                if (_selectedTask == task.TaskNumber)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.WriteLine($"[{(_selectedTask == task.TaskNumber ? "*" : " ")}] - Task {task.TaskNumber}");

                Console.ResetColor();
            }

            DrawFooter();
        }

        static void DrawHeader(string context)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Select a {context} from the menu using the directional arrows (up/down) and select using enter.");
            Console.ForegroundColor = ConsoleColor.Gray;

            if (context == "day")
            {
                Console.WriteLine("To quit, press 'q'.\r\n");
            }
            else
            {
                Console.WriteLine("To go back, press the backspace.\r\n");
            }
        }

        static void DrawFooter()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\r\nMade by Tom Day");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("GitHub: https://github.com/Elchapanebro");

            Console.ResetColor();
        }

        static void SelectYear()
        {
            bool quitting = false;
            while (!quitting)
            {
                Console.Clear();
                DrawYearMenu();

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _selectedYear--;
                        if (_selectedYear < _minYears)
                        {
                            _selectedYear = _maxYears;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        _selectedYear++;
                        if (_selectedYear > _maxYears)
                        {
                            _selectedYear = _minYears;
                        }
                        break;
                    case ConsoleKey.Enter:
                        SelectDay();
                        break;
                    case ConsoleKey.Q:
                        Console.WriteLine("Are you sure you want to Quit? Y/N");
                        var quitResponse = Console.ReadKey();
                        if (quitResponse.Key == ConsoleKey.Y)
                        {
                            quitting = true;
                        }
                        break;
                }
            }
        }

        static void SelectDay()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawDayMenu();

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _selectedDay--;
                        if (_selectedDay < _minDays)
                        {
                            _selectedDay = _maxDays;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        _selectedDay++;
                        if (_selectedDay > _maxDays)
                        {
                            _selectedDay = _minDays;
                        }
                        break;
                    case ConsoleKey.Enter:
                        SelectTask();
                        break;
                    case ConsoleKey.Backspace:
                        back = true;
                        break;
                }
            }
        }

        static void SelectTask()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawTaskMenu();

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _selectedTask--;
                        if (_selectedTask < 1)
                        {
                            _selectedTask = 2;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        _selectedTask++;
                        if (_selectedTask > 2)
                        {
                            _selectedTask = 1;
                        }
                        break;
                    case ConsoleKey.Enter:
                        StartTaskWithInstrumentation(Tasks.SingleOrDefault(x =>
                            x.Year == _selectedYear && x.Day == _selectedDay && x.TaskNumber == _selectedTask));
                        back = true;
                        break;
                    case ConsoleKey.Backspace:
                        back = true;
                        break;
                }
            }
        }

        static void Load()
        {
            Console.SetWindowSize(Console.WindowWidth, Console.LargestWindowHeight / 2);

            foreach (var t in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i == typeof(ITask))))
            {
                Tasks.Add((ITask)Activator.CreateInstance(t));
            }

            _minDays = Tasks.Min(x => x.Day);

            _maxDays = Tasks.Max(x => x.Day);

            _minYears = Tasks.Min(x => x.Year);

            _maxYears = Tasks.Max(x => x.Year);
        }
    }
}