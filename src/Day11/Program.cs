class Item
{
    public List<int> ValuesPerMonkey { get; init; }
    public List<int> Divisors { get; init; }
    public Item(List<int> divisors, int startValue)
    {
        ValuesPerMonkey = new List<int>();
        Divisors = divisors;
        foreach (int divisor in divisors)
        {
            ValuesPerMonkey.Add(startValue % divisor);
        }
    }
    public void RunOperator(Func<int, int> operation)
    {
        for (int i = 0; i < ValuesPerMonkey.Count; i++)
        {
            ValuesPerMonkey[i] = operation(ValuesPerMonkey[i]) % Divisors.ElementAt(i);
        }
    }
}

class Monkey : IComparable<Monkey>
{
    public List<Item> Items { get; set; } = new List<Item>();
    public Func<int, int> Operation { get; init; }
    public int Index { get; init; }
    public int Divisor { get; init; }
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
        List<List<int>> itemValues = new List<List<int>>();
        for (int i = 0; i < input.Length; i += 7)
        {
            itemValues.Add(input[i + 1].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => int.TryParse(x, out _))
                .Select(x => int.Parse(x))
                .ToList());
            monkeys.Add(new Monkey
            {
                Index = int.Parse(input[i].Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries)[^1]),
                Operation = GetOperation(input[i + 2]),
                Divisor = int.Parse(input[i + 3].Split(' ')[^1]),
                OnTrueTarget = int.Parse(input[i + 4].Split(' ')[^1]),
                OnFalseTarget = int.Parse(input[i + 5].Split(' ')[^1])
            });
        }
        List<int> divisors = monkeys.Select(x => x.Divisor).ToList();
        for (int i = 0; i < monkeys.Count; i++)
        {   //create items for each monkey
            monkeys[i].Items = itemValues[i].Select(x => new Item(divisors, x)).ToList();
        }

        for (int round = 0; round < rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.ItemsInspected += (ulong)monkey.Items.Count;

                for (int i = 0; i < monkey.Items.Count; i++)
                {
                    monkey.Items[i].RunOperator(monkey.Operation);
                    int newItemScore = monkey.Items[i].ValuesPerMonkey[monkey.Index];
                    if ((newItemScore % monkey.Divisor) == 0)
                    {
                        monkeys[monkey.OnTrueTarget].Items.Add(monkey.Items[i]);
                    }
                    else
                    {
                        monkeys[monkey.OnFalseTarget].Items.Add(monkey.Items[i]);
                    }
                }
                // Clear the items list
                monkey.Items.Clear();
            }
        }

        foreach (var monkey in monkeys)
        {
            Console.WriteLine($"Monkey {monkeys.IndexOf(monkey)} inspected {monkey.ItemsInspected} items");
        }
        monkeys.Sort();
        Console.WriteLine($"monkey business after {rounds} rounds: {monkeys[^1].ItemsInspected * monkeys[^2].ItemsInspected}");
    }


    static Func<int, int> GetOperation(string instruction)
    {
        var instructions = instruction.Split(' ');
        bool useParsedVal = int.TryParse(instructions[^1], out int parseOut);
        return instructions[^2] switch
        {
            "+" => x => x + (useParsedVal ? parseOut : x),    //assume that failed parse means use old value
            "*" => x => x * (useParsedVal ? parseOut : x),    //assume that failed parse means use old value
            _ => throw new ArgumentException("Invalid instruction")
        };
    }
}