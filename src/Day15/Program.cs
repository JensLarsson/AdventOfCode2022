

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
        Console.WriteLine($"Top left: {topLeft}");
        Console.WriteLine($"Bottom right: {bottomRight}");

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