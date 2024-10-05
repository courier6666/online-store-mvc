namespace Store.Application.Responses
{
    public class LogInResponse
    {
        public Guid? UserId { get; init; }
        public string[] UserRoles { get; init; }
        public bool LoginFound { get; init; }
        public bool IsPasswordCorrect { get; init; }
        public bool SignedIn { get; init; }
    }
}
