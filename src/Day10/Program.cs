using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: provide input file in form of txt file as arg");
            return;
        }
        var lines = File.ReadAllLines(args[0]);

        int registerValue = 1;
        int instructionCount = 0;

        int totalSignalValue = 0;
        StringBuilder output = new StringBuilder();

        foreach (string line in lines)
        {
            string[] instructions = line.Split(' ');
            //noop no operation or first operation of addx
            AddCharToOutputString(output, registerValue, instructionCount);
            instructionCount++;             //use instruction for operation
            totalSignalValue += GetSignalValue(registerValue, instructionCount);
            if (instructions[0] == "addx")  //add x to register
            {
                AddCharToOutputString(output, registerValue, instructionCount);
                instructionCount++;         //use another instruction for addx operation
                totalSignalValue += GetSignalValue(registerValue, instructionCount);
                registerValue += int.Parse(instructions[1]);
            }
        }
        Console.WriteLine(output.ToString());
        Console.WriteLine(totalSignalValue);
    }

    static void AddCharToOutputString(StringBuilder output, int registerValue, int instructionCount)
    {
        int i = instructionCount % 40;
        if (i == 0)
        {
            output.Append('\n');
        }
        output.Append((i >= registerValue - 1 && i <= registerValue + 1) ? '#' : '.');
    }

    /// <summary>
    /// Returns the signal value if on a designated instruction(20, or every 40 after that), otherwise return 0
    /// </summary>
    static int GetSignalValue(int registerValue, int instructionCount)
    {
        if ((instructionCount - 20) % 40 == 0)
        {
            return registerValue * instructionCount;
        }
        return 0;
    }
}