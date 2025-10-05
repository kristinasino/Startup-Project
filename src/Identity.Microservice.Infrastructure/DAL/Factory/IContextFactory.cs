namespace Infrastructure.DAL.Factory
{
    public interface IContextFactory
    {
        IDataContext DbContext { get; }
    }
}