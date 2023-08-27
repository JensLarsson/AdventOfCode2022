using System.Text.RegularExpressions;

struct Sensor
{
    public Vector2 Position { get; init; }
    public Vector2 ClosestBeacon { get; init; }
    public bool OverlapY(int y)
    {
        int distance = Vector2.GetManhattanDistance(Position, ClosestBeacon);
        return Math.Abs(Position.Y - y) <= distance;
    }
}
class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: provide input file in form of txt file as arg");
            return;
        }
        //parse data
        string[] input = File.ReadAllLines(args[0]);
        int lineToScan = int.Parse(args[1]);
        IEnumerable<Sensor> sensors = GetSensorData(input);

        IEnumerable<SensorRect> yOverlappingSensors = sensors
             .Where(s => s.OverlapY(lineToScan))
             .Select(s => new SensorRect(s, lineToScan));


        Console.WriteLine(yOverlappingSensors.Count());

        SensorRect overlappingRects = new SensorRect(yOverlappingSensors);

        //overlappingRects.Print();

        Console.WriteLine(overlappingRects.CountBlockedLocationsAtY(lineToScan));

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