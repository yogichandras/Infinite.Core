using System.ComponentModel.DataAnnotations;

namespace INFINITE.CORE.Shared.Attributes
{
    public class QueryRequest
    {
        public string Query { get; set; }
    }

    public class ListRequest
    {
        public List<FilterRequest> Filter { get; set; }

        [Required]
        public SortRequest Sort { get; set; } = null!;
        public int? Start { get; set; }
        public int? Length { get; set; }

        public ListRequest()
        {
            Filter = new List<FilterRequest>();
        }
    }
    public class FilterRequest
    {
        public string Field { get; set; } = null!;
        public string Search { get; set; } = null!;

        public FilterRequest()
        {

        }

        public FilterRequest(string field, string search)
        {
            Field = field;
            Search = search;
        }
    }

    public class SortRequest
    {
        public string Field { get; set; } = null!;
        public SortTypeEnum Type { get; set; }

        public SortRequest()
        {

        }

        public SortRequest(string field, SortTypeEnum type)
        {
            Field = field;
            Type = type;
        }
    }

    public enum SortTypeEnum
    {
        ASC,
        DESC
    }

    public class AddRequest<T>
    {
        public T Data { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime CreateDate { get { return DateTime.Now; } }
    }

    public class UpdateRequest<T>
    {
        public T Data { get; set; } = default!;
        public string UpdateBy { get; set; } = null!;
        public DateTime UpdateDate { get { return DateTime.Now; } }
    }
}
