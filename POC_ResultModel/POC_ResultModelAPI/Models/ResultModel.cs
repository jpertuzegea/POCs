namespace POC_ResultModelAPI.Models
{
    public class ResultModel<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Messages { get; set; }
        public T Result { get; set; }
    }
}
