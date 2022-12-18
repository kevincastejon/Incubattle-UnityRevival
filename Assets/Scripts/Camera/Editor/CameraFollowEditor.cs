using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor : Editor
{
    private SerializedProperty _followType;
    private SerializedProperty _updateType;
    private SerializedProperty _camera;
    private SerializedProperty _minDistance;
    private SerializedProperty _speed;
    private SerializedProperty _smoothTime;
    private SerializedProperty _maxSpeed;
    private SerializedProperty _worldConstraint;

    private CameraFollow _script;

    private void OnEnable()
    {
        _followType = serializedObject.FindProperty("_followType");
        _updateType = serializedObject.FindProperty("_updateType");
        _camera = serializedObject.FindProperty("_camera");
        _minDistance = serializedObject.FindProperty("_minDistance");
        _speed = serializedObject.FindProperty("_speed");
        _smoothTime = serializedObject.FindProperty("_smoothTime");
        _maxSpeed = serializedObject.FindProperty("_maxSpeed");
        _worldConstraint = serializedObject.FindProperty("_worldConstraint");

        _script = (CameraFollow)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_worldConstraint);
        EditorGUILayout.PropertyField(_followType);
        FollowType followType = (FollowType)_followType.enumValueIndex;
        EditorGUILayout.PropertyField(_updateType);
        EditorGUILayout.PropertyField(_camera);
        if (followType != FollowType.SNAP)
        {
            EditorGUILayout.PropertyField(_minDistance);
        }
        if (followType == FollowType.LERP)
        {
            EditorGUILayout.PropertyField(_speed);
        }
        if (followType == FollowType.SMOOTHDAMP)
        {
            EditorGUILayout.PropertyField(_smoothTime);
            EditorGUILayout.PropertyField(_maxSpeed);
        }
        

        serializedObject.ApplyModifiedProperties();
    }
}
