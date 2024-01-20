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

            //var id = Insert_should_assign_identity_to_new_entity();
            //Find_should_retrieve_existing_entity(id);
            //Modify_should_update_existing_entity(id);
            //Delete_should_remove_entity(id);

            //GetContactAndTheirAddress(1);

            //Testing strongly return type
            //List_Support_Should_Produce_Correct_Results_With_In_Statmenet();

            //Testing same response with dynamic return type
            //List_Support_Should_Produce_Correct_Results_With_In_Statmenet_With_Dynamic_Keyword();

            //Testing bulk inserts
            ///Bulk_insert_should_insert_4_rows();

            //Testing literal replacement
            //GetIllinoisAddresses();

            //Testing multi-mapping
            Get_all_should_return_6_results_with_addresses();
        }

        private static void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        #region Repository
        /// <summary>
        /// Creating repository by resolving connection string
        /// </summary>
        /// <returns></returns>
        private static IContactRepository CreateRepository()
        {
            return new ContactRepository(_config.GetConnectionString("DefaultConnection"));
            //return new ContactRepositoryUsingDapperContrib(_config.GetConnectionString("DefaultConnection"));
            //return new ContactRepositoryUsingStoredProc(_config.GetConnectionString("DefaultConnection"));
        }

        /// <summary>
        /// Creating a separate method since it does not implement IContactRepository interface
        /// </summary>
        /// <returns></returns>
        private static ContactRepositoryAdditionalOperations CreateRepositoryExtra()
        {
            return new ContactRepositoryAdditionalOperations(_config.GetConnectionString("DefaultConnection"));
        }
        #endregion

        #region Below methods are dummy methods for testing the output ContactRepository
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
            //repository.Add(contact);

            //Save method will save both Contact and address
            repository.Save(contact);

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
            //var contact = repository.Find(id);

            //Replacing Find with GetFullContact since we want to test implementation
            var contact = repository.GetFullContact(id);

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
            var contact = repository.GetFullContact(id);
            contact.FirstName = "Bob";

            contact.Addresses[0].StreetAddress = "456 Main Street";
            //Calling Save to make sure Update happens for both Address, contact
            //repository.Update(contact);

            repository.Save(contact);

            // create a new repository for verification purposes
            IContactRepository repository2 = CreateRepository();
            var modifiedContact = repository2.GetFullContact(id);

            // assert
            Console.WriteLine("*** Contact Modified ***");
            modifiedContact.Output();
            Debug.Assert(modifiedContact.FirstName == "Bob");
            Debug.Assert(modifiedContact.Addresses.First().StreetAddress == "456 Main Street");
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

        /// <summary>
        /// Testing Get address + Contacts
        /// </summary>
        /// <param name="id"></param>
        private static void GetContactAndTheirAddress(int id)
        {
            // arrange
            IContactRepository repository = CreateRepository();
            var response = repository.GetFullContact(id);
            response.Output();
        }
        #endregion

        #region ContactRepositoryExtra
        private static void List_Support_Should_Produce_Correct_Results_With_In_Statmenet()
        {
            //arrange 
            var repository = CreateRepositoryExtra();

            //Act
            var contacts = repository.GetContactsById(1, 2, 4);

            //Assert
            Debug.Assert(contacts.Count == 3);
            contacts.Output();
        }

        private static void List_Support_Should_Produce_Correct_Results_With_In_Statmenet_With_Dynamic_Keyword()
        {
            //arrange 
            var repository = CreateRepositoryExtra();

            //Act
            var contacts = repository.GetDynamicContactsById(1, 2, 4);

            //Assert
            Debug.Assert(contacts.Count == 3);
            Console.WriteLine($"First FirstName is: {contacts.First().FirstName}");
            contacts.Output();
        }

        private static void Bulk_insert_should_insert_4_rows()
        {
            // arrange
            var repository = CreateRepositoryExtra();
            var contacts = new List<Contact>
            {
                new Contact { FirstName = "Charles", LastName = "Barkley" },
                new Contact { FirstName = "Scottie", LastName = "Pippen" },
                new Contact { FirstName = "Tim", LastName = "Duncan" },
                new Contact { FirstName = "Patrick", LastName = "Ewing" }
            };

            // act - Testing Bulk insert
            var rowsAffected = repository.BulkInsertContacts(contacts);

            // assert
            Console.WriteLine($"Rows inserted: {rowsAffected}");
            Debug.Assert(rowsAffected == 4);
        }

        private static void GetIllinoisAddresses()
        {
            // arrange
            var repository = CreateRepositoryExtra();

            // act
            var addresses = repository.GetAddressesByState(17);

            // assert
            Debug.Assert(addresses.Count == 2);
            addresses.Output();
        }

        private static void Get_all_should_return_6_results_with_addresses()
        {
            var repository = CreateRepositoryExtra();

            // act
            var contacts = repository.GetAllContactsWithAddresses();

            // assert
            Console.WriteLine($"Count: {contacts.Count}");
            contacts.Output();
            Debug.Assert(contacts.Count == 6);
            Debug.Assert(contacts.First().Addresses.Count == 2);
        }
        #endregion
    }
}