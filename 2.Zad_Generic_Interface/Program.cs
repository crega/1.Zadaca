using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.Zad_Generic_Interface
{
    class Program
    {
        static  void Main()
        {
            IGenericList<string> sl = new GenericList<string>(5);
            sl.Add("Hello");
            sl.Add("World");
            sl.Add("!");
            Console.WriteLine(sl.Count);
            Console.WriteLine(sl.Contains("Hello"));
            Console.WriteLine(sl.IndexOf("Hello"));
            Console.WriteLine(sl.GetElement(2));
            try {
               
                Console.WriteLine(sl.GetElement(5));
                
            }catch(IndexOutOfRangeException){
                Console.WriteLine("Please ENTER index between 0 and {0} because you entered index out of bounds", sl.Count);
            }
            IGenericList<double> dl = new GenericList<double>();
            dl.Add(0.2);
            dl.Add(0.7);
            Console.WriteLine(dl.Count);
            Console.WriteLine(dl.Contains(0.2));
            if(dl.IndexOf(0.5) ==-1)
            Console.WriteLine("Index out of bounds");
            try {
                Console.WriteLine(dl.GetElement(2));
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Please ENTER index between 0 and {0} because you entered index out of bounds", dl.Count);
            }


        }

    }
    //-------------------------------------INTERFACE-------------------------------------------------//
    public interface IGenericList<X>
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
        bool Contains(X item);     }
    //------------------------------------IMPLEMENTACIJA--------------------------------------------//

   
    public class GenericList<X> : IGenericList<X>
       {
        private int _count;     //COUNT NE OVISI O TUPU
        private int _userSize = 4;  //TAKOĐER _userSize;
        private int returns_forIndex = -1;  //NE OVISI
        private int returns_forRemove;
        private X[] _internalStorage; // OVISI O TIPU <X>
        public GenericList()
        {
            _internalStorage = new X[_userSize];
        }
        public GenericList(int initialSize)
        {
            _userSize = initialSize;
            _internalStorage = new X[initialSize];
            _count = 0;
        }

        public int Count
        {
            get
            {
                return _count; 
            }
        }

        public void Add(X item)
        {
         
            if (Count >= _userSize)
            {
                Array.Resize(ref _internalStorage, 2 * _userSize);
                _userSize = 2 * _userSize;
            }
            _internalStorage[Count ] = item;
            _count++;
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(X item)
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

        public X GetElement(int index)
        {
            if((index< Count) && (index >= 0))
            {
                return _internalStorage[index];

            }
            throw new IndexOutOfRangeException();
          
        }

        public int IndexOf(X item)
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

        public bool Remove(X item)
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
        //------------------------------------MAIN-------------------------------------
       
    }
}

