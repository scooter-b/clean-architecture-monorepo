namespace User.Api.Models
{
    public class UpdateUserRequestDto
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string? Email { get; set; }
    }
}
