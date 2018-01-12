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


class ArmAnglesPlotter(QtArduinoPlotter):
    def __init__(self, parent, app=None, label=None):
        QtArduinoPlotter.__init__(self, parent, app, label)
        self.arduinoHandler.qnt_ch = 2
        self.biceps_plotHandler = PyQtGraphHandler(qnt_points=500, parent=parent, y_range=(0, 5), app=app)
        self.triceps_plotHandler = PyQtGraphHandler(qnt_points=500, parent=parent, y_range=(0, 5), app=app)

    def get_buffers_status(self, separator):
        return self.arduinoHandler.get_buffers_status(separator) + separator + \
               self.biceps_plotHandler.series.get_buffers_status()

    def _init_plotHandler(self, parent, app):
        self.plotHandler = None

    def consumer_function(self):
        if self.arduinoHandler.data_waiting():
            arduino_value = np.array(self.arduinoHandler.buffer_acquisition.get()) * 5.0 / 1024.0 - 2.5
            self.biceps_plotHandler.series.buffer.put(arduino_value[0])
            self.triceps_plotHandler.series.buffer.put(arduino_value[1])

    def start(self):
        self.started = True
        self.timerStatus.start()
        self.biceps_plotHandler.timer.start(0)
        self.triceps_plotHandler.timer.start(0)
        self.consumerThread.start()
        self.arduinoHandler.start_acquisition()

    def stop(self):
        self.started = False
        self.arduinoHandler.stop_acquisition()
        self.consumerThread.stop()
        self.timerStatus.stop()
        self.biceps_plotHandler.timer.stop()
        self.triceps_plotHandler.timer.stop()

def test():
    import sys
    from PyQt4 import QtGui
    app = QtGui.QApplication(sys.argv)
    form = QtGui.QMainWindow()
    form.resize(800, 600)
    central_widget = QtGui.QWidget(form)
    vertical_layout = QtGui.QVBoxLayout(central_widget)

    harry_plotter = ArduinoEMGPlotter(parent=central_widget)#, app=app)
    harry_plotter.start()

    vertical_layout.addWidget(harry_plotter.plotHandler.plotWidget)
    form.setCentralWidget(central_widget)
    form.show()
    app.exec_()
    harry_plotter.stop()

if __name__ == '__main__':
    test()