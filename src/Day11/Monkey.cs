public class Monkey : IComparable<Monkey>
{
    public List<Item> Items { get; set; } = new List<Item>();
    public Func<int, int> Operation { get; init; }
    public int Index { get; init; }
    public int Divisor { get; init; }
    public int OnTrueTarget { get; init; }
    public int OnFalseTarget { get; init; }
    public ulong ItemsInspected { get; set; } = 0;



    public int CompareTo(Monkey? other)
    {
        if (other is null)
        {
            return 1;
        }
        return ItemsInspected.CompareTo(other.ItemsInspected);
    }
}