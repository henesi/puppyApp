using AuthService.Api.Commands;
using AuthService.Api.Queries;
using AuthService.Configuration;
using AuthService.Domain;
using AuthService.Domain.Helpers;
using AuthService.Models;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Queries
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserQueryResult>
    {
        private readonly IUnitOfWork uow;
        private readonly ConfigurationOptions _configurationOptions;

        public GetUserHandler(IUnitOfWork uow, IOptions<ConfigurationOptions> configurationOptions)
        {
            this.uow = uow;
            this._configurationOptions = configurationOptions.Value;
        }

        public async Task<GetUserQueryResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await uow.User.WithUsername(request.Username);

            if (user == null) { throw new Exception("User does not exist"); }
            if(PasswordCrypt.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash))
            {
                return new GetUserQueryResult()
                {
                    Start = DateTime.Now,
                    Token = Token.CreateJwtToken(user, Encoding.ASCII.GetBytes(_configurationOptions.SECRET))
                };
            }
            else
            {
                throw new Exception("Credentials does not match");
            }
        }
    }
}
