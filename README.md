# Null Reference Exception
## 1. 개요
- 장르 : 3D, 탄막, 슈팅, 3인칭, 커스터마이징, 액션
- 플랫폼 : PC(Windows)
- 배포 : Steam, itch.io
- 개발기간 : 2개월(기획 2주 + 2024.03.06 ~ 2024.05.01)
- 개발인원 : 4인(국지윤, 이도현, 강성원, 추민규)
- 소개
    > 고도화된 실험실에서 모듈을 조합하며 전투 훈련을 받는 3D 탄막 액션 게임입니다. 플레이어는 먼 미래의 AI로서 전투 시뮬레이션에 참여합니다. 다양한 모듈을 교체하고 조합해가며 훈련을 반복합니다. 플레이어는 자신만의 모듈 조합을 가지고 3D 탄막 액션을 경험합니다.
- [게임 다운로드 - itch.io](https://ecchiclone.itch.io/null-reference-exception)
- 소개영상 : [임시 영상](https://www.youtube.com/watch?v=XH0-N9iOdQI)
- [![Null Reference Exception Trailer](https://img.youtube.com/vi/XH0-N9iOdQI/0.jpg)](https://www.youtube.com/watch?v=XH0-N9iOdQI "Null Reference Exception Trailer")
- QA 진행 : 1주, 16인 의견 수렴

## 2. 사용된 스크립팅 기술
<details>
<summary><h3>CoroutineManager (국지윤)</h3></summary>
    
사용처 : SpawnManager
    
스테이지 진행을 위한 로직에서 SpawnManager를 사용하였고 스테이지 진행을 코루틴 호출로 단계별 로직이 실행될 수 있도록 하고자 했지만 SpawnManager는 MonoBehaviour를 상속받지 않은 클래스이므로 코루틴을 호출해줄 클래스가 필요했고 전역으로 사용할 CoroutineManager 클래스를 만들고 해당 클래스에서 코루틴을 대신 호출해주는 방식을 채택하였습니다.
    
<ul>
<li>SpawnManager Code 스크린샷</li>

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/ab581568-c6c4-4028-8cce-0e15a108a704)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/09b9f52d-d7cf-4ebd-a7b8-7b8504d20d3f)

<li>CoroutineManager Code 스크린샷
    
![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/9690f940-21f5-4fca-8c58-7b4e0db5a98f)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/f49de356-4336-4263-abdb-fd71a789a19e)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/3574d2b5-720f-4fbc-8826-fb453711e07f)

</ul>

</details>


<details>
<summary><h3>Excel Importer & DataManager (국지윤)  </h3></summary>

사용처 : 플레이어 모듈의 파츠 정보, 스테이지 정보, 튜토리얼 정보

데이터 관리 시 기존의 방법은 Scriptable Object가 필요한 오브젝트의 인스펙터에 직접 연결해 정보를 할당하는 방식을 사용하였지만 관리가 힘들고 많은 수의 SO를 생성해 할당하는 과정에서 휴먼에러가 발생할 수 있는 문제가 있었습니다.

이를 Excel Importer를 활용해 필요한 데이터를 엑셀 시트로 관리하고 시트를 SO로 변환해 데이터 관리의 편의성을 높였으며 변환된 SO를 데이터 매니저를 활용해 Dictionary 에 저장해두고 필요한 때에 동적으로 사용할 수 있도록 접근성을 높였습니다.

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/7a77ba48-78fc-4189-8dff-12058f65a58a)

</details>


<details>
<summary><h3>URP & Shader Graph (국지윤)  </h3></summary>

사용처 : Sci-fi 스타일의 포스트 프로세싱 및 Shader Graph

저희 게임은 공상과학 느낌의 컨셉을 원했고 그 기능으로 네온효과가 필수였습니다. 유니티에선 URP 템플릿의 PostProcessing과 Shader Graph를 사용하여 Emission 효과와 Dissolve 효과를 줄 수 있었고 결과적으로 Sci-Fi 게임 느낌을 살릴 수 있었습니다.

<ul>
<li>Emission

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/6bd136b1-579f-40b3-ab8b-f382d2db7614)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/e95775ff-c4ff-4966-8e7f-92a72b82926a)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/da3aadaf-8255-4070-9ab3-410e370fa636)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/e2bcc233-af2c-4894-b2f1-6e18787b2458)

<li>ShaderGraph

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/874f8012-6474-4374-833b-ee2a463b538c)

![셰이더](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/1cbe7111-fd09-4791-8c86-d24830aeb237)

</ul>
</details>


<details>
<summary><h3>ObjectPooling (이도현)  </h3></summary>
    
사용처 : 적 탄막, 플레이어 탄막, 적 유닛
    
