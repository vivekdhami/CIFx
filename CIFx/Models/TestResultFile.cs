using System;
using System.IO;

namespace CIFx.Api.Models
{
    public class TestResultFile
    {
        public string FileName { get; set; }
        public long? FileSize { get; set; }
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
    }
}