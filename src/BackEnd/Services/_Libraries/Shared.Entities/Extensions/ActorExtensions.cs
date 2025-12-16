using Shared.Entities.Enums;

namespace Shared.Entities.Extensions
{
    public static class ActorExtensions
    {
        /// <summary>
        /// Returns the user ID if the actor type is <see cref="ActorType.User"/>, otherwise null.
        /// </summary>
        /// <param name="actorType">The type of actor (User, System, ExternalService, etc.).</param>
        /// <param name="createdBy">The GUID of the user who triggered the action, if applicable.</param>
        /// <returns>The GUID if actorType is User; otherwise null.</returns>
        public static Guid? ResolveUserId(this ActorType actorType, Guid? createdBy)
        {
            return actorType == ActorType.User ? createdBy : null;
        }
    }

}
