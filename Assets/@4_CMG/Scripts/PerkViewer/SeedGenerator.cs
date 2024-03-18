using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{

    System.Random _random = new System.Random();

    // TODO: 정규분포를 따르는 확률로 무작위 시드 생성기 제작
    private void Start()
    {
        Debug.Log(DecToHex(15));
    }

    private string DecToHex(int i)
    {
        // 10진수 -> 16진수 변환 함수
        string hex = i.ToString("X"); // 소문자 'x'면 소문자로 출력
        return hex;
    }

    private int HexToDec(string hex)
    {
        // 16진수 -> 10진수 변환 함수
        return Convert.ToInt32(hex, 16);
    }
}
