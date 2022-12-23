using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ColorAnimator))]
public class ColorAnimatorEditor : Editor
{
    private SerializedProperty _colorStages;
    private SerializedProperty _onColorChange;
    private SerializedProperty _onComplete;
    private SerializedProperty _currentColor;
    private SerializedProperty _cycleCount;
    private SerializedProperty _autoRun;
    private SerializedProperty _stopAtNextCycle;
    private ReorderableList _list;
    private ColorAnimator _script;

    private void OnEnable()
    {
        _colorStages = serializedObject.FindProperty("_colorStages");
        _onColorChange = serializedObject.FindProperty("_onColorChange");
        _onComplete = serializedObject.FindProperty("_onComplete");
        _currentColor = serializedObject.FindProperty("_currentColor");
        _cycleCount = serializedObject.FindProperty("_cycleCount");
        _autoRun = serializedObject.FindProperty("_autoRun");
        _stopAtNextCycle = serializedObject.FindProperty("_stopAtNextCycle");

        _list = new ReorderableList(serializedObject, _colorStages);
        _list.drawHeaderCallback = DrawHeaderCallback;
        _list.drawElementCallback = DrawElementCallback;
        _list.onCanRemoveCallback = OnCanRemoveCallback;
        _list.onAddCallback = OnAddCallback;
        _list.elementHeightCallback = ElementHeightCallback;

        _script = (ColorAnimator)target;
    }

    private float ElementHeightCallback(int index)
    {
        return EditorGUI.GetPropertyHeight(_colorStages.GetArrayElementAtIndex(index)) + 20f;
    }

    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, new GUIContent("Color Stages", "Color Stages"));
    }
    private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, 20f), "Stage " + index);
        EditorGUI.indentLevel++;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 20f, rect.width, rect.height - 20f), _colorStages.GetArrayElementAtIndex(index));
        EditorGUI.indentLevel--;
    }
    private void OnAddCallback(ReorderableList list)
    {
        _colorStages.InsertArrayElementAtIndex(list.index);
        list.Select(list.index + 1);
    }
    private bool OnCanRemoveCallback(ReorderableList list)
    {
        return list.count > 2;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Current Color", "Current Color"), GUILayout.Width(100f));
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, _currentColor.colorValue);
        texture.Apply();
        GUIStyle style = new GUIStyle();
        style.normal.background = texture;
        EditorGUILayout.LabelField(GUIContent.none, style, GUILayout.Width(20f));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(_autoRun, new GUIContent("Auto Run", "If checked the cycle will run on start"));
        EditorGUILayout.PropertyField(_cycleCount, new GUIContent("Cycle Count", "The number of cycles (0 means infinite loop)"));
        if (_cycleCount.intValue > 0)
        {
            EditorGUILayout.PropertyField(_stopAtNextCycle, new GUIContent("Stop At Next Cycle", "If checked the color state will stop at the beginning of the next cycle"));
        }
        EditorGUILayout.Space(2f);
        EditorGUI.BeginChangeCheck();
        _list.DoLayoutList();
        if (!EditorApplication.isPlaying && EditorGUI.EndChangeCheck())
        {
            _currentColor.colorValue = _colorStages.GetArrayElementAtIndex(0).FindPropertyRelative("_color").colorValue;
        }
        EditorGUILayout.PropertyField(_onColorChange);
        EditorGUILayout.PropertyField(_onComplete);

        serializedObject.ApplyModifiedProperties();
    }
}
