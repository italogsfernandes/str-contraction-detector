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
from PyQt4 import QtCore
from libraries.QtArduinoPlotter import QtArduinoPlotter
# from libraries.ArduinoEMGPlotter import ArduinoEMGPlotter
from views import base_qt4 as base
from ArmAnglesPlotter import ArmAnglesPlotter
# ------------------------------------------------------------------------------


class EMGArmAngles(QMainWindow, base.Ui_MainWindow):
    def __init__(self, parent=None):
        super(self.__class__, self).__init__(parent)
        self.setupUi(self)
        self.setup_signals_connections()
        self.remove_widgets_temporarios()

        self.arm_angle_app = ArmAnglesPlotter(parent=self.centralwidget, label=self.lbl_status)
        self.verticalLayoutChart1.addWidget(self.arm_angle_app.biceps_plotHandler.plotWidget)
        self.verticalLayoutChart2.addWidget(self.arm_angle_app.triceps_plotHandler.plotWidget)

        self.arm_angle_app.start()
        #self.cb_emg.toggle()
        #self.sl_threshould.setValue(0.25)
        #self.sl_threshould_value_changed(10)

    def remove_widgets_temporarios(self):
        self.verticalLayoutChart1.removeWidget(self.replaceChart1)
        self.replaceChart1.setParent(None)
        self.verticalLayoutChart2.removeWidget(self.replaceChart2)
        self.replaceChart2.setParent(None)
        self.verticalLayoutChart3.removeWidget(self.replaceChart3)
        self.replaceChart3.setParent(None)
        self.verticalLayoutChart4.removeWidget(self.replaceChart4)
        self.replaceChart4.setParent(None)

    def setup_signals_connections(self):
        self.btn_start.clicked.connect(self.btn_start_clicked)
        # self.cb_emg.toggled.connect(lambda x: self.emg_app.plotHandler.emg_bruto.set_visible(x))

    def closeEvent(self, q_close_event):
        self.arm_angle_app.stop()
        super(self.__class__, self).closeEvent(q_close_event)

    def btn_start_clicked(self):
        pass

    def btn_calib_clicked(self):
        pass


def main():
    app = QApplication(sys.argv)
    form = EMGArmAngles()
    form.show()
    app.exec_()

if __name__ == "__main__":
    main()
