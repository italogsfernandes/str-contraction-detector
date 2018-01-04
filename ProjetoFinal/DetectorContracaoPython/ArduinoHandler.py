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
from ctypes import c_short

from ThreadHandler import ThreadHandler, InfiniteTimer
from CircularQueue import CircularQueue
from Queue import Queue


class ArduinoConstants():
    PACKET_SIZE = 4
    PACKET_START = '$'
    PACKET_END = '\n'
    MANUFACTURER = 'Arduino (www.arduino.cc)'
    DUE_description = 'Arduino Due Prog. Port'
    UNO_description = 'ttyACM1'
    HINA_NANO_description = 'USB2.0-Serial'
    DUE_manufacturer = 'Arduino (www.arduino.cc)'
    UNO_manufacturer = 'Arduino (www.arduino.cc)'
    CHINA_NANO_manufacture = ''
    DUE_product = 'Arduino Due Prog. Port'
    UNO_product = ''
    CHINA_NANO_product = 'USB2.0-Serial'


class ArduinoHandler():
    def __init__(self, port_name=None, baudrate=115200):
        if port_name is None:
            port_name = ArduinoHandler.get_arduino_serial_port()
        self.serial_tools_obj = [s for s in serial_tools.comports() if s.device == port_name][0]
        self.serialPort = serial.Serial()
        self.serialPort.port = port_name
        self.serialPort.baudrate = baudrate
        self.thread_acquisition = ThreadHandler(self.acquire_routine,self.close)
        self.buffer_acquisition = Queue(1024)

    def data_waiting(self):
        return self.buffer_acquisition.qsize()

    def open(self):
        if not self.serialPort.isOpen():
            self.serialPort.open()
            self.serialPort.flushInput()
            self.serialPort.flushOutput()

    def close(self):
        if self.serialPort.isOpen():
            self.serialPort.close()

    def start_acquisition(self):
        self.open()
        self.thread_acquisition.start()

    def stop_acquisition(self):
        self.thread_acquisition.stop()

    @staticmethod
    def get_arduino_serial_port():
        serial_ports = serial_tools.comports()
        if len(serial_ports) == 0:
            return ""
        if len(serial_ports) == 1:
            return serial_ports[0].device
        for serial_port_found in serial_ports:
            if serial_port_found.manufacturer == ArduinoConstants.MANUFACTURER:
                return serial_port_found.device

    @staticmethod
    def to_int16(msb_byte, lsb_byte):
        return c_short((msb_byte << 8) + lsb_byte).value

    def acquire_routine(self):
        if self.serialPort.isOpen():
            if self.serialPort.inWaiting() > ArduinoConstants.PACKET_SIZE:
                _starter_byte = self.serialPort.read()
                if _starter_byte == ArduinoConstants.PACKET_START:
                    _msb = self.serialPort.read()
                    _lsb = self.serialPort.read()
                    _msb = ord(_msb)
                    _lsb = ord(_lsb)
                    _end_byte = self.serialPort.read()
                    if _end_byte == ArduinoConstants.PACKET_END:
                        self.buffer_acquisition.put(ArduinoHandler.to_int16(_msb, _lsb))

    def __str__(self):
        return "ArduinoHandlerObject" +\
               "\n\tSerialPort: " + str(self.serial_tools_obj.device) +\
               "\n\tDescription: " + str(self.serial_tools_obj.description) +\
               "\n\tOpen: " + str(self.serialPort.isOpen()) +\
               "\n\tAcquiring: " + str(self.thread_acquisition.isRunning) +\
               "\n\tInWaiting: " + str(self.serialPort.inWaiting() if self.serialPort.isOpen() else 'Closed') +\
               "\n\tBufferAcq: " + str(self.buffer_acquisition.qsize())

    def get_buffers_status(self, separator):
        return "Port: " +\
               str(self.serialPort.inWaiting() if self.serialPort.isOpen() else '-') + '/' + str(4096) +\
               separator + "Acq: " +\
               str(self.buffer_acquisition.qsize()) + '/' + str(self.buffer_acquisition.maxsize)

if __name__ == '__main__':
    myArduinoHandler = ArduinoHandler()

    def consumer():
        if myArduinoHandler.data_waiting():
            print myArduinoHandler.buffer_acquisition.get()
            # time.sleep(0.01) # Uncomment if you want to see the buffer_acquisition to get full

    consumer_thr = ThreadHandler(consumer)

    def show_status():
        print myArduinoHandler
    status_timer = InfiniteTimer(0.5, show_status)
    while True:
        print '-------------------------------'
        print myArduinoHandler
        print '-------------------------------'
        print 'Menu'
        print '-------------------------------'
        print 'start - Automatically Starts Everything'
        print 'stop - Automatically Stops Everything'
        print 'q - Quit'
        print '-------------------------------'
        print 'op - open() '
        print 'cl - close()'
        print 'ra - readall()'
        print 'sth - start Consumer'
        print 'pth - pause Consumer'
        print 'rth - resume Consumer'
        print 'kth - kill Consumer'
        print 'sacq - start Aquisition'
        print 'kacq - kill Aquisition'
        print '-------------------------------'
        str_key = raw_input()
        if 'q' in str_key:
            myArduinoHandler.stop_acquisition()
            consumer_thr.stop()
            status_timer.stop()
            break
        elif 'op' in str_key:
            myArduinoHandler.open()
        elif 'cl' in str_key:
            myArduinoHandler.close()
        elif 'ra' in str_key:
            print myArduinoHandler.serialPort.read_all()
        elif 'sth' in str_key:
            status_timer.start()
            consumer_thr.start()
        elif 'pth' in str_key:
            consumer_thr.pause()
        elif 'rth' in str_key:
            consumer_thr.resume()
        elif 'kth' in str_key:
            status_timer.stop()
            consumer_thr.stop()
        elif 'sacq' in str_key:
            status_timer.start()
            myArduinoHandler.start_acquisition()
        elif 'kacq' in str_key:
            status_timer.stop()
            myArduinoHandler.stop_acquisition()
        elif 'start' in str_key:
            status_timer.start()
            consumer_thr.start()
            myArduinoHandler.start_acquisition()
        elif 'stop' in str_key:
            status_timer.stop()
            myArduinoHandler.stop_acquisition()
            consumer_thr.stop()


