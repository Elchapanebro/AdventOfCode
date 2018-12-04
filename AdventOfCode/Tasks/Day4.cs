using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Resources;

namespace AdventOfCode.Tasks
{
    public class Day4Task1 : ITask
    {
        public int Day => 4;
        public int TaskNumber => 1;
        public object Execute()
        {
            var guards = Day4Helper.GetGuards();

            var guardWithLongestSleepTime = guards.OrderByDescending(x => x.TotalSleepTime).First();

            var minuteGroups = guardWithLongestSleepTime
                .GetSleepMinuteSets()
                .SelectMany(x => x)
                .GroupBy(g => g)
                .ToList();

            int maxKey = minuteGroups.Max(g => g.Count());
            var minute = minuteGroups.First(x => x.Count() == maxKey);

            return guardWithLongestSleepTime.GuardId * minute.Key;
        }
    }

    public class Day4Task2 : ITask
    {
        public int Day => 4;
        public int TaskNumber => 2;
        public object Execute()
        {
            var guards = Day4Helper.GetGuards();

            var maxMinuteList = new List<(Guard Guard, int Count, int Minute)>();

            foreach (var guard in guards)
            {
                var minuteGroups = guard.GetSleepMinuteSets()
                    .SelectMany(x => x)
                    .GroupBy(g => g)
                    .ToList();

                if (minuteGroups.Any())
                {
                    int maxKey = minuteGroups.Max(g => g.Count());
                    maxMinuteList.Add((guard, maxKey, minuteGroups.First(x => x.Count() == maxKey).Key));
                }
            }

            var selectedGuard = maxMinuteList.OrderByDescending(x => x.Count).First();

            return selectedGuard.Guard.GuardId * selectedGuard.Minute;
        }
    }

    internal class GuardEvent
    {
        public DateTime DateStamp { get; }

        public string Event { get; }

        public bool IsWakeUpEvent => Event.Equals("wakes up", StringComparison.CurrentCultureIgnoreCase);

        public bool IsSleepEvent => Event.Equals("falls asleep", StringComparison.CurrentCultureIgnoreCase);

        public bool IsShiftEvent => !IsWakeUpEvent && !IsSleepEvent;

        private GuardEvent(string @event, DateTime dateStamp)
        {
            Event = @event;
            DateStamp = dateStamp;
        }

        public static GuardEvent Parse(string line)
        {
            var match = Regex.Match(line, @"^\[(\d+)-(\d+)-(\d+) (\d+):(\d+)\] (.*)$");
            return new GuardEvent(
                match.Groups[6].Value, 
                new DateTime(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value),
                    int.Parse(match.Groups[5].Value),
                    0
                    ));
        }
    }

    internal sealed class Guard
    {
        public int GuardId { get; }

        public List<(DateTime SleepTime, DateTime WakeTime)> SleepTimes { get; }

        public double TotalSleepTime => SleepTimes.Sum(x => x.WakeTime.Subtract(x.SleepTime).TotalMinutes);

        public Guard(int guardId)
        {
            GuardId = guardId;
            SleepTimes = new List<(DateTime SleepTime, DateTime WakeTime)>();
        }

        public IEnumerable<List<int>> GetSleepMinuteSets()
        {
            foreach (var sleepTime in SleepTimes)
            {
                var sleepMinute = sleepTime.SleepTime.Minute;
                var duration = sleepTime.WakeTime.Subtract(sleepTime.SleepTime).TotalMinutes;

                var list = new List<int>();

                for (int i = 0; i < duration; i++)
                {
                    var minute = sleepMinute++;
                    if (minute > 59)
                    {
                        minute = 0;
                    }
                    list.Add(minute);
                }

                yield return list;
            }
        }
    }

    internal static class Day4Helper
    {
        public static List<Guard> GetGuards()
        {
            var events = new List<GuardEvent>();
            foreach (var line in Manifest.Day4.Split("\r\n"))
            {
                events.Add(GuardEvent.Parse(line));
            }

            var guards = new List<Guard>();
            var orderedEvents = events.OrderBy(e => e.DateStamp).ToArray();

            Guard guard = null;

            DateTime controlDate = DateTime.Today;
            DateTime sleepTime = controlDate;

            for (int i = 0; i < orderedEvents.Length; i++)
            {
                var guardEvent = orderedEvents[i];
                if (guardEvent.IsShiftEvent)
                {
                    var guardId = int.Parse(Regex.Match(guardEvent.Event, @"^Guard #(\d+) begins shift$").Groups[1].Value);
                    guard = guards.SingleOrDefault(x => x.GuardId == guardId) ?? NewGuard(guardId);
                }
                else if (orderedEvents[i].IsSleepEvent)
                {
                    sleepTime = orderedEvents[i].DateStamp;
                }
                else if (orderedEvents[i].IsWakeUpEvent)
                {
                    if (sleepTime != controlDate)
                    {
                        guard?.SleepTimes.Add((sleepTime, orderedEvents[i].DateStamp));
                        sleepTime = controlDate;
                    }
                }
            }

            return guards;

            Guard NewGuard(int guardId)
            {
                var g = new Guard(guardId);
                guards.Add(g);
                return g;
            }
        }
    }
}
