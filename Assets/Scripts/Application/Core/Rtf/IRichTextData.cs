namespace Application.Core.Rtf
{
    /// <summary>
    /// Tag information for encoding visual information into a string.
    /// </summary>
    public interface IRichTextData
    {
        /// <summary>
        /// Gets the first tag to use (at the beginning).
        /// </summary>
        string Opener { get; }

        /// <summary>
        /// Gets the second tag to use (at the end).
        /// </summary>
        string Closer { get; }
    }
}