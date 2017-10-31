using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAIB_Thread1
{
    public class CircularBuffer<T>
    {
        private UInt32 _capacidade;
        private UInt32 _length;
        private UInt32 _head;
        private UInt32 _tail;
        private T[] _buffer;
        private bool _is_empty;
        private bool _is_full;

        public int Count {
            get { return (int) _length;}
        }

        public CircularBuffer(UInt32 capacity = 512)
        {
            _capacidade = capacity;
            _buffer = new T[_capacidade];
            _head = 0;
            _tail = 0;
        }

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
            return true;
        }

        public T Dequeue()
        {
            T valor_out;
            valor_out = _buffer[_tail]; //Retira do buffer
            _tail++; //Incrementa
            _tail = _tail % _capacidade; //De forma circular 
            _length--;
            _is_empty = _length == 0;
            return valor_out;
        }   
    }
}
