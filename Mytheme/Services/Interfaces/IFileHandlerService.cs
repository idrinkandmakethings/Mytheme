using System;
using System.IO;
using System.Threading.Tasks;
using Mytheme.Dal.Dto;
using Mytheme.Map.Models;

namespace Mytheme.Services.Interfaces
{
    public interface IFileHandlerService
    {
        Task<string[]> ParseTableFile(MemoryStream stream);
        Task SaveFile(MemoryStream stream, Guid imageId, string fileExtension, FileType type);
        Task<MapImage> GetMapImage(string id);
        string GetBase64Image(string path);
    }
}