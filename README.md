# 칵테일러 - 조주기능사 레시피
![](https://github.com/Sundrago/Cocktailor/blob/2bcae95e3593a56407b9035aec09faf9661fb72a/Docs/00_1.png) | ![](https://github.com/Sundrago/Cocktailor/blob/2bcae95e3593a56407b9035aec09faf9661fb72a/Docs/00_2.png) | ![](https://github.com/Sundrago/Cocktailor/blob/2bcae95e3593a56407b9035aec09faf9661fb72a/Docs/00_3.png)
:---:|:---:|:---:

<p align="center"><a href="https://apple.co/3ijIa9V">
<img src="https://github.com/Sundrago/Cocktailor/blob/15a867f85fc3829dc0730dccfe3db2180e624dce/Docs/appstore.png" width="200"">
</a></p>

<p align="center"><a href="https://play.google.com/store/apps/details?id=net.sundragon.cocktail">
<img src="https://github.com/Sundrago/Cocktailor/blob/15a867f85fc3829dc0730dccfe3db2180e624dce/Docs/playstore.png" width="200"">
</a></p>

&nbsp;
<hr>
&nbsp;

### 개요
- 개발기간: 2022.01
- 개발 환경 : Unity 2021.3 LTS
- 플랫폼: Android/ iOS
- 장르: 교육/ 식음료
- 개발 인원: 1명 (개인 프로젝트)

### 성과
- 누적 다운로드: 15k+
- 구독 요금제 이용자 : 500명+

### 한줄소개
-  조주기능사 자격증 시험 준비를 위한 칵테일 레시피 암기앱

### 개발배경
- 조주기능사 자격증 시험 공부하면서 칵테일 레시피를 위한 암기장이 없어 직접 개발되었습니다. 처음에는 무료 레시피를 제공하는 간단한 앱이었지만, 사용자들의 긍정적인 반응과 피드백을 바탕으로 '가상 시험'과 '암기 노트' 등의 기능을 추가하여 부분 유료 앱으로 전환했습니다.
- 지속적인 업데이트를 통해 수험생들이 필요로 하는 기능들을 갖추게 되면서 조주기능사 자격증 시험 준비하는 수험생들 사이에서 인기를 얻고 있습니다.

<br />
<hr>
<br />

### 주요 기능 소개
<!-- 01 -->
![](https://github.com/Sundrago/Cocktailor/blob/6ffbe8952ce8ce3946b103891fcc141d14e5aace/Docs/01_1.jpeg) | ![](https://github.com/Sundrago/Cocktailor/blob/24365d5bd2aa754f2ea8e12365fe06e225134654/Docs/01.gif) | ![](https://github.com/Sundrago/Cocktailor/blob/6ffbe8952ce8ce3946b103891fcc141d14e5aace/Docs/01_2.jpeg)
:---:|:---:|:---:
#### 레시피 카드
> 조주기능사 실기 시험 출제 범위에 해당하는 40종의 레시피를 암기 카드 형태로 넘겨서 볼 수 있습니다. 각 칵테일 별로 일러스트 이미지를 제공하며, 글라스/기법/가니쉬/레시피를 한눈에 볼 수 있도록 디자인하였습니다.  
>> [Assets/Scripts/Features/RecipeViewer/RecipeCard](https://github.com/Sundrago/Cocktailor/tree/b99854d06ec347b44b16480ff70c32a6206560a7/Assets/Scripts/Features/RecipeViewer/RecipeCard)  
>> [Assets/Scripts/Features/RecipeViewer/RecipeCard/RecipeCardController.cs](https://github.com/Sundrago/Cocktailor/blob/42bc42df7a6ca030df1a005b96b2b471a0ca774d/Assets/Scripts/Features/RecipeViewer/RecipeCard/RecipeCardController.cs)
#### JSON 역직렬화
> 조주기능사 실기 레시피 관리 시스템은 매년 개정되는 레시피 데이터를 유연하게 처리하기 위해 JSON 기반 데이터 저장 및 역직렬화 방식을 채택했습니다. 앱 실행 시 JSON 데이터를 역직렬화하여 레시피 정보를 메모리에 로드하고 사용자에게 제공하는 방식으로 새로운 레시피 추가, 기존 레시피 수정, 레시피 순서 변경 등의 변경 사항을 코드 변경 없이 처리할 수 있습니다.
>>[Assets/Resources/cocktailRecipe(2024).json](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Assets/Resources/cocktailRecipe(2024).json)  
>>[Assets/Scripts/Core/CocktailRecipeManger.cs](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Assets/Scripts/Core/CocktailRecipeManger.cs)
<br />

<!-- 02 -->
![](https://github.com/Sundrago/Cocktailor/blob/3558e5cc8519f5441ee20a4bc5499ca1613d87ad/Docs/02_1.jpeg) | ![](https://github.com/Sundrago/Cocktailor/blob/3558e5cc8519f5441ee20a4bc5499ca1613d87ad/Docs/02.gif) | ![](https://github.com/Sundrago/Cocktailor/blob/3558e5cc8519f5441ee20a4bc5499ca1613d87ad/Docs/02_2.jpeg)
:---:|:---:|:---:
#### 암기장 모드
>  암기장 모드에서는 모든 정보가 가려진 상태로 제공됩니다. 해당 항목을 클릭했을 때 내용을 확인할 수 있는 모드로, 레시피를 학습하고 암기하는 데 도움을 제공합니다.
>> [Assets/Scripts/Features/RecipeViewer/RecipeViewerPanel.cs](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Assets/Scripts/Features/RecipeViewer/RecipeViewerPanel.cs)
>> [Assets/Scripts/Features/RecipeViewer/RecipeCard/RecipeCardVisibilityController.cs](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Assets/Scripts/Features/RecipeViewer/RecipeCard/RecipeCardVisibilityController.cs)
#### 메모장 기능과 유저 데이터 관리
>  칵테일별 메모 기능을 통해 사용자는 각 칵테일에 대한 개인적인 정보와 노트를 저장할 수 있습니다. 또한, PlayerData 클래스는 칵테일 별 암기 여부, 유저 노트 등과 같은 사용자 데이터를 관리합니다. 현재는 PlayerPreferences를 사용하여 기기에 데이터를 저장하지만, 향후 클라우드 동기화 기능 추가가 용이합니다.
>> [Assets/Scripts/Utility/PlayerData.cs](https://github.com/Sundrago/Cocktailor/blob/42bc42df7a6ca030df1a005b96b2b471a0ca774d/Assets/Scripts/Utility/PlayerData.cs)
<br />

<!-- 03 -->
![](https://github.com/Sundrago/Cocktailor/blob/3558e5cc8519f5441ee20a4bc5499ca1613d87ad/Docs/03_1.png) | ![](https://github.com/Sundrago/Cocktailor/blob/3558e5cc8519f5441ee20a4bc5499ca1613d87ad/Docs/03.gif) | ![](https://github.com/Sundrago/Cocktailor/blob/3558e5cc8519f5441ee20a4bc5499ca1613d87ad/Docs/03_2.png)
:---:|:---:|:---:
#### 암기 완료 여부 체크
> '다 외운 레시피'와 '못 외운 레시피'를 표시하고 따로 구분할 수 있습니다. 전체 칵테일 리스트에서 "못 외운 레시피"만 따로 모아서 확인하거나, 테스트(모의 시험) 기능에서 출제 범위를 직접 선택하여 문제를 풀 수도 있습니다.
>>[Assets/Scripts/Features/RecipeViewer/RecipeCard/RecipeCardMarkerController.cs](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Assets/Scripts/Features/RecipeViewer/RecipeCard/RecipeCardMarkerController.cs)
<br />

<!-- 04 -->
![](https://github.com/Sundrago/Cocktailor/blob/438c0fcad55d61eb19d1991b915aff3443826ea9/Docs/04_1.jpg) | ![](https://github.com/Sundrago/Cocktailor/blob/438c0fcad55d61eb19d1991b915aff3443826ea9/Docs/04.jpg) | ![](https://github.com/Sundrago/Cocktailor/blob/438c0fcad55d61eb19d1991b915aff3443826ea9/Docs/04_2.jpg)
:---:|:---:|:---:
#### 모의고사 테스트
> 조주 기능사 실기 시험과 동일한 조건에서 모의 테스트를 볼 수 있습니다. 선택한 출제 범위에서 세 개의 칵테일이 문제로 제시되고, 제한 시간 7분 안에 각 칵테일 레시피를 완성해야 합니다. 시험 결과는 합격 여부만 먼저 나타나며, 광고를 시청하거나 구독 요금제를 이용할 경우 오답 및 답안을 확인할 수 있습니다.  
>> [Assets/Scripts/Features/Quiz](https://github.com/Sundrago/Cocktailor/tree/42bc42df7a6ca030df1a005b96b2b471a0ca774d/Assets/Scripts/Features/Quiz)  
>> [Assets/Scripts/System/AdManager.cs](https://github.com/Sundrago/Cocktailor/blob/42bc42df7a6ca030df1a005b96b2b471a0ca774d/Assets/Scripts/System/AdManager.cs)

#### 재료, 레시피, JSON, 데이터 검증
> UI를 최적화하고 다양한 선택 옵션들을 제공하기 위해 100가지 이상의 재료, 기법, 가니시 데이터를 JSON 형식으로 관리했습니다. 또한, 칵테일 레시피와 용량 선택 과정을 직관적이고 사용하기 쉬운 디자인으로 구성하고, 칵테일 데이터와 레시피 재료 옵션 간의 데이터 무결성을 유지하기 위한 검증 로직을 추가했습니다.  
>> [Assets/Resources/ingredientTypes.json](https://github.com/Sundrago/Cocktailor/blob/42bc42df7a6ca030df1a005b96b2b471a0ca774d/Assets/Resources/ingredientTypes.json)  
>> [Assets/Scripts/Core/CocktailRecipeIngredientManager.cs](https://github.com/Sundrago/Cocktailor/blob/42bc42df7a6ca030df1a005b96b2b471a0ca774d/Assets/Scripts/Core/CocktailRecipeIngredientManager.cs)
<br />

<!-- 04B -->
![](https://raw.githubusercontent.com/Sundrago/Cocktailor/8d9d9b7cd072288e14b55e9cacfefc64834245b3/Docs/04_3.jpg)
#### UI 흐름 구성
> 다양한 칵테일 재료, 기법, 가니시 등 100가지 이상의 선택지를 제공하면서도 깔끔하고 사용자 친화적인 UI를 구현하기 위해 많은 고민을 했습니다.
> > [Assets/Scripts/UI](https://github.com/Sundrago/Cocktailor/tree/8d9d9b7cd072288e14b55e9cacfefc64834245b3/Assets/Scripts/UI)
<br />

<!-- 05 -->
![](https://github.com/Sundrago/Cocktailor/blob/438c0fcad55d61eb19d1991b915aff3443826ea9/Docs/05_1.jpg) | ![](https://github.com/Sundrago/Cocktailor/blob/438c0fcad55d61eb19d1991b915aff3443826ea9/Docs/05.jpg) | ![](https://github.com/Sundrago/Cocktailor/blob/438c0fcad55d61eb19d1991b915aff3443826ea9/Docs/05_2.jpg)
:---:|:---:|:---:
#### 구독 요금제
>  광고를 제거할 수 있는 기능을 추가해달라는 유저 피드백이 많았습니다. 그래서 2023 개정 과정을 업데이트 하면서 칵테일러PRO(구독 요금제)를 도입하였습니다. PRO 이용자는 광고제거 이외에도 레시피별 메모장을 작성할 수 있는 기능, 테스트 범위를 직접 선택할 수 있는 기능, 못 외운 레시피만을 모아 볼 수 있는 기능을 이용할 수 있도록 업데이트 하였습니다. 매 시험 기간마다 구독 요금제 이용자 수가 증가하고 있는 추세를 보이고 있습니다.  
>   인앱 결제 모듈의 경우 iOS와 Android를 동시에 지원하기 위해 외부 에셋(Essential Kit)을 사용했습니다.
>> [Assets/Scripts/System/SubscriptionManager.cs](https://github.com/Sundrago/Cocktailor/blob/42bc42df7a6ca030df1a005b96b2b471a0ca774d/Assets/Scripts/System/SubscriptionManager.cs)
<br />

<!-- 06 -->
![](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Docs/07_2.jpg)
#### 태블릿 해상도 지원
>   다양한 기기에서 활용할 수 있도록 UI를 최적화하고 다중 기기 구독을 지원하기 위해 원앱으로 개발했습니다. 다양한 화면 비율(screen aspect ratio)에 대한 기본적인 적응은 유니티 UI 컴포넌트와 RectTransform Anchor를 활용했으며, 태블릿과 모바일 기기 간의 DPI 차이는 canvasScaler의 referenceResolution 속성 조정을 통해 해결했습니다.
<br />

<!-- 07 -->
![](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Docs/07.jpg)
#### 유저 리뷰
>    유저 반응이 좋은 편입니다. 정확한 정보를 제공해야 하는 '교육' 앱인 만큼 빠르게 피드백에 빠른 답변을 드리고, 즉각적으로 개선/업데이트하려고 노력했습니다. 앱 초기에는 여러 커뮤니티에서 직접 앱을 소개하고, 피드백을 요청하기도 했습니다. 유저들의 피드백을 반영하고 기능을 추가/개선하여 지금은 자격증 시험 학원에서 칵테일러 앱을 수강생들에게 권장할 정도로 인정받고 있습니다.
<br />

<!-- 08 -->
![](https://github.com/Sundrago/Cocktailor/blob/b99854d06ec347b44b16480ff70c32a6206560a7/Docs/08.jpg)
#### 인앱 구독자
>  한국산업인력공단에서 주관하는 조주기능사 자격증 시험은 일년에 네 차례 이루어집니다. 차트를 보면 시험 기간마다 구독자가 상승하는 추이를 확인할 수 있습니다. 현재까지 500명 이상의 유저가 구독 요금제를 이용하고 있으며, 2024 개정 출제 범위까지 업데이트를 마쳐둔 상태입니다. 앱 내 '버그 제보 및 건의' 채널을 통해 유저 피드백을 받고 있으며, 개선 요구 사항을 반영해 수정하고 있습니다.
<br />
