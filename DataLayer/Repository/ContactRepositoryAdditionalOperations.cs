using Dapper;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataLayer.Repository
{
    /// <summary>
    /// Using this class for additional operations with respect to Contact Repository(Such as passing arrays to IN statement)
    /// </summary>
    public class ContactRepositoryAdditionalOperations
    {
        private IDbConnection _db;

        public ContactRepositoryAdditionalOperations(string connString)
        {
            _db = new SqlConnection(connString);
        }

        /// <summary>
        /// Testing SQL queries with IN
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Contact> GetContactsById(params int[] ids)
        {
            //Dapper automatically takes care of converting ids array into a comma separated values
            return _db.Query<Contact>("SELECT * FROM Contacts WHERE ID IN @Ids", new { Ids = ids }).ToList();
        }

        /// <summary>
        /// Tseting the same method with dynamic return type
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<dynamic> GetDynamicContactsById(params int[] ids)
        {
            //Removing Contact as generic type. Dapper will infer that we're seeking to use dynamic object
            return _db.Query("SELECT * FROM Contacts WHERE ID IN @Ids", new { Ids = ids }).ToList();
        }

        /// <summary>
        /// Test Bulk insert capabilities
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public int BulkInsertContacts(List<Contact> contacts)
        {
            //Syntax is similar to what we used for inserting single record in DB
            //Execute method understands second param is array/list and is smart enough to execute this multiple times
            //Important to note - This did a 4 round trip to DB. So, this is not that Performant
            var sql =
                "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES(@FirstName, @LastName, @Email, @Company, @Title); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            return _db.Execute(sql, contacts);
        }

        /// <summary>
        /// Test Literal replacement
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<Address> GetAddressesByState(int stateId)
        {
            //{=stateId} - Special index needed for literal replacement and pass stateId as anonymous type param
            return _db.Query<Address>("SELECT * FROM Addresses WHERE StateId = {=stateId}", new { stateId }).ToList();
        }

        /// <summary>
        /// Test Multi-mapping
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetAllContactsWithAddresses()
        {
            //With Inner join - If a contact has more than 1 address,we could see it return multiple values
            var sql = "SELECT * FROM Contacts AS C INNER JOIN Addresses AS A ON A.ContactId = C.Id";

            //Below code works perfectly fine for 1..1 mapping
            //First generic value is the object we're mapping to
            //Second generic value is Child object
            //Third generic value is the return type since its Func
            //var contacts = _db.Query<Contact, Address, Contact>(sql, (contact, address) =>
            //{
            //    contact.Addresses.Add(address);
            //    return contact;
            //});
            //return contacts.ToList();

            //Below code works fine for 1..* mapping
            var contactDict = new Dictionary<int, Contact>();
            var contacts = _db.Query<Contact, Address, Contact>(sql, (contact, address) =>
            {
                if (!contactDict.TryGetValue(contact.Id, out var currentContact))
                {
                    currentContact = contact;
                    contactDict.Add(currentContact.Id, currentContact);
                }

                currentContact.Addresses.Add(address);
                return currentContact;
            });
            return contacts.Distinct().ToList();

        }

        /// <summary>
        /// Async implementation
        /// </summary>
        /// <returns></returns>
        public async Task<List<Contact>> GetAllAsync()
        {
            var contacts = await _db.QueryAsync<Contact>("SELECT * FROM Contacts");
            return contacts.ToList();
        }
    }
}
