struct Vector2
{
    public int X { get; set; }
    public int Y { get; set; }
    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Vector2(string vectorString)
    {
        var XY = vectorString.Split(',');
        X = int.Parse(XY[0]);
        Y = int.Parse(XY[1]);
    }

    public override string ToString()
    {
        return $"({X}:{Y})";
    }
}

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
        IEnumerable<IEnumerable<Vector2>> lines = input
            .Select(s => s.Split(" -> ")
                .Select(xyString => new Vector2(xyString)));

        Console.WriteLine(lines.Count());

        Vector2 botLeft = new Vector2(int.MaxValue, int.MaxValue);
        Vector2 topRight = new Vector2(0, 0);

        foreach (IEnumerable<Vector2> line in lines)
        {
            foreach (Vector2 point in line)
            {
                Console.WriteLine(point.ToString());
                botLeft.X = Math.Min(point.X, botLeft.X);
                botLeft.Y = Math.Min(point.Y, botLeft.Y);
                topRight.X = Math.Max(point.X, topRight.X);
                topRight.Y = Math.Max(point.Y, topRight.Y);
            }
            Console.WriteLine("-");
        }
        Console.WriteLine(botLeft.ToString());
        Console.WriteLine(topRight.ToString());

        bool[,] map = new bool[topRight.X - botLeft.X + 1, topRight.Y - botLeft.Y + 1];
        int rows = map.GetLength(1);
        int cols = map.GetLength(0);
        Console.WriteLine(rows);
        Console.WriteLine(cols);
        foreach (IEnumerable<Vector2> line in lines)
        {
            foreach (Vector2 point in line)
            {
                map[point.X - botLeft.X, point.Y - botLeft.Y] = true;
            }
        }


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
}