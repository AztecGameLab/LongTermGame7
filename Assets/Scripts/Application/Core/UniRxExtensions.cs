namespace Application.Core
{
    using System;
    using UniRx;

    /// <summary>
    /// Static extension method for making life easier with UniRx.
    /// </summary>
    public static class UniRxExtensions
    {
        /// <summary>
        /// Binds data with the previously emitted value.
        /// </summary>
        /// <param name="source">Source observable type.</param>
        /// <typeparam name="TSource">Source observable.</typeparam>
        /// <returns>A modified observable with historical data.</returns>
        public static IObservable<WithPreviousData<TSource>> WithPrevious<TSource>(this IObservable<TSource> source)
        {
            return source.Scan(
                new WithPreviousData<TSource>(),
                (acc, current) => new WithPreviousData<TSource>(acc.Current, current));
        }
    }
}
