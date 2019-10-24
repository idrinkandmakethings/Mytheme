using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mytheme.Data
{
    public class FileHandlerService
    {
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
    }
}
