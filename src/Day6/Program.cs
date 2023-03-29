if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string[] input = File.ReadAllLines(args[0]);

foreach (string line in input)
{
    Console.WriteLine("Part One: " + FindFirstNonRepeatingChar(line, 4));
    Console.WriteLine("Part Two: " + FindFirstNonRepeatingChar(line, 14));
}


int FindFirstNonRepeatingChar(string input, int testLength)
{
    for (int i = testLength - 1; i < input.Length; i++)
    {
        bool hasDuplicate = false;
        for (int ii = testLength - 2; ii >= 0; ii--)
        {
            char testChar = input[i - ii - 1];
            string testSpace = input[(i - ii)..(i + 1)].ToString();
            hasDuplicate = testSpace.Contains(testChar);
            if (hasDuplicate)
            {
                break;
            }
        }
        if (!hasDuplicate)
        {
            return i + 1;
        }
    }
    throw new Exception($"No non-repeating char found withing test length {testLength} in input {input}");
}