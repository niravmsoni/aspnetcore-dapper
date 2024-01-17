using Dapper.Contrib.Extensions;

namespace DataLayer.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }

        //Boolean that checks against default for Int i.e. In case if Id = 0, it will be true meaning its a new record not present in Db. Else it will be false
        [Computed]
        public bool IsNew => Id == default;

        [Write(false)]
        public List<Address> Addresses { get; } = new List<Address>();
    }
}
