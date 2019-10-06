using Fenester.Lib.Core.Domain.Key;
using Fenester.Lib.Win.Service.Helpers;

namespace Fenester.Lib.Win.Domain.Key
{
    public class Key : IKey
    {
        public Key(Keys keys)
        {
            Keys = keys;

            Name = keys.ToString();
        }

        public string Name { get; private set; }

        public Keys Keys { get; }
    }
}