using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Runtime.CompilerServices;

public class TextMeshProBrokenTextEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string randomChars = "x_"; // 인스펙터에서 설정한 변수
    private string originalText; // 원본 텍스트
    private string currentText; // 현재 텍스트 상태
    [SerializeField] private int changePerOnetime=3; // 한 차례에 바꿀 글자 수
    [SerializeField] private float minTime = 0.1f; // 최소 변화 시간
    [SerializeField] private float maxTime = 0.7f; // 최대 변화 시간

    void Awake()
    {
        originalText = textComponent.text; // 원본 텍스트 설정
    }
    private void OnEnable()
    {
        // 초기 텍스트 설정
        currentText = new string('_', originalText.Length);
        textComponent.text = currentText;
        StartCoroutine(RandomizeText()); // 텍스트 랜덤화 시작
    }

    IEnumerator RandomizeText()
    {
        while (enabled)
        {
            int changesCount = Random.Range(1, changePerOnetime + 1); // 한 번에 변화시킬 문자 수

            for (int i = 0; i < changesCount; i++)
            {
                int charIndex = Random.Range(0, originalText.Length); // 변화시킬 문자의 위치

                // 현재 상태에서 문자를 하나 선택하여 랜덤하게 바꾼다
                currentText = currentText.Remove(charIndex, 1).Insert(charIndex, GetRandomChar(charIndex).ToString());
            }

            textComponent.text = currentText; // Text 컴포넌트에 랜덤화된 텍스트 적용

            float randomDelay = Random.Range(minTime, maxTime); // 랜덤 딜레이
            yield return new WaitForSeconds(randomDelay); // 다음 변화 전에 대기
        }
    }
    private char GetRandomChar(int index)
    {
        // 인스펙터에서 설정한 문자와 원래 문자를 포함한 문자 배열 생성
        char[] possibleChars = (randomChars + originalText[index]).ToCharArray();

        // 가능한 문자 중 랜덤하게 하나를 선택
        int randomIndex = Random.Range(0, possibleChars.Length);
        return possibleChars[randomIndex];
    }
}
