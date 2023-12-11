
using System.Diagnostics;
using System.Reflection;
using AdventOfCode2023.Day1;
using AdventOfCode2023.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


static IHostBuilder CreateHostBuilder(params string[] args) {
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) => {
            services
                .AddSingleton<IDayFactory, DayFactory>();
            var rules = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && typeof(IDayComputerStrategy).IsAssignableFrom(x));

            foreach (var rule in rules)
            {
                services.Add(new ServiceDescriptor(typeof(IDayComputerStrategy), rule, ServiceLifetime.Scoped));
            }
        });
}

static string RunStrategy(IServiceProvider services, int day, int part, string[] input, bool debug) {
    using var serviceScope = services.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var factory = provider.GetRequiredService<IDayFactory>();

    return factory.GetStrategy(day, part).Compute(input, debug);
}

using var host = CreateHostBuilder(args).Build();


var day = 10;
var debug = false;
//IEnumerable<int> parts = new []{1};
IEnumerable<int> parts = new []{3};
//IEnumerable<int> parts = new []{2,3};
// IEnumerable<int> parts = new []{0,1,2,3};


foreach (var part in parts)
{
    var result = (await Loader.LoadDayResult(day, part))?.FirstOrDefault();
    var input = await Loader.LoadDay(day, part);
    Stopwatch watch = new Stopwatch();
    watch.Start();
    var computedResult = RunStrategy(host.Services, day, part, input, debug);
    watch.Stop();

    Console.WriteLine($"Computed result for day {day} part {part}: {computedResult}");
    Console.WriteLine($"Took: {watch.ElapsedMilliseconds}ms");
    if (result is not null)
    {
        Console.WriteLine($"Is expected: {computedResult == result}");
    }
}



