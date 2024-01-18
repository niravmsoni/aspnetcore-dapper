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

        public List<Contact> GetContactsById(params int[] ids)
        {
            //Dapper automatically takes care of converting ids array into a comma separated values
            return _db.Query<Contact>("SELECT * FROM Contacts WHERE ID IN @Ids", new { Ids = ids }).ToList();
        }
    }
}
