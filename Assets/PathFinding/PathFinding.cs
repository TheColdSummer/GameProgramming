using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System;

/*
 * This script implements a pathfinding algorithm using the A* algorithm.
 */
public class Pathfinding : MonoBehaviour
{
    private GridMap _grid;
    private static SynchronizationContext _mainThreadContext;

    void Awake()
    {
        _grid = GetComponent<GridMap>();
        _mainThreadContext = SynchronizationContext.Current;
    }

    /*
     * This method finds a path asynchronously using a separate thread.
     */
    public void FindPathAsyncThreaded(Vector2 startPos, Vector2 targetPos, Action<List<Node>> onPathFound)
    {
        Task.Run(() =>
        {
            var path = FindPathSync(startPos, targetPos);
            _mainThreadContext.Post(_ => onPathFound?.Invoke(path), null);
        });
    }

    /*
     * This method finds a path using the A* algorithm.
     */
    public List<Node> FindPathSync(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);
        bool tmp = startNode.walkable;
        if (!tmp)
        {
            startNode.walkable = true;
        }
        if (!targetNode.walkable)
        {
            targetNode = AdjustToWalkableNeighbour(targetNode, targetPos);
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            // Find the node with the lowest fCost
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            // If the current node is the target node, retrace the path
            if (currentNode == targetNode)
            {
                startNode.walkable = tmp;
                return RetracePath(startNode, targetNode);
            }
            
            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                // If the neighbour is not walkable or already in the closed set, skip it
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;
                
                // Calculate the cost to reach the neighbour
                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        startNode.walkable = tmp;
        return null;
    }

    /*
     * This method adjusts the target node to a walkable neighbour if the original target node is not walkable.
     * The purpose is to reduce the chance of the pathfinding algorithm failing due to an unwalkable target node.
     * Because the grid map's resolution is not very high, the target node may not be walkable,
     */
    private Node AdjustToWalkableNeighbour(Node targetNode, Vector2 targetPos)
    {
        List<Node> candidates = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        int[] dx = { -2, -1, 0, 1, 2 }; // Possible x offsets to check
        int[] dy = { -2, -1, 0, 1, 2 }; // Possible y offsets to check

        for (int i = 0; i < dx.Length; i++)
        {
            // Loop through all combinations of dx and dy to find walkable neighbours
            for (int j = 0; j < dy.Length; j++)
            {
                if (dx[i] == 0 && dy[j] == 0) continue;
                int nx = targetNode.gridX + dx[i];
                int ny = targetNode.gridY + dy[j];
                int manhattan = Mathf.Abs(dx[i]) + Mathf.Abs(dy[j]);
                if (manhattan > 2 || manhattan == 0) continue;
                if (nx < 0 || ny < 0 || nx >= _grid.gridWorldSize.x / (_grid.nodeRadius * 2) || ny >= _grid.gridWorldSize.y / (_grid.nodeRadius * 2))
                    continue;
                Node node = _grid.NodeFromWorldPoint(new Vector2(
                    _grid.transform.position.x - _grid.gridWorldSize.x / 2 + nx * _grid.nodeRadius * 2 + _grid.nodeRadius,
                    _grid.transform.position.y - _grid.gridWorldSize.y / 2 + ny * _grid.nodeRadius * 2 + _grid.nodeRadius
                ));
                if (!visited.Contains(node))
                {
                    candidates.Add(node);
                    visited.Add(node);
                }
            }
        }

        Node closest = null;
        float minDist = float.MaxValue;
        foreach (var node in candidates)
        {
            if (!node.walkable) continue;
            RaycastHit2D hit = Physics2D.Linecast(targetPos, node.worldPosition, LayerMask.GetMask("Wall"));
            if (hit.collider != null && hit.collider.CompareTag("Wall")) continue;
            float dist = Vector2.Distance(targetPos, node.worldPosition);
            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }
        return closest;
    }

    /*
     * This method retraces the path from the end node to the start node.
     */
    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    /*
     * This method calculates the distance between two nodes.
     * It uses Manhattan distance.
     */
    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);
        return dstX + dstY;
    }
}