씬 진입 시, 많은 횟수의 생성과 삭제를 필요로 하는 오브젝트를 미리 준비하여 Instantiate와 Destroy사용을 최소화 하였습니다. 현재 프로젝트에서 사용된 Pooler는 추가적으로 아래와 같은 특징을 갖습니다.

<ul>
<li>오브젝트의 특성에 따라 그에 맞는 Pooler를 사용하였습니다.

<blockquote>효과 : 한 개의 Pooler Prefab에 모든 Pooled 오브젝트들을 모아두었던 방식에서, ‘Enemy’, ‘UI’, ‘PlayerBullets’ 등 특성에 맞는 Pooler를 각자 마련함으로써 다양한 씬에서 가볍게 사용될 수 있도록 활용성을 높였습니다.</blockquote>

<blockquote>개선할 점 : 현재의 Pooler는 반드시 그 속성을 필요로 하며 한 개의 씬에 같은 속성을 가진 Pooler를 두 개 이상 배치할 수 없습니다. 이 두 가지의 제한사항을 개선할 여지가 있습니다.</blockquote>

<li>Pool 생성 시, 프레임 당 생성 가능한 오브젝트 수를 제한했습니다.

<blockquote>효과 : Queue와 비동기를 사용하여 프레임당 Pool 생성량에 제한을 가한 결과, Pooler에 속한 Pool의 크기에 따라 기하급수적으로 늘어나던 씬의 로딩시간을 없애고, 각 프레임 당 부담이 되지 않을 정도로 Instantiate를 분배함으로써 게임의 지연속도를 최소화 할 수 있었습니다.</blockquote>

<blockquote>개선할 점 : 사용자 기기의 성능에 따라 프레임 당 적절한 오브젝트의 생성(Instantiate) 수에 차이가 있지만, 이러한 요인에 따라 동적으로 생성 제한량을 조절하지 않았습니다. 각 프레임 당 일정 시간을 경과하였을 경우 생성을 중지하도록(다음 프레임에서 작업을 이어나가도록) 하여 개선할 수 있을까 고민하고 있습니다.</blockquote>
</ul>
</details>


<details>
<summary><h3>Modular configuration(SO) (이도현)  </h3></summary>
    
사용처 : 적 탄막 생성 정보, 업적 정보

적의 공격 방식(탄막 생성) 프리셋 및 업적 내용 구성에 SO를 통한 모듈식 구성을 사용하였습니다.

적이 사용하는 공격로직은 패턴(Pattern) 단위로 사용이 가능하고, 패턴을 연속적 또는 연쇄적으로 배치한 페이즈(Phase) 단위로써도 사용이 가능합니다.

업적은 Achievement내에 TaskGroup와 Reward, TaskGroup은 여러 개의 Task로, Task는 Target과 필요 횟수 그리고 계산 방식SO로 구성되는 등 세분화된 SO를 조합하여 완성된 AchievementSO 하나를 만듭니다.

<blockquote>효과 : 모듈식 구성을 통해 적의 탄막 공격 모양과 반복적인 패턴 발현에 대해 효과적으로 구현이 가능했고 새로운 프리셋을 만드는 과정도 매우 간단해졌다. 업적 시스템의 경우는 업적단위, Task단위 등에서 독립적으로 할 일을 명확히 하여 이를 조합하는 방식을 사용했기 때문에 관리가 용이하였다. </blockquote>

<blockquote>개선할 점 : 현재 미리 준비된 탄막과 업적 데이터는 모두 SO로 관리되고있다. 각 모듈단위마다 데이터시트를 작성하여 관리하는 방식을 사용한다면 더 보기 쉽고 비개발자 입장에서도 관리가 쉬운 환경이 될 것이다. </blockquote>
</details>


<details>
<summary><h3>Custom Editor (이도현)  </h3></summary>
    
사용처 : 적 탄막 생성 정보가 담긴 SO의 Inspector 커스텀

에디터의 Inspector에서 보여질 속성을 더 잘 설명(번역, 상세설명 등)하고, 다른 속성의 상태에 따라 정해야 할 속성값들을 선택적으로 Inspector에서 보여주기 위해 사용하였습니다.

<details>
    
<summary>탄막 생성 정보 SO의 Inspector에서의 적용 예시</summary>
    
![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/bdf6f5f6-ab8a-42de-94b3-4ba1b2d6eabc)

</details>

<blockquote>
개선할 점 : Inspector를 그리는 데에 마주한 몇 가지 문제를 해결하지 못하였습니다.

<li>List 또는 Array의 폼을 기존 인스펙터와 같이 프레임을 갖도록 하기
    
<li>List 내 List의 속성에 대한 설정(이름, 조건부 표시 등)
    
<li>불필요하게 추가적으로 생성되는 Inpector 내 UI 삭제
</blockquote>

