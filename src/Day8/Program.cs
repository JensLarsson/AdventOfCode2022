if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string[] heightMap = File.ReadAllLines(args[0]);

int patchWidth = heightMap[0].Length;
bool[,] visibilityMap = new bool[heightMap.Length, patchWidth];
int[,] scenicScoreMap = new int[heightMap.Length, patchWidth];

//part1
for (int y = 1; y < heightMap.Length - 1; y++)
{
    for (int x = 1; x < heightMap[y].Length - 1; x++)
    {
        //Left to right
        string treesToLeft = new string(heightMap[y][..x].Reverse().ToArray());
        visibilityMap[y, x] = visibilityMap[y, x] || IsTallestChar(heightMap[y][x], treesToLeft);
        scenicScoreMap[y, x] = getVissibleTrees(heightMap[y][x], treesToLeft);

        //Top to bottom
        string treesAbove = new string(heightMap[..y].Select(s => s[x]).Reverse().ToArray());
        visibilityMap[y, x] = visibilityMap[y, x] || IsTallestChar(heightMap[y][x], treesAbove);
        scenicScoreMap[y, x] *= getVissibleTrees(heightMap[y][x], treesAbove);

        //Left to right
        string treesToRight = new string(heightMap[y][(x + 1)..]);
        visibilityMap[y, x] = visibilityMap[y, x] || IsTallestChar(heightMap[y][x], treesToRight);
        scenicScoreMap[y, x] *= getVissibleTrees(heightMap[y][x], treesToRight);

        //Bottom to top
        string treesBelow = new string(heightMap[(y + 1)..].Select(s => s[x]).ToArray());
        visibilityMap[y, x] = visibilityMap[y, x] || IsTallestChar(heightMap[y][x], treesBelow);
        scenicScoreMap[y, x] *= getVissibleTrees(heightMap[y][x], treesBelow);
    }
}

int visibleTreeCount = visibilityMap.Cast<bool>().Count(b => b);  //calculate visible trees from the inside
visibleTreeCount += heightMap.Length * 2 + patchWidth * 2 - 4;     //add trees on boarder
Console.WriteLine("vissible trees from the outside: " + visibleTreeCount);

int highestScenicScore = scenicScoreMap.Cast<int>().Max();          //Get highest scenic score
Console.WriteLine("highest scenic score: " + highestScenicScore);



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

int getVissibleTrees(char c, string comparison)
{
    int visibleTrees = 0;
    foreach (char c2 in comparison)
    {
        visibleTrees++;
        if (c2 >= c)
        {
            return visibleTrees;
        }
    }
    return visibleTrees;
}