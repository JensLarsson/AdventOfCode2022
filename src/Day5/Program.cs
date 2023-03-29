if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string[] input = File.ReadAllLines(args[0]);
int instructionStartIndex = 0;
for (int i = 0; i < input.Length; i++)          //Find the start of the instructions
{
    if (input[i].Contains("move"))
    {
        instructionStartIndex = i;
        break;
    }
}

//seperate the stacks and instructions
string[] stackStrings = input[..(instructionStartIndex - 2)].Reverse().ToArray(); //Reverse to fill bottom up later
string[] instructions = input[instructionStartIndex..];

int stackCount = input[instructionStartIndex - 2].Max<char>() - '0';

Stack<char>[] boxStacks = new Stack<char>[stackCount];
boxStacks = boxStacks.Select(_ => new Stack<char>()).ToArray();

foreach (string stackString in stackStrings)        //fill the stacks
{
    string rowString = stackString.Substring(1);
    for (int i = 0; i < rowString.Length; i += 4)
    {
        if (rowString[i] != ' ')
        {
            boxStacks[i / 4].Push(rowString[i]);
        }
    }
}
//copy the stacks for part 2
List<char>[] boxStackLists = boxStacks.Select(stack => stack.Reverse().ToList()).ToArray();

foreach (string line in instructions)
{
    List<int> argNumbers = line.Split(' ')          //get the numbers from the instructions
        .Where(s => int.TryParse(s, out _))
        .Select(int.Parse)
        .ToList();
    int moves = argNumbers[0];                      //the number of boxes to move

    //Part 1
    for (int i = 0; i < moves; i++)
    {
        boxStacks[argNumbers[2] - 1].Push(boxStacks[argNumbers[1] - 1].Pop());
    }
    //Part2
    var sourceList = boxStackLists[argNumbers[1] - 1];
    boxStackLists[argNumbers[2] - 1].AddRange(sourceList.GetRange(sourceList.Count - moves, moves));
    sourceList.RemoveRange(sourceList.Count - moves, moves);
}


Console.WriteLine(String.Concat(boxStacks.Select(stack => stack.Peek())));
Console.WriteLine(String.Concat(boxStackLists.Select(stack => stack.Last())));
