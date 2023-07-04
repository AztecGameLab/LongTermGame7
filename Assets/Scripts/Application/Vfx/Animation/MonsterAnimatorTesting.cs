using System;

namespace Application.Vfx.Animation
{
    using System.Collections.Generic;
    using System.Threading;
    using Core;
    using Core.Utility;
    using Cysharp.Threading.Tasks;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Allows easy testing of a monster animator.
    /// </summary>
    public class MonsterAnimatorTesting : MonoBehaviour
    {
        [SerializeField]
        private Vector3 menuOffset = Vector3.up;

        private OverworldContextMenu _menu;
        private CancellationTokenSource _cts;

        private void Start()
        {
            _menu = OverworldContextMenu.Create(
                transform.position + menuOffset + (Vector3.back * 0.1f),
                "Testing Animations",
                new List<OverworldContextMenu.Option>
                {
                    new OverworldContextMenu.Option
                    {
                        Name = "Walk", Callback = () =>
                        {
                            _cts?.Cancel();
                            _cts = new CancellationTokenSource();
                            WalkingRoutine(_cts.Token).Forget();
                        },
                    },
                    new OverworldContextMenu.Option { Name = "Idle", Callback = () => _cts?.Cancel() },
                });

            InputTools.ObjectSelect.Subscribe(obj =>
                {
                    if (obj.GetGameObject() == gameObject)
                    {
                        _menu.Show();
                    }
                    else
                    {
                        _menu.Hide();
                    }
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _cts?.Dispose();
        }

        private async UniTask WalkingRoutine(CancellationToken token = default)
        {
            Vector3 start = transform.position;
            Vector3 end = start + (Vector3.right * 4);

            while (!token.IsCancellationRequested)
            {
                await transform.PathFindTo(end, stopDistance: 0).ToUniTask(cancellationToken: token);
                await transform.PathFindTo(start, stopDistance: 0).ToUniTask(cancellationToken: token);
            }
        }
    }
}
