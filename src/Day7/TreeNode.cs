using System.Collections.Generic;

//TODO: Calculate the FilSize of Folders on ADDING  and REMOVING files instead of on request
public class TreeNode : IComparable<TreeNode>
{
    public int FileSize { get; private set; } = 0;
    public string Name { get; private set; }
    public TreeNode Parent { get; private set; }
    public Dictionary<string, TreeNode> Children { get; private set; }

    public TreeNode(int size, string name, TreeNode parent)
    {
        FileSize = size;
        Parent = parent;
        Name = name;
    }
    public TreeNode(string name, TreeNode parent)
    {
        Children = new Dictionary<string, TreeNode>();
        Parent = parent;
        Name = name;
    }

    public TreeNode AddChild(string name, TreeNode child)
    {
        Children.Add(name, child);
        return child;
    }

    public bool RemoveChild(string name)
    {
        if (Children.ContainsKey(name))
        {
            Children[name].Parent = null;
            return Children.Remove(name);
        }
        return false;
    }
    public string GetPath()
    {
        if (Parent == null)
        {
            return Name;
        }
        return Parent.GetPath() + "/" + Name;
    }

    public int GetTotalSize()
    {
        int size = 0;
        foreach (var child in Children ?? new Dictionary<string, TreeNode>())
        {
            size += child.Value.GetTotalSize();
        }
        return size + FileSize;
    }

    public int CompareTo(TreeNode? other)
    {
        return this.GetTotalSize().CompareTo(other?.GetTotalSize() ?? int.MinValue);
    }
}
