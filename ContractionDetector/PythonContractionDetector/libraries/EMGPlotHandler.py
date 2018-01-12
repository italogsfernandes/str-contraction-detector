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
import pyqtgraph as pg
from PyQtGraphHandler import PyQtGraphHandler, PyQtGraphSeries
# ------------------------------------------------------------------------------
# Processing:
import numpy as np
import scipy.fftpack as fftpack
from scipy.signal import butter, lfilter, freqz, filtfilt

def butter_lowpass(cutoff, fs, order=5):
    nyq = 0.5 * fs
    normal_cutoff = cutoff / nyq
    b, a = butter(order, normal_cutoff, btype='low', analog=False)
    return b, a

def butter_lowpass_filter(data, cutoff, fs, order=5):
    b, a = butter_lowpass(cutoff, fs, order=order)
    y = lfilter(b, a, data)
    return y
# ------------------------------------------------------------------------------


class EMGPlotHandler(PyQtGraphHandler):
    def __init__(self, qnt_points=2000, parent=None, y_range=(-1, 1), app=None, proc=None):
        PyQtGraphHandler.__init__(self, qnt_points, parent, y_range, app)

        self.plotWidget.removeItem(self.series.curve)
        self.emg_bruto = PyQtGraphSeries(self, pen=(0, 0, 255), name="EMG Bruto")
        self.hilbert = PyQtGraphSeries(self, pen=(90, 200, 90), name="Hilbert")
        self.hilbert_retificado = PyQtGraphSeries(self, pen=(30, 100, 10), name="Hilbert Retificado")
        self.envoltoria = PyQtGraphSeries(self, pen=(255, 0, 0), name="Envoltoria")
        self.threshold = PyQtGraphSeries(self, pen=(255, 150, 0), name="Limiar")

        self.set_threshold(2.5)

        self.contraction_region = pg.LinearRegionItem([0, 1], movable=False)
        self.contraction_region.setZValue(10)
        self.plotWidget.addItem(self.contraction_region)

        self.proc = proc
        if self.proc is not None:
            self.process_in_plotter = True
        else:
            self.process_in_plotter = False

        if self.proc == "hbt+btr": # Hilbert + butterworth
            # Processing:
            self.detection_sites = []
            self.b, self.a = butter_lowpass(7, 1000, order=2)
        if self.proc == "mva": #Only moving average
            pass

    def set_detection_visible(self, visible):
        if not visible:
            self.plotWidget.removeItem(self.contraction_region)
        else:
            self.plotWidget.addItem(self.contraction_region)

    def set_threshold(self, th_value):
        self.threshold.values = [th_value] * self.qnt_points

    def update(self):
        self.emg_bruto.update_values()
        if self.process_in_plotter:
            self.process_data()
        self.hilbert.update_values()
        self.hilbert_retificado.update_values()
        self.envoltoria.update_values()
        self.threshold.update_values()

        if self.show_fps:
            self.calculate_fps()
            self.plotWidget.setTitle('<font color="red">%0.2f fps</font>' % self.fps)

    def process_data(self):
        if self.proc == 'hbt+btr':
            self.hilbert.values = fftpack.hilbert(self.emg_bruto.values)
            self.hilbert_retificado.values = np.abs(self.hilbert.values)
            self.envoltoria.values = filtfilt(self.b, self.a, self.hilbert_retificado.values)
        if self.proc == 'mva':
            self.hilbert.values = self.emg_bruto
        self.detection_sites = self.envoltoria.values > self.threshold.values[0]

        time_inicio = self.qnt_points - 1
        for n in range(1, self.qnt_points):
            #subida
            if self.detection_sites[n] and not self.detection_sites[n-1]:
                time_inicio = n # Armazena o indes de inicio da contracao
            if not self.detection_sites[n] and self.detection_sites[n - 1]:
                time_end = n
                self.contraction_region.setRegion([time_inicio, time_end])


def test():
    import sys
    from PyQt4 import QtGui
    app = QtGui.QApplication(sys.argv)
    form = QtGui.QMainWindow()
    form.resize(800, 600)
    central_widget = QtGui.QWidget(form)
    vertical_layout = QtGui.QVBoxLayout(central_widget)

    plot_handler = EMGPlotHandler(qnt_points=5000, parent=central_widget, y_range=[-2.5, 2.5])
    plot_handler.process_in_plotter = False
    plot_handler.emg_bruto.visible = True
    plot_handler.hilbert.visible = False
    plot_handler.hilbert_retificado.visible = False
    from datetime import datetime
    import numpy as np

    def generate_point():
        agr = datetime.now()
        y_value = agr.microsecond / 1000000.0
        emg_bruto = np.sin(2*np.pi*y_value)+0.4*np.sin(20*2*np.pi*y_value)
        plot_handler.emg_bruto.buffer.put(emg_bruto+1)

    from ThreadHandler import InfiniteTimer
    timer = InfiniteTimer(0.001, generate_point)
    timer.start()

    plot_handler.timer.start(0)

    vertical_layout.addWidget(plot_handler.plotWidget)
    form.setCentralWidget(central_widget)
    form.show()
    app.exec_()

    timer.stop()
    plot_handler.timer.stop()

if __name__ == '__main__':
    test()
