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

        Vector2 botLeft = new Vector2(int.MaxValue, int.MaxValue);
        Vector2 topRight = new Vector2(0, 0);


        botLeft.X = Math.Min(sandHole.X, botLeft.X);
        botLeft.Y = Math.Min(sandHole.Y, botLeft.Y);
        topRight.X = Math.Max(sandHole.X, topRight.X);
        topRight.Y = Math.Max(sandHole.Y, topRight.Y);

        foreach (List<Vector2> line in lines)
        {
            foreach (Vector2 point in line)
            {
                botLeft.X = Math.Min(point.X, botLeft.X);
                botLeft.Y = Math.Min(point.Y, botLeft.Y);
                topRight.X = Math.Max(point.X, topRight.X);
                topRight.Y = Math.Max(point.Y, topRight.Y);
            }
        }

        bool[,] map = new bool[topRight.X - botLeft.X + 1, topRight.Y - botLeft.Y + 1];

        foreach (var line in lines)
        {
            FillLinesInMap(line, botLeft, ref map);
        }






        PrintMap(map);
    }

    public static void PrintMap(bool[,] map)
    {
        int rows = map.GetLength(1);
        int cols = map.GetLength(0);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[j, i])
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

    public static void FillLinesInMap(List<Vector2> edges, Vector2 offset, ref bool[,] map)
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