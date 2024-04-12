using Cinemachine.Editor;
using NUnit.Framework.Internal;
using System;
using System.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor.Rendering;

[CustomEditor(typeof(PatternSO))]
public class PatternSOEditor : Editor
{
    SerializedProperty patternDatasProperty;
    //private ReorderableList reorderableList;
    //bool[] expandedStates;

    private void OnEnable()
    {
        patternDatasProperty = serializedObject.FindProperty("patternDatas");

        // 아래 내용 모두 리스트 박스 형태로 만드는 데 실패한 내용. 03.18 기록에 남김.

        //reorderableList = new ReorderableList(serializedObject, patternDatasProperty, true, true, true, true);

        //// 드로우 헤더 콜백 정의
        //reorderableList.drawHeaderCallback = (Rect rect) =>
        //{
        //    EditorGUI.LabelField(rect, "Pattern Datas");
        //};

        //// 드로우 엘리먼트 콜백 정의
        //expandedStates = new bool[patternDatasProperty.arraySize];

        //reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        //{
        //    var element = patternDatasProperty.GetArrayElementAtIndex(index);
        //    rect.y += 2;
        //    rect.height = EditorGUIUtility.singleLineHeight;

        //    // Ensure the expandedStates array is appropriately sized
        //    if (index >= expandedStates.Length)
        //    {
        //        Array.Resize(ref expandedStates, patternDatasProperty.arraySize);
        //    }

        //    // Pattern Name and Description
        //    SerializedProperty patternNameProperty = element.FindPropertyRelative("patternName");
        //    SerializedProperty descProperty = element.FindPropertyRelative("Desc");

        //    // Toggle for expanding/collapsing detailed settings
        //    expandedStates[index] = EditorGUI.Foldout(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), expandedStates[index], "Details", true);
        //    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        //    if (expandedStates[index])
        //    {
        //        // Only draw the details if expanded
        //        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), patternNameProperty, new GUIContent("Pattern Name"));
        //        rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        //        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), descProperty, new GUIContent("Description"));
        //        rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        //        // Assuming DrawEnemyBulletSettings is adapted to work with EditorGUI and Rect
        //        SerializedProperty enemyBulletSettingsProperty = element.FindPropertyRelative("enemyBulletSettings");
        //        // DrawEnemyBulletSettings(rect, enemyBulletSettingsProperty); // You need to adapt this method
        //    }
        //};
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //reorderableList.DoLayoutList();

        EditorGUILayout.LabelField("※ 영어명 프로퍼티는 현재 반영 안 되는 상태", EditorStyles.boldLabel);
        if (GUILayout.Button("새 패턴 생성"))
        {
            patternDatasProperty.arraySize++;
        }

        // 각 패턴에 대한 그리기 반복
        for (int i = 0; i < patternDatasProperty.arraySize; i++)
        {
            SerializedProperty patternDataProperty = patternDatasProperty.GetArrayElementAtIndex(i);
            SerializedProperty enemyBulletSettingsProperty = patternDataProperty.FindPropertyRelative("enemyBulletSettings");
            SerializedProperty patternNameProperty = patternDataProperty.FindPropertyRelative("patternName");
            SerializedProperty descProperty = patternDataProperty.FindPropertyRelative("Desc");
            GUIContent elementLabel = new GUIContent(patternNameProperty.stringValue);

            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.PropertyField(patternDataProperty, elementLabel, true);

            //// - 버튼 누를 시 삭제
            //if (GUILayout.Button("-", GUILayout.Width(20)))
            //{
            //    patternDatasProperty.DeleteArrayElementAtIndex(i);
            //}
            //EditorGUILayout.EndHorizontal();//


            EditorGUILayout.BeginHorizontal();
            // 각 패턴 데이터의 foldout을 그리기
            patternDataProperty.isExpanded = EditorGUILayout.Foldout(patternDataProperty.isExpanded, elementLabel, true);
            // - 버튼 누를 시 삭제, 가로로 배치
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                patternDatasProperty.DeleteArrayElementAtIndex(i);
                break; // 삭제 후 루프를 종료하여 인덱스 문제 방지
            }
            EditorGUILayout.EndHorizontal();

            // 패턴 데이터 세부 정보를 수직으로 배치
            if (patternDataProperty.isExpanded)
            {
                EditorGUI.indentLevel++; // 세부 정보에 들여쓰기 적용

                // 'patternName' 및 'Desc' 속성 그리기
                EditorGUILayout.LabelField("개요", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(patternNameProperty, new GUIContent("패턴 이름"));
                EditorGUILayout.PropertyField(descProperty, new GUIContent("설명"));

                DrawEnemyBulletSettings(enemyBulletSettingsProperty);
                EditorGUI.indentLevel--;
            }
        }
        EditorGUILayout.LabelField("↓ 열어서 순서 바꾸는 용도. 내부는 위와 내용 같으며 동기화 되고 있음.", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(patternDatasProperty, true); // 열어서 순서 바꾸는 용도

        serializedObject.ApplyModifiedProperties();
    }

    // 그리지 않을 조건 표시
    private void DrawEnemyBulletSettings(SerializedProperty settingsProperty)
    {

        SerializedProperty baseProperty = settingsProperty.Copy();

        foreach (SerializedProperty property in settingsProperty) // 내부로가면 어째서인지 settingsProperty 패스도 같이 이동함. 
        {

            //Debug.Log(baseProperty.propertyPath);
            //Debug.Log(settingsProperty.propertyPath);
            // 조건부로 그리기 스킵
            if (property.name == "customPosDirection") // 현재 프로퍼티
                if (!EnumMatchCheck("posDirection", PosDirection.CustomWorld)) continue; // 조건 프로퍼티
            if (property.name == "maxSpreadAngleA")
                if (!EnumMatchCheck("spreadA", SpreadType.Spread)) continue;
            if (property.name == "concentrationA")
                if (!EnumMatchCheck("spreadA", SpreadType.Spread)) continue;

            if (property.name == "useVelocityScalerFromMuzzleDist")
                if (!EnumMatchCheck("enemyBulletShape", EnemyBulletShape.Custom) && !EnumMatchCheck("enemyBulletShape", EnemyBulletShape.RandomVertex)) continue;
            if (property.name == "customBulletPosList")
                if (!EnumMatchCheck("enemyBulletShape", EnemyBulletShape.Custom)) continue;
            if (property.name == "numOfVertex")
                if (!EnumMatchCheck("enemyBulletShape", EnemyBulletShape.RandomVertex)) continue;
            if (property.name == "isLoopingShape")
                if (!EnumMatchCheck("enemyBulletShape", EnemyBulletShape.RandomVertex)) continue;
            if (property.name == "divisionPointsPerEdge")
                if (!EnumMatchCheck("enemyBulletShape", EnemyBulletShape.Custom) && !EnumMatchCheck("enemyBulletShape", EnemyBulletShape.RandomVertex)) continue;

            if (property.name == "shotVerticalNum")
                if (!EnumMatchCheck("enemyBulletShape", EnemyBulletShape.Sphere)) continue;
            if (property.name == "initCustomDirection")
                if (!EnumMatchCheck("initDirectionType", EnemyBulletToDirection.Local)) continue;
            if (property.name == "maxSpreadAngleB")
                if (!EnumMatchCheck("spreadB", SpreadType.Spread)) continue;
            if (property.name == "concentrationB")
                if (!EnumMatchCheck("spreadB", SpreadType.Spread)) continue;

            if (property.name == "initMoveDirection")
                continue; // 현재 미사용, 임시 비활성화

            if (property.name == "initLocalYRotationSpeed")
                if (!(baseProperty.FindPropertyRelative("isCluster").boolValue == true)) continue;

            if (property.name == "enemyBulletPrefab") { EditorGUILayout.PropertyField(property, new GUIContent("탄막 오브젝트")); continue; }
            if (property.name == "initDelay") { EditorGUILayout.PropertyField(property, new GUIContent("시작지연")); continue; }
            if (property.name == "numOfSet") { EditorGUILayout.PropertyField(property, new GUIContent("세트 수")); continue; }
            if (property.name == "setDelay") { EditorGUILayout.PropertyField(property, new GUIContent("세트 간 지연")); continue; }
            if (property.name == "shotPerSet") { EditorGUILayout.PropertyField(property, new GUIContent("발사 횟수")); continue; }
            if (property.name == "shotDelay") { EditorGUILayout.PropertyField(property, new GUIContent("발사 간 지연")); continue; }

            if (property.name == "posDirection") { EditorGUILayout.PropertyField(property, new GUIContent("패턴 방향")); continue; }
            if (property.name == "customPosDirection") { EditorGUILayout.PropertyField(property, new GUIContent("직접 지정")); continue; }
            if (property.name == "spreadA") { EditorGUILayout.PropertyField(property, new GUIContent("기준벡터 오차 적용")); continue; }
            if (property.name == "maxSpreadAngleA") { EditorGUILayout.PropertyField(property, new GUIContent("범위각(0~360)")); continue; }
            if (property.name == "concentrationA") { EditorGUILayout.PropertyField(property, new GUIContent("응집도(1:높음~0:낮음)")); continue; }

            if (property.name == "enemyBulletShape") { EditorGUILayout.PropertyField(property, new GUIContent("탄막 형태")); continue; }
            if (property.name == "useVelocityScalerFromMuzzleDist") { EditorGUILayout.PropertyField(property, new GUIContent("초기 거리 비례 모양 유지")); continue; }
            if (property.name == "numOfVertex") { EditorGUILayout.PropertyField(property, new GUIContent("꼭짓점 갯수")); continue; }
            if (property.name == "isLoopingShape") { EditorGUILayout.PropertyField(property, new GUIContent("끝점을 처음과 이을지")); continue; }
            if (property.name == "customBulletPosList") { EditorGUILayout.PropertyField(property, new GUIContent("커스텀 위치")); continue; }
            if (property.name == "divisionPointsPerEdge") { EditorGUILayout.PropertyField(property, new GUIContent("점 간 보간점 갯수")); continue; }
            if (property.name == "initDistance") { EditorGUILayout.PropertyField(property, new GUIContent("생성 거리")); continue; }
            if (property.name == "numPerShot") { EditorGUILayout.PropertyField(property, new GUIContent("1회 당 탄수")); continue; }
            if (property.name == "shotVerticalNum") { EditorGUILayout.PropertyField(property, new GUIContent("층 수")); continue; }
            if (property.name == "spreadB") { EditorGUILayout.PropertyField(property, new GUIContent("각 탄막의 오차 적용")); continue; }
            if (property.name == "maxSpreadAngleB") { EditorGUILayout.PropertyField(property, new GUIContent("범위각(0~360)")); continue; }
            if (property.name == "concentrationB") { EditorGUILayout.PropertyField(property, new GUIContent("응집도(1:높음~0:낮음)")); continue; }

            if (property.name == "initDirectionType") { EditorGUILayout.PropertyField(property, new GUIContent("생성 시 방향")); continue; }
            if (property.name == "initCustomDirection") { EditorGUILayout.PropertyField(property, new GUIContent("직접 지정")); continue; }
            if (property.name == "enemyBulletMoveType") { EditorGUILayout.PropertyField(property, new GUIContent("움직임 유형")); continue; }
            if (property.name == "initSpeed") { EditorGUILayout.PropertyField(property, new GUIContent("생성 시 속도")); continue; }
            if (property.name == "initMoveDirection") { EditorGUILayout.PropertyField(property, new GUIContent("이동 방향(게 처럼 옆으로 움직이는 게 아니라면 미사용)")); continue; }
            if (property.name == "initAccelMultiple") { EditorGUILayout.PropertyField(property, new GUIContent("곱 가속도(일정:1)")); continue; }
            if (property.name == "initAccelPlus") { EditorGUILayout.PropertyField(property, new GUIContent("합 가속도(일정:0)")); continue; }
            if (property.name == "minSpeed") { EditorGUILayout.PropertyField(property, new GUIContent("최소 속도")); continue; }
            if (property.name == "maxSpeed") { EditorGUILayout.PropertyField(property, new GUIContent("최대 속도")); continue; }
            if (property.name == "initRotationSpeed") { EditorGUILayout.PropertyField(property, new GUIContent("회전 속도")); continue; }


            if (property.name == "isCluster") { EditorGUILayout.PropertyField(property, new GUIContent("군집 탄막의 여부")); continue; }
            if (property.name == "initLocalYRotationSpeed") { EditorGUILayout.PropertyField(property, new GUIContent("군집의 Y 회전속도")); continue; }
            if (property.name == "enemyBulletChangeMoveMethod") { EditorGUILayout.PropertyField(property, new GUIContent("작동 방식")); continue; }
            if (property.name == "enemyBulletChangeMoveProperty") { EditorGUILayout.PropertyField(property, new GUIContent("변화 목록")); continue; }

            if (property.name == "releaseMethod") { EditorGUILayout.PropertyField(property, new GUIContent("추가 반환 방법(미지원)")); continue; }
            if (property.name == "releaseTimer") { EditorGUILayout.PropertyField(property, new GUIContent("수명")); continue; }

            // 필드 한번 더 나오는 버그 임시조치
            if (property.name == "x" || property.name == "y" || property.name == "z")
                continue;
            // 필드 한번 더 나오는 버그 임시조치
            if (property.name == "size")
                continue;
            if (property.name == "data")
                continue;
            if (property.name.StartsWith("_"))
                continue;

            EditorGUILayout.PropertyField(property, true);
        }

        bool EnumMatchCheck(string propertyName, Enum targetEnum)
        {
            if (baseProperty.FindPropertyRelative(propertyName) == null)
            {
                Debug.LogError($"propertyName({propertyName})을 찾을 수 없습니다.");
                return false;
            }

            return baseProperty.FindPropertyRelative(propertyName).enumValueIndex == Convert.ToInt32(targetEnum);
        }
    }


}
