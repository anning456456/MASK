using System.IO;

namespace Config
{
    public static class ConfigMgr
    {
        private static string filePath = "Config/config.bytes";
        public static void Init()
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Loader.Processor = Processor.Process;
            Loader.LoadBytes(bytes);
        }
    }
}
