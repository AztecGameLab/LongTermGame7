using Application.Gameplay;
using Application.Level;
using Application.UI;
using Application.Vfx;
using UnityEngine;
using AudioSettings = Application.Audio.AudioSettings;

namespace Application
{
    [CreateAssetMenu(menuName = ApplicationConstants.AssetMenuName + "/Application Settings")]
    public class ApplicationSettings : ScriptableObject
    {
        #region Fields

        [SerializeField] 
        private LevelSettings levelSettings;
        
        [SerializeField] 
        private GameplaySettings gameplaySettings;
        
        [SerializeField] 
        private AudioSettings audioSettings;
        
        [SerializeField] 
        private VfxSettings vfxSettings;
        
        [SerializeField] 
        private UISettings uiSettings;

        #endregion
    
        public LevelSettings Level => levelSettings;
        public GameplaySettings Gameplay => gameplaySettings;
        public AudioSettings Audio => audioSettings;
        public VfxSettings Vfx => vfxSettings;
        public UISettings Ui => uiSettings;
    }
}