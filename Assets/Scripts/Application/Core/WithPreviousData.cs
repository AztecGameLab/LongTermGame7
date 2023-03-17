namespace Application.Core
{
    /// <summary>
    /// Simple data container that wraps the current and previous state of a value.
    /// </summary>
    /// <typeparam name="T">The type of data to wrap.</typeparam>
    public readonly struct WithPreviousData<T>
    {
        /// <summary>
        /// The previous value of this data.
        /// </summary>
        public readonly T Previous;

        /// <summary>
        /// The current value of this data.
        /// </summary>
        public readonly T Current;

        /// <summary>
        /// Initializes a new instance of the <see cref="WithPreviousData{T}"/> struct.
        /// </summary>
        /// <param name="previous">The previous value of the data.</param>
        /// <param name="current">The current value of the data.</param>
        public WithPreviousData(T previous, T current)
        {
            Previous = previous;
            Current = current;
        }
    }

    /// <summary>
    /// Static extension methods for WithPrevious data.
    /// </summary>
    public static class WithPreviousExtensions
    {
        /// <summary>
        /// Computes the delta between int historical data.
        /// </summary>
        /// <param name="data">The int data.</param>
        /// <returns>The delta between the data.</returns>
        public static int Delta(this WithPreviousData<int> data)
        {
            return data.Current - data.Previous;
        }

        /// <summary>
        /// Computes the delta between float historical data.
        /// </summary>
        /// <param name="data">The float data.</param>
        /// <returns>The delta between the data.</returns>
        public static float Delta(this WithPreviousData<float> data)
        {
            return data.Current - data.Previous;
        }
    }
}
