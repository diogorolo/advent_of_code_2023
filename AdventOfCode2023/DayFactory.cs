namespace AdventOfCode2023.Day1;

public class DayFactory : IDayFactory {
    private readonly IEnumerable<IDayComputerStrategy> _strategies;

    public DayFactory(IEnumerable<IDayComputerStrategy> factories) {
        this._strategies = factories ?? throw new ArgumentNullException(nameof(factories));
    }

    public IDayComputerStrategy GetStrategy(int day, int part) {
        var factory = _strategies
            .Single(x => x.AppliesTo(day, part));
        return factory;
    }
}