</details>


<details>
<summary><h3>Localization (이도현)  </h3></summary>
    
사용정보 : Localization 패키지 활용, csv파일로 데이터 관리, 한/영 지원

Unity Registry의 Localization 패키지를 활용하였고, csv파일로 locale별 표시 데이터를 관리합니다.

이미 준비된 씬 내 오브젝트와 프리팹에는 전용 컴포넌트를 부착하여 사용하며, 스크립팅을 통해 표시되는 텍스트는 전용 유틸 클래스(오브젝트에 컴포넌트를 부착)를 준비하여 활용합니다.

<ul>
<li>사용 예시는 아래와 같습니다.
    
![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/acc8714a-4f3c-4035-9214-e30cbd0c1b5e)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/08013209-0861-42c8-afe7-e45236dde289)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/c8254167-1118-43dc-9e33-e77b2cb8d71a)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/1ddf0cd1-76db-4fc4-8e9c-2d4778789143)

</ul>

<blockquote>개선할 점 : 구글 스프레드 시트를 연동할 수 있다는 사실을 들어 알고 있습니다.
    이를 통해 번역 언어 확장과 언어 번역의 자동화에 대해 개선이 이루어질 수 있다고 생각됩니다.</blockquote>
    
</details>


<details>
<summary><h3>HFSM (강성원, 국지윤) </h3></summary>
    
사용처 : 적 상태(강성원), 플레이어 상태(국지윤)

FSM은 상태가 많아지면 구조가 복잡해지고 관리가 힘들어지는 한계가 있습니다.

이러한 이유로 상태들을 계층으로 관리할 수 있는 HFSM 구조를 채택했습니다.

<ul>
<li>Player HFSM

플레이어는 상태에 따라 애니메이션 및 동작 로직이 달라야 했고 계층을 이루는 구조가 필요했습니다. 이를 적용할 방법으로 HFSM 이 적합하다고 판단되어 HFSM을 사용하여 각 상태에 따라 동작을 정의해 현재 상태를 직관적으로 알 수 있었습니다.

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/1fd96301-6acb-4854-8add-bb82273fefce)

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/df97f71d-2dc0-4f10-872d-1c2d1de50487)

<li>Enemy HFSM

자율적으로 행동하는 Enemy는 Alive 상태일 때 연결되는 상태가 계층으로 정의되어 HFSM을 적용하였습니다.

![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/21221633/1f62016a-33e5-4261-a6d2-71360c862307)

</ul>
</details>


<details>
<summary><h3>Enemy Path Finding (강성원)  </h3></summary>
    
사용정보 : AI Navigation 및 A*알고리즘 사용

등장하는 적의 길찾기 기능에는 유니티에서 제공하는 AI Navigation과 A*알고리즘 2가지를 사용하였습니다.

<ul>
<li>AI Navigation (NavMeshAgent)

지상에서 걷는 몹이 다양한 지형에 대응하는 길 찾기 기능이 필요했습니다.

유니티에서 제공해주는 AI Navigation를 사용하여 여러 지형에 대응하여 플레이어를 쫓아가는 몬스터를 구현할 수 있었습니다.

<li>A* 알고리즘

❓사용 이유❓

구체 형태의 오브젝트가 플레이어를 향해 자연스럽게 굴러가도록 하기 위해 NavMeshAgent를 사용했지만 물리적인 적용에 있어서 한계가 있어 A* 알고리즘을 활용하기로 결정했습니다.

❗얻은 효과❗

A* 알고리즘은 목표지점까지의 경로를 제공합니다. 이 경로 정보를 바탕으로 구체에 적절한 힘을 가하여 목적지까지 자연스럽게 이동하도록 구현했습니다.

</ul>
</details>


<details>
<summary><h3>Adaptive Sounds (with FMOD) (추민규)  </h3></summary>
    
사용정보 : FMOD엔진을 이용하여 환경에 따라 사운드가 능동적으로 바뀌는 적응형 사운드 적용

FMOD 엔진을 이용한 환경에 따라 사운드가 능동적으로 바뀌는 적응형 사운드를 적용했습니다.

<ol>
<li>보스 등장과 소멸 시 필드 BGM과 보스 BGM이 자연스럽게 박자에 맞춰 Crossfade-Transition이 되도록 구현했습니다.
    
<li>플레이 중 BGM이 자연스럽게 구간 반복이 되도록 Crossfade를 적용했습니다.
    
<li>게임 플레이 중 ESC를 눌러 일시정지 하게 되면, Low pass filter 효과를 걸어주게 되어 BGM에 먹먹한 효과를 내게 만들었습니다.
    
