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

        static void Main(string[] args)
        {
            Load();

            while (true)
            {
                Console.WriteLine("\r\nPlease enter Day & Task to execute:");
                string[] arguments = Console.ReadLine()?.Split(" ") ?? new string[0];

                if (arguments.Length == 1 && arguments[0].Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }

                if (arguments.Length == 2 
                    && int.TryParse(arguments[0], out int day) 
                    && int.TryParse(arguments[1], out int taskNumber))
                {
                    var task = Tasks.SingleOrDefault(t => t.Day == day && t.TaskNumber == taskNumber);
                    if (task != null)
                    {
                        StartTaskWithInstrumentation(task);
                    }
                    else
                    {
                        Console.WriteLine($"No matching executor could be found for Day {day}, Task {taskNumber}.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input.");
                }
            }
        }

        static void StartTaskWithInstrumentation(ITask task)
        {
            try
            {
                Console.Clear();
                Console.WriteLine($"Executing Day {task.Day}, Task {task.TaskNumber}...");

                Stopwatch.Start();

                var result = task.Execute();

                Stopwatch.Stop();

                Console.WriteLine(
                    $"Day {task.Day}, Task {task.TaskNumber} Result:\r\n{result}\r\nElapsed Time: {Stopwatch.Elapsed.TotalSeconds} seconds");

                Clipboard.CopyToClipboard(result.ToString());

                if (Stopwatch.Elapsed.TotalSeconds > TargetNfr)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"\r\nTask took longer than set threshold of {TargetNfr}");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occured during the running of Day {task.Day}, Task {task.TaskNumber}:\r\n\r\n{e}");
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Stopwatch.Reset();
            }
        }

        static void Load()
        {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i == typeof(ITask))))
            {
                Tasks.Add((ITask)Activator.CreateInstance(t));
            }
        }
    }
}
