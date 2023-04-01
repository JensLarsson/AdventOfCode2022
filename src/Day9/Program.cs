if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

int size = 1000;
bool[,] visitationMap = new bool[size, size];

int headX = size / 2;
int headY = size / 2;

int tailX = size / 2;
int tailY = size / 2;

foreach (string line in File.ReadAllLines(args[0]))
{
    string[] instructions = line.Split(' ');
    int steps = int.Parse(instructions[1]);
    for (int i = 0; i < steps; i++)
    {
        switch (instructions[0])
        {
            case "L":
                headX--;
                break;
            case "R":
                headX++;
                break;
            case "U":
                headY++;
                break;
            case "D":
                headY--;
                break;
        }
        int directionX = headX - tailX;
        int directionY = headY - tailY;

        int absX = Math.Abs(directionX);
        int absY = Math.Abs(directionY);
        if (absX > 1 || absY > 1)
        {
            if (directionX != 0)
                tailX += directionX / absX;
            if (directionY != 0)
                tailY += directionY / absY;
        }
        visitationMap[tailY, tailX] = true;
    }
}

Console.WriteLine(visitationMap.Cast<bool>().Count(b => b));