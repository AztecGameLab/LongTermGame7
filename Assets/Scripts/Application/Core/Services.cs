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
        /// <value>
        /// The global EventBus.
        /// </value>
        public static EventBus EventBus { get; set; }
    }
}
