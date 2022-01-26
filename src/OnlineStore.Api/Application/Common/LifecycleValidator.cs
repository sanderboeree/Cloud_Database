using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Domain;
using OnlineStore.Api.Domain.Common;
using OnlineStore.Api.Infrastructure.Crud.Interfaces;

namespace OnlineStore.Api.Application.Common
{
    public static class LifecycleValidatorExtension
    {
        public static IRuleBuilderOptions<T, TElement> ValidLifecycle<T, TElement, TEntity, TDto>(this IRuleBuilder<T, TElement> ruleBuilder, ICrudService<TEntity, TDto> crudService, EntityStatus[] validStatuses)
            where TEntity : Entity
            where TDto : Dto<TEntity>
        {
            return ruleBuilder.SetValidator(new LifecycleValidator<TEntity, TDto>(crudService, validStatuses));
        }
    }
    public class LifecycleValidator<TEntity, TDto> : AsyncValidatorBase
        where TEntity : Entity
        where TDto : Dto<TEntity>
    {
        private readonly ICrudService<TEntity, TDto> _crudService;
        private readonly EntityStatus[] _validStatuses;

        public LifecycleValidator(ICrudService<TEntity, TDto> crudService, EntityStatus[] validStatuses)
        {
            _crudService = crudService;
            _validStatuses = validStatuses;
        }

        protected override string GetDefaultMessageTemplate() => "{PropertyName} does not have a valid status.";

        protected override async Task<bool> IsValidAsync(PropertyValidatorContext context, CancellationToken cancellation)
        {
            var guid = context.PropertyValue as Guid?;
            if (guid == null || guid == Guid.Empty)
            {
                return true;
            }
            var id = (Guid)guid;
            var entity = await _crudService.GetByIdAsync(id) as ILifecycleEntity;
            return _validStatuses.Contains(entity.EntityStatus);
        }

    }
}
