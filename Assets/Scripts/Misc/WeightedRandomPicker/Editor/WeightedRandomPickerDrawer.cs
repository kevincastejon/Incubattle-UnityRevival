using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

[CustomPropertyDrawer(typeof(WeightedRandomPicker))]
public class WeightedRandomPickerDrawer : PropertyDrawer
{
    private Dictionary<string, ReorderableList> _objectsEditorLists = new Dictionary<string, ReorderableList>();
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!_objectsEditorLists.ContainsKey(property.propertyPath))
        {
            SerializedProperty objects = property.FindPropertyRelative("_objects");
            _objectsEditorLists.Add(property.propertyPath, new ReorderableList(property.serializedObject, objects, false, true, true, true));
            _objectsEditorLists[property.propertyPath].drawHeaderCallback = (Rect rect) => ObjectsDrawHeaderCallback(rect, label);
            _objectsEditorLists[property.propertyPath].drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => ObjectsDrawElementCallback(rect, index, objects);
            _objectsEditorLists[property.propertyPath].elementHeightCallback = (int index) => ObjectsElementHeightCallback(index, objects);
        }
        return _objectsEditorLists[property.propertyPath].GetHeight();

    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.IndentedRect(position);
        Rect rect = new Rect(position);
        SerializedProperty _objects = property.FindPropertyRelative("_objects");
        EditorGUI.indentLevel++;
        rect.height = _objectsEditorLists[property.propertyPath].GetHeight();
        _objectsEditorLists[property.propertyPath].DoList(rect);
        rect.y += rect.height;
        EditorGUI.indentLevel--;

    }
    private void ObjectsDrawHeaderCallback(Rect rect, GUIContent label)
    {
        EditorGUI.LabelField(rect, label);
    }

    private void ObjectsDrawElementCallback(Rect rect, int index, SerializedProperty objects)
    {
        float totalSummedWeight = 0f;
        for (int i = 0; i < objects.arraySize; i++)
        {
            totalSummedWeight += objects.GetArrayElementAtIndex(i).FindPropertyRelative("_weight").floatValue;
        }
        SerializedProperty property = objects.GetArrayElementAtIndex(index);
        SerializedProperty name = property.FindPropertyRelative("_name");
        SerializedProperty weight = property.FindPropertyRelative("_weight");
        SerializedProperty obj = property.FindPropertyRelative("_object");
        SerializedProperty summedWeight = property.FindPropertyRelative("_summedWeight");
        rect.height = 20f;
        GUIContent label = EditorGUIUtility.IconContent(property.isExpanded ? "d_icon dropdown@2x" : "d_PlayButton");
        label.text = name.stringValue + " " + weight.floatValue + " (" + ((weight.floatValue / totalSummedWeight) * 100).ToString("F2") + "%)";
        label.tooltip = name.stringValue + " " + weight.floatValue.ToString("F2");
        if (GUI.Button(rect, label))
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
            EditorGUI.BeginChangeCheck();
            weight.floatValue = Mathf.Max(EditorGUI.DelayedFloatField(rect, new GUIContent("Weight (" + ((weight.floatValue / totalSummedWeight) * 100).ToString("F2") + " %)"), weight.floatValue), 0f);
            rect.y += rect.height;
            if (EditorGUI.EndChangeCheck())
            {
                int insertingIndex = FindInsertIndex(weight.floatValue, objects);
                if (insertingIndex != index)
                {
                    objects.MoveArrayElement(index, insertingIndex > index ? insertingIndex - 1 : insertingIndex);
                    CalculateSummedWeights(objects);
                }
            }
            rect.height = EditorGUI.GetPropertyHeight(obj, true);
            EditorGUI.PropertyField(rect, obj);
            rect.y += rect.height;
            EditorGUI.BeginDisabledGroup(true);
            rect.height = EditorGUI.GetPropertyHeight(summedWeight, true);
            EditorGUI.PropertyField(rect, summedWeight);
            rect.y += rect.height;
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
        }
    }

    private void CalculateSummedWeights(SerializedProperty array)
    {
        float sum = 0f;
        for (int i = 0; i < array.arraySize; i++)
        {
            SerializedProperty sp = array.GetArrayElementAtIndex(i);
            sum += sp.FindPropertyRelative("_weight").floatValue;
            sp.FindPropertyRelative("_summedWeight").floatValue = sum;
        }
    }

    private int FindInsertIndex(float weight, SerializedProperty array)
    {
        for (int i = 0; i < array.arraySize; i++)
        {
            if (array.GetArrayElementAtIndex(i).FindPropertyRelative("_weight").floatValue > weight)
            {
                return i;
            }
        }
        return array.arraySize;
    }

    private float ObjectsElementHeightCallback(int index, SerializedProperty objects)
    {
        SerializedProperty property = objects.GetArrayElementAtIndex(index);
        if (!property.isExpanded)
        {
            return 20f;
        }
        SerializedProperty name = property.FindPropertyRelative("_name");
        SerializedProperty weight = property.FindPropertyRelative("_weight");
        SerializedProperty obj = property.FindPropertyRelative("_object");
        SerializedProperty summedWeight = property.FindPropertyRelative("_summedWeight");
        return 20f + EditorGUI.GetPropertyHeight(name, true) +
           EditorGUI.GetPropertyHeight(weight, true) +
           EditorGUI.GetPropertyHeight(obj, true) +
           EditorGUI.GetPropertyHeight(summedWeight, true);
    }
}
