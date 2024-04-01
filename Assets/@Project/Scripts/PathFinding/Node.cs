using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition; // 노드 중심 실제 위치
    public int gridX; // coordinate x of this grid
    public int gridY; // coordinate y of this grid

    public int gCost; // distance from starting node
    public int hCost; // ditance from end node
    public Node parent; // 부모 노드 참조

    public int HeapIndex { get; set; }

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get{ return gCost + hCost; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
