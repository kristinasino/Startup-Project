using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Identity.Microservice.Core.Dto;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Commands;

public record TonageInsertFormCreateCommand(TonageInsertFormCreateOrUpdateDto Command) : IRequest<TonageInsertFormListDto>;

public class TonageInsertFormCreateHandler : BaseService, IRequestHandler<TonageInsertFormCreateCommand, TonageInsertFormListDto>
{
    public TonageInsertFormCreateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }

    public async Task<TonageInsertFormListDto> Handle(TonageInsertFormCreateCommand request, CancellationToken cancellationToken)
    {
        TonageInsertForm entity = Mapper.Map<TonageInsertForm>(request.Command);
        await UnitOfWork.GetRepository<TonageInsertForm>()
            .AddAsync(entity);

        await UnitOfWork.CommitAsync();

        TonageInsertForm result = await UnitOfWork.GetRepository<TonageInsertForm>()
            .AsTableNoTracking
            .Include(x => x.CustomerLocation)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        return Mapper.Map<TonageInsertFormListDto>(result);
    }
}

public sealed class TonageInsertFormCreateCommandValidator : AbstractValidator<TonageInsertFormCreateCommand>
{
    //TODO Validations
    public TonageInsertFormCreateCommandValidator(IUnitOfWork unitOfWork)
    {

    }
}