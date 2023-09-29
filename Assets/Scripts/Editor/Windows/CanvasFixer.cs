using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor.Windows
{
    public class CanvasFixer : EditorWindow
    {
        [MenuItem("Window/Custom/Canvas Fixer", false, 0)]
        private static void ShowWindow()
        {
            GetWindow<CanvasFixer>("Sprite to GameObject");
        }

        private void OnGUI()
        {
            var objs = Selection.gameObjects;

            if (GUILayout.Button($"Convert {objs.Length}"))
            {
                foreach (GameObject selectedObject in objs)
                    ConvertObject(selectedObject);
            }
        }

        private void ConvertObject(GameObject gameObject)
        {
            var camera = Camera.main;

            if (gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                var image = Undo.AddComponent<Image>(gameObject);
                image.sprite = spriteRenderer.sprite;
                image.color = spriteRenderer.color;
                Undo.DestroyObjectImmediate(spriteRenderer);
            }
        }
    }
}
