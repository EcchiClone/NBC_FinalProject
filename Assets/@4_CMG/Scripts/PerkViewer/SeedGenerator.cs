using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class SeedGenerator : MonoBehaviour
{
    private void Start()
    {
        PermuWithRepetition(8, 0, 40319);
    }

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
        // 2-5. 다음 순열 완성! + count값 증가
        // 3. 원하는 count값이 등장할 때까지 반복.
        // 4. num 값만큼의 알파벳을 1로 치환, 나머지는 0으로 치환.
        // 5. 원하는 중복순열 완성. 값을 return.

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
            Permutation(chars, length);
            // 2-5.
            count++;
        }

        // 5.
        string answer = "";

        foreach (char c in chars)
        {
            answer += c;
        }

        Debug.Log(answer);
    }

    private char[] Permutation(char[] chars, int length)
    {
        // 2-1.
        int i = 0;

        for (int k = length - 1; k > 0; k--)
        {
            int right = (int)chars[k];
            int left = (int)chars[k - 1];

            if (left < right)
            {
                i = k - 1;
                break;
            }
        }

        int iVal = (int)chars[i];

        // 2-2.
        int j = 0;

        for (int k = length - 1; k > 0; k--)
        {
            int right = (int)chars[k];

            if (right > iVal)
            {
                j = k;
                break;
            }
        }

        int jVal = (int)chars[j];

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
                int left = (int)chars[k];
                int right = (int)chars[l];

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
}
