namespace Zhzt.Exam.StaticFileSystem
{
    public class FileSystemSettings
    {
        public bool UseStaticServer { get; set; } = true;
        public string StaticServerUrl { get; set; } = "http://localhost:9090";
        public string StaticServerRoot { get; set; } = "C:/static";
    }
}