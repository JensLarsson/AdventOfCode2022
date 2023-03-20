if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

int GetShape(char c)
    => c < 'D' ? c - 'A' : c - 'X';

int GetGameScore(int oponent, int player)
    => (player - oponent + 4) % 3 * 3;

int GetShapeFromWinState(char a, char b)
    => (a - 'A' + b - 'X' + 2) % 3;

string[] lines = File.ReadAllLines(args[0]);
int totalPart1Score = lines
    .Sum(line => GetShape(line[^1]) + 1 + GetGameScore(GetShape(line[0]), GetShape(line[^1])));
int totalPart2Score = lines.
    Sum(line => GetShapeFromWinState(line[0], line[^1]) + 1 + GetGameScore(GetShape(line[0]), GetShapeFromWinState(line[0], line[^1])));

Console.WriteLine($"Part1: {totalPart1Score}");
Console.WriteLine($"Part2: {totalPart2Score}");