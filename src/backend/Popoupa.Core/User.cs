using Castle.Components.DictionaryAdapter.Xml;

namespace Popoupa.API.APIClasses
{
    public class User
    {
        public User(string userName, string userPassword)
        {
            _userName = userName;
        }
        private string _userName;
        public string UserName => _userName;
        public Guid UserId { get; }

        
    }
}
