using System.Collections.Generic;

namespace FarmFresh.Backend.Shared
{
    public class BaseResponse<T>
    {
        public string Query { get; set; } = string.Empty;
        public int SkipCount { get; set; }

        public int FetchSize { get; set; }

        public IEnumerable<T> Rows { get; set; } = new List<T>();

        public int TotalRows { get; set; }
    }
}
