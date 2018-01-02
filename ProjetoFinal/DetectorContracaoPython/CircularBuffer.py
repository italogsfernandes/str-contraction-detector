class CircularBuffer():
    """docstring for ."""
    def __init__(self, capacity):

        if capacity < 0:
            raise NameError('Capacidade deve ser maior que zero')
        self.capacity = capacity
        self.clear()

    def clear(self):
        self._tail = 0
        self._head = 0
        self.count = 0
        self.is_empty = True
        self.is_full = False
        self._buffer = [0 for n in range(self.capacity)]

    def enqueue(self, item):
        if self.is_full:
            return False

        self._buffer[self._head] = item
        self._head += 1
        self._head %= self.capacity
        self.count += 1

        self.is_full = self.count == self.capacity
        self.is_empty = False
        return True

    def dequeue(self):
        if self.is_empty:
            raise NameError('Buffer vazio')

        valor_out = self._buffer[self._tail]
        self._tail += 1
        self._tail %= self.capacity

        self.count -= 1
        self.is_empty = self.count == 0
        self.is_full = False

        return valor_out

    def toArray(self):
        temp_array = []
        temp_tail = self._tail

        for n in range(self.count):
            temp_array.append(self._buffer[temp_tail])
            temp_tail += 1
            temp_tail %= self.capacity

        return temp_array
