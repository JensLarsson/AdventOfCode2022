if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

var lines = File.ReadAllLines(args[0]);

int itemPriority = 0;
int badgePriority = 0;
foreach (string line in lines)
{
    // Part 1
    int halfLenght = line.Length / 2;
    string firstHalf = line.Substring(0, halfLenght);
    string secondHalf = line.Substring(halfLenght, halfLenght);
    foreach (char c in firstHalf)
    {
        if (secondHalf.Contains(c))
        {
            Console.WriteLine("Found Item: " + c);
            itemPriority += char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
            break;
        }
    }
}
for (int i = 0; i < lines.Length; i += 3)
{
    // Part 2
    foreach (char c in lines[i])
    {
        if (lines[i + 1].Contains(c) && lines[i + 2].Contains(c))
        {
            Console.WriteLine("Found Badge: " + c);
            badgePriority += char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
            break;
        }
    }
}
Console.WriteLine("Total Item Priority: " + itemPriority);
Console.WriteLine("Total Badge Priority: " + badgePriority);