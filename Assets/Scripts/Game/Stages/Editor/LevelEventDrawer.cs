using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelEvent))]
public class LevelEventDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 40f;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        position = EditorGUI.IndentedRect(position);
        Rect rect = new Rect(position);
        SerializedProperty _type = property.FindPropertyRelative("_type");
        SerializedProperty _wait = property.FindPropertyRelative("_wait");
        SerializedProperty _text = property.FindPropertyRelative("_text");
        SerializedProperty _animation = property.FindPropertyRelative("_animation");
        SerializedProperty _wave = property.FindPropertyRelative("_wave");
        EditorGUI.indentLevel++;
        if (_type.enumValueIndex == 0)
        {
            rect.height = EditorGUI.GetPropertyHeight(_text, true);
            EditorGUI.PropertyField(rect, _text);
        }
        else if (_type.enumValueIndex == 1)
        {
            rect.height = EditorGUI.GetPropertyHeight(_animation, true);
            EditorGUI.PropertyField(rect, _animation);
        }
        else if (_type.enumValueIndex == 2)
        {
            rect.height = EditorGUI.GetPropertyHeight(_wave, true);
            EditorGUI.PropertyField(rect, _wave);
        }
        rect.y += rect.height;
        _wait.boolValue = EditorGUI.ToggleLeft(rect, new GUIContent("Wait", "Should this event be blocking the flow before it ends"), _wait.boolValue);
        EditorGUI.indentLevel--;

    }
}
