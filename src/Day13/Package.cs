using System.Text.Json;
class Package : IComparable<Package>, IEquatable<Package>
{
    public JsonDocument Signal { get; init; }
    public string SignalString { get; init; }
    public Package(string signal)
    {
        this.Signal = JsonDocument.Parse(signal);
        this.SignalString = signal;
    }

    public int CompareTo(Package? other)
    {
        int leftLenght = Signal.RootElement.GetArrayLength();
        int rightLenght = other.Signal.RootElement.GetArrayLength();
        int shortestLength = Math.Min(leftLenght, rightLenght);
        for (int i = 0; i < shortestLength; i++)
        {
            JsonElement leftProperty = Signal.RootElement[i];
            JsonElement rightProperty = other.Signal.RootElement[i];

            // If either property is an array, we need to compare the arrays
            if (leftProperty.ValueKind == JsonValueKind.Array || rightProperty.ValueKind == JsonValueKind.Array)
            {
                bool leftIsNumber = leftProperty.ValueKind == JsonValueKind.Number;
                bool IsRightNumber = rightProperty.ValueKind == JsonValueKind.Number;
                Package left = new Package(leftIsNumber ? "[" + leftProperty.ToString() + "]" : leftProperty.ToString());
                Package right = new Package(IsRightNumber ? "[" + rightProperty.ToString() + "]" : rightProperty.ToString());

                int result = left.CompareTo(right);
                if (result == 0)
                {
                    continue;
                }
                return result;
            }

            // If both properties are numbers, we can compare the numbers
            int compare = leftProperty.GetInt32().CompareTo(rightProperty.GetInt32());
            if (compare == 0)
            {
                continue;
            }
            return compare;
        }
        // If we get here, we have compared all the properties and they are all equal
        return leftLenght.CompareTo(rightLenght);
    }

    public bool Equals(Package? other)
    {
        return this.SignalString.Equals(other?.SignalString);
    }
}