if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

var lines = File.ReadAllLines(args[0]);
int encapsulations = 0;
int overlaps = 0;
for (int i = 0; i < lines.Length; i++)
{
    var ranges = lines[i].Split(',', '-').Select(int.Parse).ToArray();
    if (IsEncapsulated(ranges[0], ranges[1], ranges[2], ranges[3]))
    {
        encapsulations++;
    }
    if (IsOverlapping(ranges[0], ranges[1], ranges[2], ranges[3])
        || IsEncapsulated(ranges[0], ranges[1], ranges[2], ranges[3]))
    {
        overlaps++;
    }
}
Console.WriteLine("encapsulated pairs: " + encapsulations);
Console.WriteLine("overlaps: " + overlaps);


bool IsOverlapping(int a1, int a2, int b1, int b2)
{
    return (a1 <= b1 && a2 >= b1) || (a1 <= b2 && a2 >= b2);
}
bool IsEncapsulated(int a1, int a2, int b1, int b2)
{
    return (a1 <= b1 && a2 >= b2) || (b1 <= a1 && b2 >= a2);
}