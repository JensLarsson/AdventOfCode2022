if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

var lines = File.ReadAllLines(args[0]);

int itemPriority = 0;
int badgePriority = 0;
for (int i = 0; i < lines.Length; i++)
{
    // Part 1
    int halfLenght = lines[i].Length / 2;
    string firstHalf = lines[i].Substring(0, halfLenght);
    string secondHalf = lines[i].Substring(halfLenght, halfLenght);
    foreach (char c in firstHalf)
    {
        if (secondHalf.Contains(c))
        {
            itemPriority += char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
            break;
        }
    }

    if (i % 3 != 0) continue; // Skip every other line (3 lines per group

    // Part 2
    foreach (char c in lines[i])
    {
        if (lines[i + 1].Contains(c) && lines[i + 2].Contains(c))
        {
            badgePriority += char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
            break;
        }
    }
}
Console.WriteLine("Total Item Priority: " + itemPriority);
Console.WriteLine("Total Badge Priority: " + badgePriority);