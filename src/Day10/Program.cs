class Program
{
    static char[] outputString = new char[40];
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
                case "noop":        //no operation
                    instructionCount++;
                    totalSignalValue += HandleInstructionTick(registerValue, instructionCount);
                    break;
                case "addx":        //add x to register
                    instructionCount++;
                    totalSignalValue += HandleInstructionTick(registerValue, instructionCount);
                    instructionCount++;
                    registerValue += int.Parse(instructions[1]);
                    totalSignalValue += HandleInstructionTick(registerValue, instructionCount);
                    break;
            }
        }
        Console.WriteLine(totalSignalValue);
    }


    /// <summary>
    /// Returns the signal value if on a designated instruction(20, or every 40 after that), otherwise return 0
    /// </summary>
    static int HandleInstructionTick(int registerValue, int instructionCount)
    {
        int i = instructionCount % 40;
        outputString[i] = (i >= registerValue - 1 && i <= registerValue + 1) ? '#' : '.';

        if (instructionCount % 40 == 0)
        {
            Console.WriteLine(outputString);
        }

        if ((instructionCount - 20) % 40 == 0)
        {
            return registerValue * instructionCount;
        }
        return 0;
    }
}