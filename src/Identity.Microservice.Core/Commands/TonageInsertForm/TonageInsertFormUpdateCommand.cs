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


public record TonageInsertFormUpdateCommand(TonageInsertFormCreateOrUpdateDto Command) : IRequest<TonageInsertFormListDto>;


public class TonageInsertFormUpdateHandler : BaseService, IRequestHandler<TonageInsertFormUpdateCommand, TonageInsertFormListDto>
{
    public TonageInsertFormUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<TonageInsertFormListDto> Handle(TonageInsertFormUpdateCommand request, CancellationToken cancellationToken)
    {
        TonageInsertForm entity = UnitOfWork.GetRepository<TonageInsertForm>().Get(request.Command.Id);
        Mapper.Map(request.Command, entity);

        UnitOfWork.GetRepository<TonageInsertForm>().Update(entity);
        await UnitOfWork.CommitAsync();

        TonageInsertForm result = await UnitOfWork.GetRepository<TonageInsertForm>()
            .AsTableNoTracking
            .Include(x => x.CustomerLocation)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        return Mapper.Map<TonageInsertFormListDto>(result);
    }
}

public sealed class TonageInsertFormUpdateCommandValidator : AbstractValidator<TonageInsertFormUpdateCommand>
{
    //TODO: Validations

    public TonageInsertFormUpdateCommandValidator(IUnitOfWork unitOfWork)
    {

    }
}
