namespace DataLayer.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string AddressType { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }

        internal bool IsNew => Id == default;

        //Useful for mapping delete against a particular address from UI
        public bool IsDeleted { get; set; }
    }
}
