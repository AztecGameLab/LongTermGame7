using UnityEngine;

[ExecuteAlways]
public class CpuAnimationPlayer : MonoBehaviour
{
    [SerializeField] 
    private CpuAnimationData initialAnimation;

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
    
    private CpuAnimationData _currentAnimation;
    private bool _hasAnimation;
    
    public CpuAnimationData CurrentAnimation
    {
        get => _currentAnimation;
        set
        {
            _hasAnimation = value != null;
            _currentAnimation = value;
        }
    }

    private void OnValidate()
    {
        CurrentAnimation = initialAnimation;
        MeshRenderer = meshRenderer;
    }

    private void Update()
    {
        if (_hasAnimation && _hasMeshRenderer)
        {
            var currentFrame = (int) (Time.time * CurrentAnimation.FrameRate) % CurrentAnimation.Frames.Count;
            MeshRenderer.material.mainTexture = CurrentAnimation.Frames[currentFrame];
        }
    }
}