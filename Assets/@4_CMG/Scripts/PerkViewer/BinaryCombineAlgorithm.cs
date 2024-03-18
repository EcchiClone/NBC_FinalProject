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

    // 시도 1 - 실패. 삭제 예정
    private void PermuWithRepetition(int length, int num, int index)
    {
        // 원소가 두 개(0, 1)인 중복순열 알고리즘
        //
        // 1. length(길이) 값을 받아서 길이만큼 알파벳 배열 생성 e.g. { "a", "b", "c" }
        // 2. 순열 알고리즘 시작.
        // 2-1. i 찾기. 마지막 index부터 인접하는 두 개의 원소를 비교. 왼쪽이 작다면 왼쪽 값의 index가 i.
        // 2-2. j 찾기. 마지막 index부터 i의 원소와 비교. 더 크다면 그 값의 index가 j.
        // 2-3. i 값과 j 값을 swap.
        // 2-4. i+1 인덱스부터 끝까지 오름차순 정렬.
        // 2-5. 다음 순열 완성!, num 값만큼의 알파벳을 1로 치환, 나머지는 0으로 치환.
        // 2-6. 전의 중복 조합 값과 같지 않다면 count값 증가
        // 3. 원하는 count값이 등장할 때까지 반복.
        // 4. 원하는 중복순열 완성. 값을 return.

        // 1.
        char[] chars = new char[length];

        for (int k = 0; k < length; k++)
        {
            chars[k] = (char)(97 + k); // a = 97
        }

        int count = 0;

        // 3.
        while (count != index)
        {
            string before = ReplaceAlphabetToNumeric(chars, num);

            // 2.
            Permutation(chars, length);

            // 2-5.
            string after = ReplaceAlphabetToNumeric(chars, num);

            // 2-6.
            if (before != after)
            {
                count++;
            }
        }

        // 4.
        string answer = ReplaceAlphabetToNumeric(chars, num);

        Debug.Log(answer);
    }

    private char[] Permutation(char[] chars, int length)
    {
        // 2-1.
        int i = 0;

        for (int k = length - 1; k > 0; k--)
        {
            int right = chars[k];
            int left = chars[k - 1];

            if (left < right)
            {
                i = k - 1;
                break;
            }
        }

        int iVal = chars[i];

        // 2-2.
        int j = 0;

        for (int k = length - 1; k > 0; k--)
        {
            int right = chars[k];

            if (right > iVal)
            {
                j = k;
                break;
            }
        }

        int jVal = chars[j];

        // 2-3.
        {
            char temp = chars[i];
            chars[i] = chars[j];
            chars[j] = temp;
        }

        // 2-4.
        for (int k = i + 1; k < length; k++)
        {
            for (int l = k + 1; l < length; l++)
            {
                int left = chars[k];
                int right = chars[l];

                if (left > right)
                {
                    char temp = chars[k];
                    chars[k] = chars[l];
                    chars[l] = temp;
                }
            }
        }

        return chars;
    }

    private string ReplaceAlphabetToNumeric(char[] chars, int num)
    {
        string after = "";

        foreach (char c in chars)
        {
            if (c < (97 + num))
            {
                after += "1";
            }
            else
            {
                after += "0";
            }
        }

        return after;
    }

    // 시도 2 - 성공
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

    private string ConvertStructureToBinary(int length, int num, int idx)
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
}
