using System;
using System.Linq;
using System.Threading;

namespace LibrariesExample
{
    //
    // Summary:
    //     Represents a first-in, first-out collection of objects organized in a circular way.
    //
    // Type parameters:
    //   T:
    //     Specifies the type of elements in the circular buffer.
    public class CircularBuffer<T>
    { 
        private UInt32 _capacidade; //Max size of the CircularBuffer and size of array _buffer.
        private UInt32 _length; //Amount of elements in the _buffer
        private UInt32 _head; //Pointer to the index where the new elements will be placed
        private UInt32 _tail; //Ponter to the index where the last elements will be poped up
        private T[] _buffer; //Array filled with the elements of the buffer
        private bool _is_empty; //Flag saying if is it full
        private bool _is_full; //Flag saying if is it empty
        private Mutex _security_control;

        //
        // Summary:
        //     Gets the number of elements contained in the CircularBuffer`1.
        //
        // Returns:
        //     The number of elements contained in the CircularBuffer`1.
        public int Count {
            get { return (int) _length;}
        }

        //
        // Summary:
        //     Gets the maximum number of elements that can be contained in the CircularBuffer`1.
        //
        // Returns:
        //     The maximum number of elements that can be contained in the CircularBuffer`1.
        public int Capacity {
            get { return (int)_capacidade; }
        }

        public T[] GetBuffer {
            get { return _buffer; }
        }

        public int GetHead {
            get { return (int)_head; }
        }
        public int GetTail {
            get { return (int)_tail; }
        }

        //
        // Summary:
        //     Initializes a new instance of the CircularBuffer`1 class that
        //     is empty and has the specified initial capacity.
        //
        // Parameters:
        //   capacity:
        //     The initial number of elements that the CircularBuffer`1 can
        //     contain.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     capacity is less than zero.
        public CircularBuffer(int capacity = 512)
        {
            if(capacity <= 0)
            {
                throw new System.ArgumentOutOfRangeException("capacity","capacty is less than zero");
            }
            _capacidade = (UInt32) capacity;
            _buffer = new T[_capacidade];
            _head = 0;
            _tail = 0;
            _security_control = new Mutex();
        }

        //
        // Summary:
        //     Adds an object to the end of the CircularBufffer`1.
        //
        // Parameters:
        //   item:
        //     The object to add to the CircularBufffer`1. The value can be
        //     null for reference types.
        //
        // Returns:
        //     true if item is added with success in the CircularBuffer`1; otherwise, false.
        public bool Enqueue(T item)
        {
            if (_is_full)
            {
                return false;
            }
            _buffer[_head] = item; //Insere o dado
            _head++; //Incrementa
            _head = _head % _capacidade; //De forma circular
            _length++; //Avisa que o tamanho aumentou
            _is_full = _length == _capacidade; //Atualizar a flag is full
            _is_empty = false;
            return true;
        }

        //
        // Summary:
        //     Removes and returns the object at the beginning of the CircularBuffer`1.
        //
        // Returns:
        //     The object that is removed from the beginning of the CircularBuffer`1.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The CircularBuffer`1 is empty.
        public T Dequeue()
        {
            if (_is_empty)
            {
                throw new System.InvalidOperationException("The CircularBuffer is empty");
            }

            T valor_out;
            valor_out = _buffer[_tail]; //Retira do buffer
            _tail++; //Incrementa
            _tail = _tail % _capacidade; //De forma circular 
            _length--;
            _is_empty = _length == 0;
            _is_full = false;
            return valor_out;
        }

        //
        // Summary:
        //     Adds an object to the end of the CircularBufffer`1 using a Mutex.
        //
        // Parameters:
        //   item:
        //     The object to add to the CircularBufffer`1. The value can be
        //     null for reference types.
        //
        // Returns:
        //     true if item is added with success in the CircularBuffer`1; otherwise, false.
        public bool SecureEnqueue(T item)
        {
            bool res;

            _security_control.WaitOne();
            res = this.Enqueue(item);
            _security_control.ReleaseMutex();

            return res;
        }

        //
        // Summary:
        //     Removes and returns the object at the beginning of the CircularBuffer`1 using a Mutex.
        //
        // Returns:
        //     The object that is removed from the beginning of the CircularBuffer`1.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The CircularBuffer`1 is empty.
        public T SecureDequeue()
        {
            T res;

            _security_control.WaitOne();
            res = this.Dequeue();
            _security_control.ReleaseMutex();

            return res;
        }

        //
        // Summary:
        //     Removes all objects from the CircularBuffer`1.
        public void Clear()
        {
            _tail = 0;
            _head = 0;
            _length = 0;
            _is_empty = true;
            _is_full = false;
            _buffer = new T[_capacidade];
        }

        //
        // Summary:
        //     Determines whether an element is in the CircularBuffer`1.
        //
        // Parameters:
        //   item:
        //     The object to locate in the CircularBuffer`1. The value can
        //     be null for reference types.
        //
        // Returns:
        //     true if item is found in the CircularBuffer`1; otherwise, false.
        public bool Contains(T item)
        {
            return _buffer.Contains<T>(item);
        }

        //
        // Summary:
        //     Copies the CircularBuffer`1 elements to an existing one-dimensional
        //     System.Array, starting at the specified array index.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements copied
        //     from CircularBuffer`1. The System.Array must have zero-based
        //     indexing.
        //
        //   arrayIndex:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     array is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     arrayIndex is less than zero.
        //
        //   T:System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.Queue`1 is greater
        //     than the available space from arrayIndex to the end of the destination array.
        public void CopyTo(T[] array, int arrayIndex = 0)
        {
            uint temp_tail;
            temp_tail = _tail;
            for (int i = arrayIndex; i < arrayIndex+ _length; i++)
            {
                array[i] = _buffer[temp_tail];
                temp_tail++;
                temp_tail = temp_tail % _capacidade;
             }
        }

        //
        // Summary:
        //     Copies the CicularBuffer`1 elements to a new array.
        //
        // Returns:
        //     A new array containing elements copied from the CicularBuffer`1.
        public T[] ToArray()
        {
            T[] array;
            array = new T[_length];
            this.CopyTo(array);
            return array;
        }

        //
        // Summary:
        //     Sets the capacity to the actual number of elements in the CircularBuffer`1,
        //     if that number is less than 90 percent of current capacity and if it is not
        //     empty.
        public void TrimExcess()
        {
            if(_length < 0.9 * _capacidade && !_is_empty)
            {
                _buffer = this.ToArray();
                _head = 0;
                _tail = 0;
                _is_empty = false;
                _is_full = true; //Atualizar a flag is full
            }
        }
    }
}
