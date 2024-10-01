using Store.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Responses
{
    public class CreateUserResponse
    {
        public IUser? User { get; set; }
        public bool Success { get; init; }
        public string[]? Errors { get; init; }
    }
}
