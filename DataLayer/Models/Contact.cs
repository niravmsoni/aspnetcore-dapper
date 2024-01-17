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

        [Computed]
        public bool IsNew => Id == default;

        [Write(false)]
        public List<Address> Addresses { get; } = new List<Address>();
    }
}
