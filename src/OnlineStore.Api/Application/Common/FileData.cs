using System.IO;

namespace OnlineStore.Api.Application.Common
{
    public class FileData
    {
        public string Filename { get; set; }
        public Stream FileStream { get; set; }
    }
}
