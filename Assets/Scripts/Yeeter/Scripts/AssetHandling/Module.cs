namespace Yeeter
{
    /// <summary>
    /// Contains module data.
    /// </summary>
    public class Module
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string[] SupportedVersions { get; set; }
        public string[] CompatibleWith { get; set; }
        public string[] IncompatibleWith { get; set; }

        public Module()
        {
        }
    }
}