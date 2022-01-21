namespace FarmFresh.Backend.Shared
{
    public class BaseRequest
    {
        public string Query { get; set; } = string.Empty;
        public int SkipCount { get; set; } = 0;

        public int FetchSize { get; set; } = 100;    
    }
}
