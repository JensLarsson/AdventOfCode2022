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
    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }

    public override string ToString()
    {
        return $"({X}:{Y})";
    }
}