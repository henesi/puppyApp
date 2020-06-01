using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Api.Queries
{
    public class GetUserQueryResult
    {
        public DateTime Start { get; set; }
        public string Token { get; set; }
    }

}
