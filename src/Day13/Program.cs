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
        IEnumerable<int> result = GetRightOrderListIndexes(input);
        Console.WriteLine(result.Sum());
    }

    static IEnumerable<int> GetRightOrderListIndexes(string[] input)
    {
        for (int i = 0; i < input.Length; i += 3)
        {
            var left = new Package(input[i]);
            var right = new Package(input[i + 1]);
            if (left.CompareTo(right) <= 0)
            {
                yield return (i + 3) / 3;
            }
        }
    }
}