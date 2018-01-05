# -*- coding: utf-8 -*-
# ------------------------------------------------------------------------------
# FEDERAL UNIVERSITY OF UBERLANDIA
# Faculty of Electrical Engineering
# Biomedical Engineering Lab
# ------------------------------------------------------------------------------
# Author: Andrei Nakagawa, MSc
# Contact: nakagawa.andrei@gmail.com
# Class: ThreadHander
# Modifications: Italo G S Fernandes
#                 (italogsfernandes@gmail.com, github.com/italogfernandes)
# ------------------------------------------------------------------------------
# Description:
# ------------------------------------------------------------------------------
from threading import Thread
from threading import Event
# ------------------------------------------------------------------------------

class ThreadHandler:
    def __init__(self, worker=None, on_end_function=None):
        self.thread = Thread(target=self.run)
        self.worker = worker
        self.on_end_function = on_end_function
        self.isAlive = False
        self.isRunning = False
        self.isFinished = False

    def start(self):
        self.isAlive = True
        self.isRunning = True
        self.thread.start()
        self.isFinished = False

    def pause(self):
        self.isRunning = False

    def resume(self):
        self.isRunning = True

    def stop(self):
        self.isAlive = False
        self.isRunning = False
        self.thread = Thread(target=self.run)

    def run(self):
        while self.isAlive:
            if self.isRunning:
                if self.worker is not None:
                    self.worker()
        if self.on_end_function is not None:
            self.on_end_function()
        self.isFinished = True

    def __str__(self):
        return "ThreadHandler Object" +\
               "\n\tAlive: " + str(self.isAlive) +\
                "\n\tRunning: " + str(self.isRunning) +\
                "\n\tFinished: " + str(self.isFinished) +\
                "\n\tWorker: " + str(self.worker) +\
                "\n\tEndFuction: " + str(self.on_end_function)


class InfiniteTimer(ThreadHandler):
    """Call a function after a specified number of seconds:

            t = Timer(30.0, f, args=[], kwargs={})
            t.start()
            t.cancel()     # stop the timer's action if it's still waiting

    """

    def __init__(self, interval, worker, on_end_function=None, args=[], kwargs={}):
        ThreadHandler.__init__(self)
        self.interval = interval
        self.worker = worker
        self.on_end_function = on_end_function
        self.args = args
        self.kwargs = kwargs
        self.waiter = Event()

    def run(self):
        while self.isAlive:
            if self.isRunning:
                self.waiter.wait(self.interval)
                self.worker()
        if self.on_end_function is not None:
            self.on_end_function()
        self.isFinished = True


if __name__ == '__main__':              # if we're running file directly and not importing it
    from time import sleep
    from datetime import datetime

    def counter():
        print '-'*10
        print "Next 10 seconds:"
        for n in range(10, 0, -1):
            print str(n) + ": " + str(datetime.now())
            sleep(1)

    myThreadHandler = ThreadHandler(counter)

    def show_time():
        print str(datetime.now())

    myInfiniteTimer = InfiniteTimer(1, show_time)

    while True:
        print '-------------------------------'
        print myThreadHandler
        print '-------------------------------'
        print 'Menu'
        print '*' * 5 + 'Thread' + '*' * 5
        print 'st - start() '
        print 'sp - stop()'
        print 'p - pause()'
        print 'r - resume()'
        print '*' * 5 + 'Timer' + '*' *5
        print 'stt - start() '
        print 'spt - stop()'
        print 'pt - pause()'
        print 'rt - resume()'
        print '-------------------------------'
        print 'q - Quit'
        print '-------------------------------'
        str_key = raw_input()
        if str_key == 'q':
            myThreadHandler.stop()
            myInfiniteTimer.stop()
            break
        elif str_key == 'st':
            myThreadHandler.start()
        elif str_key == 'sp':
            myThreadHandler.stop()
        elif str_key == 'p':
            myThreadHandler.pause()
        elif str_key == 'r':
            myThreadHandler.resume()
        elif str_key == 'stt':
            myInfiniteTimer.start()
        elif str_key == 'spt':
            myInfiniteTimer.stop()
        elif str_key == 'pt':
            myInfiniteTimer.pause()
        elif str_key == 'rt':
            myInfiniteTimer.resume()