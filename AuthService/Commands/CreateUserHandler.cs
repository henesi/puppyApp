using System.Threading;
using System.Threading.Tasks;
using AuthService.Api.Commands;
using AuthService.Domain;
using MediatR;

namespace AuthService.Commands
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        private readonly IUnitOfWork uow;

        public CreateUserHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var o = new User(
                request.Username,
                request.Email,
                request.Password);

            uow.User.Add(o);
            await uow.CommitChanges();

            return new CreateUserResult
            {
                UserId = o.UserId
            };
        }
    }
}
