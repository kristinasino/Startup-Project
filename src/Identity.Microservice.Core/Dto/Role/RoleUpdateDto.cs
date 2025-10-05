
namespace Core.Dto.Role
{
    public class RoleUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] Permissions { get; set; }
    }

    // public class RoleUpdateValidator : AbstractValidator<RoleUpdateDto>
    // {
    //     private readonly IUnitOfWork _unitOfWork;
    //
    //     public RoleUpdateValidator(IUnitOfWork unitOfWork)
    //     {
    //         _unitOfWork = unitOfWork;
    //
    //         RuleFor(x => x.Name)
    //             .NotEmpty()
    //             .WithMessage("Name is required")
    //             .MustAsync(async (x, y, z) => await IsNameTaken(x))
    //             .WithMessage(x => $"Name {x.Name} is taken");
    //     }
    //
    //     public async Task<bool> IsNameTaken(RoleUpdateDto roleUpdateDto)
    //     {
    //         var role = await _unitOfWork.GetRepository<Identity.Microservice.Domain.Entities.Role>().AsTableNoTracking
    //             .FirstOrDefaultAsync(x => x.Name.ToLower() == roleUpdateDto.Name.ToLower().Replace(" ", ""));
    //
    //         return role == null || role.Id == roleUpdateDto.Id;
    //     }
    // }
}