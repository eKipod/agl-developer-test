using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GetPeople
{
    public class NamedEnumerable<T> : IEnumerable<T>
    {
        public string Name { get; set; }

        private IEnumerable<T> _items;
        public IEnumerable<T> Items
        {
            get => _items ?? Enumerable.Empty<T>();
            set => _items = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}