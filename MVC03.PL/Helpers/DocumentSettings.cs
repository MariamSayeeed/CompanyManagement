namespace MVC03.PL.Helpers
{
    public class DocumentSettings
    {
        public DocumentSettings() { }

        //1. Upload
        // return imageName

        public static string UploadFile(IFormFile file, string fileFolder)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\files" ,fileFolder);

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create); 
            file.CopyTo(fileStream);

            return fileName ;
        }

        //2. Delete

        public static void DeleteFile(string fileName, string fileFolder)

        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", fileFolder , fileName);

            if (File.Exists(filePath)) 
                     File.Delete(filePath);

        }   


    }
}
