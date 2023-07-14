namespace INFINITE.CORE.Shared.Attributes
{
    public class FileObject
    {
        public string Filename { get; set; }
        public string Base64 { get; set; }
    }

    public class FileDetailObject : FileObject
    {
        public string MimeType { get; set; }
    }
}
