using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

//[CustomEditor(typeof(WeightedRandomPicker))]
public class WeightedRandomPickerEditor : Editor
{
    private SerializedProperty _objects;
    private ReorderableList _objectsEditorList;

    private WeightedRandomPicker _object;

    private void OnEnable()
    {
        _objects = serializedObject.FindProperty("_objects");
        _objectsEditorList = new ReorderableList(serializedObject, _objects, false, true, true, true);
        _objectsEditorList.drawHeaderCallback = ObjectsDrawHeaderCallback;
        _objectsEditorList.drawElementCallback = ObjectsDrawElementCallback;
        _objectsEditorList.elementHeightCallback = ObjectsElementHeightCallback;

        //_object = (WeightedRandomPicker)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        _objectsEditorList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            _object.Objects = _object.Objects.OrderBy((x) => x.Weight).ToArray();
            serializedObject.Update();
            float sum = 0f;
            for (int i = 0; i < _objects.arraySize; i++)
            {
                SerializedProperty sp = _objects.GetArrayElementAtIndex(i);
                sum += sp.FindPropertyRelative("_weight").floatValue;
                sp.FindPropertyRelative("_summedWeight").floatValue = sum;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
    private void ObjectsDrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, _objects.displayName);
    }

    private void ObjectsDrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        float totalSummedWeight = 0f;
        for (int i = 0; i < _objects.arraySize; i++)
        {
            totalSummedWeight+=_objects.GetArrayElementAtIndex(i).FindPropertyRelative("_weight").floatValue;
        }
        SerializedProperty property = _objects.GetArrayElementAtIndex(index);
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
            weight.floatValue = Mathf.Max(EditorGUI.DelayedFloatField(rect, new GUIContent("Weight (" + ((weight.floatValue / totalSummedWeight) * 100).ToString("F2") + " %)"), weight.floatValue), 0f);
            rect.y += rect.height;
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

    private float ObjectsElementHeightCallback(int index)
    {
        SerializedProperty property = _objects.GetArrayElementAtIndex(index);
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
