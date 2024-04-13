using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;

    Grid grid;

    private void Awake()
    {
        GetOrAddComponent(out requestManager);
        GetOrAddComponent(out grid);
    }


    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos)); // 경로 찾기 코루틴 시작
    }

    private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos); // 실제 위치에 부합하는 그리드 상의 노드를 가져옴
        Node targetNode = grid.NodeFromWorldPoint(targetPos); // 상동

        if (startNode.walkable && targetNode.walkable) // 두 위치가 갈 수 있는 곳인지 판단
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // 처리해야 할 노드
            HashSet<Node> closedSet = new HashSet<Node>(); // 확인한 노드 저장하는 배열 (HashSet:중복없는 자료구조)
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); // 노드를 가져옴
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    RetracePath(startNode, targetNode);
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) // 이웃 노드의 코스트 계산, 업데이트
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor)) // 열린 집합에 추가해주기
                            openSet.Add(neighbor);
                    }
                }
            }
        }
        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode); 
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess); // 웨이 포인트와 경로 찾기 성공 여부 전달
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) // 끝 노드부터 부모쪽으로 거슬러서 시작 노드까지 추가?
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints; // 시작점 -> 끝점 까지의 경로
    }

    Vector3[] SimplifyPath(List<Node> path) // 경로를 단순화 시킨다.
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    public void GetOrAddComponent<T>(out T reference) where T : Component
    {
        reference = GetComponent<T>();
        if (null == reference)
            reference = gameObject.AddComponent<T>();
    }
}
