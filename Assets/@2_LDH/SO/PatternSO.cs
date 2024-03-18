using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patterns", menuName = "EnemyBulletSO/Pattern", order = 0)]
public class PatternSO : ScriptableObject
{
    public List<EnemyBulletPatternData> patternDatas;

    // 패턴 이름을 통해 패턴 정보를 찾기 -> 추후 최적화를 위한 수정 필요. 어떻게 쓰일지도 살짝 애매모호한 상태.
    // ▲ 현재 상태 : PhaseSO에서 patternName을 입력하여 저장, Enemy에서 'GetSpawnInfoByPatternName' 이용하여 탄막패턴 사용. 일단 OK
    // TODO.
    // But, 커스텀 고려해서 수정 필요.
    // 그리고 GetSpawnInfoByPatternName 자체에 대해서도 연산 더 낮은 방식으로 바꾸어야 함.
    public EnemyBulletPatternData GetSpawnInfoByPatternName(string patternName)
    {
        foreach (var patternData in patternDatas)
        {
            if (patternData.patternName == patternName)
            {
                return patternData;
            }
        }
        return null;
    }
}

[System.Serializable]
public class EnemyBulletPatternData
{
    public string patternName;
    public string Desc;
    public EnemyBulletSettings enemyBulletSettings;
}

[System.Serializable]
public struct EnemyBulletSettings // 추가 할 게 진짜 많다.. 트리 이미지로 {1.생성-2.이동-3.하층생성-4.반환} 명심하여 작성
{
    // 아래의 내용은 전-부 PhaseSO 작성 시 커스텀이 가능하도록(현재 불가능).
    // 다만 선택적 커스텀을 어떻게 해야할지 떠오르지가 않음. 커스텀 값을 기본적으로 모두 null로 둘 수 있을까?

    // 1. 생성 ---------------------------------//------------------------------------------------------------------

    // 1-1. 탄막의 모양
    //[Header("탄막 오브젝트")]
    public GameObject enemyBulletPrefab;            // 탄막 기본 프리팹

    // 1-2. 생성 시간과 횟수에 관련된 정보
    [Header("시작 지연")]
    public float initDelay;                     // 첫 탄막 생성까지의 지연. 불필요 가능성 큼.
    [Header("세트 수와 그 간격")]
    public int numOfSet;                        // 총 세트 수
    public float setDelay;                      // 세트 사이의 지연
    [Header("한 세트 내 발사 횟수와 그 간격")]
    public int shotPerSet;                      // 한 세트에서 탄막을 몇 차례 생성할지
    public float shotDelay;                     // 탄막 생성 사이의 지연

    // 1-3. 생성 모양에 관한 정보
    // Memo. 여기서 할게 꽤 많음. 차차 작성.

    // a. 어느 방향을 기준으로 생성을 시작할 것인지
    [Header("마스터 기준 생성 방향 벡터")]
    public PosDirection posDirection;           // 마스터 기준으로 생성될 방향
    public Vector3 customPosDirection;          // > World: 직접지정
    // a-plug. 기준 방향 지정 시 탄퍼짐
    //[Header("오차")]
    public SpreadType spreadA;                  // 탄퍼짐 유무
    public float maxSpreadAngleA;               // > 최대 퍼짐 각도
    public float concentrationA;                // > 집중 정도 (0.0 ~ 1.0)
                                                // public PosDirectionRandomType posDirectionRandomType;    // > 랜덤성이 직선인지, 평면인지. 이후에 고려할 사항도 다수
                                                // >a> 랜덤성이 직선일 경우, 그 직선의 형태
                                                // >a> 랜덤성이 직선일 경우, 그 직선의 범위 또는 양 방향 각각의 범위(각도가 될 듯)
                                                // >b> 랜덤성이 평면일 경우, 그 평면의 형태. 능력이 안된다면, 아래의 조건만 보아 콘 형태부터.
                                                // >b> 랜덤성이 평면일 경우, 허용 범위. 즉 기준점으로부터의 허용 각도

    // b. 기준방향을 중심으로 어떤 형태의 방사를 사용할지. 거리와 방향을 포함.
    // 간단한 선형 단일 발사부터, 정육면체 모양으로 속도를 달리 한 발사, 특별한 모양으로 생성되어 각각이 랜덤한 타이밍에 발사 등 다양한 형태.
    // b-1. 형태에 관해. 기본적인 프리셋을 제공하되, 유저가 Vector3를 직접 작성하여 입력할 수 있도록도 하자.
    [Header("탄막 형태")]
    public EnemyBulletShape enemyBulletShape;           // 탄막 모양의 타입
    // b-2. 거의 모든 모양에서 사용할 변수들
    //[Header("생성 거리")]
    public float initDistance;                  // 모든 탄막에 대한 생성거리의 기준
                                                // 기준 거리에 대한 랜덤성 부여. 여유가 되면 작성.
                                                // 이 랜덤성을, 모든 탄막에 동일부여할지, 각 탄막에 따로 부여할지의 여부.
    //[Header("1회 발사 당 탄수")]
    public int numPerShot;                      // 한번 발사에 사용되는 탄막 갯수. 
    // 참고: 일부 Shape들()에 대해서는 numPerShot으로 해결이 되기 때문에 이러한 형태들은 b-3항목 불필요.
    // b-3. 탄막 모양에 따라 선택적 변수들(이후, 조건부로 Inspector에 보여주는 것이 과제)
    public float shotVerticalDistance;          // Circle: 원의 면과 보스의 수직거리
    public int shotVerticalNum;                 // Sphere: 구의 '단' 갯수
                                                // Cone: 허용각도. 얘는 자료형을 뭘로 해야할지 모르겠음.
                                                // 전체 모양의 회전을 틀어버릴 요소(정해진 값)
                                                // 전체 모양의 회전을 틀어버릴 값의 랜덤 여부. true라면 위 값을 범위로 사용. // 이 두 랜덤변수는 a-plug에서 커버 가능한 부분으로 보임. 삭제 예정

