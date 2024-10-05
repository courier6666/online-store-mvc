namespace Store.Domain.Entities.Interfaces
{
    /// <summary>
    /// Interface that represents account info of user account.
    /// </summary>
    public interface IAccountInfo
    {
        /// <summary>
        /// Contains user's login.
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Contains user's password in a hashed form. 
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
