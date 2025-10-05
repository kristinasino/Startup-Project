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


public record RecycleInsertFormUpdateCommand(RecycleInsertFormCreateOrUpdateDto Command) : IRequest<RecycleInsertFormListDto>;


public class RecycleInsertFormUpdateHandler : BaseService, IRequestHandler<RecycleInsertFormUpdateCommand, RecycleInsertFormListDto>
{
    public RecycleInsertFormUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<RecycleInsertFormListDto> Handle(RecycleInsertFormUpdateCommand request, CancellationToken cancellationToken)
    {
        RecycleInsertForm entity = UnitOfWork.GetRepository<RecycleInsertForm>().Get(request.Command.Id);
        Mapper.Map(request.Command, entity);

        UnitOfWork.GetRepository<RecycleInsertForm>().Update(entity);
        await UnitOfWork.CommitAsync();

        RecycleInsertForm result = await UnitOfWork.GetRepository<RecycleInsertForm>()
            .AsTableNoTracking
            .Include(x => x.CustomerLocation)
            .Include(x => x.MethodType)
            .Include(x => x.MaterialType)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        return Mapper.Map<RecycleInsertFormListDto>(result);
    }
}

public sealed class RecycleInsertFormUpdateCommandValidator : AbstractValidator<RecycleInsertFormUpdateCommand>
{
    //TODO: Validations

    public RecycleInsertFormUpdateCommandValidator(IUnitOfWork unitOfWork)
    {

    }
}
