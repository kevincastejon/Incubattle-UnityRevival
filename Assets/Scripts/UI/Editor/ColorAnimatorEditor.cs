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
    private SerializedProperty _currentColor;
    private ReorderableList _list;
    private ColorAnimator _script;

    private void OnEnable()
    {
        _colorStages = serializedObject.FindProperty("_colorStages");
        _onColorChange = serializedObject.FindProperty("_onColorChange");
        _currentColor = serializedObject.FindProperty("_currentColor");

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
        list.Select(list.index+1);
    }
    private bool OnCanRemoveCallback(ReorderableList list)
    {
        return list.count > 2;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        _list.DoLayoutList();
        if (!EditorApplication.isPlaying && EditorGUI.EndChangeCheck())
        {
            _currentColor.colorValue = _colorStages.GetArrayElementAtIndex(0).FindPropertyRelative("_color").colorValue;
        }
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
        EditorGUILayout.Space(5f);
        //EditorGUI.BeginDisabledGroup(true);
        //EditorGUILayout.PropertyField(_currentColor);
        //EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(_onColorChange);

        serializedObject.ApplyModifiedProperties();
    }
}
