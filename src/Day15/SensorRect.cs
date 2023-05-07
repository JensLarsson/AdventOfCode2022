class SensorRect
{
    public List<Sensor> Sensors { get; init; }
    private Vector2 _topLeft;
    private Vector2 _bottomRight;
    private bool[][] _takenLocationsMap;


    public Vector2 TopLeft => _topLeft;
    public Vector2 BottomRight => _bottomRight;

    public SensorRect(Sensor sensor)
    {
        Vector2 center = sensor.Position;
        int manhattanDistance = Vector2.GetManhattanDistance(sensor.Position, sensor.ClosestBeacon);
        Sensors = new List<Sensor> { sensor };
        Vector2 offset = new Vector2(manhattanDistance, manhattanDistance);
        _topLeft = center - offset;
        _bottomRight = center + offset;
        int size = manhattanDistance * 2 + 1;

        _takenLocationsMap = new bool[size][];
        for (int x = 0; x < size; x++)
        {
            _takenLocationsMap[x] = new bool[size];
            for (int y = 0; y < size; y++)
            {
                _takenLocationsMap[x][y] = Vector2.GetManhattanDistance(center, (new Vector2(x, y) + _topLeft)) <= manhattanDistance;
            }
        }
    }
    public SensorRect(Sensor sensor, int index)
    {
        Vector2 center = sensor.Position;
        int manhattanDistance = Vector2.GetManhattanDistance(sensor.Position, sensor.ClosestBeacon);
        Sensors = new List<Sensor> { sensor };
        Vector2 offset = new Vector2(manhattanDistance, manhattanDistance);
        _topLeft = center - offset;
        _topLeft.Y = index;
        _bottomRight = center + offset;
        _bottomRight.Y = index;
        int size = manhattanDistance * 2 + 1;

        _takenLocationsMap = new bool[size][];
        for (int x = 0; x < size; x++)
        {
            _takenLocationsMap[x] = new bool[1];
            _takenLocationsMap[x][0] = Vector2.GetManhattanDistance(center, (new Vector2(x, 0) + _topLeft)) <= manhattanDistance;
        }
    }

    public SensorRect(IEnumerable<SensorRect> rects)
    {
        Sensors = rects.SelectMany(r => r.Sensors).ToList();
        _topLeft = new Vector2(int.MaxValue, int.MaxValue);
        _bottomRight = new Vector2(int.MinValue, int.MinValue);
        foreach (SensorRect rect in rects)
        {
            _topLeft.X = Math.Min(_topLeft.X, rect._topLeft.X);
            _topLeft.Y = Math.Min(_topLeft.Y, rect._topLeft.Y);
            _bottomRight.X = Math.Max(_bottomRight.X, rect.BottomRight.X);
            _bottomRight.Y = Math.Max(_bottomRight.Y, rect.BottomRight.Y);
        }
        _takenLocationsMap = new bool[_bottomRight.X - _topLeft.X + 1][];
        for (int x = 0; x < _takenLocationsMap.Length; x++)
        {
            _takenLocationsMap[x] = new bool[_bottomRight.Y - _topLeft.Y + 1];
        }
        foreach (SensorRect rect in rects)
        {
            for (int x = 0; x < rect._takenLocationsMap.Length; x++)
            {
                for (int y = 0; y < rect._takenLocationsMap[x].Length; y++)
                {
                    _takenLocationsMap[x + rect._topLeft.X - _topLeft.X][y + rect._topLeft.Y - _topLeft.Y] |= rect._takenLocationsMap[x][y];
                }
            }
        }
    }

    public int CountBlockedLocationsAtY(int y)
    {
        int count = 0;
        for (int x = 0; x < _takenLocationsMap.Length; x++)
        {
            if (Sensors.Any(s => s.Position == new Vector2(x + _topLeft.X, y)))
            {
                continue;
            }
            if (Sensors.Any(s => s.ClosestBeacon == new Vector2(x + _topLeft.X, y)))
            {
                continue;
            }
            if (_takenLocationsMap[x][y - _topLeft.Y])
            {
                count++;
            }
        }
        return count;
    }

    public void Print()
    {
        for (int y = 0; y < _takenLocationsMap[0].Length; y++)
        {
            Console.Write($"{y + _topLeft.Y}: ".PadRight(4));
            for (int x = 0; x < _takenLocationsMap.Length; x++)
            {
                char c = _takenLocationsMap[x][y] ? '#' : '.';
                if (Sensors.Any(s => s.Position == new Vector2(x, y) + _topLeft))
                {
                    c = 'S';
                }
                else if (Sensors.Any(s => s.ClosestBeacon == new Vector2(x, y) + _topLeft))
                {
                    c = 'B';
                }
                Console.Write(c);
            }
            Console.WriteLine();
        }
    }


    public static bool Overlap(SensorRect a, SensorRect b)
    {
        return a._topLeft.X < b.BottomRight.X &&
               a.BottomRight.X > b._topLeft.X &&
               a._topLeft.Y < b.BottomRight.Y &&
               a.BottomRight.Y > b._topLeft.Y;
    }
    public static bool OverlapX(SensorRect a, int x)
    {
        return a._topLeft.X < x && a.BottomRight.X > x;
    }
    public static bool OverlapY(SensorRect a, int y)
    {
        return a._topLeft.Y < y && a.BottomRight.Y > y;
    }
}