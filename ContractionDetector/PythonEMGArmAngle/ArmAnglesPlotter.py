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
# import sys
# sys.path.append('../libraries')
from libraries.ArduinoHandler import ArduinoHandler
from libraries.ThreadHandler import ThreadHandler, InfiniteTimer
from libraries.PyQtGraphHandler import PyQtGraphHandler
from libraries.QtArduinoPlotter import QtArduinoPlotter
from ArmGraph import ArmGraph
# ------------------------------------------------------------------------------

import numpy as np
import scipy.fftpack as fftpack
from scipy.signal import butter, lfilter, freqz, filtfilt
import sys
if sys.version_info.major == 2:
    from Queue import Queue
else:
    from queue import Queue
# from collections import deque


class ArmAnglesProcessing:
    def __init__(self):
        self.regressor = None

    def get_angle(self, x):
        # self.regressor.predict(x)
        if self.regressor is None:
            return x[0] * 10
        else:
            return 3


class ArmAnglesPlotter(QtArduinoPlotter):
    def __init__(self, parent, app=None, label=None):
        QtArduinoPlotter.__init__(self, parent, app, label)
        self.arduinoHandler.qnt_ch = 2
        self.biceps_plotHandler = PyQtGraphHandler(qnt_points=300, parent=parent, y_range=(0, 5), app=app)
        self.triceps_plotHandler = PyQtGraphHandler(qnt_points=300, parent=parent, y_range=(0, 5), app=app)
        self.angles_plotHandler = PyQtGraphHandler(qnt_points=300, parent=parent, y_range=(0, 180), app=app)
        self.arm_plot = ArmGraph(parent=parent)
        self.processer = ArmAnglesProcessing()

    def get_buffers_status(self, separator):
        return self.arduinoHandler.get_buffers_status(separator) + separator + \
               self.biceps_plotHandler.series.get_buffers_status()

    def _init_plotHandler(self, parent, app):
        self.plotHandler = None

    def consumer_function(self):
        if self.arduinoHandler.data_waiting():
            arduino_value = np.array(self.arduinoHandler.buffer_acquisition.get()) * 5.0 / 1024.0
            angle = self.processer.get_angle(arduino_value)
            self.arm_plot.arm.set_angle(angle)
            self.angles_plotHandler.series.buffer.put(angle)
            self.biceps_plotHandler.series.buffer.put(arduino_value[0])
            self.triceps_plotHandler.series.buffer.put(arduino_value[1])

    def start(self):
        self.started = True
        self.timerStatus.start()
        self.biceps_plotHandler.timer.start(0)
        self.triceps_plotHandler.timer.start(0)
        self.angles_plotHandler.timer.start(0)
        self.arm_plot.timer.start(100)
        self.consumerThread.start()
        self.arduinoHandler.start_acquisition()

    def stop(self):
        self.started = False
        self.arduinoHandler.stop_acquisition()
        self.consumerThread.stop()
        self.timerStatus.stop()
        self.biceps_plotHandler.timer.stop()
        self.triceps_plotHandler.timer.stop()
        self.angles_plotHandler.timer.stop()
        self.arm_plot.timer.stop()


def test():
    pass

if __name__ == '__main__':
    test()