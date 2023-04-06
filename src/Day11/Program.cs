class Monkey : IComparable<Monkey>
{
    public List<long?> Items { get; set; } = new List<long?>();
    public Func<long, long> Operation { get; init; }
    public int TestInt { get; init; }
    public int OnTrueTarget { get; init; }
    public int OnFalseTarget { get; init; }
    public int ItemsInspected { get; set; } = 0;

    public int CompareTo(Monkey? other)
    {
        if (other is null)
        {
            return 1;
        }
        return ItemsInspected.CompareTo(other.ItemsInspected);
    }
}


class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: provide input file in form of txt file as arg");
            return;
        }
        var input = File.ReadAllLines(args[0]);

        List<Monkey> monkeys = new List<Monkey>();
        for (int i = 0; i < input.Length; i += 7)
        {
            monkeys.Add(new Monkey
            {
                Items = input[i + 1].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => long.TryParse(x, out long y) ? y : (long?)null)
                .Where(x => x.HasValue).ToList(),

                Operation = GetOperation(input[i + 2]),
                TestInt = int.Parse(input[i + 3].Split(' ')[^1]),
                OnTrueTarget = int.Parse(input[i + 4].Split(' ')[^1]),
                OnFalseTarget = int.Parse(input[i + 5].Split(' ')[^1])
            });
        }
        for (int round = 0; round < 20; round++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (long item in monkey.Items)
                {
                    monkey.ItemsInspected++;
                    long newItemScore = monkey.Operation(item) / 3;     //floored division through long
                    if (newItemScore % monkey.TestInt == 0)
                    {
                        monkeys[monkey.OnTrueTarget].Items.Add(newItemScore);
                    }
                    else
                    {
                        monkeys[monkey.OnFalseTarget].Items.Add(newItemScore);
                    }
                }
                monkey.Items = new List<long?>();
            }
        }
        monkeys.Sort();
        Console.WriteLine($"monkey business after 20 rounds: {monkeys[^1].ItemsInspected * monkeys[^2].ItemsInspected}");
    }

    static Func<long, long> GetOperation(string instruction)
    {
        var instructions = instruction.Split(' ');
        return instructions[^2] switch
        {
            "+" => x => x + (long.TryParse(instructions[^1], out long y) ? y : x),
            "*" => x => x * (long.TryParse(instructions[^1], out long y) ? y : x),
            _ => throw new ArgumentException("Invalid instruction")
        };
    }
}