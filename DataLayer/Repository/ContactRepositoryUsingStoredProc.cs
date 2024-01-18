using Dapper;
using DataLayer.Interface;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace DataLayer.Repository
{
    public class ContactRepositoryUsingStoredProc : IContactRepository
    {
        private IDbConnection _db;

        public ContactRepositoryUsingStoredProc(string connString)
        {
            _db = new SqlConnection(connString);
        }

        public Contact Add(Contact contact)
        {
            throw new NotImplementedException();
        }

        public Contact Find(int id)
        {
            //Using same SP that selects from 2 separate tables. Using SingleOrDefault so that it takes the first result and ignores the second one from result set
            return _db.Query<Contact>("GetContact", new { Id = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public List<Contact> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Using the same QueryMultiple method but passing command type as StoredProc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contact GetFullContact(int id)
        {
            //Similar implementation as GetFullContact in ContactRepository.
            //Only difference is we're passing command type as SP
            using (var multipleResults = _db.QueryMultiple("GetContact", new { Id = id }, commandType: CommandType.StoredProcedure))
            {
                var contact = multipleResults.Read<Contact>().SingleOrDefault();

                var addresses = multipleResults.Read<Address>().ToList();
                if (contact != null && addresses != null)
                {
                    contact.Addresses.AddRange(addresses);
                }

                return contact;
            }
        }

        public void Remove(int id)
        {
            _db.Execute("DeleteContact", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public void Save(Contact contact)
        {
            using var txScope = new TransactionScope();
            //Explicitly opening a connection and using the same one for both operations
            _db.Open();
            //Most common way to pass parameters to SP
            //Explicitly adding parameters and assigning their properties from contact object.
            //We can also pass direction
            var parameters = new DynamicParameters();
            parameters.Add("@Id", value: contact.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            parameters.Add("@FirstName", contact.FirstName);
            parameters.Add("@LastName", contact.LastName);
            parameters.Add("@Company", contact.Company);
            parameters.Add("@Title", contact.Title);
            parameters.Add("@Email", contact.Email);

            _db.Execute("SaveContact", parameters, commandType: CommandType.StoredProcedure);
            contact.Id = parameters.Get<int>("@Id");

            foreach (var addr in contact.Addresses.Where(a => !a.IsDeleted))
            {
                addr.ContactId = contact.Id;

                var addrParams = new DynamicParameters(new
                {
                    ContactId = addr.ContactId,
                    AddressType = addr.AddressType,
                    StreetAddress = addr.StreetAddress,
                    City = addr.City,
                    StateId = addr.StateId,
                    PostalCode = addr.PostalCode
                });
                addrParams.Add("@Id", addr.Id, DbType.Int32, ParameterDirection.InputOutput);
                _db.Execute("SaveAddress", addrParams, commandType: CommandType.StoredProcedure);
                addr.Id = addrParams.Get<int>("@Id");
            }

            foreach (var addr in contact.Addresses.Where(a => a.IsDeleted))
            {
                _db.Execute("DeleteAddress", new { Id = addr.Id }, commandType: CommandType.StoredProcedure);
            }
            _db.Close();
            txScope.Complete();
        }

        /// <summary>
        /// Not implemented since Save SP takes care of insert and update
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Contact Update(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
