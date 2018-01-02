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
# Decription:
# ------------------------------------------------------------------------------

# PyQt
from PyQt4 import QtGui # Import the PyQt4 module we'll need
import sys # We need sys so that we can pass argv to QApplication
import mainwindow  # This file holds our MainWindow and all design related things

# CircularBuffer
from CircularBuffer import CircularBuffer

# ThreadHandler
from ThreadHandler import ThreadHandler
from datetime import datetime

# ------------------------------------------------------------------------------


class ExampleApp(QtGui.QMainWindow, mainwindow.Ui_MainWindow):
    def __init__(self):
        super(self.__class__, self).__init__()
        self.setupUi(self)

        self.circular_buffer_btn_bindings()
        self.myCircularBuffer = CircularBuffer(8)

        self.thread_btn_bindings()
        self.myThreadHandler = ThreadHandler(self.count_to_ten)
        self.actual_num = 0
        self.ultima_vez = 0

    def circular_buffer_btn_bindings(self):
        self.btnEnqueue.clicked.connect(self.do_enqueue)
        self.btnDequeue.clicked.connect(self.do_dequeue)
        self.btnClear.clicked.connect(self.do_clear)

    def update_status_label(self):
        self.lblStatus.setText("Status:" +
                               "\nArray: " + str(self.myCircularBuffer.toArray()) +
                               "\nCapacity: " + str(self.myCircularBuffer.capacity) +
                               "\nSize: " + str(self.myCircularBuffer.count) +
                               "\nEmpty: " + str(self.myCircularBuffer.is_empty) +
                               "\nFull: " + str(self.myCircularBuffer.is_full) +
                               "\nItems: " + str(self.myCircularBuffer._buffer) +
                               "\nHead: " + str(self.myCircularBuffer._head) +
                               "\nTail: " + str(self.myCircularBuffer._tail))

    def do_enqueue(self):
        self.myCircularBuffer.enqueue(str(self.lineEditEnqueue.text()))
        self.update_status_label()

    def do_dequeue(self):
        self.lineEditDequeue.setText(str(self.myCircularBuffer.dequeue()))
        self.update_status_label()

    def do_clear(self):
        self.myCircularBuffer.clear()
        self.update_status_label()

    def thread_btn_bindings(self):
        self.btnStartThr.clicked.connect(self.do_start)
        self.btnPauseThr.clicked.connect(self.do_pause)
        self.btnResumeThr.clicked.connect(self.do_resume)
        self.btnStopThr.clicked.connect(self.do_stop)

    def do_start(self):
        self.actual_num = 0
        self.myThreadHandler.start()

    def do_pause(self):
        self.myThreadHandler.pause()
        self.update_status_thread()

    def do_resume(self):
        self.myThreadHandler.resume()
        self.update_status_thread()

    def do_stop(self):
        self.myThreadHandler.stop()
        self.update_status_thread()

    def update_status_thread(self):
        self.lblResultThr.setText("Status:" +
                                  "\nAlive: " + str(self.myThreadHandler.isAlive) +
                                  "\nPaused: " + str(self.myThreadHandler.isPaused) +
                                  "\nValor Atual: " + str(self.actual_num))

    def count_to_ten(self):
        if datetime.now().second != self.ultima_vez:
            self.ultima_vez = datetime.now().second
            self.actual_num += 1
            self.actual_num %= 10
            self.update_status_thread()



def main():
    app = QtGui.QApplication(sys.argv)  # A new instance of QApplication
    form = ExampleApp()                 # We set the form to be our ExampleApp (design)
    form.show()                         # Show the form
    app.exec_()                         # and execute the app


if __name__ == '__main__':              # if we're running file directly and not importing it
    main()                              # run the main function
