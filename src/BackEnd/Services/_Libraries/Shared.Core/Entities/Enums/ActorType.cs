namespace Shared.Core.Entities.Enums
{
    /// <summary>
    /// The type of actor performing an action within the system.
    /// </summary>
    public enum ActorType
    {
        /// <summary>
        /// Denotes an individual user who interacts with the system, typically representing a human operator or end-user.
        /// </summary>
        User,

        /// <summary>
        /// Denotes the system itself acting autonomously, such as automated processes, scheduled tasks, or background services.
        /// </summary>
        System,

        /// <summary>
        /// Denotes an external service or application that interacts with the system, often through APIs or integrations.
        /// </summary>
        ExternalService
    }
}
