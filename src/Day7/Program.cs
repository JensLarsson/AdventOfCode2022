if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string[] input = File.ReadAllLines(args[0]);

//Setup tree
TreeNode rootNode = new TreeNode("/", null);
TreeNode currentNode = rootNode;
for (int i = 1; i < input.Length; i++)
{
    string[] splittedString = input[i].Split(" ");

    if (int.TryParse(splittedString[0], out int result))
    {
        currentNode.AddChild(splittedString[1], new TreeNode(result, splittedString[1], parent: currentNode));
        continue;
    }

    if (splittedString[0] == "dir")
    {
        currentNode.AddChild(splittedString[1], new TreeNode(splittedString[1], parent: currentNode));
        continue;
    }

    if (splittedString[1] == "cd")
    {
        if (splittedString[2] == "..")
        {
            currentNode = currentNode.Parent ?? throw new Exception("No parent");
            continue;
        }
        currentNode = currentNode.Children
            .TryGetValue(splittedString[2], out TreeNode child) ? child : throw new Exception("No child named " + splittedString[1]);
        continue;
    }
}
//Sort folders by size
List<TreeNode> nodes = new List<TreeNode>() { rootNode };
GetAllFolders(ref nodes, rootNode.Children);
nodes.Sort();

//Part 1
int totalSizePartA = 0;
foreach (var node in nodes)
{
    if (node.GetTotalSize() > 100000)
    {
        break;
    }
    totalSizePartA += node.GetTotalSize();
}
Console.WriteLine("Part1: " + totalSizePartA);

//part 2
int rootSize = rootNode.GetTotalSize();
int sizeNeeded = 70000000 - 30000000 - rootSize;    //calculate size needed to remove
if (sizeNeeded > 0)                                 //if size needed is positive, then nothing needs to be removed
{
    Console.WriteLine("Part2: " + 0);
    return;
}
sizeNeeded *= -1;                                   //make size needed positive

foreach (var node in nodes)                         //find first node that is bigger than size needed
{
    if (node.GetTotalSize() > sizeNeeded)
    {
        Console.WriteLine("Part2: " + node.GetTotalSize());
        break;
    }
}


/// <summary>
/// Recursively gets all folders 
/// </summary>
/// <param name="nodes">Return list to put nodes in</param>
/// <param name="children">child nodes to recursively add</param>
static void GetAllFolders(ref List<TreeNode> nodes, Dictionary<string, TreeNode> children)
{
    foreach (var child in children?.Values ?? new Dictionary<string, TreeNode>().Values)
    {
        if (child.Children == null)
        {
            continue;
        }
        nodes.Add(child);
        GetAllFolders(ref nodes, child.Children);
    }
}