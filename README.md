# 스파르타코딩클럽 심화 10기_3조 팀 프로젝트 입니다

# The Tower of Wishes

스파르타 코딩클럽 10기, 유니티 심화 3조 팀 프로젝트 작업물입니다.
2D 탑다운 시점의 보스 레이드 중심 게임입니다. 이전에 경험해본 탑다운 슈팅 게임을 기반으로 기획되었습니다.
다양한 이펙트, 스킬 구성, 그리고 세분화된 저장 시스템 등의 기술적인 구현 요소에 집중했습니다. 반복되는 전투를 통해 레벨업 및 성장을 진행하는 흐름에, 
몰입감 있는 전투 연출과 전략적 스킬 운용이 조화를 이루는 것이 이 프로젝트의 핵심 의도입니다.

## 📷 스크린샷

![Main](https://github.com/user-attachments/assets/ed89f164-6f10-4989-91fd-2afbd456c309)



## 시연 영상 주소


## 🕹️ 기능
<details>
<summary><input type="checkbox" checked disabled> (필수) 플레이어 이동 및 조작 </summary>

![Moving](https://github.com/user-attachments/assets/0348a4c8-80bb-47bf-8c83-bbab5ff8d0f9)

- 탑 다운 환경에 맞춰 4방향으로 애니메이션과 8방향 이동이 가능합니다.
- 점프 및 대쉬시 파티클과 이펙트가 추가되었습니다.

</details>
<details>
<summary><input type="checkbox" checked disabled> (필수) 충돌 처리 및 피해량 계산 </summary>

  ![Attack](https://github.com/user-attachments/assets/b889bd80-f0d8-495c-86bb-0c056cd86dbc)

- 각자가 공격할때에 지정된 위치에 Collision Trigger 로 레이어를 왁인하겨 공격을 전달합니다.

-
</details>
<details>
<summary><input type="checkbox" checked disabled> (필수) UI/UX </summary>

![UI](https://github.com/user-attachments/assets/5464e84b-d921-427b-aa9a-a37c859066a7)

- 체력, 마나, 경험치와 같은 기본 UI 를 구성했습니다.
- 인벤토리와 스킬트리 UI를 구현했습니다.
- 스킬 사용시 쿨타임을 확인 할 수 있는 UI를 구성했습니다.


</details>
<details>
<summary><input type="checkbox" checked disabled> (필수) 인트로 씬 </summary>

![Intro](https://github.com/user-attachments/assets/9bfcd991-fc94-460f-befb-e7b2ed2bc9aa)

- 게임의 대략적인 스토리를 알 수 있는 인트로 씬을 커스텀 씬 시스템을 개발하여 구현하였습니다.
- 카메라 컷씬, 이미지, 대화 다이얼로그 기능이 있습니다.
- 대화 씬은 DoTween을 사용하여 UX경험의 질을 상승시켰습니다.
- 대화 선택지 시스템 또한 구현되어 있습니다.
  
</details>
<details>
<summary><input type="checkbox" checked disabled> (도전) 저장 및 불러오기 시스템 </summary>

![Save](https://github.com/user-attachments/assets/c5e75f0b-0ec7-45a7-89bf-fcfa357dd7b1)


- 실시간으로 데이터를 저장하는(종료시 저장) 시스템을 구성했습니다.
- 씬 위치와 적 & 플레이어 체력, 위치값 등 모든 정보를 저장하고 불러 올 수 있습니다.
- 적이나 플레이어 데이터는 Scriptable Object로, 실시간 데이터 저장은 Json 을 사용 했습니다.


</details>
<details>
<summary><input type="checkbox" checked disabled> (도전) 경험치 및 스킬 시스템 </summary>

![Skill](https://github.com/user-attachments/assets/15caaf37-0733-4e08-9589-37bb42a90dfc)

- 적 처치시 경험치 구슬을 얻게 되고, 이를 통해 레벨업을 진행 할 수 있습니다.
- 레벨업시 스킬 포인트가 주어지고 스킬 트리에 따라 스킬을 획득 -> 사용 할 수 있습니다.


</details>
<details>
<summary><input type="checkbox" checked disabled> (도전) 복잡한 적 AI </summary>

![AI](https://github.com/user-attachments/assets/4008032f-9c65-441f-ade9-f58be739a01a)

- FSM 을 통해 적 AI 를 구성했고 상황에 맞게 공격, 회피, 추격을 진행합니다.
  
</details>
<details>
<summary><input type="checkbox" checked disabled> (도전) 파티클, 셰이더 시스템</summary>


-![Effect](https://github.com/user-attachments/assets/21134fc9-3a9b-466a-a81f-09f4c5d98d19)

- URP + 셰이더, 파티클 시스템을 이용해 게임에 여러 효과들을 추가했습니다
- 폭탄이 터질때 충격파가 발생하는 이펙트를 추가했습니다.
- 정확한 타이밍에 회피하면 주변이 회색으로 변하면서 속도가 느려지는 효과를 구현했습니다.

</details>
<details>
<summary><input type="checkbox" checked disabled> (도전) 업적 시스템 </summary>

![achive](https://github.com/user-attachments/assets/caf6ec7a-565e-418e-ba39-a1997a40e5b0)

- 특정 조건을 달성하면 상단에 보여지는 업적 시스템을 작성했습니다.

</details>
<details>
<summary><input type="checkbox" checked disabled> (도전) 환경과 배경 설정 </summary>

![Map](https://github.com/user-attachments/assets/8bf66843-9563-436e-8e9d-fbf445867d75)

- 파티클 시스템과 포스트 프로세싱을 이용하여 좀 더 풍성한 환경을 구성했습니다.

</details>


## 🛠️ 기술 스택

- C#
- .NET Core 3.1
- Unity 22.3.17f1

## 🧙 사용법

1. 이 저장소를 클론하거나 PC 빌드를 다운받습니다,
2. 조작은 아래와 같습니다
   이동 : 화살표, / 스킬 : A, S, D, Q, W / 달리기 : 쉬프트 / 대쉬 : 쉬프트 두번 / 점프 : 스페이스바 / 그 외 버튼들은 마우스 클릭입니다.
3. 씬 구성은 : Opening -> Main -> Lobby -> Battle 순서입니다.
4. 저장은 게임을 끄면 자동으로 진행됩니다.

## 🙋 개발자 정보

- 이름: 설민우(팀장)
- 연락처 : sataka1853@naver.com

- 이름: 김선우(팀원)
- 연락처 : qwerty2142427@gmail.com

 - 이름: 이자연(팀원)
- 연락처 : 

- 이름: 박유빈(팀원)
- 연락처 : 

- 이름: 손종욱(팀원)
- 연락처 : 






