# -*- coding: utf-8 -*-
#------------------------------------------------------------------------------
# FEDERAL UNIVERSITY OF UBERLANDIA
# Faculty of Electrical Engineering
# Biomedical Engineering Lab
#------------------------------------------------------------------------------
# Author: Italo Gustavo Sampaio Fernandes
# Contact: italogsfernandes@gmail.com
# Git: www.github.com/italogfernandes
#------------------------------------------------------------------------------
# Decription:
#------------------------------------------------------------------------------

from Queue import Queue
from CircularBuffer import CircularBuffer


class CircularQueue(Queue):
    """Circular Variant of Queue."""

    # Initialize the queue representation
    def _init(self, maxsize):
        self.queue = CircularBuffer(maxsize)

    def _qsize(self, len=len):
        return self.queue.count

    # Put a new item in the queue
    def _put(self, item):
        self.queue.put(item)

    # Get an item from the queue
    def _get(self):
        return self.queue.get()

if __name__ == '__main__':              # if we're running file directly and not importing it
    myCircularQueue = CircularQueue(4)
    while True:
        print '-------------------------------'
        print myCircularQueue.queue
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
            myCircularQueue.put(str_key[1:], True, 1)
        elif str_key == 'g':
            print "Result: " + str(myCircularQueue.get(True, 1))
        elif str_key == 'c':
            myCircularQueue.queue.clear()

