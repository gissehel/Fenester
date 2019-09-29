using Fenester.Lib.Core.Domain.Fenester;
using Fenester.Lib.Core.Domain.Os;

namespace Fenester.Lib.Business.Domain.Fenester
{
    public class Desktop : IDesktop
    {
        private int Number { get; }
        private string Name { get; set; }

        public Desktop(int number)
        {
            Number = number;
        }

        string IDesktop.Name
        {
            get => Name ?? Id;
            set => Name = value;
        }

        public string Id
        {
            get => Number.ToString();
            set => throw new System.NotImplementedException();
        }

        public IInternalScreen Screen { get; set; }
    }
}