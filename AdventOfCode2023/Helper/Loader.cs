namespace AdventOfCode2023.Helper;

public static class Loader
{
    public static async Task<string[]?> LoadDay(int day, int part)
    {
        var path = $"Day{day}/i{part}.txt";

        return await ReadAllLinesAsync(path);
    }

    public static async Task<string[]?> LoadDayResult(int day, int part)
    {
        var path = $"Day{day}/i{part}res.txt";
        return await ReadAllLinesAsync(path);
    }

    private static async Task<string[]?> ReadAllLinesAsync(string path)
    {
        if (File.Exists(path))
        {
            return await File.ReadAllLinesAsync(path);
        }

        return null;
    }
}