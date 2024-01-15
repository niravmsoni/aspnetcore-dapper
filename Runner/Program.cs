using DataLayer.Interface;
using DataLayer.Repository;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Runner
{
    public class Program
    {
        private static IConfigurationRoot _config;
        public static void Main(string[] args)
        {
            //Call Initialize to setup configuration object
            Initialize();

            //Test GetAll()
            Get_all_should_return_6_results();
        }

        private static void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        /// <summary>
        /// Creating repository by resolving connection string
        /// </summary>
        /// <returns></returns>
        private static IContactRepository CreateRepository()
        {
            return new ContactRepository(_config.GetConnectionString("DefaultConnection"));
        }

        #region Below methods are dummy methods for testing the output
        private static void Get_all_should_return_6_results()
        {
            // arrange
            var repository = CreateRepository();

            // act
            var contacts = repository.GetAll();

            // assert
            Console.WriteLine($"Count: {contacts.Count}");
            Debug.Assert(contacts.Count == 6);
            contacts.Output();
        }
        #endregion
    }
}