using UnityEngine;

public class FlipbookAnimationPlayer : MonoBehaviour
{
    [SerializeField] 
    private FlipbookAnimationData initialAnimation;

    [SerializeField] 
    private MeshRenderer meshRenderer;

    private MeshRenderer _meshRenderer;
    private bool _hasMeshRenderer;

    private MeshRenderer MeshRenderer
    {
        get => _meshRenderer;
        set
        {
            _hasMeshRenderer = value != null;
            _meshRenderer = value;
        }
    }
    
    private FlipbookAnimationData _currentAnimation;
    private bool _hasAnimation;
    
    public FlipbookAnimationData CurrentAnimation
    {
        get => _currentAnimation;
        set
        {
            _hasAnimation = value != null;
            
            if (_hasMeshRenderer && _hasAnimation)
                MeshRenderer.material = value.Material;
        }
    }

    private void OnValidate()
    {
        CurrentAnimation = initialAnimation;
        MeshRenderer = meshRenderer;
    }
}