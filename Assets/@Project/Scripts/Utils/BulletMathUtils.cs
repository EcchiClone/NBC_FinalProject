using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletMathUtils
{

    #region 커스텀 모양
    #region Shape: 원형
    public static List<Vector3> GenerateCirclePoints(int pointsPerLayer, float distanceMultiplier)
    {
        // int pointsPerLayer : 한 층 둘레를 이룰 갯수
        // float distanceMultiplier : 거리계수

        List<Vector3> spherePoints = new List<Vector3>();

        for (int pointIndex = 0; pointIndex < pointsPerLayer; pointIndex++)
        {
            float theta = pointIndex * 2 * Mathf.PI / pointsPerLayer;

            spherePoints.Add(new Vector3(
                Mathf.Sin(theta) * distanceMultiplier,
                0f,
                Mathf.Cos(theta) * distanceMultiplier
            ));
        }
        return spherePoints;
    }
    #endregion
    #region Shape: 구형 타입1
    public static List<Vector3> GenerateSpherePointsTypeA(int pointsPerLayer, int numberOfLayers, float distanceMultiplier)
    {
        // int pointsPerLayer : 한 층 둘레를 이룰 갯수
        // int numberOfLayers : 층 수
        // float distanceMultiplier : 거리계수

        List<Vector3> spherePoints = new List<Vector3>();
        
        for (int layerIndex = 0; layerIndex < numberOfLayers; layerIndex++)
        {
            float layerHeightRatio = (numberOfLayers == 1 ? 0.5f : layerIndex / (float)(numberOfLayers - 1));

            for (int pointIndex = 0; pointIndex < pointsPerLayer; pointIndex++)
            {
                float theta = pointIndex * 2 * Mathf.PI / pointsPerLayer;   // 둘레 방향 각도
                float phi = layerHeightRatio * Mathf.PI;                    // 높이 방향 각도

                spherePoints.Add(new Vector3(
                    Mathf.Sin(phi) * Mathf.Sin(theta) * distanceMultiplier,
                    Mathf.Cos(phi) * distanceMultiplier,
                    Mathf.Sin(phi) * Mathf.Cos(theta) * distanceMultiplier
                ));
                if ( (layerIndex == 0 || layerIndex == numberOfLayers - 1) && numberOfLayers!=1 ) // 최상단 및 최하단은 1개만 생성. 단 층이 하나일 경우 제외
                    break;
            }
        }

        return spherePoints;
    }
    #endregion
    #region RandomShape: 무작위 도형(한붓)
    public static List<Vector3> AutoDrawVector3List(int numOfVertex, bool isLoop)
    {
        List<Vector3> randomPoints = new List<Vector3>();
        for (int i = 0; i < numOfVertex; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;// 단위 구면에서 무작위 방향의 벡터를 생성
            float randomDistance = UnityEngine.Random.value / 2f + 0.5f;// 랜덤 거리 값을 생성
            randomPoints.Add(randomDirection * randomDistance);// 랜덤 정점을 추가
        }
        if (isLoop && numOfVertex > 0)// 필요한 경우 마지막 정점을 추가하여 폐쇄형 그림
        {
            randomPoints.Add(randomPoints[0]);
        }
        return randomPoints;
    }
    #endregion
    #endregion

    #region 탄 조절
    #region 원점 기준 Vector3 전체 회전
    // 전체 회전(아직 테스트 안 해봄)
    public static List<Vector3> RotateVectors(List<Vector3> originalVectors, Quaternion rotation, float distanceMultiplier)
    {
        // 벡터 집단에 대해 회전 및 거리계수를 곱함

        List<Vector3> rotatedVectors = new List<Vector3>();

        foreach (Vector3 originalVector in originalVectors)
        {
            // 회전 적용 및 거리 계수 적용
            Vector3 rotatedVector = rotation * originalVector * distanceMultiplier;
            rotatedVectors.Add(rotatedVector);
        }
        return rotatedVectors;
    }
    #endregion
    #region 탄퍼짐(각,집중도)
    public static Vector3 CalculateSpreadDirection(Vector3 originalDirection, float maxSpreadAngle, float concentration)
    {
        // 랜덤 각도를 계산
        float spreadAngle = Random.Range(0, maxSpreadAngle/2.0f);
        spreadAngle *= Mathf.Lerp(1.0f, 0.0f, concentration); // 집중 정도에 따라 스케일 조정

        // 랜덤 회전 축을 계산 (originalDirection에 수직인 벡터)
        Vector3 randomAxis = Vector3.Cross(originalDirection, Random.insideUnitSphere).normalized;

        // originalDirection을 랜덤 축으로 spreadAngle만큼 회전
        Quaternion spreadRotation = Quaternion.AngleAxis(spreadAngle, randomAxis);
        Vector3 spreadDirection = spreadRotation * originalDirection;


        return spreadDirection.normalized; // 노멀라이즈된 조정된 방향 반환
    }
    #endregion
    #region 가장 먼 점을 거리 1로 List<Vector3> 정규화
    public static List<Vector3> NormalizeVector3List(List<Vector3> points)
    {
        float maxDistance = points.Max(point => point.magnitude);

        List<Vector3> normalizedPoints = new List<Vector3>();

        if (maxDistance > 0)// 가장 먼 거리가 0이 아닌 경우에만 정규화를 진행
        {
            foreach (var point in points)
            {
                normalizedPoints.Add(point / maxDistance);// 각 점을 가장 먼 거리로 나누어 정규화
            }
        }
        return normalizedPoints;
    }
    #endregion

    #endregion

    #region 그 외
    #region 플레이어 위치 찾기
    public static Vector3 GetPlayerPos(bool reposit = false)
    {
        try
        {
            if (reposit)
                return PlayerPivotReposition(Managers.Module.CurrentModule.LowerPosition.position);
            return Managers.Module.CurrentModule.LowerPosition.position;


        }
        catch
        {
            GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
            if (playerGo != null)
            {
                if (reposit)
                    return PlayerPivotReposition(playerGo.transform.position);
                return playerGo.transform.position;
            }
            else
            {
                if (reposit)
                    return PlayerPivotReposition(new Vector3(0, 0, 0)); // 플레이어 찾을 수 없음
                return new Vector3(0, 0, 0); // 플레이어 찾을 수 없음
            }
        }
    }
    #endregion

    #region 플레이어 피봇 조정
    public static Vector3 PlayerPivotReposition(GameObject playerGo)
    {
        return PlayerPivotReposition(playerGo.transform.position);
    }
    public static Vector3 PlayerPivotReposition(Transform playerTf)
    {
        return PlayerPivotReposition(playerTf.position);
    }
    public static Vector3 PlayerPivotReposition(Vector3 playerPos)
    {
        return playerPos + Vector3.up;
    }
    #endregion
    #endregion
}
