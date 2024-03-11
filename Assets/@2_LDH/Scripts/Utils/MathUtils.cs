using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벡터 계산 등 필요한 복잡한 연산을 모두 모아두는 역할
public class MathUtils : MonoBehaviour
{
    // 예시
    // public static Vector3 RandomVector3(Vector3 _v){return new Vector3(0,0,0);}

    // Sphere: 구 형태에서, '단' 별 갯수를 새로 계산.
    public static int[] getSphereNumInRows(int _numMain, int _numRows)
    {
        // Rows의 둘레 길이에 비례하여 갯수를 반환.
        // _numMain * root(-_numRows/2 + i) for i in range(0,numRows) 비슷하게 하면 될 듯. 계산식 조정 필요 해 보임.
        return new int[] { };
    }

}
