using AutoMapper;
using System.Text.RegularExpressions;

namespace OnlineCourses.Helper
{
    public class FileHandler
    {
        public static string UploadFile(IFormFile file, string folderName, string fileName = null)
        {
            // Combine the folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Sanitize and generate the filename
            string sanitizedFileName = string.IsNullOrEmpty(fileName)
                ? SanitizeFileName(file.FileName)
                : SanitizeFileName(fileName);
            string uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
            string filePath = Path.Combine(folderPath, uniqueFileName);

            // Save the file to the path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filePath;
        }

        public static string ConvertToRelativePath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return null;

            // Extract the part after the last wwwroot (e.g., ImagesFiles/filename.jpg)
            var index = fullPath.LastIndexOf("wwwroot\\", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                var relativePath = fullPath.Substring(index + "wwwroot\\".Length).Replace("\\", "/");
                return "/" + relativePath; // Ensure it starts with /
            }

            // If already a relative path, return as-is with leading slash
            return "/" + fullPath.Replace("\\", "/");
        }

        public static bool DeleteFile(string filePath)
        {
            bool x = false;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                x = true;
            }
            return x;
        }

        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;

            // Replace spaces with hyphens and remove invalid characters
            return Regex.Replace(fileName, @"[\s]+", "-")
                .Replace(":", "")
                .Replace(";", "")
                .Replace("&", "")
                .Replace("?", "")
                .Replace("#", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "")
                .Trim();
        }
    }
}
