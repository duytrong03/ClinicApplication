namespace ClinicApplication.Models
{
    public class UploadResult
    {
        public bool Success {get; set;}
        public string? Message {get; set;}
        public string? FilePath {get; set;}

        public UploadResult(bool success, string? message, string? filePath)
        {
            Success = success;
            Message = message;
            FilePath = filePath;
        }
    }
}