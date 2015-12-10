using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.Zad_Interface  //   kAD ZAVRSIS SVE IMPLEMENTACIJE POGLEDAJ JELI MOZES KORISTITI JEDNU UNTAR DRUGE( KEY STOKE)
{
    class Program
    {

        static void Main(string[] args)
        {
            IntegerList l = new IntegerList();
            l.Add(1);
            l.Add(2);
            l.Add(3);
            Console.WriteLine( l.Remove(5));
            Console.WriteLine(l.RemoveAt(3));
            Console.WriteLine(l.Remove(3));
            Console.WriteLine(l.IndexOf(2));
            try {
                Console.WriteLine(l.GetElement(2));
                Console.WriteLine(l.GetElement(3));
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Please enter index between 0 and {0}", l.Count);
            }
            Console.WriteLine(l.Count);


        }



        //------------------------------------------SUČELJE-----------------------------------------------------//
        public interface IIntegerList  
        {
            /// <summary> /// Adds an item to the collection. /// </summary>
            void Add(int item);
            /// <summary> /// Removes the first occurrence of an item from the collection. /// If the item was not found, method does nothing. /// </summary>
            bool Remove(int item);
            /// <summary> /// Removes the item at the given index in the collection. /// </summary>
            bool RemoveAt(int index);
            /// <summary> /// Returns the item at the given index in the collection. /// </summary>
            int GetElement(int index);
            /// <summary> /// Returns the index of the item in the collection./// If item is not found in the collection, method returns -1. /// </summary>
            int IndexOf(int item);
            /// <summary> /// Readonly property. Gets the number of items contained in the collection. /// </summary>
            int Count { get; }
            /// <summary> /// Removes all items from the collection. /// </summary>
            void Clear();
            /// <summary> /// Determines whether the collection contains a specific value. /// </summary>
            bool Contains(int item);
        }

        // --------------------------------------IMPLEMENTACIJA------------------------------------------------//

        public class IntegerList : IIntegerList
        {
            private int _count;      
            private int returns_forRemove;   // Ako ne nađe vrijednost u polju onda vraća -1 koji se inicijalizira pri svakom pozivu
            private int returns_forIndex = -1;  // U slučaju da ne nađe vraća -1
            private int _userSize = 4;          // Velicina polja koje opcionalno korisnik odabrere u svhru Add property//
            private int[] _internalStorage;
            public IntegerList()                    // Prazan konstruktor koji inicijalizira polje od 4 clana
            {

                _internalStorage = new int[_userSize];

            }
            public IntegerList(int initialSize)     // Int konstruktor koji inicijalizira polje na initialSize velicinu
            {
                _userSize = initialSize;
                _internalStorage = new int[initialSize];
                _count = 0;
            }





            public int Count    // vrati broj clanova
            {
                get
                {
                  return _count;
               }
                
            }

            public void Add(int item)  
            {
               

                if (Count >= _userSize)
                {

                    Array.Resize(ref _internalStorage, 2 * _userSize);
                    _userSize = 2 * _userSize;
                }
              
                _internalStorage[Count] = item;
                _count++;



            }

            public void Clear()         // praznjenje trenutnog spremnika neovisno jeli se prije "poziva" mijenjala _userSize 
            {
                _count = 0;
            }

            public bool Remove(int item)           
            {
               
                returns_forRemove = -1;
                for (int i = 0; i < Count; i++)
                {
                    if (_internalStorage[i] == item)
                    {
                        returns_forRemove = i;
                        break;
                    }


                }
               
                return RemoveAt(returns_forRemove);
                // Ako ne nađe , ne čini ništa
            }

            public bool RemoveAt(int index)         
            {
               
                if (index >= 0)
                {                               // U slučaju da remove nije nista nasao vraća -1, koji nam nije dobar jer bi sve maknuo iz niza
                    if (index >= Count)     // .Count koristeno jer postoji mogucnost da korisnik priije .Count svojstva
                                                //moze iskoristiti  ovo svojstvo te onda mozda nebi dobro radila ako 
                                                // _userSize != .Count

                    {
                        return false;
                    }
                   
                    for (int i = index; i < Count; i++)
                    {
                        _internalStorage[i] = _internalStorage[i + 1];
                    }
                    _count--;
                    return true;
                }
                return false;
            }

            public bool Contains(int item)         
            {
                return _internalStorage.Contains(item);
            }
            public int GetElement(int index)            
            {
                if ((index < Count) && (index >= 0))
                {
                    return _internalStorage[index];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }

            public int IndexOf(int item)                   
            {
                for (int i = 0; i < Count; i++)
                {
                    if (_internalStorage[i] == item)
                    {
                        returns_forIndex = i;
                        break;
                    }
                }
                return returns_forIndex;


            }           


        }
    }
}