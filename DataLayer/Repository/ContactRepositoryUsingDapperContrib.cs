using DataLayer.Interface;
using DataLayer.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataLayer.Repository
{
    public class ContactRepositoryUsingDapperContrib : IContactRepository
    {
        private IDbConnection _db;

        public ContactRepositoryUsingDapperContrib(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }
        public Contact Add(Contact contact)
        {
            throw new NotImplementedException();
        }

        public Contact Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<Contact> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Contact Update(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
