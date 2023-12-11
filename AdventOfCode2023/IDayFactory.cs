namespace AdventOfCode2023.Day1;

public interface IDayFactory
{
    IDayComputerStrategy GetStrategy(int day, int part);
}