using Dapper;
using DataLayer.Interface;
using DataLayer.Models;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Repository
{

    public class ContactRepository : IContactRepository
    {
        private IDbConnection _db;

        public ContactRepository(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Save record to DB
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public Contact Add(Contact contact)
        {
            var sql =
                "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES(@FirstName, @LastName, @Email, @Company, @Title); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            
            //We could also use command here. We're using Query since we want to get the ID and return it back
            //_db.CreateCommand().ExecuteNonQuery();
            var id = _db.Query<int>(sql, contact).Single();
            contact.Id = id;
            return contact;
        }

        public Contact Find(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all contacts from Table
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetAll()
        {
            return _db.Query<Contact>("select * from Contacts").ToList();
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
