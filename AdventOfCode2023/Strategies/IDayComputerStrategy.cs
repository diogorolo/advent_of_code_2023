namespace AdventOfCode2023.Day1;

public interface IDayComputerStrategy
{
    protected int SupportedDay { get; init; }
    protected IEnumerable<int> SupportedParts { get; init; }

    string Compute(string[] input, bool debug);

    public bool AppliesTo(int day, int part) {
        return day == SupportedDay && SupportedParts.Contains(part);
    }
}