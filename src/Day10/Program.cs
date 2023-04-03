class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: provide input file in form of txt file as arg");
            return;
        }
        string[] lines = File.ReadAllLines(args[0]);

        int registerValue = 1;
        int instructionCount = 0;
        int totalSignalValue = 0;
        foreach (string line in lines)
        {
            string[] instructions = line.Split(' ');
            switch (instructions[0])
            {
                case "noop":
                    instructionCount++;
                    totalSignalValue += CheckAndGetRegisterSignalValue(registerValue, instructionCount);
                    break;
                case "addx":
                    instructionCount++;
                    totalSignalValue += CheckAndGetRegisterSignalValue(registerValue, instructionCount);
                    instructionCount++;
                    totalSignalValue += CheckAndGetRegisterSignalValue(registerValue, instructionCount);
                    registerValue += int.Parse(instructions[1]);
                    break;
            }
        }
        Console.WriteLine(totalSignalValue);
    }


    /// <summary>
    /// Returns the signal value if on a designated instruction(20, or every 40 after that), otherwise return 0
    /// </summary>
    static int CheckAndGetRegisterSignalValue(int registerValue, int instructionCount)
    {
        if ((instructionCount - 20) % 40 == 0)
        {
            return registerValue * instructionCount;
        }
        return 0;
    }
}