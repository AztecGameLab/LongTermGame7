namespace Editor.Windows.Sprite
{
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// An editor utility that generates GameObjects from a sprite.
    /// </summary>
    public class SpriteToGameObjectEditor : EditorWindow
    {
        private const float CustomWidth = 12f;
        private const float MaxScale = 100f;

        private readonly string[] _faceOptions = { "Two Sided", "One Sided" };

        private float _scale = 1f;
        private Vector3 _rotation = new Vector3(15f, -90f, 0f);
        private Sprite _sprite;
        private Shader _shader;
        private int _selectedFace;

        [MenuItem("Window/Custom/Sprite/GameObject Creator", false, 0)]
        private static void ShowWindow()
        {
            GetWindow<SpriteToGameObjectEditor>("Sprite to GameObject");
        }

        private void OnEnable()
        {
            _shader = Shader.Find("Universal Render Pipeline/Lit");
        }

        private void OnGUI()
        {
            // title of tool
            GUILayout.Label("Create GameObjects from Sprites", EditorStyles.boldLabel);
            EditorGUILayout.Space(CustomWidth);

            // get shader
            _shader = (Shader)EditorGUILayout.ObjectField("Shader", _shader, typeof(Shader), true);
            EditorGUILayout.Space(CustomWidth);

            // render face
            _selectedFace = EditorGUILayout.Popup("Render Face", _selectedFace, _faceOptions);
            EditorGUILayout.Space(CustomWidth);
            GUILayout.Label("Render face doesnt apply to default shader.", EditorStyles.toolbar);
            EditorGUILayout.Space(CustomWidth);

            // allow for change of scale of instantiated GameObject
            _scale = EditorGUILayout.Slider("Scale", _scale, 0f, MaxScale);
            EditorGUILayout.Space(CustomWidth);

            // allow for change of rotation of instantiated GameObject
            _rotation = EditorGUILayout.Vector3Field("Rotation Offset", _rotation);
            EditorGUILayout.Space(CustomWidth);

            // get sprite
            _sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", _sprite, typeof(Sprite), true);
            EditorGUILayout.Space(CustomWidth);

            if (GUILayout.Button("Instantiate GameObject"))
            {
                if (_sprite == null)
                {
                    Debug.LogWarning("Select a sprite to create GameObject");
                    return;
                }

                if (_shader == null)
                {
                    Debug.LogWarning("Select a shader to create GameObject");
                    return;
                }

                Undo.SetCurrentGroupName("Generate Sprite GameObject");
                int group = Undo.GetCurrentGroup();

                var candidateGameObject = CreateGameObject();
                PlaceObjectWithValues(candidateGameObject);
                ApplySpriteTextureAlt(candidateGameObject);

                Undo.CollapseUndoOperations(group);
            }
        }

        private GameObject CreateGameObject()
        {
            var currentGameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            currentGameObject.name = _sprite.name;
            DestroyImmediate(currentGameObject.GetComponent<MeshCollider>());
            Undo.RegisterCreatedObjectUndo(currentGameObject, "Create Sprite Object");
            return currentGameObject;
        }

        private void PlaceObjectWithValues(GameObject currentGameObject)
        {
            if (Physics.Raycast(
                    SceneView.lastActiveSceneView.camera.transform.position,
                    SceneView.lastActiveSceneView.camera.transform.TransformDirection(Vector3.forward),
                    out var hit,
                    Mathf.Infinity))
            {
                // place the object were the scene camera is looking at
                float y = 0.5f * _scale;
                currentGameObject.transform.position = hit.point;
                currentGameObject.transform.localPosition += new Vector3(0f, y, 0f);
            }

            // adjust scale and rotation by given values
            currentGameObject.transform.Rotate(_rotation);
            currentGameObject.transform.localScale = new Vector3(_scale, _scale, _scale);
        }

        private void ApplySpriteTextureAlt(GameObject currentGameObject)
        {
            MeshRenderer meshRenderer = currentGameObject.GetComponent<MeshRenderer>();

            Material curMaterial = _selectedFace switch
            {
                0 => AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Windows/Sprite/Default_Two_Sided_Material.mat"),
                1 => AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Windows/Sprite/Default_Two_Sided_Material.mat"),
                _ => AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Windows/Sprite/Default_One_Sided_Material.mat")
            };

            Material newMat = new Material(_shader);

            newMat.CopyPropertiesFromMaterial(curMaterial);
            newMat.name = _sprite.name + "_Material";
            newMat.mainTexture = _sprite.texture;

            var spritePath = AssetDatabase.GetAssetPath(_sprite);
            var savePath = spritePath.Substring(0, spritePath.LastIndexOf("/", StringComparison.Ordinal) + 1) + newMat.name + ".mat";

            AssetDatabase.CreateAsset(newMat, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            meshRenderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(savePath);
            Debug.Log("<color=yellow>Material Saved Under: " + savePath + "</color>");
        }
    }
}
