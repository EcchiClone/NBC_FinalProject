using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벡터 계산 등 필요한 복잡한 연산을 모두 모아두는 역할
public class MathUtils : MonoBehaviour
{
    // 예시
    // public static Vector3 RandomVector3(Vector3 _v){return new Vector3(0,0,0);}

    public static int[] getSphereNumInRows(int _numMain, int _numRows)
    {
        // Sphere: 구 형태에서, '단' 별 갯수를 새로 계산.
        // Rows의 둘레 길이에 비례하여 갯수를 반환.
        // _numMain * root(-_numRows/2 + pointIndex) for pointIndex in range(0,numRows) 비슷하게 하면 될 듯. 계산식 조정 필요 해 보임.
        return new int[] { };
    }

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

    // TYPE: 새로운 형태의 생성
    public static List<Vector3> GenerateSpherePointsTypeA(int pointsPerLayer, int numberOfLayers, float distanceMultiplier)
    {
        // 구형 타입A(최상/최하단 채우기. 가장 일반적인 이미지.)
        // int pointsPerLayer : 한 층 둘레를 이룰 갯수
        // int numberOfLayers : 층 수
        // float distanceMultiplier : 거리계수

        List<Vector3> spherePoints = new List<Vector3>();
        
        for (int layerIndex = 0; layerIndex < numberOfLayers; layerIndex++)
        {
            float layerHeightRatio = (numberOfLayers == 1 ? 0 : layerIndex / (float)(numberOfLayers - 1));

            for (int pointIndex = 0; pointIndex < pointsPerLayer; pointIndex++)
            {
                float theta = pointIndex * 2 * Mathf.PI / pointsPerLayer;   // 둘레 방향 각도
                float phi = layerHeightRatio * Mathf.PI;                    // 높이 방향 각도

                spherePoints.Add(new Vector3(
                    Mathf.Sin(phi) * Mathf.Sin(theta) * distanceMultiplier,
                    Mathf.Cos(phi) * distanceMultiplier,
                    Mathf.Sin(phi) * Mathf.Cos(theta) * distanceMultiplier
                ));
            }
        }

        return spherePoints;
    }
}
