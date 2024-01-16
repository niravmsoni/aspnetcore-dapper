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
            var id = Insert_should_assign_identity_to_new_entity();

            //Testing GetById(id)
            //Find_should_retrieve_existing_entity(id);

            //Testing Update(id)
            //Modify_should_update_existing_entity(id);

            //Testing Delete(id)
            Delete_should_remove_entity(id);
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
        private static int Insert_should_assign_identity_to_new_entity()
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

        /// <summary>
        /// Test Get by ID
        /// </summary>
        /// <param name="id"></param>
        private static void Find_should_retrieve_existing_entity(int id)
        {
            // arrange
            IContactRepository repository = CreateRepository();

            // act
            var contact = repository.Find(id);

            // assert
            Console.WriteLine("*** Get Contact ***");
            contact.Output();
            Debug.Assert(contact.FirstName == "Joe");
            Debug.Assert(contact.LastName == "Blow");
            Debug.Assert(contact.Addresses.Count == 1);
            Debug.Assert(contact.Addresses.First().StreetAddress == "123 Main Street");
        }

        /// <summary>
        /// Test Update
        /// </summary>
        /// <param name="id"></param>
        private static void Modify_should_update_existing_entity(int id)
        {
            // arrange
            IContactRepository repository = CreateRepository();

            // act
            var contact = repository.Find(id);
            contact.FirstName = "Bob";
            repository.Update(contact);

            // create a new repository for verification purposes
            IContactRepository repository2 = CreateRepository();
            var modifiedContact = repository2.Find(id);

            // assert
            Console.WriteLine("*** Contact Modified ***");
            modifiedContact.Output();
            Debug.Assert(modifiedContact.FirstName == "Bob");
        }

        /// <summary>
        /// Test delete
        /// </summary>
        /// <param name="id"></param>
        private static void Delete_should_remove_entity(int id)
        {
            // arrange
            IContactRepository repository = CreateRepository();

            // act
            repository.Remove(id);

            // create a new repository for verification purposes
            IContactRepository repository2 = CreateRepository();
            var deletedEntity = repository2.Find(id);

            // assert
            Debug.Assert(deletedEntity == null);
            Console.WriteLine("*** Contact Deleted ***");
        }
        #endregion
    }
}