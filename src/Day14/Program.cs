class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: provide input file in form of txt file as arg");
            return;
        }

        string[] input = File.ReadAllLines(args[0]);

        Vector2 sandHole = new Vector2(500, 0);

        IEnumerable<List<Vector2>> lines = input
            .Select(s => s.Split(" -> ")                                //String array for each line
                .Select(xyString => new Vector2(xyString)).ToList());   //cast string array to vector2 list

        Vector2 topLeft = new Vector2(int.MaxValue, int.MaxValue);
        Vector2 botRight = new Vector2(0, 0);


        topLeft.X = Math.Min(sandHole.X, topLeft.X);
        topLeft.Y = Math.Min(sandHole.Y, topLeft.Y);
        botRight.X = Math.Max(sandHole.X, botRight.X);
        botRight.Y = Math.Max(sandHole.Y, botRight.Y);

        foreach (List<Vector2> line in lines)
        {
            foreach (Vector2 point in line)
            {
                topLeft.X = Math.Min(point.X, topLeft.X);
                topLeft.Y = Math.Min(point.Y, topLeft.Y);
                botRight.X = Math.Max(point.X, botRight.X);
                botRight.Y = Math.Max(point.Y, botRight.Y);
            }
        }

        bool[,] part1Map = new bool[botRight.X - topLeft.X + 1, botRight.Y - topLeft.Y + 1];

        foreach (var line in lines)
        {
            FillLinesInMap(line, topLeft, ref part1Map);
        }

        int countPart1 = FillWithSand(sandHole - topLeft, part1Map);
        PrintMap(part1Map);
        Console.WriteLine(countPart1);
    }

    private static int FillWithSand(Vector2 sandHole, bool[,] map)
    {
        Vector2 sandPos = sandHole;
        int count = 0;
        while (true)
        {
            if (sandPos.Y == map.GetLength(1) - 1)
            {
                break;
            }
            if (!map[sandPos.X, sandPos.Y + 1])
            {
                sandPos.Y += 1;
                continue;
            }
            if (sandPos.X == 0)
            {
                break;
            }
            if (!map[sandPos.X - 1, sandPos.Y + 1])
            {
                sandPos.X -= 1;
                sandPos.Y += 1;
                continue;
            }
            if (sandPos.X == map.GetLength(0) - 1)
            {
                break;
            }
            if (!map[sandPos.X + 1, sandPos.Y + 1])
            {
                sandPos.X += 1;
                sandPos.Y += 1;
                continue;
            }
            map[sandPos.X, sandPos.Y] = true;
            sandPos = sandHole;
            count++;
        }
        return count;
    }

    private static void PrintMap(bool[,] map)
    {
        int cols = map.GetLength(0);
        int rows = map.GetLength(1);
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (map[x, y])
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    private static void FillLinesInMap(List<Vector2> edges, Vector2 offset, ref bool[,] map)
    {
        for (int i = 1; i < edges.Count; i += 1)
        {
            Vector2 start = edges[i - 1];
            Vector2 end = edges[i];

            if (start.X == end.X) // Vertical line
            {
                int x = start.X;
                int y1 = Math.Min(start.Y, end.Y);
                int y2 = Math.Max(start.Y, end.Y);

                for (int y = y1; y <= y2; y++)
                {
                    map[x - offset.X, y - offset.Y] = true;
                }
            }
            else if (start.Y == end.Y) // Horizontal line
            {
                int y = start.Y;
                int x1 = Math.Min(start.X, end.X);
                int x2 = Math.Max(start.X, end.X);

                for (int x = x1; x <= x2; x++)
                {
                    map[x - offset.X, y - offset.Y] = true;
                }
            }
        }
    }
}