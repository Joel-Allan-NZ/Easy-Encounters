using System.Text;

using EasyEncounters.Core.Contracts.Services;

using Newtonsoft.Json;

namespace EasyEncounters.Core.Services;

public class FileService : IFileService
{
    public async Task AppendAsync<T>(string folderPath, string fileName, T content)
    {
        var fileContent = JsonConvert.SerializeObject(content, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        await File.AppendAllTextAsync(Path.Combine(folderPath, fileName), fileContent, Encoding.Unicode);
    }

    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }

    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path, Encoding.Unicode);
            var d = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return d;
        }

        return default;
    }

    public async Task<T> ReadAsync<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);

        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path, Encoding.Unicode);
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        }
        return default;
    }

    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.Unicode);
    }

    public async Task SaveAsync<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        await File.WriteAllTextAsync(Path.Combine(folderPath, fileName), fileContent, Encoding.Unicode);
    }
}