namespace DataLayer.Models
{
    public class Contact
    {
        public int Id { get; set; }

        //Changing FirstName to FName to test mapping when output from query does not match object. In DB, it's FirstName
        //public string FirstName { get; set; }
        public string FName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }

        public bool IsNew => Id == default;

        public List<Address> Addresses { get; } = new List<Address>();
    }
}
