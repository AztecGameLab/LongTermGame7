namespace Application.Core.Utility
{
    using ElRaccoone.Tweens.Core;

    /// <summary>
    /// utilities for working with tweens.
    /// </summary>
    public static class TweenUtil
    {
        public static void Override<T>(this Tween<T> tween, ref ITween existing)
        {
            // ReSharper disable once UseNullPropagation
            if (existing != null)
            {
                existing.Cancel();
            }

            existing = tween;
        }
    }
}
