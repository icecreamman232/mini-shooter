using UnityEngine;

public class BinarySpacePartionTree
{
    [Range(0, 1)] public float SplitHorizontallyProbability = 0.7f;
    public int MaxDepth = 4;
    public int MinSize = 16;
    public BSPNode Root;

    public BinarySpacePartionTree(int x, int y, int width, int height)
    {
        Root = new BSPNode(new RectInt(x, y, width, height));
        Split(Root, 0);
    }

    public void Draw()
    {
        DrawPartitions(Root);
    }
    

    private void Split(BSPNode node, int depth)
    {
        if (depth >= MaxDepth) return;
        
        bool canSplitH = node.Bounds.height >= MinSize * 2;
        bool canSplitV = node.Bounds.width >= MinSize * 2;

        if (!canSplitH && !canSplitV) return;
        
        bool splitHorizontally = canSplitH && canSplitV ? Random.value < SplitHorizontallyProbability : canSplitH;

        if (splitHorizontally)
        {
            int cutY = Random.Range(node.Bounds.yMin + MinSize, node.Bounds.yMax - MinSize);
            node.Left = new BSPNode(new RectInt(node.Bounds.x, node.Bounds.y, node.Bounds.width, cutY - node.Bounds.y));
            node.Right = new BSPNode(new RectInt(node.Bounds.x, cutY, node.Bounds.width, node.Bounds.yMax - cutY));
            
        }
        else
        {
            int cutX = Random.Range(node.Bounds.xMin + MinSize, node.Bounds.xMax - MinSize);
            node.Left = new BSPNode(new RectInt(node.Bounds.x, node.Bounds.y, cutX - node.Bounds.x, node.Bounds.height));
            node.Right = new BSPNode(new RectInt(cutX, node.Bounds.y, node.Bounds.xMax - cutX, node.Bounds.height));
        }
        
        Split(node.Left, depth + 1);
        Split(node.Right, depth + 1);
    }
    
    private void DrawPartitions(BSPNode node)
    {
        if (node == null) return;

        // Depth-based color: node càng sâu càng nhạt
        float t = Mathf.Clamp01((float)GetDepth(node) / MaxDepth);
        //Gizmos.color = Color.Lerp(Color.red, new Color(1,1,1,0.15f), t);
        Gizmos.color = Color.darkGreen;
        
        var bounds = node.Bounds;
        Gizmos.DrawWireCube(
            new Vector3(bounds.x + bounds.width * 0.5f, bounds.y + bounds.height * 0.5f, 0),
            new Vector3(bounds.width, bounds.height, 0)
        );

        DrawPartitions(node.Left);
        DrawPartitions(node.Right);
    }
    
    int GetDepth(BSPNode node)
    {
        int depth = 0;
        var current = Root;
        while (current != null && current != node)
        {
            depth++;
            current = Contains(current.Left, node) ? current.Left : current.Right;
        }
        return depth;
    }

    bool Contains(BSPNode subtree, BSPNode target)
    {
        if (subtree == null) return false;
        if (subtree == target) return true;
        return Contains(subtree.Left, target) || Contains(subtree.Right, target);
    }
}


public class BSPNode
{
    public RectInt Bounds;
    public BSPNode Left;
    public BSPNode Right;
    public RectInt? Room;

    public BSPNode(RectInt bounds)
    {
        Bounds = bounds;
    }
    
    public bool IsLeaf => Left == null && Right == null;
}