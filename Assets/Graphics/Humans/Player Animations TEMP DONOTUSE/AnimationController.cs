using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer player;

    [SerializeField]
    private List<Material> idleMaterials;

    [SerializeField]
    private List<Material> runMaterials;

    [SerializeField]
    private int _materialIndex = 0;

    void Start()
    {
        player.material = idleMaterials[_materialIndex];
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetKey("down"))
        {
            _materialIndex = 0;
            player.material = runMaterials[_materialIndex];
        }
        else if (Input.GetKey("left"))
        {
            _materialIndex = 1;
            player.material = runMaterials[_materialIndex];
        }
        else if (Input.GetKey("right")) {
            _materialIndex = 2;
            player.material = runMaterials[_materialIndex];
        }
        else if (Input.GetKey("up")){
            _materialIndex = 3;
            player.material = runMaterials[_materialIndex];
        }
        else
        {
            player.material = idleMaterials[_materialIndex];
        }
    }
}

