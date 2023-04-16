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
        IEnumerable<int> result = Enumerable.Range(0, packages.Count / 2)
            .Where(i => packages[2 * i].CompareTo(packages[2 * i + 1]) <= 0)        //Ignore packages of wrong order
            .Select(i => i + 1);                                                    //add 1 since the challenge is 1-based

        Console.WriteLine("part1:" + result.Sum());

        //Part 2
        List<Package> newPackages = (new[] { new Package("[[2]]"), new Package("[[6]]") }).ToList();
        packages.AddRange(newPackages);
        packages.Sort();

        IEnumerable<int> indexes = newPackages.Select(p => packages.IndexOf(p) + 1);//add 1 since the challenge is 1-based
        Console.WriteLine("part2: " + indexes.Aggregate((x, y) => x * y));
    }
}