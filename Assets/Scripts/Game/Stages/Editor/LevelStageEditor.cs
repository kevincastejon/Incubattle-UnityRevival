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
    private SerializedProperty _triggerAreaOffset;
    private SerializedProperty _camConstraintArea;
    private SerializedProperty _camConstraintAreaOffset;
    private SerializedProperty _enemiesConstraintArea;
    private SerializedProperty _enemiesConstraintAreaOffset;
    private SerializedProperty _events;
    private ReorderableList _eventsEditorList;
    private SerializedProperty _onStarted;
    private SerializedProperty _onCompleted;

    private LevelStage _object;

    private bool _eventExpanded;
    private bool _camConstraintFocus;
    private bool _enemiesConstraintFocus;
    private bool _triggerFocus;

    private List<string> _triggerControlNames = new List<string> { "LevelStageTriggerAreaFieldLabel", "LevelStageTriggerAreaFieldWidth", "LevelStageTriggerAreaFieldHeight", "LevelStageTriggerAreaOffsetFieldWidth", "LevelStageTriggerAreaOffsetFieldHeight" };
    private List<string> _camConstraintControlNames = new List<string> { "LevelStageConstraintCamAreaFieldLabel", "LevelStageConstraintCamAreaFieldWidth", "LevelStageConstraintCamAreaFieldHeight", "LevelStageConstraintCamAreaOffsetFieldWidth", "LevelStageConstraintCamAreaOffsetFieldHeight" };
    private List<string> _enemiesConstraintControlNames = new List<string> { "LevelStageConstraintEnemiesAreaFieldLabel", "LevelStageConstraintEnemiesAreaFieldWidth", "LevelStageConstraintEnemiesAreaFieldHeight", "LevelStageConstraintEnemiesAreaOffsetFieldWidth", "LevelStageConstraintEnemiesAreaOffsetFieldHeight" };

    private void OnEnable()
    {
        _triggerArea = serializedObject.FindProperty("_triggerArea");
        _triggerAreaOffset = serializedObject.FindProperty("_triggerAreaOffset");
        _camConstraintArea = serializedObject.FindProperty("_camConstraintArea");
        _camConstraintAreaOffset = serializedObject.FindProperty("_camConstraintAreaOffset");
        _enemiesConstraintArea = serializedObject.FindProperty("_enemiesConstraintArea");
        _enemiesConstraintAreaOffset = serializedObject.FindProperty("_enemiesConstraintAreaOffset");
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
        Color camInnerConstraintColor = new Color(0f, 0.5f, 1f, 0f);
        Color camBorderConstraintColor = new Color(0f, 0.5f, 1f, 1f);
        Color enemiesInnerConstraintColor = new Color(0.5f, 0f, 1f, 0f);
        Color enemiesBorderConstraintColor = new Color(0.5f, 0f, 1f, 1f);
        if (_triggerFocus)
        {
            innerTriggerColor = new Color(0.5f, 1f, 0f, 0.25f);
            borderTriggerColor = new Color(0f, 0f, 0f, 1f);
        }
        if (_camConstraintFocus)
        {
            camInnerConstraintColor = new Color(0f, 0.5f, 1f, 0.25f);
            camBorderConstraintColor = new Color(0f, 0f, 0f, 1f);
        }
        if (_enemiesConstraintFocus)
        {
            enemiesInnerConstraintColor = new Color(0.5f, 0f, 1f, 0.25f);
            enemiesBorderConstraintColor = new Color(0.5f, 0f, 1f, 1f);
        }
        Handles.DrawSolidRectangleWithOutline(new Rect(_camConstraintAreaOffset.vector2Value.x + _object.transform.position.x - _camConstraintArea.vector2Value.x * 0.5f, _camConstraintAreaOffset.vector2Value.y + _object.transform.position.y - _camConstraintArea.vector2Value.y * 0.5f, _camConstraintArea.vector2Value.x, _camConstraintArea.vector2Value.y), camInnerConstraintColor, camBorderConstraintColor);
        Handles.DrawSolidRectangleWithOutline(new Rect(_enemiesConstraintAreaOffset.vector2Value.x + _object.transform.position.x - _enemiesConstraintArea.vector2Value.x * 0.5f, _enemiesConstraintAreaOffset.vector2Value.y + _object.transform.position.y - _enemiesConstraintArea.vector2Value.y * 0.5f, _enemiesConstraintArea.vector2Value.x, _enemiesConstraintArea.vector2Value.y), enemiesInnerConstraintColor, enemiesBorderConstraintColor);
        Handles.DrawSolidRectangleWithOutline(new Rect(_triggerAreaOffset.vector2Value.x + _object.transform.position.x - _triggerArea.vector2Value.x * 0.5f, _triggerAreaOffset.vector2Value.y + _object.transform.position.y - _triggerArea.vector2Value.y * 0.5f, _triggerArea.vector2Value.x, _triggerArea.vector2Value.y), innerTriggerColor, borderTriggerColor);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        float labelWidthBkp = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50f;
        EditorGUILayout.LabelField(new GUIContent("Trigger area", "The area that will trigger the stage"), EditorStyles.boldLabel);
        EditorGUIUtility.labelWidth = 40f;
        EditorGUILayout.BeginHorizontal();
        GUI.SetNextControlName("LevelStageTriggerAreaFieldWidth");
        float x = EditorGUILayout.FloatField("Width", _triggerArea.vector2Value.x);
        GUI.SetNextControlName("LevelStageTriggerAreaFieldHeight");
        float y = EditorGUILayout.FloatField("Height", _triggerArea.vector2Value.y);
        _triggerArea.vector2Value = new Vector2(x, y);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUI.SetNextControlName("LevelStageTriggerAreaOffsetFieldWidth");
        float offsetX = EditorGUILayout.FloatField("Offset X", _triggerAreaOffset.vector2Value.x);
        GUI.SetNextControlName("LevelStageTriggerAreaOffsetFieldHeight");
        float offsetY = EditorGUILayout.FloatField("Offset Y", _triggerAreaOffset.vector2Value.y);
        _triggerAreaOffset.vector2Value = new Vector2(offsetX, offsetY);
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 50f;
        EditorGUILayout.LabelField(new GUIContent("Cam Constraint Area", "The area that will constraint the camera during the stage"), EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 40f;
        GUI.SetNextControlName("LevelStageConstraintCamAreaFieldWidth");
        EditorGUI.BeginChangeCheck();
        x = EditorGUILayout.FloatField("Width", _camConstraintArea.vector2Value.x);
        GUI.SetNextControlName("LevelStageConstraintCamAreaFieldHeight");
        y = EditorGUILayout.FloatField("Height", _camConstraintArea.vector2Value.y);
        _camConstraintArea.vector2Value = new Vector2(x, y);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUI.SetNextControlName("LevelStageConstraintCamAreaOffsetFieldWidth");
        offsetX = EditorGUILayout.FloatField("Offset X", _camConstraintAreaOffset.vector2Value.x);
        GUI.SetNextControlName("LevelStageConstraintCamAreaOffsetFieldHeight");
        offsetY = EditorGUILayout.FloatField("Offset Y", _camConstraintAreaOffset.vector2Value.y);
        _camConstraintAreaOffset.vector2Value = new Vector2(offsetX, offsetY);
        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        EditorGUIUtility.labelWidth = 50f;
        EditorGUILayout.LabelField(new GUIContent("Enemies Constraint Area", "The area that will constraint the enemies during the stage"), EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 40f;
        GUI.SetNextControlName("LevelStageConstraintEnemiesAreaFieldWidth");
        x = EditorGUILayout.FloatField("Width", _enemiesConstraintArea.vector2Value.x);
        GUI.SetNextControlName("LevelStageConstraintEnemiesAreaFieldHeight");
        y = EditorGUILayout.FloatField("Height", _enemiesConstraintArea.vector2Value.y);
        _enemiesConstraintArea.vector2Value = new Vector2(x, y);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUI.SetNextControlName("LevelStageConstraintEnemiesAreaOffsetFieldWidth");
        offsetX = EditorGUILayout.FloatField("Offset X", _enemiesConstraintAreaOffset.vector2Value.x);
        GUI.SetNextControlName("LevelStageConstraintEnemiesAreaOffsetFieldHeight");
        offsetY = EditorGUILayout.FloatField("Offset Y", _enemiesConstraintAreaOffset.vector2Value.y);
        _enemiesConstraintAreaOffset.vector2Value = new Vector2(offsetX, offsetY);
        bool hasEnemiesConstraintChanged = EditorGUI.EndChangeCheck();
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = labelWidthBkp;
        _eventExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_eventExpanded, new GUIContent("Events"));
        if (_eventExpanded)
        {
            EditorGUILayout.PropertyField(_onStarted);
            EditorGUILayout.PropertyField(_onCompleted);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.LabelField("Sequences");
        _eventsEditorList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if (hasEnemiesConstraintChanged)
        {
            _object.TransformWalls();
        }
        bool lastTriggerValue = _triggerFocus;
        bool lastCamConstraintValue = _camConstraintFocus;
        bool lastEnemiesConstraintValue = _enemiesConstraintFocus;
        _triggerFocus = _triggerControlNames.Contains(GUI.GetNameOfFocusedControl());
        _camConstraintFocus = _camConstraintControlNames.Contains(GUI.GetNameOfFocusedControl());
        _enemiesConstraintFocus = _enemiesConstraintControlNames.Contains(GUI.GetNameOfFocusedControl());
        if (_triggerFocus != lastTriggerValue || _camConstraintFocus != lastCamConstraintValue || _enemiesConstraintFocus != lastEnemiesConstraintValue)
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
