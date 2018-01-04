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
from ThreadHandler import InfiniteTimer
from Queue import Queue

from pyqtgraph.Qt import QtGui, QtCore
import numpy as np
import pyqtgraph as pg
from pyqtgraph.ptime import time
from PyQt4.QtGui import QBrush, QColor, QPen, QGraphicsRectItem
# ------------------------------------------------------------------------------
from pyqtgraphHandler import pyqtgraphHandler

# Processing:
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

class emgplotHandler(pyqtgraphHandler):
    def __init__(self, qnt_pontos=10):
        pyqtgraphHandler.__init__(self, qnt_pontos)
        self.plot.setYRange(-2.5, 2.5)
        self.plot.setWindowTitle("Detector de Contrações")
        self.emg_bruto_values = [0] * self.qnt_pontos
        self.hilbert_values = [0] * self.qnt_pontos
        self.hilbert_retificado_values = [0] * self.qnt_pontos
        self.envoltoria_values = [0] * self.qnt_pontos
        self.limiar_values = [0.5] * self.qnt_pontos
        self.detection_sites = []

        self.emg_bruto_curve = self.plot.plot(pen=(0, 0, 255), name="EMG Bruto Curve")
        self.hilbert_curve = self.plot.plot(pen=(90, 200, 90), name="Hilbert Curve")
        self.hilbert_retificado_curve = self.plot.plot(pen=(30, 200, 100), name="Hilbert Retificado Curve")
        self.envoltoria_curve = self.plot.plot(pen=(255, 0, 0), name="Envoltoria Curve")
        self.limiar_curve = self.plot.plot(pen=(150, 150, 0), name="Limiar Curve")

        self.emg_bruto_visible = True
        self.hilbert_visible = False
        self.hilbert_retificado_visible = False
        self.envoltoria_visible = False
        self.limiar_visible = False
        self.detection_sites_visible = False

        self.b, self.a = butter_lowpass(7, 1000, order=2)

        self.lr = pg.LinearRegionItem([0, 1], movable=False)
        self.lr.setZValue(-10)
        self.plot.addItem(self.lr)

    def update_y_points(self):
        points_to_add = self.plot_buffer.qsize()
        if points_to_add > 0:
            for n in range(points_to_add): # obtains the new values
                num = self.plot_buffer.get()
                self.emg_bruto_values.append(num)
                if len(self.emg_bruto_values) > self.qnt_pontos:
                    self.emg_bruto_values.pop(0)
            self.calcular_coisas()
            self.update()

    def calcular_coisas(self):
        self.hilbert_values = fftpack.hilbert(self.emg_bruto_values)
        self.hilbert_retificado_values = np.abs(self.hilbert_values)
        self.envoltoria_values = filtfilt(self.b, self.a, self.hilbert_retificado_values) * 3.2
        self.detection_sites = self.envoltoria_values > self.limiar_values[0]

        time_inicio = self.qnt_pontos - 1
        time_end = 0
        for n in range(1, self.qnt_pontos):
            #subida
            if self.detection_sites[n] and not self.detection_sites[n-1]:
                time_inicio = n # Armazena o indes de inicio da contracao
                time_end = self.qnt_pontos - 1 # E reseta o index de termino
            if not self.detection_sites[n] and self.detection_sites[n - 1]:
                time_end = n
                # lr = pg.LinearRegionItem([time_inicio, time_end], movable=False)
                self.lr.setRegion([time_inicio, time_end])


    def update(self):
        self.emg_bruto_curve.setData(self.emg_bruto_values if self.emg_bruto_visible else [])
        self.hilbert_curve.setData(self.hilbert_values if self.hilbert_visible else [])
        self.hilbert_retificado_curve.setData(self.hilbert_retificado_values if self.hilbert_retificado_visible else [])
        self.envoltoria_curve.setData(self.envoltoria_values if self.envoltoria_visible else [])
        self.limiar_curve.setData(self.limiar_values if self.limiar_visible else [])


        now = time()
        dt = now - self.lastTime
        self.lastTime = now
        if self.fps is None:
            self.fps = 1.0 / dt
        else:
            s = np.clip(dt * 3., 0, 1)
            self.fps = self.fps * (1 - s) + (1.0 / dt) * s
        #self.plot.setTitle('<font color="black"> <b> EMG Contraction Detector </b> - <i> %0.2f fps </i></font>' % self.fps)
        self.plot.setTitle('<font color="black"> <i> %0.2f fps </i></font>' % self.fps)
        self.app.processEvents()  # force complete redraw for every plot

if __name__ == '__main__':

    my_plot = emgplotHandler(5000)
    mv_avg = [0.0] * 200
    from datetime import datetime

    def generate_point():
        agr = datetime.now()
        y_value = agr.microsecond / 1000000.0
        emg_bruto = np.sin(2*np.pi*y_value)+0.4*np.sin(20*2*np.pi*y_value)
        mv_avg.append(emg_bruto)
        mv_avg.pop(0)
        my_plot.put([emg_bruto, np.array(mv_avg).mean()])
        #print y_value

    timer = InfiniteTimer(0.001, generate_point)
    timer.start()
    my_plot.appear()
