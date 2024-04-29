# Null Reference Exception
## 1. 개요
- 장르 : 3D, 탄막, 슈팅, 3인칭, 커스터마이징, 액션
- 플랫폼 : PC(Windows)
- 배포 : Steam, itch.io
- 개발기간 : 2개월(기획 2주 + 2024.03.06 ~ 2024.05.01)
- 개발인원 : 4인
- 소개
    > 고도화된 실험실에서 모듈을 조합하며 전투 훈련을 받는 3D 탄막 액션 게임입니다. 플레이어는 먼 미래의 AI로서 전투 시뮬레이션에 참여합니다. 다양한 모듈을 교체하고 조합해가며 훈련을 반복합니다. 플레이어는 자신만의 모듈 조합을 가지고 3D 탄막 액션을 경험합니다.
- 게임 다운로드 : [itch.io - Null Reference Exception](https://ecchiclone.itch.io/null-reference-exception)
- 소개영상 : https://www.youtube.com/watch?v=XH0-N9iOdQI
- QA 진행 : 1주, 16인 의견 수렴
  
## 2. 게임 소개 상세
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

## 3. 사용된 도구
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

## 4. 사용된 스크립팅 기술
### CoroutineManager
- Spawn매니저
### Excel Importer & DataManager
- 플레이어 모듈의 파츠 정보, 스테이지 정보, 튜토리얼 정보
### ObjectPooling
- 적 탄막, 플레이어 탄막, 적 유닛
### Modular configuration(SO)
- 적 탄막 생성, 업적 정보
### Custom Editor
- 적 탄막 커스텀
### HFSM
- 플레이어 상태, 적 상태
### URP & Shader Graph
- Sci-fi 스타일의 포스트 프로세싱 및 Shader Graph
### Enemy Path Finding
- AI Navigation 및 A*알고리즘
### Adaptive Sounds (with FMOD)
- FMOD엔진을 이용하여 환경에 따라 사운드가 능동적으로 바뀌는 적응형 사운드 적용
### Random Seed Generating Algorithm
- 퍽 지도의 생성과 저장 및 불러오기
### Localization
- Localization 패키지 활용, csv파일로 데이터 관리, 한/영 지원




-- -
## ↓↓↓ 중간발표 시점 README ↓↓↓

# NBC_1st_FinalProject
내배캠 최종 팀 프로젝트 1조

## 프로젝트 개요

1. 프로젝트명 : NRE - Null Reference Exception
2. 장르 : 3D, 탄막, 슈팅, 3인칭, 커스터마이징, 액션
3. 소개
   - 고도화된 실험실에서 모듈을 조합하며 전투 훈련을 받는 3D 탄막 액션 게임입니다.
     - 플레이어는 먼 미래의 AI로서 전투 시뮬레이션에 참여합니다.
     - 다양한 모듈을 교체하고 조합해가며 훈련을 반복합니다.
     - 플레이어는 자신만의 모듈 조합을 가지고 3D 탄막 액션을 경험합니다.
![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/83f5852c-78ab-4d3d-bff8-81b7d0675f19)

## 게임 사이클
![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/c4fb0a4c-6a27-4205-9024-b8ea30bd832e)

### 메뉴 선택 장면
- 메인 메뉴 : 플레이어의 모듈과 Perk(테크) 관리를 위한 장면, 업적 목록을 확인하기 위한 장면으로 이동할 수 있으며, 설정과 게임 시작 버튼이 위치한다.
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/48f04c9e-9b76-44b8-9e5f-d1b72ecf6763)
- 모듈 선택 : 플레이어가 전투에서 사용할 상하체의 모듈을 관리한다.
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/c4c341ed-8b54-471f-9611-97c7058fdf4a)
- Perk 선택 : 플레이어가 사용할 특성을 관리한다. Tier1부터 Tier3까지의 특성이 있으며, 매 게임마다 랜덤성을 가지고 생성된다. (리세마라를 위한 초기화도 여기서 가능하면 좋을 듯)
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/2e62203f-2f38-4c02-ab72-13a0aade61d7)
- 업적(구현 예정) : 진행중인 업적과 달성한 업적을 확인 할 수 있으며, 달성 정도에 따라 업적포인트를 획득 할 수 있다.
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/16e79bfc-e8d2-480c-b4eb-42ab33df668f)
- 설정 : 사운드의 조절, 데이터의 초기화 등을 관리한다.
- 게임 시작 : 플레이어가 선택한 모듈과 Perk를 갖고 게임 스테이지로 이동한다

### 게임 스테이지 장면
- 게임목적 : 계속해서 나타나는 미니언과 보스를 처치하여 높은 점수를 획득하고 연구포인트를 획득한다. 플레이어는 연구포인트와 업적포인트를 사용하여 플레이어 모듈과 Perk를 수집한다.
- 플레이어 : 장착한 모듈에 따라 사용할 수 있는 무기와 움직임이 달라지고, Perk 특성에 따른 능력을 가진다. 적의 공격을 피하며 체력을 관리하고 적을 처치한다.
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/cf4751f0-36a6-425d-a7e2-e5e63b9a4c87)
- 적(Minion) : 낮은 체력을 가지며 단순한 공격패턴을 구사한다.(구현 예정)
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/066b2642-59a3-4713-894e-b5647ccb1110)
- 적(Boss) : 높은 체력을 가지고 다양하고 위협적인 공격 패턴을 구사한다.
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/d5f55510-d3f6-4eba-a3fd-29aaacd6f928)
  ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/7883cc0c-4997-478f-90a9-3efd8cfcb9f6)

