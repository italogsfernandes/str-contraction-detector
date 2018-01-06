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
from pyqtgraph.Qt import QtCore
import pyqtgraph as pg
from pyqtgraph.ptime import time
from PyQt4.QtGui import QBrush, QColor, QPen
from numpy import clip

import sys
if sys.version_info.major == 2:
    from Queue import Queue
else:
    from queue import Queue

# ------------------------------------------------------------------------------


class PyQtGraphSeries:
    def __init__(self, parent=None, pen=(0, 0, 255), name="Curve"):
        self.parent = parent
        self.values = [0] * self.parent.qnt_points
        self.curve = self.parent.plotWidget.plot(self.values, pen=pen, name=name)
        self.visible = True
        self.buffer = Queue(self.parent.qnt_points)

    def update_values(self):
        points_to_add = self.buffer.qsize()
        if points_to_add > 0:
            for n in range(points_to_add):  # obtains the new values
                num = self.buffer.get()
                self.values.append(num)
                if len(self.values) > self.parent.qnt_points:
                    self.values.pop(0)
        self.curve.setData(self.values if self.visible else [])

    def get_buffers_status(self):
        return "Plot: %4d" % (self.buffer.qsize()) + '/' + str(self.buffer.maxsize)


class PyQtGraphHandler:
    def __init__(self, qnt_points=10, parent=None, y_range=(-1, 1), app=None):
        self.app = app
        self.__y_range = y_range
        self.qnt_points = qnt_points

        self.plotWidget = pg.PlotWidget(parent)
        self.series = PyQtGraphSeries(self, (0, 0, 255), "Values")
        self.configure_plot()

        self.timer = QtCore.QTimer()
        self.timer.timeout.connect(self.update)

        self.show_fps = True
        self.lastTime = 0
        self.fps = 0

    def update(self):
        self.series.update_values()

        if self.show_fps:
            self.calculate_fps()
            self.plotWidget.setTitle('<font color="red">%0.2f fps</font>' % self.fps)

        if self.app is not None:
            self.app.processEvents()

    def calculate_fps(self):
        now = time()
        dt = now - self.lastTime
        self.lastTime = now
        if self.fps is None:
            self.fps = 1.0 / dt
        else:
            s = clip(dt * 3., 0, 1)
            self.fps = self.fps * (1 - s) + (1.0 / dt) * s

    def configure_plot(self):
        self.configure_area()
        self.configure_title("Graph")

    def configure_area(self, x_title='Index', x_unit='',  y_title='Values', y_unit=''):
        self.plotWidget.showGrid(True, True)
        # Colors:
        self.plotWidget.setBackgroundBrush(QBrush(QColor.fromRgb(255, 255, 255)))
        self.plotWidget.getAxis('left').setPen(QPen(QColor.fromRgb(0, 0, 0)))
        self.plotWidget.getAxis('bottom').setPen(QPen(QColor.fromRgb(0, 0, 0)))
        self.plotWidget.getAxis('left').setPen(QPen(QColor.fromRgb(0, 0, 0)))
        self.plotWidget.getAxis('bottom').setPen(QPen(QColor.fromRgb(0, 0, 0)))
        # Axis:
        self.plotWidget.setXRange(0, self.qnt_points)
        self.plotWidget.setYRange(self.__y_range[0], self.__y_range[1])
        self.plotWidget.setLabel('bottom', x_title, units=x_unit)
        self.plotWidget.setLabel('left', y_title, units=y_unit)

    def configure_title(self, title="Graph"):
        self.plotWidget.setTitle('<font color="black"> %s </font>' % title)


def test():
    import sys
    from PyQt4 import QtGui
    app = QtGui.QApplication(sys.argv)
    form = QtGui.QMainWindow()
    form.resize(800, 600)
    central_widget = QtGui.QWidget(form)
    vertical_layout = QtGui.QVBoxLayout(central_widget)

    plot_handler = PyQtGraphHandler(qnt_points=5000, parent=central_widget)

    plot_handler.plotWidget.setYRange(-1, 1)

    from datetime import datetime
    import numpy as np

    def generate_point():
        agr = datetime.now()
        y_value = agr.microsecond / 1000000.0
        plot_handler.series.buffer.put(np.sin(2 * np.pi * y_value))

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
