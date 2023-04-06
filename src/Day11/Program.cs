using System.Diagnostics;
using System.Numerics;

class Monkey : IComparable<Monkey>
{
    public List<BigInteger> Items { get; set; } = new List<BigInteger>();
    public Func<BigInteger, BigInteger> Operation { get; init; }
    public ulong TestInt { get; init; }
    public int OnTrueTarget { get; init; }
    public int OnFalseTarget { get; init; }
    public ulong ItemsInspected { get; set; } = 0;

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
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: provide input file in form of txt file and number of rounds as second arg");
            return;
        }
        var input = File.ReadAllLines(args[0]);
        int rounds = int.Parse(args[1]);

        List<Monkey> monkeys = new List<Monkey>();
        for (int i = 0; i < input.Length; i += 7)
        {
            monkeys.Add(new Monkey
            {
                Items = input[i + 1].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => BigInteger.TryParse(x, out BigInteger y) ? y : (BigInteger?)null) //parse to ulong or null
                .Where(x => x.HasValue)                                         //filter out nulls
                .Select(x => x.Value).ToList(),                                 //unwrap optionals

                Operation = GetOperation(input[i + 2]),
                TestInt = ulong.Parse(input[i + 3].Split(' ')[^1]),
                OnTrueTarget = int.Parse(input[i + 4].Split(' ')[^1]),
                OnFalseTarget = int.Parse(input[i + 5].Split(' ')[^1])
            });
        }

        Stopwatch watch = new Stopwatch();
        watch.Start();
        for (int round = 0; round < rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.ItemsInspected += (ulong)monkey.Items.Count;
                Parallel.ForEach(monkey.Items, item =>
                {
                    BigInteger newItemScore = monkey.Operation(item) / 3; //divide by 3 and round down to whole number
                    if (newItemScore % monkey.TestInt == 0)
                    {
                        lock (monkeys[monkey.OnTrueTarget])
                            monkeys[monkey.OnTrueTarget].Items.Add(newItemScore);
                    }
                    else
                    {
                        lock (monkeys[monkey.OnFalseTarget])
                            monkeys[monkey.OnFalseTarget].Items.Add(newItemScore);
                    }
                });
                monkey.Items = new List<BigInteger>();
            }
        }
        watch.Stop();
        Console.WriteLine($"Took {watch.ElapsedMilliseconds}ms to run {rounds} rounds");
        foreach (var monkey in monkeys)
        {
            Console.WriteLine($"Monkey {monkeys.IndexOf(monkey)} inspected {monkey.ItemsInspected} items");
        }
        monkeys.Sort();
        Console.WriteLine($"monkey business after {rounds} rounds: {monkeys[^1].ItemsInspected * monkeys[^2].ItemsInspected}");
    }

    static Func<BigInteger, BigInteger> GetOperation(string instruction)
    {
        var instructions = instruction.Split(' ');
        return instructions[^2] switch
        {
            "+" => x => x + (BigInteger.TryParse(instructions[^1], out BigInteger y) ? y : x),    //assume that failed parse means use old value
            "*" => x => x * (BigInteger.TryParse(instructions[^1], out BigInteger y) ? y : x),    //assume that failed parse means use old value
            _ => throw new ArgumentException("Invalid instruction")
        };
    }
}