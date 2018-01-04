# -*- coding: utf-8 -*-
# ------------------------------------------------------------------------------
# FEDERAL UNIVERSITY OF UBERLANDIA
# Faculty of Electrical Engineering
# Biomedical Engineering Lab
# ------------------------------------------------------------------------------
# Author: Italo Gustavo Sampaio Fernandes
# Contact: italogsfernandes@gmail.com
# Git: www.github.com/italogfernandes
# ------------------------------------------------------------------------------
# Description:
# ------------------------------------------------------------------------------


class CircularBuffer:
    """docstring for ."""
    def __init__(self, maxsize):
        if maxsize < 0:
            raise Exception('Maxsize must be greater than zero.')
        self.maxsize = maxsize
        self._tail = 0
        self._head = 0
        self.count = 0
        self.buffer = [0] * self.maxsize

    def is_empty(self):
        """Return True if the CircularBuffer is empty, False otherwise."""
        n = not self.count
        return n

    def is_full(self):
        """Return True if the CircularBuffer is full, False otherwise."""
        n = 0 < self.maxsize == self.count
        return n

    def qsize(self):
        return self.count

    def put(self, item):
        """Only enqueue the item if a free slot is immediately available.
        Otherwise raise the Full exception."""
        if self.is_full():
            raise Exception('The circular buffer is full.')
        self.buffer[self._head] = item
        self._head += 1
        self._head %= self.maxsize
        self.count += 1

    def get(self):
        """Remove and return an item from the circular buffer.
        Otherwise raise the Empty exception."""
        if self.is_empty():
            raise Exception('The circular buffer is empty.')
        n = self.buffer[self._tail]
        self._tail += 1
        self._tail %= self.maxsize
        self.count -= 1
        return n

    def clear(self):
        self._tail = 0
        self._head = 0
        self.count = 0
        self.buffer = [0] * self.maxsize

    def to_array(self):
        if self.is_empty():
            return []

        if self._tail < self._head:
            return self.buffer[self._tail:self._head]
        else:
            return self.buffer[self._tail:]+self.buffer[:self._head]

    def __str__(self):
        return "CircularBuffer Object:" +\
               "\n\tArray: " + str(self.to_array()) +\
               "\n\tCapacity: " + str(self.maxsize) +\
               "\n\tSize: " + str(self.count) +\
               "\n\tEmpty: " + str(self.is_empty()) +\
               "\n\tFull: " + str(self.is_full()) +\
               "\n\tItems: " + str(self.buffer) +\
               "\n\tHead: " + str(self._head) +\
               "\n\tTail: " + str(self._tail)

if __name__ == '__main__':              # if we're running file directly and not importing it
    myCircularBuffer = CircularBuffer(4)
    while True:
        print '-------------------------------'
        print myCircularBuffer
        print '-------------------------------'
        print 'Menu'
        print 'px - put(x) '
        print 'g - get()'
        print 'c - clear()'
        print 'q - Quit'
        print '-------------------------------'
        str_key = raw_input()
        if str_key == 'q':
            break
        elif str_key.startswith('p'):
            myCircularBuffer.put(str_key[1:])
        elif str_key == 'g':
            print "Result: " + str(myCircularBuffer.get())
        elif str_key == 'c':
            myCircularBuffer.clear()

