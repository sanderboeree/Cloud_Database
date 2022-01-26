using OnlineStore.Api.Infrastructure.Specifications;

namespace OnlineStore.Api.Application.Common
{
    public class BaseFilter
    {
        public string TextValue { get; set; }
        public string OrderBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public int PageNumber { get; set; }
    }
}