<li>개틀링 건 발사 SFX와 같이 하나의 Source가 빠르게 반복되어 단조롭고 인위적인 소리라고 인식되는 것을 방지하기 위해 발사될 때마다 랜덤으로 Pitch와 Volume 값을 일정 범위 내에서 변경하게 하여 사운드의 무작위성을 더했습니다.
</ol>

<li>FMOD로 적용한 기술 시연 영상(1~3번)

https://www.youtube.com/watch?v=BEy7qAcn-5E

</details>


<details>
<summary><h3>Random Seed Generating Algorithm (추민규)  </h3></summary>
    
사용처 : 퍽 지도의 생성과 저장 및 불러오기

<li>게임을 처음 시작하거나 중앙의 Origin Perk를 구성 초기화하여 새롭게 랜덤한 시드를 얻어 새로운 퍼크의 구조를 얻을 수 있습니다.

<li>설계된 구조의 경우의 수는 134,104,039,237,168으로 숫자가 매우 크지만 하나의 시드가 하나의 구조를 무조건 일대일 대응으로 가지고 있어 같은 시드가 나오면 같은 구조가 되는 시스템을 가지고 있습니다.

</details>

### README.md에 적지 못한 스크립팅 기술 추가 기재(편집 여지를 남기기 위함, 각자의 문서의 작성)
- [(링크)프로젝트 내에서 사용한 추가 스크립팅 기술(국지윤)](https://github.com/JY-LemongO/NBC_1st_FinalProject/wiki/%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EB%82%B4%EC%97%90%EC%84%9C-%EC%82%AC%EC%9A%A9%ED%95%9C-%EC%B6%94%EA%B0%80-%EC%8A%A4%ED%81%AC%EB%A6%BD%ED%8C%85-%EA%B8%B0%EC%88%A0(%EA%B5%AD%EC%A7%80%EC%9C%A4))
- [(링크)프로젝트 내에서 사용한 추가 스크립팅 기술(이도현)](https://github.com/JY-LemongO/NBC_1st_FinalProject/wiki/%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EB%82%B4%EC%97%90%EC%84%9C-%EC%82%AC%EC%9A%A9%ED%95%9C-%EC%B6%94%EA%B0%80-%EC%8A%A4%ED%81%AC%EB%A6%BD%ED%8C%85-%EA%B8%B0%EC%88%A0(%EC%9D%B4%EB%8F%84%ED%98%84))
- [(링크)프로젝트 내에서 사용한 추가 스크립팅 기술(강성원)](https://github.com/JY-LemongO/NBC_1st_FinalProject/wiki/%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EB%82%B4%EC%97%90%EC%84%9C-%EC%82%AC%EC%9A%A9%ED%95%9C-%EC%B6%94%EA%B0%80-%EC%8A%A4%ED%81%AC%EB%A6%BD%ED%8C%85-%EA%B8%B0%EC%88%A0(%EA%B0%95%EC%84%B1%EC%9B%90))
- [(링크)프로젝트 내에서 사용한 추가 스크립팅 기술(추민규)](https://github.com/JY-LemongO/NBC_1st_FinalProject/wiki/%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EB%82%B4%EC%97%90%EC%84%9C-%EC%82%AC%EC%9A%A9%ED%95%9C-%EC%B6%94%EA%B0%80-%EC%8A%A4%ED%81%AC%EB%A6%BD%ED%8C%85-%EA%B8%B0%EC%88%A0(%EC%B6%94%EB%AF%BC%EA%B7%9C))

## 3. 게임 컨텐츠 소개
### [(링크)모든 장면의 상세 설명(편집중)](https://github.com/JY-LemongO/NBC_1st_FinalProject/wiki/%EA%B2%8C%EC%9E%84-%EC%9E%A5%EB%A9%B4-%EC%83%81%EC%84%B8)
### 플레이어 커스텀 모듈
- 상체, 하체, 무장(팔), 무장(어깨) 각 부위에 대한 파츠를 선택
### 능력 강화(Perk)
- 플레이어 능력치와 특수능력에 관여하는 강화 시스템
### 전투 웨이브
- 게임 시작 시 전투 튜토리얼, 수십 개의 적 소환 웨이브로 이루어진 스테이지
### 업적 달성
- 도전과제를 달성하여 파츠 해금 및 업적포인트 획득
### 환경설정
- 사운드 상세 설정, 그래픽 설정, 언어 설정

## 4. 사용된 도구
### 협업 도구 & 언어
- Git, Github, Github Desktop
- C#
### 개발 환경
- Unity 2022.3.2f1
- Visual Studio
- Windows10
### 데이터 관리
- SO imported from Excel DataSheet
- SO in Inspector
- Json
- PlayerPrefs
### 디자인
- Blender 4.0
- Adobe Photoshop
- Figma
- Aseprite
### 사운드
- FMOD Studio 2.02
- FL Studio 21
