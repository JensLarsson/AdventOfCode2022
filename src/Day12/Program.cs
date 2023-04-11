class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: dotnet run <input file>");
            return;
        }
        string[] lines = File.ReadAllLines(args[0]);
        int rowCount = lines.Length;
        int columnCount = lines[0].Length;
        int[,] charArray = new int[rowCount, columnCount];
        GridPosition start = null;
        GridPosition goal = null;


        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (lines[i][j] == 'S')
                {
                    start = new GridPosition(i, j);
                    charArray[i, j] = 0;                //Start on 0
                    continue;
                }
                else if (lines[i][j] == 'E')
                {
                    goal = new GridPosition(i, j);
                    charArray[i, j] = 27;               //End on 27
                    continue;
                }
                charArray[i, j] = lines[i][j] - '`';    //Convert to int where 'a' is 1 and 'z' is 26
            }
        }
        if (start == null || goal == null)
        {
            Console.WriteLine("No start or end position found");
            return;
        }

        var path = AStar.FindPath(charArray, start, goal, 1);
        Console.WriteLine((path?.Count ?? 0) - 1);
    }
}