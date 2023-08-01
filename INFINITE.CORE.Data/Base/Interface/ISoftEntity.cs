namespace INFINITE.CORE.Data.Base.Interface
{
    public interface ISoftEntity : IEntity
    {
        bool IsDeleted { get; set; }
    }
}
