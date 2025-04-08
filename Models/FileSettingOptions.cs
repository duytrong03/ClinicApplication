public class FileSettingOptions
{
    public string ImagePath { get; set; } = string.Empty;
    public int MaxFileSize { get; set; }
    public List<string> AllowedExtensions {get; set;} = new();
}