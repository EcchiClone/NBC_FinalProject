using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatternSO))]
public class PatternSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PatternSO script = (PatternSO)target;

        if (script.patternDatas != null)
        {
            for (int i = 0; i < script.patternDatas.Count; i++)
            {
                EditorGUILayout.LabelField("Pattern Name", script.patternDatas[i].patternName);
                if (script.patternDatas[i].enemyBulletSettings.posDirection == PosDirection.World)
                {
                    EditorGUILayout.LabelField("Custom Position Settings");
                    script.patternDatas[i].enemyBulletSettings.customPosDirection = EditorGUILayout.Vector3Field("Position from Parent", script.patternDatas[i].enemyBulletSettings.customPosDirection);
                }
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
