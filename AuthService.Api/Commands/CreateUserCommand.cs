using AuthService.Api.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Api.Commands
{
    public class CreateUserCommand : IRequest<CreateUserResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
