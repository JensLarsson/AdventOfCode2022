public class Item
{
    public List<int> ValuesPerMonkey { get; init; }
    public List<int> Divisors { get; init; }
    public Item(List<int> divisors, int startValue)
    {
        ValuesPerMonkey = new List<int>();
        Divisors = divisors;
        foreach (int divisor in divisors)
        {
            ValuesPerMonkey.Add(startValue % divisor);
        }
    }
    public void RunOperator(Func<int, int> operation)
    {
        for (int i = 0; i < ValuesPerMonkey.Count; i++)
        {
            ValuesPerMonkey[i] = operation(ValuesPerMonkey[i]) % Divisors.ElementAt(i);
        }
    }
}