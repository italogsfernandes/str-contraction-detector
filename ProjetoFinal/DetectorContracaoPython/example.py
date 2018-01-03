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
from PyQt4.QtCore import SIGNAL
from QThreadHandler import QThreadHandler

# Arduino Handler
from ArduinoHandler import ArduinoHandler

# ------------------------------------------------------------------------------

class ExampleApp(QtGui.QMainWindow, mainwindow.Ui_MainWindow):
    def __init__(self):
        super(self.__class__, self).__init__()
        self.setupUi(self)

        # Circular Buffer
        self.circular_buffer_btn_bindings()
        self.myCircularBuffer = CircularBuffer(8)

        # QThread Handler
        self.thread_btn_bindings()
        self.actual_num = 0
        self.myCounter = QThreadHandler(self.generate_value)
        self.connect(self.myCounter, SIGNAL("new_value(int)"), self.update_status_thread)

        # Arduino Handler
        self.myArduinoHandler = ArduinoHandler('/dev/ttyACM0')
        self.arduinoDataConsumer = QThreadHandler(self.arduino_consumer)
        self.connect(self.arduinoDataConsumer, SIGNAL('new_arduino_data(int)'), self.arduino_data_ready)


    # Circular Buffer
    def circular_buffer_btn_bindings(self):
        self.btnEnqueue.clicked.connect(self.do_enqueue)
        self.btnDequeue.clicked.connect(self.do_dequeue)
        self.btnClear.clicked.connect(self.do_clear)

    def update_status_label(self):
        self.lblStatus.setText("Status:" +
                               "\nArray: " + str(self.myCircularBuffer.to_array()) +
                               "\nCapacity: " + str(self.myCircularBuffer.capacity) +
                               "\nSize: " + str(self.myCircularBuffer.count) +
                               "\nEmpty: " + str(self.myCircularBuffer.is_empty) +
                               "\nFull: " + str(self.myCircularBuffer.is_full) +
                               "\nItems: " + str(self.myCircularBuffer._buffer) +
                               "\nHead: " + str(self.myCircularBuffer._head) +
                               "\nTail: " + str(self.myCircularBuffer._tail))

    def do_enqueue(self):
        self.myCircularBuffer.secure_enqueue(str(self.lineEditEnqueue.text()))
        self.update_status_label()

    def do_dequeue(self):
        self.lineEditDequeue.setText(str(self.myCircularBuffer.secure_dequeue()))
        self.update_status_label()

    def do_clear(self):
        self.myCircularBuffer.clear()
        self.update_status_label()

    # QThread Handler
    def thread_btn_bindings(self):
        self.btnStartThr.clicked.connect(self.do_start)
        self.btnPauseThr.clicked.connect(self.do_pause)
        self.btnResumeThr.clicked.connect(self.do_resume)
        self.btnStopThr.clicked.connect(self.do_stop)

    def do_start(self):
        self.do_start_arduino_handler()
        #self.actual_num = 0
        #self.myCounter.start()

    def do_pause(self):
        self.myCounter.pause()
        self.lblResultThr.setText("Status:" +
                                  "\nAlive: " + str(self.myCounter.isAlive) +
                                  "\nRunning: " + str(self.myCounter.isRunning) +
                                  "\nValor Atual: " + str(self.actual_num))

    def do_resume(self):
        self.myCounter.resume()

    def do_stop(self):
        self.myCounter.stop()
        self.lblResultThr.setText("Status:" +
                                  "\nAlive: " + str(self.myCounter.isAlive) +
                                  "\nRunning: " + str(self.myCounter.isRunning) +
                                  "\nValor Atual: " + str(self.actual_num))

    def update_status_thread(self, generated_value):
        self.lblResultThr.setText("Status:" +
                                  "\nAlive: " + str(self.myCounter.isAlive) +
                                  "\nRunning: " + str(self.myCounter.isRunning) +
                                  "\nValor Atual: " + str(generated_value))

        self.editResultThr.setPlainText("Valor Atual: " + str(generated_value) +
                                         "\n" + self.editResultThr.toPlainText())

    def generate_value(self):
        self.myCounter.msleep(500)
        self.actual_num += 1
        self.actual_num %= 16
        self.myCounter.emit(SIGNAL("new_value(int)"), self.actual_num)

    # Arduino Handler

    def do_start_arduino_handler(self):
        self.arduinoDataConsumer.start()
        self.myArduinoHandler.start_acquisition()

    def do_stop_arduino_handler(self):
        self.myArduinoHandler.stop_acquisition()
        self.arduinoDataConsumer.stop()


    def arduino_consumer(self):
        if self.myArduinoHandler.data_waiting():
            aux = self.myArduinoHandler.buffer_acquisition.secure_dequeue()

            self.arduinoDataConsumer.emit(
                SIGNAL('new_arduino_data(int)'),
                aux)

    def arduino_data_ready(self,arduino_value):
        self.lblResultThr.setText(str(arduino_value))

    # Closing
    def closeEvent(self, QCloseEvent):
        # QThread Handler
        self.myCounter.stop()

        # Arduino Handler
        self.myArduinoHandler.stop_acquisition()
        self.arduinoDataConsumer.stop()
        super(self.__class__, self).closeEvent(QCloseEvent)


def main():
    app = QtGui.QApplication(sys.argv)  # A new instance of QApplication
    form = ExampleApp()                 # We set the form to be our ExampleApp (design)
    form.show()                         # Show the form
    app.exec_()                         # and execute the app


if __name__ == '__main__':              # if we're running file directly and not importing it
    main()                              # run the main function
