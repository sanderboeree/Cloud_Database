namespace OnlineStore.Api.Domain
{
    public abstract class SoftDeleteEntity : Entity
    {
        public bool IsDeleted { get; set; }
    }
}
