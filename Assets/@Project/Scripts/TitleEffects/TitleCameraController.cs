using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가
using UnityEngine.UI; // UI 관련 작업을 위해 추가

public class TitleCameraControl : MonoBehaviour
{
    public Transform[] waypoints; // 카메라가 이동할 위치들
    public float moveDuration = 2f; // 위치 사이의 이동 시간
    public Image fadeImage; // Fade 효과를 위한 Image 컴포넌트
    public float fadeDuration = 1f; // Fade 인/아웃 지속 시간

    private void Start()
    {
        // 게임 시작 시 fadeImage를 불투명하게 설정
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        Sequence sequence = DOTween.Sequence();

        // 각 waypoint에 대한 이동과 Fade 처리 추가
        foreach (Transform waypoint in waypoints)
        {
            sequence.AppendCallback(() => { transform.position = waypoint.position; transform.rotation = waypoint.rotation; }) // 위치 변경
                    .Append(fadeImage.DOFade(0, fadeDuration)) // Fade in
                    .Append(transform.DOMove(waypoint.position, moveDuration).SetEase(Ease.InOutQuad)) // 이동은 현재 마련하지 않음
                    .Append(fadeImage.DOFade(1, fadeDuration)); // Fade out;
        }

        // 무한 루프
        sequence.SetLoops(-1);
    }
}
