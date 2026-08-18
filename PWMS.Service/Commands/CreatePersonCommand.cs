using ModernMediator;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;

namespace PWMS.Service.Commands
{
	public record CreatePersonCommand(string name, string document, string address) : IRequest<int>;

	public class CreatePersonCommandHandler(IPersonRepository db) : IValueTaskRequestHandler<CreatePersonCommand, int>
	{
		public async ValueTask<int> Handle(CreatePersonCommand request, CancellationToken ct = default)
		{
			Person ctx = new Person(request.name, request.document, request.address);

			return await db.InsertAsync(ctx);
		}
	}
}
