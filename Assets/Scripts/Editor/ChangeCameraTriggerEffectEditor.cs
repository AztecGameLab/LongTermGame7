﻿namespace Editor
{
    using Application.Gameplay;
    using Cinemachine;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Renders a custom editor for the Camera Change trigger effect.
    /// </summary>
    [CustomEditor(typeof(ChangeCameraTrigger))]
    public class ChangeCameraTriggerEffectEditor : Editor
    {
        private SerializedProperty _camera;

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Camera"))
            {
                serializedObject.Update();
                var cameraTrigger = (ChangeCameraTrigger)target;

                var prefab = UnityEngine.AddressableAssets.Addressables.InstantiateAsync("cam-tracking")
                    .WaitForCompletion().GetComponent<CinemachineVirtualCamera>();
                var newCamera = Instantiate(prefab, cameraTrigger.transform, true);
                newCamera.Priority = 0;
                Selection.activeObject = newCamera;

                Undo.RegisterCreatedObjectUndo(newCamera.gameObject, "Create Trigger Camera");
                _camera.objectReferenceValue = newCamera;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnEnable()
        {
            _camera = serializedObject.FindProperty("targetCamera");
        }
    }
}
