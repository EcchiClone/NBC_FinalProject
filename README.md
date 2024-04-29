# Null Reference Exception
## 1. 개요
- 장르 : 3D, 탄막, 슈팅, 3인칭, 커스터마이징, 액션
- 플랫폼 : PC(Windows)
- 배포 : Steam, itch.io
- 개발기간 : 2개월(기획 2주 + 2024.03.06 ~ 2024.05.01)
- 개발인원 : 4인
- 소개
    > 고도화된 실험실에서 모듈을 조합하며 전투 훈련을 받는 3D 탄막 액션 게임입니다. 플레이어는 먼 미래의 AI로서 전투 시뮬레이션에 참여합니다. 다양한 모듈을 교체하고 조합해가며 훈련을 반복합니다. 플레이어는 자신만의 모듈 조합을 가지고 3D 탄막 액션을 경험합니다.
- [게임 다운로드 - itch.io](https://ecchiclone.itch.io/null-reference-exception)
- [Wiki - 프로젝트 상세](https://github.com/JY-LemongO/NBC_1st_FinalProject/wiki)
- 소개영상 : https://www.youtube.com/watch?v=XH0-N9iOdQI
- [![Null Reference Exception Trailer](https://img.youtube.com/vi/XH0-N9iOdQI/0.jpg)](https://www.youtube.com/watch?v=XH0-N9iOdQI "Null Reference Exception Trailer")
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


