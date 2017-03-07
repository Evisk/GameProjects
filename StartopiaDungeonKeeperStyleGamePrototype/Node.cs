using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int nodeX, nodeY;
    public int hCost, gCost;
    public Node parent;
    public List<Node> Neighbours = new List<Node>();
    public GameObject gridNodeObject;

    public enum NodeState
    {
        walkable,
        occupied,
        soonToBeOccupied,
        door,
        interiorPoint
    }

    public NodeState nodeState = NodeState.walkable;




    public void GetNeighbors()
    {

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = nodeX + x;
                int checkY = nodeY + y;

                if (checkX >= 0 && checkX < GameManager.Instance.gridSize.x && checkY >= 0 && checkY < GameManager.Instance.gridSize.y)
                {
                    Neighbours.Add(GameManager.Instance.gridNodes[checkX, checkY]);
                }
            }
        }
    }

    public Node(int x,int y,NodeState _state)
    {
        nodeX = x;
        nodeY = y;
        nodeState = _state;
    }
    public Node()
    {

    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
        
    }

    public void  setNode(int _x, int _y, NodeState _state)
    {
        nodeX = _x;
        nodeY = _y;
        nodeState = _state;
    }

    public int GetX()
    {
        return nodeX;
    }
    public int GetY()
    {
        return nodeY;
    }



}
