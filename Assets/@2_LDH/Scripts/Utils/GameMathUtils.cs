using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벡터 계산 등 필요한 복잡한 연산을 모두 모아두는 역할
public class GameMathUtils : MonoBehaviour
{
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

    #region Shape: 구형 타입A
    // 구형 타입A(최상/최하단 채우기. 가장 일반적인 이미지.)
    public static List<Vector3> GenerateSpherePointsTypeA(int pointsPerLayer, int numberOfLayers, float distanceMultiplier)
    {
        // int pointsPerLayer : 한 층 둘레를 이룰 갯수
        // int numberOfLayers : 층 수
        // float distanceMultiplier : 거리계수

        //Debug.Log($"{pointsPerLayer}, {numberOfLayers}");

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


}
