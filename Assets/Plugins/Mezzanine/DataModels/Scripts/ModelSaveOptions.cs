using Mz.Serializers;

namespace Mz.Models
{
    public class ModelSaveOptions
    {
        public ModelSaveOptions(
            string fileName,
            string directoryPath = null,
            bool isRelativePath = true,
            DataFormat dataFormat = DataFormat.Json,
            bool isPrettyPrint = false,
            bool isThrowExceptions = true
        )
        {
            FileName = fileName;
            DirectoryPath = directoryPath;
            IsRelativePath = isRelativePath;
            DataFormat = dataFormat;
            IsPrettyPrint = isPrettyPrint;
            IsThrowExceptions = isThrowExceptions;
        }
        
        public string FileName { get; set; }
        public string FileExtension { get; set; } = "txt";
        public string DirectoryPath { get; set; }
        public bool IsRelativePath { get; set; }
        public DataFormat DataFormat { get; set; }
        public bool IsPrettyPrint { get; set; }
        public bool IsThrowExceptions { get; set; }

        public ModelSaveOptions Clone()
        {
            return new ModelSaveOptions(
                FileName,
                DirectoryPath,
                IsRelativePath,
                DataFormat,
                IsPrettyPrint,
                IsThrowExceptions
            );
        }
    }
}