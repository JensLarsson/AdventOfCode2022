﻿if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string[] input = File.ReadAllLines(args[0]);
int instructionStartIndex = 0;
for (int i = 0; i < input.Length; i++) //Find the start of the instructions
{
    if (input[i].Contains("move"))
    {
        instructionStartIndex = i;
        break;
    }
}

//seperate the stacks and instructions
string[] stackStrings = input[..(instructionStartIndex - 2)];
string[] instructions = input[instructionStartIndex..];

int stackCount = input[instructionStartIndex - 2].Max<char>() - '0';

Stack<char>[] boxStacks = new Stack<char>[stackCount];
for (int i = 0; i < stackCount; i++) //initialize the stacks
{
    boxStacks[i] = new Stack<char>();
}

for (int i = stackStrings.Count() - 1; i >= 0; i--) //fill the stacks
{
    string rowString = stackStrings[i].Substring(1);
    for (int ii = 0; ii < rowString.Length; ii += 4)
    {
        if (rowString[ii] != ' ')
        {
            boxStacks[ii / 4].Push(rowString[ii]);
        }
    }
}
//copy the stacks for part 2
List<char>[] boxStackLists = boxStacks.Select(stack => stack.Reverse().ToList()).ToArray();

foreach (string line in instructions)
{
    string[] arguments = line.Split(' ');           //split the arguments in each instruction
    List<int> argNumbers = new List<int>();
    foreach (string arg in arguments)               //convert the arguments to numbers
    {
        if (int.TryParse(arg, out int argNumber))
        {
            argNumbers.Add(argNumber);
        }
    }
    int count = argNumbers[0];                      //the number of boxes to move

    //Part 1
    for (int i = 0; i < count; i++)
    {
        boxStacks[argNumbers[2] - 1].Push(boxStacks[argNumbers[1] - 1].Pop());
    }
    //Part2
    var sourceList = boxStackLists[argNumbers[1] - 1];
    boxStackLists[argNumbers[2] - 1].AddRange(sourceList.GetRange(sourceList.Count - count, count));
    sourceList.RemoveRange(sourceList.Count - count, count);
}


Console.WriteLine(String.Concat(boxStacks.Select(stack => stack.Peek())));
Console.WriteLine(String.Concat(boxStackLists.Select(stack => stack.Last())));
