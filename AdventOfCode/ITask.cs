namespace AdventOfCode
{
    internal interface ITask
    {
        int Year { get; }

        int Day { get; }

        int TaskNumber { get; }

        object Execute();
    }
}
