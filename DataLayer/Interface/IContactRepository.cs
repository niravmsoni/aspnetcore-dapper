using DataLayer.Models;

namespace DataLayer.Interface
{
    public interface IContactRepository
    {
        Contact Find(int id);

        List<Contact> GetAll();

        Contact Add(Contact contact);

        Contact Update(Contact contact);

        void Remove(int id);

        public Contact GetFullContact(int id);

        void Save(Contact contact);
    }
}
