using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(LevelStage))]
public class LevelStageEditor : Editor
{
    private SerializedProperty _triggerArea;
    private SerializedProperty _constraintArea;
    private SerializedProperty _events;
    private ReorderableList _eventsEditorList;
    private SerializedProperty _onStarted;
    private SerializedProperty _onCompleted;

    private LevelStage _object;

    private bool _eventExpanded;
    private bool _constraintFocus;
    private bool _triggerFocus;

    private List<string> _triggerControlNames = new List<string> { "LevelStageTriggerAreaFieldLabel", "LevelStageTriggerAreaFieldWidth", "LevelStageTriggerAreaFieldHeight" };
    private List<string> _constraintControlNames = new List<string> { "LevelStageConstraintAreaFieldLabel", "LevelStageConstraintAreaFieldWidth", "LevelStageConstraintAreaFieldHeight" };

    private void OnEnable()
    {
        _triggerArea = serializedObject.FindProperty("_triggerArea");
        _constraintArea = serializedObject.FindProperty("_constraintArea");
        _events = serializedObject.FindProperty("_events");
        _eventsEditorList = new ReorderableList(serializedObject, _events, true, false, true, true);
        _eventsEditorList.drawHeaderCallback = EventsDrawHeaderCallback;
        _eventsEditorList.drawElementCallback = EventsDrawElementCallback;
        _eventsEditorList.elementHeightCallback = EventsElementHeightCallback;
        _eventsEditorList.onAddDropdownCallback = OnAddDropdownCallback;
        _onStarted = serializedObject.FindProperty("_onStarted");
        _onCompleted = serializedObject.FindProperty("_onCompleted");

        _object = (LevelStage)target;
    }

    private void OnAddDropdownCallback(Rect buttonRect, ReorderableList list)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("TEXT"), false, EventsOnAddCallback, 0);
        menu.AddItem(new GUIContent("ANIMATION"), false, EventsOnAddCallback, 1);
        menu.AddItem(new GUIContent("WAVE"), false, EventsOnAddCallback, 2);
        menu.ShowAsContext();
    }
    private void EventsOnAddCallback(object type)
    {
        int enumType = (int)type;
        int index = _eventsEditorList.count == 0 ? 0 : _eventsEditorList.index + 1;
        _events.InsertArrayElementAtIndex(index);
        _events.GetArrayElementAtIndex(index).FindPropertyRelative("_type").enumValueIndex = enumType;
        _events.GetArrayElementAtIndex(index).FindPropertyRelative("_text").objectReferenceValue = null;
        _events.GetArrayElementAtIndex(index).FindPropertyRelative("_animation").objectReferenceValue = null;
        _events.GetArrayElementAtIndex(index).FindPropertyRelative("_wave").objectReferenceValue = null;
        _eventsEditorList.Select(index);
        _eventsEditorList.serializedProperty.serializedObject.ApplyModifiedProperties();
    }
    private void OnSceneGUI()
    {
        Color innerTriggerColor = new Color(0.5f, 1f, 0f, 0f);
        Color borderTriggerColor = new Color(0.5f, 1f, 0f, 1f);
        Color innerConstraintColor = new Color(0f, 0.5f, 1f, 0f);
        Color borderConstraintColor = new Color(0f, 0.5f, 1f, 1f);
        if (_triggerFocus)
        {
            innerTriggerColor = new Color(0.5f, 1f, 0f, 0.25f);
            borderTriggerColor = new Color(0f, 0f, 0f, 1f);
        }
        if (_constraintFocus)
        {
            innerConstraintColor = new Color(0f, 0.5f, 1f, 0.25f);
            borderConstraintColor = new Color(0f, 0f, 0f, 1f);
        }
        Handles.DrawSolidRectangleWithOutline(new Rect(_object.transform.position.x - _constraintArea.vector2Value.x * 0.5f, _object.transform.position.y - _constraintArea.vector2Value.y * 0.5f, _constraintArea.vector2Value.x, _constraintArea.vector2Value.y), innerConstraintColor, borderConstraintColor);
        Handles.DrawSolidRectangleWithOutline(new Rect(_object.transform.position.x - _triggerArea.vector2Value.x * 0.5f, _object.transform.position.y - _triggerArea.vector2Value.y * 0.5f, _triggerArea.vector2Value.x, _triggerArea.vector2Value.y), innerTriggerColor, borderTriggerColor);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();
        float labelWidthBkp = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50f;
        GUI.SetNextControlName("LevelStageTriggerAreaFieldLabel");
        EditorGUILayout.LabelField(new GUIContent("Trigger area", "The area that will trigger the stage"));
        EditorGUIUtility.labelWidth = 40f;
        GUI.SetNextControlName("LevelStageTriggerAreaFieldWidth");
        float x = EditorGUILayout.FloatField("Width", _triggerArea.vector2Value.x);
        GUI.SetNextControlName("LevelStageTriggerAreaFieldHeight");
        float y = EditorGUILayout.FloatField("Height", _triggerArea.vector2Value.y);
        _triggerArea.vector2Value = new Vector2(x, y);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 50f;
        GUI.SetNextControlName("LevelStageConstraintAreaFieldLabel");
        EditorGUILayout.LabelField(new GUIContent("Constraint area", "The area that will constraint the camera and enemies during the stage"));
        EditorGUIUtility.labelWidth = 40f;
        GUI.SetNextControlName("LevelStageConstraintAreaFieldWidth");
        EditorGUI.BeginChangeCheck();
        x = EditorGUILayout.FloatField("Width", _constraintArea.vector2Value.x);
        GUI.SetNextControlName("LevelStageConstraintAreaFieldHeight");
        y = EditorGUILayout.FloatField("Height", _constraintArea.vector2Value.y);
        bool hasConstraintChanged = EditorGUI.EndChangeCheck();
        _constraintArea.vector2Value = new Vector2(x, y);
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = labelWidthBkp;
        _eventExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_eventExpanded, new GUIContent("Events"));
        if (_eventExpanded)
        {
            EditorGUILayout.PropertyField(_onStarted);
            EditorGUILayout.PropertyField(_onCompleted);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        _eventsEditorList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if (hasConstraintChanged)
        {
            _object.TransformWalls();
        }
        bool lastTriggerValue = _triggerFocus;
        bool lastConstraintValue = _constraintFocus;
        _triggerFocus = _triggerControlNames.Contains(GUI.GetNameOfFocusedControl());
        _constraintFocus = _constraintControlNames.Contains(GUI.GetNameOfFocusedControl());
        if (_triggerFocus != lastTriggerValue || _constraintFocus != lastConstraintValue)
        {
            SceneView.RepaintAll();
        }
    }

    private void EventsDrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, _events.displayName);
    }

    private void EventsDrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, _events.GetArrayElementAtIndex(index));
    }

    private float EventsElementHeightCallback(int index)
    {
        return EditorGUI.GetPropertyHeight(_events.GetArrayElementAtIndex(index));
    }
}
