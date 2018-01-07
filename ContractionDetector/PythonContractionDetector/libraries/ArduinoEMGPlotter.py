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
    def __init__(self, parent, app=None, label=None):
        QtArduinoPlotter.__init__(self, parent, app, label)
        self.plotHandler.process_in_plotter = True
        self.plotHandler.emg_bruto.set_visible(True)
        self.plotHandler.hilbert.set_visible(False)
        self.plotHandler.hilbert_retificado.set_visible(False)
        self.plotHandler.envoltoria.set_visible(False)
        self.plotHandler.threshold.set_visible(False)
        self.plotHandler.set_detection_visible(False)

    def get_buffers_status(self, separator):
        return self.arduinoHandler.get_buffers_status(separator) + separator + \
               self.plotHandler.emg_bruto.get_buffers_status()

    def _init_plotHandler(self, parent, app):
        self.plotHandler = EMGPlotHandler(qnt_points=5000, parent=parent, y_range=(-2.5, 2.5), app=app)

    def consumer_function(self):
        if self.arduinoHandler.data_waiting():
            self.plotHandler.emg_bruto.buffer.put(self.arduinoHandler.buffer_acquisition.get()*5.0/1024.0 - 2.5)


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
