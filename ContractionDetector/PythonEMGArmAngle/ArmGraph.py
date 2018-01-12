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
import numpy as np

import sys
if sys.version_info.major == 2:
    from Queue import Queue
else:
    from queue import Queue

# ------------------------------------------------------------------------------


class ArmSkeleton:
    def __init__(self):
        self.hand = [0, 0]
        self.elbow = [0, 0]
        self.shoulder = [0, 0]
        self.init_positions()

    def init_positions(self):
        self.hand = [0.7, 0]
        self.elbow = [0, 0]
        self.shoulder = [0, 1]

    def set_angle(self, angle):
        r = 0.7
        theta = np.deg2rad(-angle+90)
        self.hand[0] = r*np.cos(theta)
        self.hand[1] = r*np.sin(theta)

    def get_x_values(self):
        return [self.shoulder[0], self.elbow[0], self.hand[0]]

    def get_y_values(self):
        return [self.shoulder[1], self.elbow[1], self.hand[1]]


class ArmGraph:
    def __init__(self, parent=None, app=None):
        self.app = app
        self.plotWidget = pg.PlotWidget(parent)
        self.arm = ArmSkeleton()

        self.configure_plot()
        self.curve = self.create_arm()
        self.timer = QtCore.QTimer()
        self.timer.timeout.connect(self.update)

        self.show_fps = True
        self.lastTime = 0
        self.fps = 0

    def configure_pen(self):
        pen = QPen()
        pen.setColor(QColor.fromRgb(255, 10, 0))
        pen.setWidthF(0.025)
        return pen

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
        self.plotWidget.setXRange(-0.5, 0.9)
        self.plotWidget.setYRange(-0.5, 0.9)
        self.plotWidget.setLabel('bottom', x_title, units=x_unit)
        self.plotWidget.setLabel('left', y_title, units=y_unit)

    def configure_title(self, title="Graph"):
        self.plotWidget.setTitle('<font color="black"> %s </font>' % title)

    def create_arm(self):
        return self.plotWidget.plot(self.arm.get_x_values(), self.arm.get_y_values(),
                                    pen=self.configure_pen(), symbol='o', symbolSize=25)

    def update(self):
        self.curve.setData(self.arm.get_x_values(), self.arm.get_y_values())

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


def test():
    import sys
    from PyQt4 import QtGui
    app = QtGui.QApplication(sys.argv)
    form = QtGui.QMainWindow()
    form.resize(800, 600)
    central_widget = QtGui.QWidget(form)
    vertical_layout = QtGui.QVBoxLayout(central_widget)

    plot_handler = ArmGraph(parent=central_widget)

    vertical_layout.addWidget(plot_handler.plotWidget)
    form.setCentralWidget(central_widget)
    form.show()
    app.exec_()

if __name__ == '__main__':
    test()
