using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Api.Queries
{
    public class GetUserQuery : IRequest<GetUserQueryResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
