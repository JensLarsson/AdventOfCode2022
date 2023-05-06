

using System.Text.RegularExpressions;

struct Sensor
{
    public Vector2 Position { get; init; }
    public Vector2 ClosestBeacon { get; init; }
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
        //parse data
        string[] input = File.ReadAllLines(args[0]);
        IEnumerable<Sensor> sensors = GetSensorData(input);

        //find bounds
        Vector2 topLeft = new Vector2(int.MaxValue, int.MaxValue);
        Vector2 bottomRight = new Vector2(int.MinValue, int.MinValue);
        foreach (Sensor sensor in sensors)
        {
            topLeft.X = Math.Min(topLeft.X, sensor.Position.X);
            topLeft.X = Math.Min(topLeft.X, sensor.ClosestBeacon.X);
            topLeft.Y = Math.Min(topLeft.Y, sensor.Position.Y);
            topLeft.Y = Math.Min(topLeft.Y, sensor.ClosestBeacon.Y);
            bottomRight.X = Math.Max(bottomRight.X, sensor.Position.X);
            bottomRight.X = Math.Max(bottomRight.X, sensor.ClosestBeacon.X);
            bottomRight.Y = Math.Max(bottomRight.Y, sensor.Position.Y);
            bottomRight.Y = Math.Max(bottomRight.Y, sensor.ClosestBeacon.Y);
        }
        string[] map = new string[bottomRight.Y - topLeft.Y + 1];
        //fill map with '.' for width
        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new string('.', bottomRight.X - topLeft.X + 1);
        }
        foreach (Sensor sensor in sensors)
        {
            Vector2 pos = sensor.Position - topLeft;
            Vector2 beacon = sensor.ClosestBeacon - topLeft;
            int distance = Vector2.GetManhattanDistance(pos, beacon);

            //Manhattan distance loop
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    Vector2 current = new Vector2(x, y);
                    int currentDistance = Vector2.GetManhattanDistance(current, pos);
                    if (currentDistance <= distance)
                    {
                        map[y] = map[y].Remove(x, 1).Insert(x, "#");
                    }
                }
            }

            map[pos.Y] = map[pos.Y].Remove(pos.X, 1).Insert(pos.X, "S");
            map[beacon.Y] = map[beacon.Y].Remove(beacon.X, 1).Insert(beacon.X, "B");
        }

        //print map
        foreach (string line in map)
        {
            Console.WriteLine(line);
        }

        int count = map[10].Count(c => c == '#');
        Console.WriteLine($"Part 1: {count}");
    }

    static IEnumerable<Sensor> GetSensorData(string[] lines)
    {
        string pattern = @"-?\d+";
        foreach (string line in lines)
        {
            MatchCollection matches = Regex.Matches(line, pattern);
            int x = int.Parse(matches[0].Value);
            int y = int.Parse(matches[1].Value);
            int vx = int.Parse(matches[2].Value);
            int vy = int.Parse(matches[3].Value);
            yield return new Sensor
            {
                Position = new Vector2(x, y),
                ClosestBeacon = new Vector2(vx, vy)
            };
        }
    }
}