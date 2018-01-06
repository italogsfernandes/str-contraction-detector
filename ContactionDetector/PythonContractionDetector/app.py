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
import sys

# PyQt5
# from PyQt5.QtWidgets import *
# from views import base_qt5 as base
# PyQt4
from PyQt4.QtGui import *

from libraries.QtArduinoPlotter import QtArduinoPlotter
from libraries.QtArduinoPlotter import ArduinoEMGPlotter
from views import base_qt4 as base


# ------------------------------------------------------------------------------


class ContractionDetector(QMainWindow, base.Ui_MainWindow):
    def __init__(self, parent=None):
        super(self.__class__, self).__init__(parent)
        self.setupUi(self)
        self.setup_signals_connections()

        self.emg_app = ArduinoEM(parent=self.centralwidget)
        self.verticalLayoutGraph.addWidget(self.emg_app.plotHandler.plotWidget)

        self.verticalLayoutGraph.removeWidget(self.label_replace)
        self.label_replace.setParent(None)

    def setup_signals_connections(self):
        self.btn_start.clicked.connect(self.btn_start_clicked)
        self.btn_calib.clicked.connect(self.btn_calib_clicked)
        # self.sl_threshould.valueChanged.connect()
        self.cb_emg.toggled.connect(self.cb_emg_toggled)

    def cb_emg_toggled(self, cb_state):
        self.emg_app.emg_bruto.visible = cb_state

    def btn_start_clicked(self):
        if self.emg_app.started:
            self.emg_app.stop()
        else:
            self.emg_app.start()

    def btn_calib_clicked(self):
        print('calib clicked')

    def sl_threshould_value_changed(self,sl_value):
        print('th changed to: %d' % sl_value)



def main():
    app = QApplication(sys.argv)
    form = ContractionDetector()
    form.show()
    app.exec_()

if __name__ == "__main__":
    main()
