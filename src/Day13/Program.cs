class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: dotnet run <input file>");
            return;
        }
        var input = File.ReadAllLines(args[0]);
        List<Package> packages = input
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => new Package(line))
            .ToList();

        //Part 1
        IEnumerable<int> result = GetRightOrderListIndexes(packages);
        Console.WriteLine("part1:" + result.Sum());

        //Part 2
        List<Package> newPackages = (new[] { new Package("[[2]]"), new Package("[[6]]") }).ToList();
        packages.AddRange(newPackages);
        packages.Sort();

        //collect indexes of new packages
        //add 1 since the challenge is 1-based
        IEnumerable<int> indexes = newPackages.Select(p => packages.IndexOf(p) + 1);
        Console.WriteLine("part2: " + indexes.Aggregate((x, y) => x * y));

    }

    static IEnumerable<int> GetRightOrderListIndexes(List<Package> packages)
    {
        for (int i = 0; i < packages.Count; i += 2)
        {
            if (packages[i].CompareTo(packages[i + 1]) <= 0)
            {
                yield return (i + 2) / 2;
            }
        }
    }
}