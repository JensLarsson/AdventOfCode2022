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

        Vector2 botLeft = new Vector2(int.MaxValue, int.MaxValue);
        Vector2 topRight = new Vector2(0, 0);

        foreach (IEnumerable<Vector2> line in lines)
        {
            foreach (Vector2 point in line)
            {
                botLeft.X = Math.Min(point.X, botLeft.X);
                botLeft.Y = Math.Min(point.Y, botLeft.Y);
                topRight.X = Math.Max(point.X, topRight.X);
                topRight.Y = Math.Max(point.Y, topRight.Y);
            }
        }

        bool[,] map = new bool[topRight.X - botLeft.X, topRight.Y - botLeft.Y];
        foreach (IEnumerable<Vector2> line in lines)
        {
            foreach (Vector2 point in line)
            {
                map[point.X, point.Y] = true;
            }
        }
    }
}