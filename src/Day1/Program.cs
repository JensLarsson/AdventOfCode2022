using System.Linq;


if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string inputPath = args[0];
string input = File.ReadAllText(inputPath);

var containers = input.Split("\n\n")
    .Select(line => line.Split('\n', StringSplitOptions.RemoveEmptyEntries))
    .Select(container =>
    container.Select(item => int.TryParse(item, out int parsedVal) ? parsedVal : 0).Sum()).ToList();

containers.Sort();
Console.WriteLine(containers[^1] + containers[^2] + containers[^3]);