if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string[] heightMap = File.ReadAllLines(args[0]);
int patchWidth = heightMap[0].Length;
bool[,] vissibilityMap = new bool[heightMap.Length, patchWidth];
Console.WriteLine(vissibilityMap.Cast<bool>().Count(b => b));

int vissibleTrees = 0;
for (int y = 1; y < heightMap.Length - 1; y++)
{
    //Left to right
    for (int x = 1; x < heightMap[y].Length - 1; x++)
    {
        vissibilityMap[y, x] = vissibilityMap[y, x] || IsTallestChar(heightMap[y][x], heightMap[y][..x]);
    }
    //Left to right
    for (int x = heightMap[y].Length - 2; x > 0; x--)
    {
        string comparison = new string(heightMap[y][(x + 1)..]);
        vissibilityMap[y, x] = vissibilityMap[y, x] || IsTallestChar(heightMap[y][x], comparison);
    }
    //Top to bottom
    for (int x = 1; x < heightMap[y].Length - 1; x++)
    {
        string comparison = new string(heightMap[..y].Select(s => s[x]).ToArray());
        vissibilityMap[y, x] = vissibilityMap[y, x] || IsTallestChar(heightMap[y][x], comparison);
    }
    //Bottom to top
    for (int x = 1; x < heightMap[y].Length - 1; x++)
    {
        string comparison = new string(heightMap[(y + 1)..].Select(s => s[x]).ToArray());
        vissibilityMap[y, x] = vissibilityMap[y, x] || IsTallestChar(heightMap[y][x], comparison);
    }
}

for (int i = 0; i < vissibilityMap.GetLength(0); i++)
{
    for (int j = 0; j < vissibilityMap.GetLength(1); j++)
    {
        Console.Write(vissibilityMap[i, j] ? "1" : "0");
    }
    Console.WriteLine(); // move to the next line after printing each row
}
int vissibleTreeCount = vissibilityMap.Cast<bool>().Count(b => b) + heightMap.Length * 2 + patchWidth * 2 - 4;
Console.WriteLine("vissible trees from the outside: " + vissibleTreeCount);


bool IsTallestChar(char c, string comparison)
{
    foreach (char c2 in comparison)
    {
        if (c2 >= c)
        {
            return false;
        }
    }
    return true;
}