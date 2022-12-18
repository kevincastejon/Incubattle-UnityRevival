using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(WeightedObject))]
public class WeightedLootDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
        {
            return 20f;
        }
        SerializedProperty name = property.FindPropertyRelative("_name");
        SerializedProperty weight = property.FindPropertyRelative("_weight");
        SerializedProperty loot = property.FindPropertyRelative("_loot");
        SerializedProperty summedWeight = property.FindPropertyRelative("_summedWeight");
        return 20f +
            EditorGUI.GetPropertyHeight(name, true) +
            EditorGUI.GetPropertyHeight(weight, true) +
            EditorGUI.GetPropertyHeight(loot, true) +
            EditorGUI.GetPropertyHeight(summedWeight, true);

    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.IndentedRect(position);
        Rect rect = new Rect(position);
        SerializedProperty name = property.FindPropertyRelative("_name");
        SerializedProperty weight = property.FindPropertyRelative("_weight");
        SerializedProperty loot = property.FindPropertyRelative("_loot");
        SerializedProperty summedWeight = property.FindPropertyRelative("_summedWeight");
        rect.height = 20f;
        GUIContent lab = EditorGUIUtility.IconContent(property.isExpanded ? "d_icon dropdown@2x" : "d_PlayButton");
        label.text = name.stringValue + " " + weight.floatValue;
        label.tooltip = name.stringValue + " " + weight.floatValue.ToString("F2");
        if (GUI.Button(rect, lab))
        {
            property.isExpanded = !property.isExpanded;
        }
        if (property.isExpanded)
        {
            rect.y += rect.height;
            EditorGUI.indentLevel++;
            rect.height = EditorGUI.GetPropertyHeight(name, true);
            EditorGUI.PropertyField(rect, name);
            rect.y += rect.height;
            rect.height = EditorGUI.GetPropertyHeight(weight, true);
            weight.floatValue = Mathf.Max(EditorGUI.DelayedFloatField(rect, new GUIContent("Weight"), weight.floatValue), 0f);
            rect.y += rect.height;
            rect.height = EditorGUI.GetPropertyHeight(loot, true);
            EditorGUI.PropertyField(rect, loot);
            rect.y += rect.height;
            EditorGUI.BeginDisabledGroup(true);
            rect.height = EditorGUI.GetPropertyHeight(summedWeight, true);
            EditorGUI.PropertyField(rect, summedWeight);
            rect.y += rect.height;
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
        }
        EditorGUI.EndFoldoutHeaderGroup();

    }
}
