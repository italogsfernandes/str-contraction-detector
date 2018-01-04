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
from ThreadHandler import ThreadHandler, InfiniteTimer
from ArduinoHandler import ArduinoHandler
from matplotlibHandler import PyPlotHandler
# ------------------------------------------------------------------------------


class ArduinoPlotter:
    def __init__(self):
        self.plotHandler = PyPlotHandler(100, [0, 10, 0, 5])
        self.arduinoHandler = ArduinoHandler()
        self.consumerThread = ThreadHandler(self.consumer_function)
        self.timerStatus = InfiniteTimer(0.1, self.print_buffers_status)

    def consumer_function(self):
        if self.arduinoHandler.data_waiting():
            self.plotHandler.put(self.arduinoHandler.buffer_acquisition.get()*5.0/1024.0)

    def get_buffers_status(self,separator):
        return self.arduinoHandler.get_buffers_status() + separator + self.plotHandler.get_buffers_status()

    def print_buffers_status(self):
        print self.get_buffers_status(" - ")

    def start(self):
        self.timerStatus.start()
        self.consumerThread.start()
        self.arduinoHandler.start_acquisition()

    def stop(self):
        self.arduinoHandler.stop_acquisition()
        self.consumerThread.stop()
        self.timerStatus.stop()

if __name__ == '__main__':
    my_arduino_ploter = ArduinoPlotter()

    my_arduino_ploter.start()
    my_arduino_ploter.plotHandler.show()
    my_arduino_ploter.stop()
