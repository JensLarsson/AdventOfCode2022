if (args.Length == 0)
{
    Console.WriteLine("Usage: provide input file in form of txt file as arg");
    return;
}

string[] input = File.ReadAllLines(args[0]);

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
            currentNode = currentNode.Parent ?? currentNode;
            continue;
        }
        currentNode = currentNode.Children
            .TryGetValue(splittedString[2], out TreeNode child) ? child : throw new Exception("No child named " + splittedString[1]);
        continue;
    }
}
int totalSize = 0;
GetSize(rootNode, ref totalSize);
Console.WriteLine(totalSize);

static void GetSize(TreeNode node, ref int totalSize)
{
    int size = node.GetTotalSize();
    // foreach (var child in node.Children ?? new Dictionary<string, TreeNode>())
    // {
    //     size += child.Value.GetTotalSize();
    //     GetSize(child.Value, ref totalSize);
    // }
    if (size < 100000)
    {
        totalSize += size;
    }
    foreach (var child in node.Children ?? new Dictionary<string, TreeNode>())
    {
        if (child.Value.Children == null)
        {
            continue;
        }
        GetSize(child.Value, ref totalSize);
    }
}
