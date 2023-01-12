namespace Application.Core
{
    /// <summary>
    /// A global access point for cross-cutting concerns.
    /// </summary>
    public static class Services
    {
        /// <summary>
        /// Gets or sets the global EventBus.
        /// </summary>
        public static EventBus EventBus { get; set; }
    }
}