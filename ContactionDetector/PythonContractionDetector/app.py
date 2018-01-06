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
from PyQt5.QtWidgets import *
from views import base_qt5 as base
# PyQt4
# from PyQt4.QtGui import *
# from views import base_qt4 as base
# ------------------------------------------------------------------------------


class ContractionDetector(QMainWindow, base.Ui_MainWindow):
    def __init__(self, parent=None):
        super(self.__class__, self).__init__(parent)
        self.setupUi(self)
        self.setup_signals_connections()

    def setup_signals_connections(self):
        self.btn_start.clicked.connect(self.btn_start_clicked)
        self.btn_calib.clicked.connect(self.btn_calib_clicked)
        self.sl_threshould.valueChanged.connect(lambda x: print(x))
        self.cb_emg.toggled.connect(lambda x: print(x))

    def btn_start_clicked(self):
        print('start clicked')

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
