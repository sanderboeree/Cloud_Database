namespace OnlineStore.Api.Domain.Common
{
    public interface ILifecycleEntity
    {
        public EntityStatus EntityStatus { get; set; }
    }
}
