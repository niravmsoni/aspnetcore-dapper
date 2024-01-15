using DataLayer.Interface;
using DataLayer.Models;
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
            // Call Initialize to setup configuration object
            Initialize();

            // Test GetAll()
            //Get_all_should_return_6_results();

            // Test Add()
            Insert_should_assign_identity_to_new_entity();
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
        /// <summary>
        /// Testing GetAll()
        /// </summary>
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

        /// <summary>
        /// Test Add()
        /// </summary>
        /// <returns></returns>
        static int Insert_should_assign_identity_to_new_entity()
        {
            // arrange
            IContactRepository repository = CreateRepository();
            var contact = new Contact
            {
                FirstName = "Joe",
                LastName = "Blow",
                Email = "joe.blow@gmail.com",
                Company = "Microsoft",
                Title = "Developer"
            };
            var address = new Address
            {
                AddressType = "Home",
                StreetAddress = "123 Main Street",
                City = "Baltimore",
                StateId = 1,
                PostalCode = "22222"
            };
            contact.Addresses.Add(address);

            // act
            repository.Add(contact);

            // assert
            Debug.Assert(contact.Id != 0);
            Console.WriteLine("*** Contact Inserted ***");
            Console.WriteLine($"New ID: {contact.Id}");
            return contact.Id;
        }
        #endregion
    }
}