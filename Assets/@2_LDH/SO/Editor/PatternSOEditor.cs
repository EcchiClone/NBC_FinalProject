using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatternSO))]
public class PatternSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 ScriptableObject 인스펙터 UI 그리기
        base.OnInspectorGUI();

        PatternSO script = (PatternSO)target;

        // patternDatas 리스트를 순회하며 각 패턴 데이터에 대해 조건부 UI 그리기
        if (script.patternDatas != null)
        {
            foreach (var patternData in script.patternDatas)
            {
                EditorGUILayout.LabelField("Pattern Name", patternData.patternName);
                // posDirection에 따라 다른 필드 보이기
                switch (patternData.danmakuSettings.posDirection)
                {
                    case PosDirection.World:
                        // World 설정에 필요한 추가 필드 그리기
                        EditorGUILayout.LabelField("Custom Position Settings");
                        patternData.danmakuSettings.customPosDirection = EditorGUILayout.Vector3Field("Position from Parent", patternData.danmakuSettings.customPosDirection);
                        break;
                    case PosDirection.Look:
                        // Look 설정에 필요한 추가 필드 그리기
                        break;
                        // 추가 케이스 구현
                }
            }
        }

        // 변경 사항이 있을 경우 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
