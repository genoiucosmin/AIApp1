namespace WebApplication1.Service
{
    public class DocumentService : IDocumentService
    {
        public string LoadAll()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Docs");

            if (!Directory.Exists(path))
                return string.Empty;

            var files = Directory.GetFiles(path);
            return string.Join("\n\n", files.Select(File.ReadAllText));
        }
    }
}
