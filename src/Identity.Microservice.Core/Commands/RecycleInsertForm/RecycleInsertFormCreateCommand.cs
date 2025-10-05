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

public record RecycleInsertFormCreateCommand(RecycleInsertFormCreateOrUpdateDto Command) : IRequest<RecycleInsertFormListDto>;

public class RecycleInsertFormCreateHandler : BaseService, IRequestHandler<RecycleInsertFormCreateCommand, RecycleInsertFormListDto>
{
    public RecycleInsertFormCreateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }

    public async Task<RecycleInsertFormListDto> Handle(RecycleInsertFormCreateCommand request, CancellationToken cancellationToken)
    {
        RecycleInsertForm entity = Mapper.Map<RecycleInsertForm>(request.Command);
        await UnitOfWork.GetRepository<RecycleInsertForm>()
            .AddAsync(entity);

        await UnitOfWork.CommitAsync();

        RecycleInsertForm result = await UnitOfWork.GetRepository<RecycleInsertForm>()
            .AsTableNoTracking
            .Include(x => x.CustomerLocation)
            .Include(x => x.MethodType)
            .Include(x => x.MaterialType)
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);

        return Mapper.Map<RecycleInsertFormListDto>(result);
    }
}

public sealed class RecycleInsertFormCreateCommandValidator : AbstractValidator<RecycleInsertFormCreateCommand>
{
    //TODO Validations
    public RecycleInsertFormCreateCommandValidator(IUnitOfWork unitOfWork)
    {

    }
}