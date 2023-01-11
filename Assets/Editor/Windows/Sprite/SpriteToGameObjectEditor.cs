using System;
using UnityEditor;
using UnityEngine;

public class SpriteToGameObjectEditor : EditorWindow
{
    private float _scale = 1f;
    private Vector3 _rotation = new Vector3(15f, -90f, 0f);
    private Sprite _sprite;
    private Shader _shader;
    private const float _customWidth = 12f;
    private int _selectedFace = 0;
    private readonly string[] _faceOptions = {"Two Sided", "One Sided"};

    [MenuItem("Window/Custom/Sprite/GameObject Creator", false, 0)]
    public static void ShowWindow()
    {
        GetWindow<SpriteToGameObjectEditor>("Sprite to GameObject");
    }

    private void OnEnable()
    {
        _shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
    }

    void OnGUI()
    {
        //title of tool
        GUILayout.Label("Create GameObjects from Sprites", EditorStyles.boldLabel);
        EditorGUILayout.Space(_customWidth);

        //get shader
        _shader = (Shader) EditorGUILayout.ObjectField("Shader", _shader, typeof(Shader), true);
        EditorGUILayout.Space(_customWidth);

        //render face
        _selectedFace = EditorGUILayout.Popup("Render Face", _selectedFace, _faceOptions);
        EditorGUILayout.Space(_customWidth);
        GUILayout.Label("Render face doesnt apply to default shader.",EditorStyles.toolbar);
        EditorGUILayout.Space(_customWidth);

        //allow for change of scale of instantiated gameobject
        _scale = EditorGUILayout.Slider("Scale", _scale, 0f, 100f);
        EditorGUILayout.Space(_customWidth);

        //allow for change of rotation of instantiated gameobject
        _rotation = EditorGUILayout.Vector3Field("Rotation Offset", _rotation);
        EditorGUILayout.Space(_customWidth);

        //get sprite
        _sprite = (Sprite) EditorGUILayout.ObjectField("Sprite", _sprite, typeof(Sprite), true);
        EditorGUILayout.Space(_customWidth);
        

        if (GUILayout.Button("Instantiate GameObject"))
        {
            if (_sprite is null)
            {
                Debug.LogWarning("Select a sprite to create GameObject");
                return;
            }

            var candidateGameObject = CreateGameObject();

            PlaceObjectWithValues(candidateGameObject);

            ApplySpriteTextureAlt(candidateGameObject);
            
        }
    }

    private GameObject CreateGameObject()
    {
        var currentGameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        currentGameObject.name = _sprite.name;
        DestroyImmediate(currentGameObject.GetComponent<MeshCollider>());

        return currentGameObject;
    }

    private void PlaceObjectWithValues(GameObject currentGameObject)
    {
        if (Physics.Raycast(SceneView.lastActiveSceneView.camera.transform.position,
                SceneView.lastActiveSceneView.camera.transform.TransformDirection(Vector3.forward),
                out var hit, Mathf.Infinity))
        {
            // place the object were the scene camera is looking at
            currentGameObject.transform.position = hit.point;
            currentGameObject.transform.localPosition += new Vector3(0f, 0.5f * _scale, 0f);
        }

        //adjust scale and rotation by given values
        currentGameObject.transform.Rotate(_rotation);
        currentGameObject.transform.localScale = new Vector3(_scale, _scale, _scale);
    }

    private void ApplySpriteTextureAlt(GameObject currentGameObject)
    {
        MeshRenderer meshRenderer = currentGameObject.GetComponent<MeshRenderer>();
        Material curMaterial;

        curMaterial = AssetDatabase.LoadAssetAtPath<Material>(_selectedFace == 0
            ? "Assets/Editor/Windows/Sprite/Default_Two_Sided_Material.mat"
            : "Assets/Editor/Windows/Sprite/Default_One_Sided_Material.mat");

        Material newMat = new Material(_shader);

        newMat.CopyPropertiesFromMaterial(curMaterial);
        newMat.name = _sprite.name + "_Material";
        newMat.mainTexture = _sprite.texture;

        var spritePath = AssetDatabase.GetAssetPath(_sprite);
        var savePath = spritePath.Substring(0, spritePath.LastIndexOf("/") + 1) + newMat.name + ".mat";

        AssetDatabase.CreateAsset(newMat, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        meshRenderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(savePath);
        Debug.Log("<color=yellow>Material Saved Under: " + savePath + "</color>");
    }
}