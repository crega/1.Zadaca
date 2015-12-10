using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4.Zad_Generic_IEnumerable
{
    class Program
    {
        static void Main()
        {

        }

    }
    //-------------------------------------INTERFACE-------------------------------------------------//
    public interface IGenericList<X> : IEnumerable<X>
    {         /// <summary>         /// Adds an item to the collection.         /// </summary>         
        void Add(X item);
        /// <summary>         /// Removes the first occurrence of an item from the collection.         /// If the item was not found, method does nothing.         /// </summary>         
        bool Remove(X item);
        /// <summary>         /// Removes the item at the given index in the collection.         /// </summary>      
        bool RemoveAt(int index);
        /// <summary>         /// Returns the item at the given index in the collection. 
        /// </summary>         
        X GetElement(int index);
        /// <summary>         /// Returns the index of the item in the collection.         /// If item is not found in the collection, method returns -1.         /// </summary>         
        int IndexOf(X item);
        /// <summary>         /// Readonly property. Gets the number of items contained in the collection.         /// </summary>        
        int Count { get; }
        /// <summary>         /// Removes all items from the collection.         /// </summary>        
        void Clear();
        /// <summary>         /// Determines whether the collection contains a specific value.         /// </summary>       
        bool Contains(X item);
    }
    //------------------------------------IMPLEMENTACIJA--------------------------------------------//


    public class GenericList<T> : IGenericList<T>
    {
        private int _count;     //COUNT NE OVISI O TUPU
        private int _userSize = 4;  //TAKOĐER _userSize;
        private int returns_forIndex = -1;  //NE OVISI
        private int returns_forRemove;
        private T[] _internalStorage; // OVISI O TIPU <X>

        public GenericList()
        {
            _internalStorage = new T[_userSize];
        }
        public GenericList(int initialSize)
        {
            _userSize = initialSize;
            _internalStorage = new T[initialSize];
            _count = 0;
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public void Add(T item)
        {

            if (Count >= _userSize)
            {
                Array.Resize(ref _internalStorage, 2 * _userSize);
                _userSize = 2 * _userSize;
            }
            _internalStorage[Count] = item;
            _count++;
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public T GetElement(int index)
        {
            if ((index < Count) && (index >= 0))
            {
                return _internalStorage[index];

            }
            throw new IndexOutOfRangeException();

        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    returns_forIndex = i;
                    break;
                }
            }
            return returns_forIndex;

        }

        public bool Remove(T item)
        {
            returns_forRemove = -1;
            if (IndexOf(item) >= 0 && IndexOf(item) <= Count)
            {
                returns_forRemove = IndexOf(item);

            }
            return RemoveAt(returns_forRemove);
        }

        public bool RemoveAt(int index)
        {
            if (index >= 0)
            {
                if (index >= Count)
                    return false;
                for (int i = index; i < Count; i++)
                {
                    _internalStorage[i] = _internalStorage[i + 1];
                }
                _count--;
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new  GenericListEnumerator<T> (this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        //-------------------------------IMPLEMENTACIJA IEnumerable <X>-----------------------------------------//


    }
    public class GenericListEnumerator<T> : IEnumerator<T>
    {
        private IGenericList<T> _collection;
        private int count = -1;
        
        public  GenericListEnumerator(IGenericList<T> collection) {
            _collection = collection;
            
        }
        public bool MoveNext()
            
        {
            ++count;
            if (count < _collection.Count)
            {
                return true;
            }
            return false;
            // Zove se prije svake iteracije.
            // Vratite true ako treba ući u iteraciju, false ako ne  
            // Hint: čuvajte neko globalno stanje po kojem pratite gdje se 
            // nalazimo u kolekciji        

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public T Current
        {
            get
                
            {
                return _collection.GetElement(count);
                // Zove se na svakom ulasku u iteraciju  
                // Hint: Koristite stanje postavljeno u MoveNext() dijelu 
                // kako bi odredili što se zapravo vraća u ovom koraku    
            }
        }

        object IEnumerator.Current
        { get
            {
                return Current;
            }
        }





    }

}

