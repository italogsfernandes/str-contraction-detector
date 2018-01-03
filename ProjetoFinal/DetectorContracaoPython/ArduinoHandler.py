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

import serial
import serial.tools.list_ports as serial_tools

import QThreadHandler
from PyQt4.QtCore import QObject, SIGNAL
from PyQt4.QtGui import QMessageBox

import CircularBuffer
from ctypes import c_short

class ArduinoConstants():
    PACKETSIZE = 4
    PACKETSTART = '$'
    PACKETEND = '\n'
    ARDUINODESCRIPTION = "Arduino"


class ArduinoHandler(QObject):
    def __init__(self, port_name = "None", baudrate = 115200, readtimeout = 1000):
        super(self.__class__, self).__init__()

        self.serialPort = serial.Serial()
        self.serialPort.setPort(port_name)
        self.serialPort.baudrate = baudrate
        self.serialPort.timeout = readtimeout

        self.thread_acquisition = QThreadHandler.QThreadHandler(self.acquire_routine)
        self.connect(self.thread_acquisition, SIGNAL('finished()'), self.on_end_function)

        self.buffer_acquisition  = CircularBuffer.CircularBuffer(1024)

    def data_waiting(self):
        return self.buffer_acquisition.count > 0

    def on_end_function(self):
        if self.serialPort.isOpen():
            self.serialPort.close()

    def open(self):
        try:
            self.serialPort.open()
            self.serialPort.flushInput()
            self.serialPort.flushOutput()
        except:
            msg = QMessageBox()
            msg.setIcon(QMessageBox.Warning)
            msg.setText("Não foi possivel abrir a porta serial")
            msg.setWindowTitle("Erro")
            retval = msg.exec_()

    def close(self):
        try:
            self.serialPort.close()
        except:
            msg = QMessageBox()
            msg.setIcon(QMessageBox.Warning)
            msg.setText("Não foi possivel fechar a porta serial")
            msg.setWindowTitle("Erro")
            retval = msg.exec_()

    def start_acquisition(self):

        if "None" in str(self.serialPort.port):
            self.serialPort.setPort(str(ArduinoHandler.get_arduino_serial_port()))

        print "trying to open" + str(self.serialPort.port)
        self.open()
        self.thread_acquisition.start()

    def stop_acquisition(self):
        self.thread_acquisition.stop()

    @staticmethod
    def get_arduino_serial_port():
        for serial_port_found in serial_tools.comports():
            if ArduinoConstants.ARDUINODESCRIPTION in serial_port_found.description:
                return str(serial_port_found).split(" ")[0]

        if len(serial_tools.comports()) > 0:
            return str(serial_tools.comports()[0]).split(" ")[0]

        return "/dev/ttyACM0"

    @staticmethod
    def to_int16(msb_byte, lsb_byte):
        return c_short((msb_byte << 8) + lsb_byte).value

    def acquire_routine(self):
        if self.serialPort.isOpen():
            if self.serialPort.inWaiting() > ArduinoConstants.PACKETSIZE:

                _starter_byte = self.serialPort.read()
                if _starter_byte == ArduinoConstants.PACKETSTART:
                    _msb = self.serialPort.read()
                    _lsb = self.serialPort.read()
                    _msb = ord(_msb)
                    _lsb = ord(_lsb)
                    _end_byte = self.serialPort.read()
                    if _end_byte == ArduinoConstants.PACKETEND:

                        _res = self.buffer_acquisition.secure_enqueue(
                            ArduinoHandler.to_int16(_msb, _lsb))
                        if not _res:
                            self.emit(SIGNAL("aquisition_error"))





