using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BinaryCombineAlgorithm : MonoBehaviour
{

    private int _before;
    private int _count;
    private int[] _log;


    private void Start()
    {
        //Debug.Log(ConvertStructureToBinary(24, 12, 1));
        //Debug.Log(ConvertStructureToBinary(24, 12, 2704156));
    }

    public string ConvertStructureToBinary(int length, int num, int idx)
    {
        FindIdxRange(length, num, idx);

        string binary = "";
        int[] log = _log;
        int relativeIdx = idx - _before;

        int[] leftPart = new int[log[0]];
        int[] rightPart = new int[log.Length - 1];

        // 배열 슬라이싱, 2개 선택부분과 나머지
        for (int i = 0; i < leftPart.Length; i++)
        {
            leftPart[i] = 0;
        }

        for (int i = 0; i < rightPart.Length; i++)
        {
            rightPart[i] = log[i + 1] - log[i];
        }

        // 선택부분 순회와 값 할당
        int count = 0;

        for (int i = leftPart.Length - 1; i >= 0; i--)
        {
            for (int j = i - 1;  j >= 0; j--)
            {
                count++;
                if (relativeIdx == count)
                {
                    leftPart[i] = 1;
                    leftPart[j] = 1;
                    break;
                }
            }

            if (relativeIdx == count)
                break;
        }

        // 값 합치기
        foreach (int i in leftPart)
        {
            binary += i.ToString();
        }

        foreach (int i in rightPart)
        {
            for (int j = 0; j < i; j++)
            {
                if (j == 0)
                    binary += "1";
                else
                    binary += "0";
            }
        }

        return binary;
    }

    private void FindIdxRange(int length, int num, int idx)
    {
        // 값 초기 설정
        _before = 0;
        _count = 0;
        _log = new int[num - 1];
        _log[num - 2] = length;

        RecursiveFunction(length, num, idx);
    }

    private void RecursiveFunction(int length, int num, int idx)
    {
        // 재귀 함수.

        _before = _count;

        if (IsActive(idx))
            return;

        if (num > 2)
        {
            for (int i = (length - 1); i >= (num - 1); i--)
            {
                _log[num - 3] = i;
                RecursiveFunction(i, num - 1, idx);
                if (IsActive(idx))
                    return;
            }
        }
        else if (num == 2)
        {
            _count += GaussSum(length);
        }
        else
        {
            Debug.Log("예상되는 num값의 범위를 벗어났습니다.");
        }
    }

    private int GaussSum(int num)
    {
        // num이 6이라면, 5+4+3+2+1 = 15를 계산해서 반환해줍니다.
        // n(n-1) * 1/2
        return (num) * (num - 1) / 2;
    }

    private bool IsActive(int idx)
    {
        // 원하는 순번의 값이 구해졌나용?
        if (idx >= _before && idx < _count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