## 역할 분담
- 국지윤 - Player Module 시스템, Data 관리(미구현)
- 이도현 - 적 탄막 시스템, 업적 시스템(미구현)
- 강성원 -
- 추민규 - 

## 구현 상세
- 국지윤
  - 플레이어 모듈 선택
    - 프로젝트에 쓰일 하체, 상체 파츠를 분리하여 ModuleManager 에서 총 관리.
    - Dictionary를 사용해 해당 타입(Key)에 배열로 저장. (프리팹 저장) 플레이어의 본체가 되는 Module 오브젝트 - 하체 역할의 Lower - 상체 역할의 Upper 순으로 모듈을 생성.
    - 각 파츠 선택 창에서 Button UI에 현재 Dictionary에 저장된 Upper or Lower 파츠를 순서대로 나열하여 선택가능.
    - ScriptableObject를 사용해 파츠당 스펙 수치를 할당.
    - 파츠 변경 시 변경된 파츠의 정보가 ModuleManager의 CurrentUpper / CurrentLower 로 저장. PlayScene으로 넘어갈 때 해당 파츠들이 생성.
      ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/ede5727e-03c7-4af1-a03e-701124f22e7b)
      ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/94a32a18-94fa-44e4-ae80-5da382f81cd8)
      ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/ad72116f-7648-4eb2-afa6-732dcceb4535)
      ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/58685edf-75d0-4c40-bbba-cde1836608ec)
      ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/cda51f94-870c-442c-9c65-54ed0649dfe1)      
      
  - 플레이어 로직
    - PlayerStateMachine을 이용하여 각 상태에 해당하는 로직을 수행
    - 큰 범주로 Non-CombatMode / CombatMode 상태로 나뉘어 이동 및 카메라 이동이 바뀜
    - Non-CombatMode에서 FreeLook 카메라 시점을 가지고 CombatMode에서 카메라가 바라보는 곳으로 Aiming을 함
    - LockOn 시 카메라 시점이 변경되며 CombatMode 시 LockOn 된 대상을 바라봄
      ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/1c621278-a8e9-40b8-a1bd-3f7655a6ec0b)

  - UI연동
    - Module 선택과 Player 조작에 따른 UI의 변화가 동시에 일어나기 때문에 Action을 이용하여 반응할 수 있도록 함
    - 다만 변화해야 하는 UI의 종류가 다양하고 그 수가 많아 전역으로 관리하는 ActionManager를 만들어 총괄하고 구독 및 호출 할 수 있는 클래스를 만듦
    - Manager들은 Managers 클래스에서 모두 싱글톤으로 관리하기 때문에 Static 및 Singleton을 내부에서 구현이 불필요
      ![image](https://github.com/JY-LemongO/NBC_1st_FinalProject/assets/122505119/3874388e-2b17-4553-99a0-50c9cf5e5470)

---

- 이도현
  - 탄막 시스템
    - 적이 사용할 공격 패턴을 구현하였다.
    - 기술스택 요약 - 오브젝트 풀링 및 최적화, 스크립터블 오브젝트 및 모듈화, 커스텀 에디터
    - 오브젝트 풀링과 큐를 통한 관리를 통해 대량의 탄막에 대한 최적화
    - 스크립터블 오브젝트를 사용하여 패턴과 페이즈의 데이터를 관리하고, 패턴을 조합하여 페이즈를 구성할 수 있도록 구현
    - 커스텀 에디터를 통한 개발 편의성 향상

 (사진1)  
 (사진2)  
 (사진3)

  - 업적 시스템(구현 예정)
    - 게임을 진행하며 다양한 종류의 업적을 달성 할 수 있으며, 달성 정도에 따라 업적포인트를 획득 할 수 있다. 해당 업적포인트는 해금 요소에 사용될 수 있다.
    - 기능 구현에 데이터 관리 및 모듈화 등의 스킬이 사용될 수 있다.

---

- 강성원
  - 적 AI(FSM)
    - 스테이트 머신...넌 뭐가 잘났닝
  - UI(자동화)
    - 루키스 짱짱

---

- 추민규
  - Perk
    - 랜덤 시드를 바탕으로 일정한 퍼크의 배치를 구현
      - 원소가 늘어나는 중복 순열 알고리즘 개발
      - 같은 시드를 넣으면 같은 배치가 나오는 구조의 퍼크
  - Sound(Fmod)
    - Fmod Studio <-> Unity 간 통신과 기본 세팅 구현
      - 볼륨 조절 시스템 구현
      - AudioManager & FMODEvents 스크립트 구현


