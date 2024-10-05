using Store.Domain.Entities.Interfaces;

namespace Store.Application.Responses
{
    public class CreateUserResponse
    {
        public IUser? User { get; set; }
        public bool Success { get; init; }
        public string[]? Errors { get; init; }
    }
}
