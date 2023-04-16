using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: dotnet run <input file>");
            return;
        }
        var input = File.ReadAllLines(args[0]);
        IEnumerable<int> result = GetRightOrderListIndexes(input);
        Console.WriteLine(result.Sum());
    }

    static IEnumerable<int> GetRightOrderListIndexes(string[] input)
    {
        for (int i = 0; i < input.Length; i += 3)
        {
            var left = JsonDocument.Parse(input[i]);
            var right = JsonDocument.Parse(input[i + 1]);
            if (IsRightOrder(left, right) != Order.Wrong) //treat undecided as correct
            {
                yield return (i + 3) / 3;
            }
        }
    }

    enum Order
    {
        Correct, Wrong, Undecided
    }
    static Order IsRightOrder(JsonDocument leftList, JsonDocument rightList)
    {
        int leftLenght = leftList.RootElement.GetArrayLength();
        int rightLenght = rightList.RootElement.GetArrayLength();
        int shortestLength = Math.Min(leftLenght, rightLenght);
        for (int i = 0; i < shortestLength; i++)
        {
            JsonElement leftProperty = leftList.RootElement[i];
            JsonElement rightProperty = rightList.RootElement[i];

            // If either property is an array, we need to compare the arrays
            if (leftProperty.ValueKind == JsonValueKind.Array || rightProperty.ValueKind == JsonValueKind.Array)
            {
                bool leftIsNumber = leftProperty.ValueKind == JsonValueKind.Number;
                bool IsRightNumber = rightProperty.ValueKind == JsonValueKind.Number;
                JsonDocument letfArray = JsonDocument.Parse(leftIsNumber ? "[" + leftProperty.ToString() + "]" : leftProperty.ToString());
                JsonDocument rightArray = JsonDocument.Parse(IsRightNumber ? "[" + rightProperty.ToString() + "]" : rightProperty.ToString());
                Order result = IsRightOrder(letfArray, rightArray);
                if (result == Order.Undecided)
                {
                    continue;
                }
                return result;
            }

            // If both properties are numbers, we can compare the numbers
            switch (leftProperty.GetInt32().CompareTo(rightProperty.GetInt32()))
            {
                case -1:
                    return Order.Correct;
                case 1:
                    return Order.Wrong;
                default:
                    continue;
            }
        }
        // If we get here, we have compared all the properties and they are all equal
        return (leftLenght.CompareTo(rightLenght)) switch
        {
            -1 => Order.Correct,
            1 => Order.Wrong,
            _ => Order.Undecided
        };
    }
}