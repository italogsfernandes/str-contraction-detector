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
from ArduinoHandler import ArduinoHandler
from ThreadHandler import ThreadHandler, InfiniteTimer

from EMGPlotHandler import EMGPlotHandler
from QtArduinoPlotter import QtArduinoPlotter

# ------------------------------------------------------------------------------

import numpy as np
import scipy.fftpack as fftpack
from scipy.signal import butter, lfilter, freqz, filtfilt
import sys
if sys.version_info.major == 2:
    from Queue import Queue
else:
    from queue import Queue
from collections import deque

class EMGProcessing:
    def __init__(self):
        self.window_size = 256
        self.fs = 1000
        self.filter_params = dict()

        [b, a] = EMGProcessing.design_butter_lowpass(7, self.fs, 2)
        self.filter_params['b'] = b
        self.filter_params['a'] = a

        self.buffer = deque(maxlen=self.window_size)
        self.emg_bruto = [0.0] * self.window_size
        self.moving_avg = [0.0] * self.window_size
        self.hilbert = np.zeros(self.window_size, dtype='float')
        self.hilbert_retificado = np.zeros(self.window_size, dtype='float')
        self.envoltoria = np.zeros(self.window_size, dtype='float')
        self.threshold = 0.25
        self.detection_sites = np.zeros(self.window_size, dtype='bool')
        self.window = 100
        self.weights = np.hamming(self.window) / (self.window / 2)

    def update_values(self):
        points_to_add = len(self.buffer)
        if points_to_add > 0:
            for n in range(points_to_add):  # obtains the new values
                num = self.buffer.popleft()
                self.emg_bruto.append(num)
                self.emg_bruto.pop(0)
            self.do_process()

    def do_process(self):
        #self.hilbert = fftpack.hilbert(self.emg_bruto)#+self.emg_bruto[::-1])
        self.hilbert = self.emg_bruto - EMGProcessing.do_moving_average(self.emg_bruto, 5) #+self.emg_bruto[::-1])
        self.hilbert_retificado = np.abs(self.hilbert)

        #self.envoltoria = \
        #   filtfilt(self.filter_params['b'], self.filter_params['a'], self.hilbert_retificado)
        #self.envoltoria = \
        #    EMGProcessing.do_moving_average(self.hilbert_retificado, 128)*3.8
        self.envoltoria = \
            np.convolve(self.hilbert_retificado, self.weights, 'same') * 3.8
        # self.envoltoria = EMGProcessing.do_moving_average(self.hilbert_retificado,100)
        self.detection_sites = self.envoltoria > self.threshold

        #self.hilbert = self.hilbert[0: self.window_size]
        #self.hilbert_retificado = self.hilbert_retificado[0: self.window_size]
        #self.envoltoria = self.envoltoria[0: self.window_size]
        #self.detection_sites = self.detection_sites[0: self.window_size]


    @staticmethod
    def do_moving_average(values, window):
        weights = np.repeat(1.0, window) / window
        sma = np.convolve(values, weights, 'same')
        return sma

    @staticmethod
    def design_butter_lowpass(cutoff, fs, order=5):
        nyq = 0.5 * fs
        normal_cutoff = cutoff / nyq
        [b, a] = butter(order, normal_cutoff, btype='low', analog=False)
        return b, a


class ArduinoEMGPlotter(QtArduinoPlotter):
    def __init__(self, parent, app=None, label=None):
        QtArduinoPlotter.__init__(self, parent, app, label)
        self.process = EMGProcessing()
        self.plotHandler.emg_bruto.set_visible(True)
        self.plotHandler.hilbert.set_visible(False)
        self.plotHandler.hilbert_retificado.set_visible(False)
        self.plotHandler.envoltoria.set_visible(False)
        self.plotHandler.threshold.set_visible(False)
        self.plotHandler.set_detection_visible(False)
        self.emg_value = 0

        self.as_simple_as_possible = False
        #self.offset_values = np.zeros(128)
        self.offset_values = [0] * 1000
        #self.mva_values = np.zeros(128)
        self.mva_values = [0] * 128

    def get_buffers_status(self, separator):
        return self.arduinoHandler.get_buffers_status(separator) + separator + \
               self.plotHandler.emg_bruto.get_buffers_status()

    def _init_plotHandler(self, parent, app):
        self.plotHandler = EMGPlotHandler(qnt_points=4096, parent=parent, y_range=(-2.5, 2.5),
                                          app=app, proc='hbt+btr')

    def consumer_function(self):
        if self.plotHandler.process_in_plotter:
            if self.arduinoHandler.data_waiting():
                #print("help! i need")
                self.emg_value = self.arduinoHandler.buffer_acquisition.get() * 5.0 / 1024.0 - 2.5
                self.plotHandler.emg_bruto.buffer.put(self.emg_value)
        elif self.as_simple_as_possible:
            if self.arduinoHandler.data_waiting():
                self.emg_value = self.arduinoHandler.buffer_acquisition.get() * 5.0 / 1024.0 - 2.5
                #self.offset_values = np.roll(self.offset_values, -1)
                #self.offset_values[:-1] = self.offset_values[1:]
                #self.offset_values[-1] = self.emg_value
                self.offset_values.pop(0)
                self.offset_values.append(self.emg_value)
                hbt = self.emg_value - sum(self.offset_values)/len(self.offset_values)
                hbt_ret = abs(hbt)
                #self.mva_values = np.roll(self.mva_values,-1)
                #self.mva_values[:-1] = self.mva_values[1:]
                #self.mva_values[-1] = hbt_ret
                self.mva_values.pop(0)
                self.mva_values.append(abs(hbt))
                env = sum(self.mva_values)/len(self.mva_values)
                self.plotHandler.emg_bruto.buffer.put(self.emg_value)
                self.plotHandler.hilbert.buffer.put(hbt)
                self.plotHandler.hilbert_retificado.buffer.put(hbt_ret)
                self.plotHandler.envoltoria.buffer.put(env*3.83)
        else:
            if self.arduinoHandler.data_waiting():
                # print "hello i'm here"
                points_to_process = self.arduinoHandler.buffer_acquisition.qsize()
                for n in range(points_to_process):
                    self.emg_value = self.arduinoHandler.buffer_acquisition.get()*5.0/1024.0 - 2.5
                    self.process.buffer.append(self.emg_value)
                self.process.update_values()
                self.process.do_process()
                for n in range(points_to_process):
                    index_out = self.process.window_size - points_to_process + n
                    self.plotHandler.emg_bruto.buffer.put(self.process.emg_bruto[index_out])
                    self.plotHandler.hilbert.buffer.put(self.process.hilbert[index_out])
                    self.plotHandler.hilbert_retificado.buffer.put(self.process.hilbert_retificado[index_out])
                    self.plotHandler.envoltoria.buffer.put(self.process.envoltoria[index_out])


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
