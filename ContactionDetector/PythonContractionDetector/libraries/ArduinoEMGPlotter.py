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


class ArduinoEMGPlotter(QtArduinoPlotter):
    def __init__(self, parent, app=None):
        self.plotHandler = EMGPlotHandler(qnt_points=5000, parent=parent, y_range=(0, 5), app=app)
        self.arduinoHandler = ArduinoHandler()
        self.consumerThread = ThreadHandler(self.consumer_function)
        self.timerStatus = InfiniteTimer(0.05, self.print_buffers_status)
        self.started = False


    def consumer_function(self):
        if self.arduinoHandler.data_waiting():
            self.emg_bruto = self.arduinoHandler.buffer_acquisition.get()*5.0/1024.0
            self.plotHandler.put(self.emg_bruto - 2.5)


def test():
    import sys
    from PyQt4 import QtGui
    app = QtGui.QApplication(sys.argv)
    form = QtGui.QMainWindow()
    form.resize(800, 600)
    central_widget = QtGui.QWidget(form)
    vertical_layout = QtGui.QVBoxLayout(central_widget)

    harry_plotter = ArduinoEMGPlotter(parent=central_widget)# , app=app)
    harry_plotter.start()

    vertical_layout.addWidget(harry_plotter.plotHandler.plotWidget)
    form.setCentralWidget(central_widget)
    form.show()
    app.exec_()
    harry_plotter.stop()

if __name__ == '__main__':
    test()
