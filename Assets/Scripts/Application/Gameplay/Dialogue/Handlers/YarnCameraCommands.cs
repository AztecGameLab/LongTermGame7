namespace Application.Gameplay
{
    using System;
    using System.Collections;
    using Cinemachine;
    using Cysharp.Threading.Tasks;
    using Dialogue;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using UniRx;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Yarn.Unity;
    using Object = UnityEngine.Object;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Yarn camera commands.
    /// </summary>
    [Serializable]
    public class YarnCameraCommands : IYarnCommandHandler
    {
        private const int ActivePriority = 30;

        [SerializeField]
        private CinemachineVirtualCamera dialogueCameraPrefab;

        [SerializeField]
        private EaseType easeType;

        [SerializeField]
        private Vector3 trackedObjectOffset = new Vector3(0, 2, 0);

        [SerializeField]
        private float followSpeed = 5;

        private CinemachineVirtualCamera _cam;
        private Transform _currentTarget;
        private bool _hasTarget;

        private Vector3 TargetPosition => trackedObjectOffset + (_hasTarget ? _currentTarget.position : Vector3.zero);

        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            Application.quitting += Cleanup;

            _cam = Object.Instantiate(dialogueCameraPrefab, runner.transform);
            _cam.gameObject.SetActive(false);
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(runner);

            runner.AddCommandHandler<float, float, float, float>("cam-offset", RunOffsetHandler);
            runner.AddCommandHandler<GameObject, float>("cam-follow", RunFollowHandler);
            runner.AddCommandHandler("cam-free", FreeCamera);

            runner.onNodeStart.AddListener(ActivateDialogueCamera);
            runner.onNodeComplete.AddListener(DeactivateDialogueCamera);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("cam-offset");
            runner.RemoveCommandHandler("cam-follow");
            runner.RemoveCommandHandler("cam-refocus");

            runner.onNodeStart.RemoveListener(ActivateDialogueCamera);
            runner.onNodeComplete.RemoveListener(DeactivateDialogueCamera);
        }

        private void Cleanup()
        {
            SceneManager.activeSceneChanged -= HandleSceneChange;
            Application.quitting -= Cleanup;
            _hasTarget = false;
        }

        private async void HandleSceneChange(Scene arg0, Scene arg1)
        {
            await Task.Delay(10);
            ActivateCam();
        }

        private Coroutine RunOffsetHandler(float arg1, float arg2, float arg3, float arg4 = 1) =>
            _cam.StartCoroutine(MoveAbs(arg1, arg2, arg3, arg4).ToCoroutine());

        private Coroutine RunFollowHandler(GameObject arg, float time = 1) =>
            _cam.StartCoroutine(Follow(arg, time).ToCoroutine());

        private async UniTask Follow(GameObject target, float time = 1)
        {
            if (target == null)
            {
                Debug.LogWarning("Unable to find Follow target from yarn!");
                return;
            }

            await MoveTo(target.transform.position, time);
            _currentTarget = target.transform;
            _hasTarget = true;
            Debug.Log("done!");
        }

        private async UniTask MoveTo(Vector3 target, float time)
        {
            _cam.TweenPosition(trackedObjectOffset + target, time).SetEase(easeType);
            await UniTask.Delay(TimeSpan.FromSeconds(time));
        }

        private async UniTask MoveAbs(float x, float y, float z, float timeScalar = 0)
        {
            FreeCamera();
            await MoveTo(new Vector3(x, y, z), timeScalar);
        }

        private void FreeCamera()
        {
            _hasTarget = false;
            _currentTarget = null;
        }

        private void ActivateDialogueCamera(string node)
        {
            SceneManager.activeSceneChanged += HandleSceneChange;
            ActivateCam();
        }

        private void ActivateCam()
        {
            _cam.gameObject.SetActive(true);
            _cam.Priority = ActivePriority;
            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _currentTarget = player.transform;
                _hasTarget = true;
                _cam.transform.position = TargetPosition;
            }
        }

        private void DeactivateCam()
        {
            _cam.gameObject.SetActive(false);
            _cam.Priority = 0;
            FreeCamera();
        }

        private void DeactivateDialogueCamera(string node)
        {
            SceneManager.activeSceneChanged -= HandleSceneChange;
            DeactivateCam();
        }

        private void Update()
        {
            if (_hasTarget)
            {
                Transform camTransform = _cam.transform;
                camTransform.position = Vector3.Lerp(camTransform.position, TargetPosition, followSpeed * Time.deltaTime);
            }
        }
    }
}
