using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorStage))]
public class ColorStageDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        SerializedProperty _duration = property.FindPropertyRelative("_duration");
        SerializedProperty _color = property.FindPropertyRelative("_color");
        return 
            EditorGUI.GetPropertyHeight(_duration, true) +
            EditorGUI.GetPropertyHeight(_color, true) ;

    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        position = EditorGUI.IndentedRect(position);
        Rect rect = new Rect(position);
        SerializedProperty _duration = property.FindPropertyRelative("_duration");
        SerializedProperty _color = property.FindPropertyRelative("_color");
        rect.height = EditorGUI.GetPropertyHeight(_duration, true);
        EditorGUI.PropertyField(rect, _duration);
        rect.y += rect.height;
        rect.height = EditorGUI.GetPropertyHeight(_color, true);
        EditorGUI.PropertyField(rect, _color);
        rect.y += rect.height;

    }
}
