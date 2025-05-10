using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector2Int gridPosition;
    public int gCost = 0;//到起始点的成本
    public int hCost = 0;//到终点的成本
    public Node parentNode;

    public Node(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;

        parentNode = null;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int CompareTo(Node nodeToCompare)//返回小于0则代表成本低于nodeToCompare，反之，0则相同
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return compare;
    }
}
