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
from QtArduinoPlotter import QtArduinoPlotter
from emgplotHandler import emgplotHandler
# ------------------------------------------------------------------------------
import numpy as np
import scipy as sp
import scipy.fftpack as fftpack

class QtArduinoEMGPlotter(QtArduinoPlotter):
    def __init__(self):
        self.plotHandler = emgplotHandler(5000)
        self.arduinoHandler = ArduinoHandler()
        self.consumerThread = ThreadHandler(self.consumer_function)
        self.timerStatus = InfiniteTimer(0.1, self.print_buffers_status)
        self.emg_bruto = 0

    def consumer_function(self):
        if self.arduinoHandler.data_waiting():
            self.emg_bruto = self.arduinoHandler.buffer_acquisition.get()*5.0/1024.0 - 2.5
            self.plotHandler.put(self.emg_bruto)


if __name__ == '__main__':
    my_arduino_ploter = QtArduinoEMGPlotter()
    my_arduino_ploter.plotHandler.emg_bruto_visible = False
    my_arduino_ploter.plotHandler.hilbert_visible = True
    my_arduino_ploter.plotHandler.hilbert_retificado_visible = False
    my_arduino_ploter.plotHandler.envoltoria_visible = True
    my_arduino_ploter.plotHandler.limiar_visible = True
    my_arduino_ploter.plotHandler.detection_sites_visible = False
    my_arduino_ploter.start()
    my_arduino_ploter.plotHandler.appear()
    my_arduino_ploter.stop()
