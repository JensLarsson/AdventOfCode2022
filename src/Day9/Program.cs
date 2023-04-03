if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

int size = 1000;
int snakeLength = 10;
bool[,] visitationMap = new bool[size, size];

int[] snakeX = Enumerable.Range(0, snakeLength).Select(i => size / 2).ToArray();
int[] snakeY = Enumerable.Range(0, snakeLength).Select(i => size / 2).ToArray();


foreach (string line in File.ReadAllLines(args[0])) //for each instruction
{
    string[] instructions = line.Split(' ');
    int steps = int.Parse(instructions[1]);         //How many moves instructed
    for (int step = 0; step < steps; step++)
    {
        //move head
        switch (instructions[0])
        {
            case "L":
                snakeX[0]--;
                break;
            case "R":
                snakeX[0]++;
                break;
            case "U":
                snakeY[0]++;
                break;
            case "D":
                snakeY[0]--;
                break;
        }
        //move body
        for (int i = 1; i < snakeX.Length; i++)
        {
            int directionX = snakeX[i - 1] - snakeX[i];
            int directionY = snakeY[i - 1] - snakeY[i];

            int absX = Math.Abs(directionX);
            int absY = Math.Abs(directionY);
            if (absX > 1 || absY > 1)                   //only move i part is 2 or more spaces away
            {
                if (directionX != 0)
                {
                    snakeX[i] += directionX / absX;
                }
                if (directionY != 0)
                {
                    snakeY[i] += directionY / absY;
                }
            }
            visitationMap[snakeY[^1], snakeX[^1]] = true; //use position of last part of tail to mark visited
        }
    }
}

Console.WriteLine(visitationMap.Cast<bool>().Count(b => b));