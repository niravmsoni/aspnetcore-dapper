using DataLayer.Interface;
using DataLayer.Repository;
using Microsoft.Extensions.Configuration;

namespace Runner
{
    public class Program
    {
        private static IConfigurationRoot _config;
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
        }

        private static void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        private static IContactRepository CreateRepository()
        {
            return new ContactRepository(_config.GetConnectionString("DefaultConnection"));
        }
    }
}