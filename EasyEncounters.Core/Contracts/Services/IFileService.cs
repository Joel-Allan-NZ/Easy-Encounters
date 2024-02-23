namespace EasyEncounters.Core.Contracts.Services;

public interface IFileService
{
    Task AppendAsync<T>(string folderPath, string fileName, T content);

    void Delete(string folderPath, string fileName);

    T Read<T>(string folderPath, string fileName);

    Task<T> ReadAsync<T>(string folderPath, string fileName);

    void Save<T>(string folderPath, string fileName, T content);

    Task SaveAsync<T>(string folderPath, string fileName, T content);
}