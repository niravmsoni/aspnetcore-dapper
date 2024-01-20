using Dapper;
using DataLayer.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataLayer.Repository
{
    public class ContactRepositoryMySql
    {
        private IDbConnection _db;

        public ContactRepositoryMySql(string connString)
        {
            _db = new MySqlConnection(connString);
        }

        public List<Contact> GetAll()
        {
            return _db.Query<Contact>("SELECT * FROM Contacts").ToList();
        }
    }
}
