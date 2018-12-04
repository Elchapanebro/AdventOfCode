using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    internal sealed class Program
    {
        private static List<ITask> _tasks = new List<ITask>();
        static void Main(string[] args)
        {
            Load();

            while (true)
            {
                Console.WriteLine("Please enter Day & Task to execute:");
                string[] arguments = Console.ReadLine()?.Split(" ") ?? new string[0];

                if (arguments.Length == 1 && arguments[0].Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }

                if (arguments.Length == 2 
                    && int.TryParse(arguments[0], out int day) 
                    && int.TryParse(arguments[1], out int taskNumber))
                {
                    var task = _tasks.SingleOrDefault(t => t.Day == day && t.TaskNumber == taskNumber);
                    if (task != null)
                    {

                        var result = task.Execute();
                        Console.WriteLine($"Day {day}, Task {taskNumber} Result:\r\n{result}");
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

        static void Load()
        {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i == typeof(ITask))))
            {
                _tasks.Add((ITask)Activator.CreateInstance(t));
            }
        }
    }
}
