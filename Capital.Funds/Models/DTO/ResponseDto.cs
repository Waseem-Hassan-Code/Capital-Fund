namespace Capital.Funds.Models.DTO
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Object Results {  get; set; }
    }
}
