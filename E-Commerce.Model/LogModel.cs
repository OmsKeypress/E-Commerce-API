namespace E_Commerce.Model
{
    public class FileLogData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; }
        public string FilePathWithName { get; set; }
    }
}
