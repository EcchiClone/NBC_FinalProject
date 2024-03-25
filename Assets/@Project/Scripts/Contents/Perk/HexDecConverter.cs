using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDecConverter : MonoBehaviour
{
    public string DecToHex(int i, int length)
    {
        // 10진수 -> 16진수 변환 함수, length는 자릿수 맞춰줌
        string hex = i.ToString($"X" + length.ToString()); // 소문자 'x'면 소문자로 출력
        return hex;
    }

    public int HexToDec(string hex)
    {
        // 16진수 -> 10진수 변환 함수
        return Convert.ToInt32(hex, 16);
    }
}
