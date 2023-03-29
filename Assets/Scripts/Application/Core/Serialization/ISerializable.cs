namespace Application.Core.Serialization
{
    /// <summary>
    /// An item that can be saved and loaded from the disk.
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// Gets an identifier for this object.
        /// Objects with the same ID will share the same saved state.
        /// </summary>
        /// <returns>The ID of this object.</returns>
        string Id { get; }

        /// <summary>
        /// Reads the saved data for this item from an object.
        /// </summary>
        /// <param name="data">Whatever data you previously saved, and are now loading.</param>
        void ReadData(object data);

        /// <summary>
        /// Writes the saved data for this item into an object.
        /// </summary>
        /// <returns>Whatever data you need to save.</returns>
        object WriteData();
    }
}
