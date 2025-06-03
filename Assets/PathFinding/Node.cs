using UnityEngine;

public class Node
{
    public Vector2 worldPosition;
    public bool walkable;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;
    public int fCost => gCost + hCost;

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