    // b-99. 유저 커스텀 입력
    // 유저입력1. 원하는 범위에 a.N개를 균일배치(어려울듯), b.N개를 랜덤배치
    //            원하는 범위는... 일단 x, y, z의 범위? 이것만으로는 마음엔 들진 않을 듯.(이 방식으론 직육면체 밖에 불가능)
    //            그래프의 형태로 입력받아 활용할 수 있을 것 같지만 난이도가 있을 듯 하다.
    // 유저입력2. 완전한 위치들의 리스트를 전달받기



    [Header("탄막 움직임")]
    // 탄막의 방향 : 일단 마스터기준으로 밖으로 퍼지도록 Outer로 설정하여 테스트
    public EnemyBulletToDirection initDirectionType;
    public Vector3 initCustomDirection;

    // 2. 이동 ---------------------------------//------------------------------------------------------------------
    // 탄막 자체의 세팅
    public EnemyBulletMoveType enemyBulletMoveType;     // 
    public float initSpeed;                     // 시작속도. 일단은 정속으로 테스트, 추후 수정.
                                                //public Vector3 initMoveDirection;         // 시작이동방향. 일단 보는방향으로 테스트, 추후 수정.
                                                // 변속정보.
                                                // 변향정보. 플레이어에 유도 등 여러가지 요인으로 변수 추가 가능성 높음.
                                                // 변속정보(불연속) 리스트
                                                // 변향정보(불연속) 리스트
                                                // 나중에 필요하면 이어서 추가

    // 3. 클론 ---------------------------------//------------------------------------------------------------------ // 이 경우, PhaseSO에서 담당.
    //public NextPatternMethod[] nextPatternMethod;// 하위 탄막 생성의 조건.
    //public float[] nextPatternTimer;            // 타이머일 경우, 그 시간
    //                                            // UserTrigger일 경우, 이벤트 구독
    //                                            // 마찬가지로 테스트 이후 필요한만큼 추가
    //public List<PhaseSO.PatternHierarchy> subPatternSO; // 하위 패턴 정보 필드 추가

    // 4. 반환 ---------------------------------//------------------------------------------------------------------
    public ReleaseMethod releaseMethod;         // Pool 반환의 조건.
    public float releaseTimer;                  // 방법1. 반환까지의 타이머. 일단 이거로 테스트.
                                                // 방법2. 충돌체크. Ground를 만날 시 반환 여부. 이벤트 감지 로직은 각 탄막에서보다 Ground에서 작성하는 것이 자원을 아낄 수 있을 것으로 보임.
                                                // 방법3. 하위 탄막의 모든 세트 생성을 끝마친 경우
                                                // 방법4. 마스터의 트리거(구독)
    
    // 그리고 이 모든것을 하나의 뭉치로하여, 하위 탄막에 전해주거나 할 것으로 일단 보임.
    // 하위 탄막에 전해줄 내용 : 뭉탱이.
    // 하위 탄막이 뭉탱이를 언패킹하여, 위의 내용을 모두 적용, 하위 뭉탱이가 있으면 이를 반복.
}

public enum SpreadType
{
    None,
    Spread,
}

public enum PosDirection
{
    World,              // 마스터 또는 플레이어의 방향과 무관계한
    Forward,            // 마스터가 바라보는
    ToPlayer,           // 마스터가 플레이어를 바라볼 경우
    CompletelyRandom,   // 완전히 랜덤한 방향으로
}

public enum PosDirectionRandomType
{
    Line,               // 직선 범위에서 랜덤
    Plane               // 평면 범위에서 랜덤
}

public enum EnemyBulletToDirection
{
    World,              // 탄막의 방향과 무관계한
    MasterOut,          // 마스터와 반대방향
    MasterToPlayer,     // 마스터가 플레이어를 바라보도록
    ToPlayer,           // 탄막이 플레이어를 바라보도록
    CompletelyRandom,   // 완전히 랜덤한 방향으로
}
public enum EnemyBulletMoveType
{
    Forward,
}
//public enum NextPatternMethod // PhaseSO에서 담당
//{
//    Timer,              // 특정 시간 뒤 터뜨리기
//    WithRelease,        // 반환과 함께 터뜨리기(삭제예정)
//    UserTrigger,        // Manager 또는 Enemy에서 관리. 트리거 작동 시, 구독한 탄막들 일괄 생성
//}
public enum ReleaseMethod
{
    Timer,              // 특정 시간 뒤 터뜨리기
    WithRelease,        // 반환과 함께 터뜨리기(삭제예정)
    UserTrigger,        // Manager 또는 Enemy에서 관리. 트리거 작동 시, 구독한 탄막들 일괄 터뜨리기.
}
public enum EnemyBulletShape
{
    Linear,             // 가장 단순한 선형 사출
    Circle,             // 원형 (참고: 플레이어를 본 방향으로 원형으로 만들어 Enemy->Player 벡터로 방향을 주거나, 무작위 방향으로 원형 바깥방향으로 사출하면 자연스러울 듯.)
    Sphere,             // 구형
    Cube,               // 큐브형태. (참고: 레퍼런스 있음)
    Custom,             // 유저 입력을 받아 모양을 커스텀. Vector3리스트의 깡 입력으로 여러가지 모양을 만들 수도 있도록.
}