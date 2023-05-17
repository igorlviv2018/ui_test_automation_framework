using System.IO;
using System.Text.Json;

namespace Taf.UI.Core.Helpers
{
    public class FileHelper
    {
        public static string WriteToJsonFile(object objToSerialize, string filePath)
        {
            string err = string.Empty;

            string pathWithoutFileName = Path.GetDirectoryName(filePath);

            if (Directory.Exists(pathWithoutFileName))
            {
                string json = JsonSerializer.Serialize(objToSerialize);

                File.WriteAllText(filePath, json);
            }
            else
            {
                err = $"Failed to save file: {Path.GetFileName(filePath)} path does not exist: {pathWithoutFileName}";
            }

            return err;
        }

        public static T ReadJsonFile<T>(string filePath) where T : new()
        {
            T result = new T();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                result = JsonSerializer.Deserialize<T>(json);
            }

            return result;
        }
    }
}
