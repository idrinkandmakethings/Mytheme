using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mytheme.Data
{
    public class FileHandlerService
    {

        private readonly string appDataPath;
        private readonly string imagePath;

        public FileHandlerService()
        {
            appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mytheme");
            imagePath = Path.Combine(appDataPath, "images");
            Directory.CreateDirectory(imagePath);
        }

        public async Task<string[]> ParseTableFile(MemoryStream stream)
        {
            return await Task.Run(async () =>
            {
                var result = new List<string>();
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        result.Add(line);
                    }
                }

                return result.ToArray();
            });
        }

        public async Task SaveFile(MemoryStream stream, Guid imageId, string fileExtension)
        {
            stream.Position = 0;

            await using var fs = new FileStream(Path.Combine(imagePath, $"{imageId}{fileExtension}"), FileMode.Create);
            stream.CopyTo(fs);
            await fs.FlushAsync();
        }
    }
}
