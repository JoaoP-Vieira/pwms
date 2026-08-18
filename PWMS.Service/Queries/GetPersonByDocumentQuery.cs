using ModernMediator;
using PWMS.Core.Interfaces;
using PWMS.Service.DTO;

namespace PWMS.Service.Queries
{
	public record GetPersonByDocumentQuery(string document) : IRequest<PersonDTO>;

	public class GetPersonByDocumentQueryHandler : IValueTaskRequestHandler<GetPersonByDocumentQuery, PersonDTO>
	{

		private readonly IPersonRepository _personRepository;

		public GetPersonByDocumentQueryHandler(IPersonRepository personRepository)
		{
			_personRepository = personRepository;
		}

		public async ValueTask<PersonDTO> Handle(GetPersonByDocumentQuery request, CancellationToken cancellationToken = default)
		{
			var result = await _personRepository.SelectByDocument(request.document);

			if (result == null)
				throw new InvalidOperationException("Person not found");

			return new PersonDTO()
			{
				Id = result.Id,
				Name = result.Name,
				Document = result.GetDocument(),
				Address = result.Address,
			};
		}
	}
}
