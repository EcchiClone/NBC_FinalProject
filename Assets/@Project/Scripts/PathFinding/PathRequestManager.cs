using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static public PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    public Grid grid;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
        grid = GetComponent<Grid>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback); // 새로운 경로 요청 생성
        instance.pathRequestQueue.Enqueue(newRequest); // 경로 요청 큐에 Enqueue
        instance.TryProcessNext();
    }

    void TryProcessNext() // 경로 찾기 시도
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0) // 경로 처리 중 아니고 대기열에 요청 있을 때
        {
            currentPathRequest = pathRequestQueue.Dequeue(); // 맨 앞 요청 꺼내오기
            isProcessingPath = true; // 경로 처리 중으로 변경
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd); // 경로 요청 구조체의 정보 전달
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success); // 유닛에서 보낸 콜백 함수 호출 => 보낸 객체의 함수
        isProcessingPath = false; // 경로 처리 중 아님으로 변경
        TryProcessNext(); // 다시 경로 찾기 시도
    }

    struct PathRequest // 경로 요청 
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }

    }
}
