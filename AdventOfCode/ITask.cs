namespace AdventOfCode
{
    internal interface ITask
    {
        int Day { get; }

        int TaskNumber { get; }

        object Execute();
    }
}
