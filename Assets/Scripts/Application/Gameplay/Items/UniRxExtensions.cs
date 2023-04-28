using System;
using TMPro;
using UniRx;

namespace Application.Gameplay.Items
{
    public static class UniRxExtensions
    {
        public static IDisposable SubscribeToText(this IObservable<string> observable, TMP_Text text)
        {
            return observable.Subscribe(value => text.text = value);
        }
    }
}