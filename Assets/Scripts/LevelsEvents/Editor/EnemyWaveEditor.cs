using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyWave))]
[CanEditMultipleObjects]
public class EnemyWaveEditor : Editor
{
    private SerializedProperty _enemiesContainer;
    private SerializedProperty _enemy1;
    private SerializedProperty _enemy2;
    private SerializedProperty _enemy3;
    private SerializedProperty _dp1;
    private SerializedProperty _dp2;
    private SerializedProperty _dp3;
    private SerializedProperty _dp4;
    private SerializedProperty _dp5;
    private SerializedProperty _boss;
    private SerializedProperty _enemy1Count;
    private SerializedProperty _enemy2Count;
    private SerializedProperty _enemy3Count;
    private SerializedProperty _dp1Count;
    private SerializedProperty _dp2Count;
    private SerializedProperty _dp3Count;
    private SerializedProperty _dp4Count;
    private SerializedProperty _dp5Count;
    private SerializedProperty _bossCount;
    private SerializedProperty _spawningArea;
    private SerializedProperty _onComplete;

    private EnemyWave _script;

    private void OnEnable()
    {
        _enemiesContainer = serializedObject.FindProperty("_enemiesContainer");
        _enemy1 = serializedObject.FindProperty("_enemy1");
        _enemy2 = serializedObject.FindProperty("_enemy2");
        _enemy3 = serializedObject.FindProperty("_enemy3");
        _dp1 = serializedObject.FindProperty("_dp1");
        _dp2 = serializedObject.FindProperty("_dp2");
        _dp3 = serializedObject.FindProperty("_dp3");
        _dp4 = serializedObject.FindProperty("_dp4");
        _dp5 = serializedObject.FindProperty("_dp5");
        _boss = serializedObject.FindProperty("_boss");
        _enemy1Count = serializedObject.FindProperty("_enemy1Count");
        _enemy2Count = serializedObject.FindProperty("_enemy2Count");
        _enemy3Count = serializedObject.FindProperty("_enemy3Count");
        _dp1Count = serializedObject.FindProperty("_dp1Count");
        _dp2Count = serializedObject.FindProperty("_dp2Count");
        _dp3Count = serializedObject.FindProperty("_dp3Count");
        _dp4Count = serializedObject.FindProperty("_dp4Count");
        _dp5Count = serializedObject.FindProperty("_dp5Count");
        _bossCount = serializedObject.FindProperty("_bossCount");
        _spawningArea = serializedObject.FindProperty("_spawningArea");
        _onComplete = serializedObject.FindProperty("_onComplete");

        _script = (EnemyWave)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (!serializedObject.isEditingMultipleObjects)
        {
            EditorGUILayout.PropertyField(_enemiesContainer);
            EditorGUILayout.PropertyField(_enemy1);
            EditorGUILayout.PropertyField(_enemy2);
            EditorGUILayout.PropertyField(_enemy3);
            EditorGUILayout.PropertyField(_dp1);
            EditorGUILayout.PropertyField(_dp2);
            EditorGUILayout.PropertyField(_dp3);
            EditorGUILayout.PropertyField(_dp4);
            EditorGUILayout.PropertyField(_dp5);
            EditorGUILayout.PropertyField(_boss);

            DrawPrefabCountField(_enemy1, _enemy1Count);
            DrawPrefabCountField(_enemy2, _enemy2Count);
            DrawPrefabCountField(_enemy3, _enemy3Count);
            DrawPrefabCountField(_dp1, _dp1Count);
            DrawPrefabCountField(_dp2, _dp2Count);
            DrawPrefabCountField(_dp3, _dp3Count);
            DrawPrefabCountField(_dp4, _dp4Count);
            DrawPrefabCountField(_dp5, _dp5Count);
            DrawPrefabCountField(_boss, _bossCount);
        }

        EditorGUILayout.PropertyField(_spawningArea);
        EditorGUILayout.PropertyField(_onComplete);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawPrefabCountField(SerializedProperty _enemy, SerializedProperty _enemyCount)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent(_enemyCount.intValue.ToString()), GetPrefabPreview(_enemy.objectReferenceValue), GUILayout.Width(50f), GUILayout.Height(50f));
        if (GUILayout.Button("+", GUILayout.Width(50f), GUILayout.Height(50f)))
        {
            _enemyCount.intValue++;
        }
        if (GUILayout.Button("-", GUILayout.Width(50f), GUILayout.Height(50f)))
        {
            _enemyCount.intValue = Mathf.Max(0, _enemyCount.intValue - 1);
        }
        EditorGUILayout.EndHorizontal();
    }

    private GUIStyle GetPrefabPreview(Object prefab)
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.LowerCenter;
        style.normal.textColor = Color.white;
        style.normal.background = AssetPreview.GetAssetPreview(prefab);
        return style;
    }
}
