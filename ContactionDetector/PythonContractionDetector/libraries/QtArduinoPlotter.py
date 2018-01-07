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
# import sys
# sys.path.append('libraries/')
from ThreadHandler import ThreadHandler, InfiniteTimer
from ArduinoHandler import ArduinoHandler
from PyQtGraphHandler import PyQtGraphHandler
from EMGPlotHandler import EMGPlotHandler
# ------------------------------------------------------------------------------


class QtArduinoPlotter:
    def __init__(self, parent, app=None):
    	self.plotHandler = None
    	self._init_plotHandler(parent, app)
        self.arduinoHandler = ArduinoHandler()
        self.consumerThread = ThreadHandler(self.consumer_function)
        self.timerStatus = InfiniteTimer(0.05, self.print_buffers_status)
        self.started = False
    
    def _init_plotHandler(self, parent, app):
		self.plotHandler = PyQtGraphHandler(qnt_points=5000, parent=parent, y_range=(0, 5), app=app)
        
		
    def consumer_function(self):
        if self.arduinoHandler.data_waiting():
            self.plotHandler.series.buffer.put(self.arduinoHandler.buffer_acquisition.get()*5.0/1024.0)

    def get_buffers_status(self, separator):
        return self.arduinoHandler.get_buffers_status(separator) + separator + self.plotHandler.series.get_buffers_status()

    def print_buffers_status(self):
        print(self.get_buffers_status(" - "))

    def start(self):
        self.started = True
        self.timerStatus.start()
        self.plotHandler.timer.start(0)
        self.consumerThread.start()
        self.arduinoHandler.start_acquisition()

    def stop(self):
        self.started = False
        self.arduinoHandler.stop_acquisition()
        self.consumerThread.stop()
        self.timerStatus.stop()
        self.plotHandler.timer.stop()


def test():
    import sys
    from PyQt4 import QtGui
    app = QtGui.QApplication(sys.argv)
    form = QtGui.QMainWindow()
    form.resize(800, 600)
    central_widget = QtGui.QWidget(form)
    vertical_layout = QtGui.QVBoxLayout(central_widget)

    harry_plotter = QtArduinoPlotter(parent=central_widget)# , app=app)
    harry_plotter.start()

    vertical_layout.addWidget(harry_plotter.plotHandler.plotWidget)
    form.setCentralWidget(central_widget)
    form.show()
    app.exec_()
    harry_plotter.stop()

if __name__ == '__main__':
    test()
