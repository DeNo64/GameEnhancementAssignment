using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        //FindPath(entity.position, target.position);
    }

	public Vector3[] FindPath(Vector3 startPos, Vector3 goalPos)
    {
        float startTime = Time.realtimeSinceStartup;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node goalNode = grid.NodeFromWorldPoint(goalPos);
        bool goalWalkableChanged = false;

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        open.Add(startNode);

        if (!goalNode.walkable)
        {
            goalNode.walkable = true;
            goalWalkableChanged = true;
        }

        while(open.Count > 0)
        {
            Node currentNode = open[0];
            for(int i = 1; i < open.Count; i++)
            {
                if(open[i].fCost < currentNode.fCost || open[i].fCost == currentNode.fCost && open[i].hCost < currentNode.hCost)
                {
                    currentNode = open[i];
                }
            }

            open.Remove(currentNode);
            closed.Add(currentNode);

            if(currentNode == goalNode)
            {
                Vector3[] path = GetPath(startNode, goalNode);
                //print("Time Taken: " + (Time.realtimeSinceStartup - startTime));
                if (goalWalkableChanged)
                    goalNode.walkable = false;
                return path;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closed.Contains(neighbour))
                    continue;

                int Cost2Neighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (Cost2Neighbour < neighbour.gCost || !open.Contains(neighbour))
                {
                    neighbour.gCost = Cost2Neighbour;
                    neighbour.hCost = GetDistance(neighbour, goalNode);
                    neighbour.parent = currentNode;

                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                    else
                    {
                        open.Remove(neighbour);
                        open.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    Vector3[] GetPath(Node startNode, Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        waypoints.Add(path[0].worldPos);
        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, 
                                               path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
}