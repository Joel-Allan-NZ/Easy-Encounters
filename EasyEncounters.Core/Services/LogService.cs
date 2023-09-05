using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;

namespace EasyEncounters.Core.Services;
public class LogService : ILogService
{
    private static readonly string folderPath = @"F:\D&D\DND Tools";
    private readonly IDataService _dataService;
    private readonly IFileService _fileService;
    public LogService(IDataService dataService, IFileService fileService)
    {
        _dataService = dataService;
        _fileService = fileService;
    }
    public void Log(string message)
    {
        _fileService.SaveAsync(folderPath, $"Logging.txt", message);
    }
}
