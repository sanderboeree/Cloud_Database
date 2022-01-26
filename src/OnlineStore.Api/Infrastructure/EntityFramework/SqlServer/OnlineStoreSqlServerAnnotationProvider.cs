using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Api.Infrastructure.EntityFramework.SqlServer
{
    public class OnlineStoreSqlServerAnnotationProvider : SqlServerAnnotationProvider
    {
        public OnlineStoreSqlServerAnnotationProvider(RelationalAnnotationProviderDependencies dependencies) : base(dependencies)
        {

        }

        public override IEnumerable<IAnnotation> For(IColumn column)
        {
            var property = column.PropertyMappings.Select(mapping => mapping.Property).FirstOrDefault();

            return property != null
                ? base.For(column).Concat(property.GetAnnotations().Where(annotation => annotation.Name.StartsWith(OnlineStoreAnnotation.Namespace)))
                : base.For(column);
        }
    }
}
