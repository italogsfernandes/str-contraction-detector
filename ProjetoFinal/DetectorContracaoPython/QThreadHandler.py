# -*- coding: utf-8 -*-
# ------------------------------------------------------------------------------
# FEDERAL UNIVERSITY OF UBERLANDIA
# Faculty of Electrical Engineering
# Biomedical Engineering Lab
# ------------------------------------------------------------------------------
# Creator of Base Class: Andrei Nakagawa, MSc
# Contact: nakagawa.andrei@gmail.com
# Class: QThreadHander
# Author: Italo G S Fernandes
#                 (italogsfernandes@gmail.com, github.com/italogfernandes)
# ------------------------------------------------------------------------------
# Description:
# ------------------------------------------------------------------------------
from PyQt4.QtCore import QThread, SIGNAL
# ------------------------------------------------------------------------------


class QThreadHandler(QThread):
    def __init__(self, worker=None):
        super(self.__class__, self).__init__()
        self.worker = worker
        self.isAlive = False
        self.isRunning = False

    def __del__(self):
        self.wait()

    def run(self):
        while self.isAlive:
            if self.isRunning:
                self.worker()

    def start(self):
        self.isAlive = True
        self.isRunning = True
        super(self.__class__, self).start()

    def stop(self):
        self.isAlive = False
        self.isRunning = False

    def pause(self):
        self.isRunning = False

    def resume(self):
        self.isRunning = True
