using System;
using System.Linq;
using System.Threading;

namespace DetectorContracao
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
        private UInt32 _capacidade; //Max size of the CircularBuffer and size of array _buffer. - tamanho buffer
        private UInt32 _length; //Amount of elements in the _buffer - quantidade de elementos no buffer
        private UInt32 _head; //Pointer to the index where the new elements will be placed - ponteiro novos elementos
        private UInt32 _tail; //Ponter to the index where the last elements will be poped up - ponteiro últimos elementos serão exibidos
        private T[] _buffer; //Array filled with the elements of the buffer - array com elementos do buffer
        private bool _is_empty; //Flag saying if is it full - bandeira se está cheio
        private bool _is_full; //Flag saying if is it empty - bandeira se está vazio
        private Mutex _security_control; //mutex para garantir segurança


        //
        // Summary:
        //     Gets the number of elements contained in the CircularBuffer`1.
        //
        // Returns:
        //     The number of elements contained in the CircularBuffer`1.
        public int Count {
            get { return (int) _length;} //pega e retorna o número de elementos do buffer
            //get = executado quando a propriedade é lida
            //set = executado quando um novo valor é atribuído a propriedade
        }

        //
        // Summary:
        //     Xablaus in the CircularBuffer`1.
        //
        // Returns:
        //     Xablaus in the CircularBuffer`1.
        public bool Full {
            get { return _length == _capacidade; } //retorna verdadeiro se a quantidade de elementos é igual a capacidade
        }

        //
        // Summary:
        //     Gets the maximum number of elements that can be contained in the CircularBuffer`1.
        //
        // Returns:
        //     The maximum number of elements that can be contained in the CircularBuffer`1.
        public int Capacity {
            get { return (int)_capacidade; } //retorna a capacidade do buffer
        }

        public T[] GetBuffer {
            get { return _buffer; } //retorna o array com elementos do buffer
        }

        public int GetHead {
            get { return (int)_head; } //retorna o ponteiro para novos elementos 
        }
        public int GetTail {
            get { return (int)_tail; } //retorna o ponteiro para os últimos elementos
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
        public CircularBuffer(int capacity = 512) //construtor
        {
            if(capacity <= 0)
            {
                throw new System.ArgumentOutOfRangeException("capacity","capacty is less than zero");
            }
            _capacidade = (UInt32) capacity; //capacidade do buffer como especificado
            _buffer = new T[_capacidade]; //buffer do tamanho da capacidade especificada
            _head = 0; //inicializa o ponteiro dos novos elementos com 0 (início)
            _tail = 0; //inicializa o ponteiro dos últimos elementos com 0 (início)
            _security_control = new Mutex(); //mutex 
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
        public bool Enqueue(T item) //adicionar um elemento do tipo T
        {
            if (_is_full)
            {
                return false; //buffer está cheio e não se pode adicionar o novo item
            }
            _buffer[_head] = item; //Insere o dado na posição apontada pelo ponteiro dos novos elementos
            _head++; //Incrementa o ponteiro dos novos elementos
            _head = _head % _capacidade; //De forma circular - retornar a primeira posição quando chega a última
            _length++; //Avisa que o tamanho do buffer aumentou
            _is_full = _length == _capacidade; //Atualizar a flag is full (verdadeiro se o comprimento igual a capacidade)
            _is_empty = false; //possui elementos no buffer
            return true;
        }

        public void OverflowEnqueue(T item) //adicionar um elemento do tipo T
        {
            if (_is_full)
            {
                Dequeue();
            }
            Enqueue(item);
        }

        public void FillWith(T item)
        {
            for (int i = 0; i < Capacity; i++)
            {
                Enqueue(item);
            }
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
        public T Dequeue() //remover um elemento do tipo T
        {
            if (_is_empty) //se estiver vazio, não tem como retirar elementos
            {
                throw new System.InvalidOperationException("The CircularBuffer is empty");
            }

            T valor_out; //guarda o valor a ser retirado
            valor_out = _buffer[_tail]; //Retira do buffer o elemento apontado pelo ponteiro dos últimos elementos
            _tail++; //Incrementa o ponteiro dos últimos elementos - FIFO (primeiro a entrar, primeiro a sair)
            _tail = _tail % _capacidade; //De forma circular 
            _length--; //diminuir o comprimento do buffer
            _is_empty = _length == 0; //recebe verdadeiro se o comprimento for 0 (está vazio)
            _is_full = false; //não está cheio, pois retirou um elemento
            return valor_out; //retornar o elemento retirado
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
        public bool SecureEnqueue(T item) //adicionar no buffer usando mutex
        {
            bool res; //retornar verdadeiro se o item foi adicionado

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
        public T SecureDequeue() //remover do buffer usando mutex
        {
            T res;

            _security_control.WaitOne();
            res = this.Dequeue();
            _security_control.ReleaseMutex();

            return res; //retorna o elemento removido
        }

        //
        // Summary:
        //     Removes all objects from the CircularBuffer`1.
        public void Clear() //remover todos os elementos do buffer 
        {
            _tail = 0; //voltar o ponteiro dos últimos elementos para 0 (início)
            _head = 0; //voltar o ponteiro dos novos elementos para 0 (início)
            _length = 0; //comprimento igual a 0 (sem elementos)
            _is_empty = true; //está vazio
            _is_full = false; //logo, não está cheio
            _buffer = new T[_capacidade]; //buffer de elementos do tipo T com capacidade _capacidade
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
        public bool Contains(T item)  //determinar se um elemento está no buffer
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
        public void CopyTo(T[] array, int arrayIndex = 0) //copiar os elementos do buffer para um array
        {
            uint temp_tail;
            temp_tail = _tail; //recebe a posição indicada pelo ponteiro dos últimos elementos a serem recebidos
            for (int i = arrayIndex; i < arrayIndex+ _length; i++) //começando do index especificado
            {
                array[i] = _buffer[temp_tail]; //recebe os elementos na ordem em que devem ser retirados 
                temp_tail++;
                temp_tail = temp_tail % _capacidade; //de maneira circular 
             }
        }

        //
        // Summary:
        //     Copies the CicularBuffer`1 elements to a new array.
        //
        // Returns:
        //     A new array containing elements copied from the CicularBuffer`1.
        public T[] ToArray() //copia os elementos do buffer em um array
        {
            T[] array;
            array = new T[_length]; //array com o número de elementos do comprimento do buffer
            this.CopyTo(array); //copiar para o array a partir da posição 0 (início)
            return array; //retorna o array
        }

        //
        // Summary:
        //     Sets the capacity to the actual number of elements in the CircularBuffer`1,
        //     if that number is less than 90 percent of current capacity and if it is not
        //     empty.
        public void TrimExcess() //??
        {
            if(_length < 0.9 * _capacidade && !_is_empty) //comprimento menor que 90% da capacidade e não está vazio
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
