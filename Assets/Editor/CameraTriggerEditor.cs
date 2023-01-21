using Application.Gameplay;
using Cinemachine;
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraTrigger))]
public class CameraTriggerEditor : UnityEditor.Editor
{
    private SerializedProperty _camera;

    private void OnEnable()
    {
        _camera = serializedObject.FindProperty("targetCamera");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Camera"))
        {
            serializedObject.Update();
            var cameraTrigger = (CameraTrigger)target;

            var prefab = Resources.Load<CinemachineVirtualCamera>("Tracking Camera");
            var newCamera = Instantiate(prefab, cameraTrigger.transform, true);
            newCamera.Priority = 0;
            newCamera.Follow = FindObjectOfType<PlayerMovement>().transform;
            Selection.activeObject = newCamera;

            Undo.RegisterCreatedObjectUndo(newCamera.gameObject, "Create Trigger Camera");
            _camera.objectReferenceValue = newCamera;
            serializedObject.ApplyModifiedProperties();

            // cameraTrigger.SetCamera(newCamera);
        }
    }
}
