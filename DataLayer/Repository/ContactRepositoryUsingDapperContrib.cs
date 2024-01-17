using DataLayer.Interface;
using DataLayer.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataLayer.Repository
{
    /// <summary>
    /// Implementation for methods provided by DapperContrib. Compare and contrast differences with ContactRepository
    /// </summary>
    public class ContactRepositoryUsingDapperContrib : IContactRepository
    {
        private IDbConnection _db;

        public ContactRepositoryUsingDapperContrib(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }
        public Contact Add(Contact contact)
        {
            //It generates standard insert statement based on contact object
            var id = _db.Insert(contact);
            contact.Id = (int)id;
            return contact;
        }

        public Contact Find(int id)
        {
            return _db.Get<Contact>(id);
        }

        public List<Contact> GetAll()
        {
            return _db.GetAll<Contact>().ToList();
        }

        /// <summary>
        /// Leaving it as it is
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Contact GetFullContact(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            _db.Delete(new Contact { Id = id });
        }

        public Contact Update(Contact contact)
        {
            _db.Update(contact);
            return contact;
        }
    }
}
