using YamlDotNet.Serialization;

namespace Runner
{
    public static class Extensions
    {
        /// <summary>
        /// Strictly for READABILITY PURPOSE. Requires a NuGet package - YamlDotNet
        /// This is just used for output - Displaying content in human readable format
        /// </summary>
        /// <param name="item"></param>
        public static void Output(this object item)
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(item);
            Console.WriteLine(yaml);
        }
    }
}
