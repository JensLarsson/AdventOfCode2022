

if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string inputPath = args[0];
string input = File.ReadAllText(inputPath);

int totalPart1Score = 0;
int totalPart2Score = 0;

foreach (string line in input.Split("\n"))
{
    totalPart1Score += GetPickScore(line[^1]);
    totalPart1Score += GetGameScore(line[0], line[^1]);
    char shape = GetShape(line[0], line[^1]);
    totalPart2Score += GetPickScore(shape);
    totalPart2Score += GetGameScore(line[0], shape);
}

Console.WriteLine($"Part1: {totalPart1Score}");
Console.WriteLine($"Part2: {totalPart2Score}");


int GetPickScore(char c) => c switch
{
    'X' => 1,
    'Y' => 2,
    'Z' => 3,
    _ => 0
};
int GetGameScore(char a, char b) => a switch
{
    'A' =>
        b switch
        {
            'X' => 3,
            'Y' => 6,
            'Z' => 0,
            _ => 0
        },
    'B' =>
        b switch
        {
            'X' => 0,
            'Y' => 3,
            'Z' => 6,
            _ => 0
        },
    'C' =>
        b switch
        {
            'X' => 6,
            'Y' => 0,
            'Z' => 3,
            _ => 0
        },
    _ => 0,
};
char GetShape(char a, char b) => a switch
{
    'A' =>
        b switch
        {
            'X' => 'Z',
            'Y' => 'X',
            'Z' => 'Y',
            _ => throw new NotImplementedException()
        },
    'B' =>
        b switch
        {
            'X' => 'X',
            'Y' => 'Y',
            'Z' => 'Z',
            _ => throw new NotImplementedException()
        },
    'C' =>
        b switch
        {
            'X' => 'Y',
            'Y' => 'Z',
            'Z' => 'X',
            _ => throw new NotImplementedException()
        },
    _ => throw new NotImplementedException()
};