# -*- coding: utf-8 -*-
# FEDERAL UNIVERSITY OF UBERLANDIA
# Faculty of Electrical Engineering
# Author: Italo Gustavo Sampaio Fernandes
# Contact: italogsfernandes@gmail.com
# Git: www.github.com/italogfernandes
# Decription:

from PyQt4.QtCore import *
from PyQt4.QtGui import *
from italopaint_mainwindow import Ui_MainWindow


class PAINT_TOOLS():
    PENCIL = 0
    RECTANGLE = 1
    CIRCLE = 2


class Main(QMainWindow, Ui_MainWindow):
    def __init__(self):
        super(Main, self).__init__()
        self.setupUi(self)

        #Iniciando Variaveis da Classe
        self.actual_tool = None
        self.scene = QGraphicsScene(self)
        self.sceneMainColor = QGraphicsScene(self)
        self.sceneBackColor = QGraphicsScene(self)
        self.mainColor = QBrush(QColor.fromRgb(0, 0, 0))
        self.backColor = QBrush(QColor.fromRgb(255, 255, 255))
        self.color_dialog = QColorDialog()
        self.back_color_dialog = QColorDialog()
        self.mainPen = QPen(Qt.black)

        #Eventos e Configuracoes Iniciais
        self.connect_events()
        self.initiate_first_configs()

    def connect_events(self):
        # Tools events
        self.btnPencil.clicked.connect(self.do_pencil_selected)
        self.btnRectangle.clicked.connect(self.do_rectangle_selected)
        self.btnCircle.clicked.connect(self.do_circle_selected)

        # Options Events
        self.sliderEspessura.valueChanged.connect(self.do_change_espessura)
        self.btnClear.clicked.connect(self.do_clear)
        self.btnSelectColor.clicked.connect(self.do_color_selection)
        self.btnSelectBackColor.clicked.connect(self.do_back_color_selection)

        # Mouse Events
        '''
        self.graphicsView.mouseMoveEvent()
        self.graphicsView.mousePressEvent()
        self.graphicsView.mouseReleaseEvent()
        self.graphicsViewMainColor.mousePressEvent()
        self.graphicsViewBackColor.mousePressEvent()
        '''

    def initiate_first_configs(self):
        self.graphicsView.setScene(self.scene)
        self.graphicsViewMainColor.setScene(self.sceneMainColor)
        self.graphicsViewBackColor.setScene(self.sceneBackColor)
        self.sceneMainColor.setBackgroundBrush(self.mainColor)
        self.sceneBackColor.setBackgroundBrush(self.backColor)
        self.color_dialog.setCurrentColor(self.mainColor.color())
        self.back_color_dialog.setCurrentColor(self.backColor.color())
        self.scene.setBackgroundBrush(self.backColor)
        self.sliderEspessura.setValue(1)

    def do_change_espessura(self, new_value):
        self.labelTitleEspessura.setText("Espessura: %i px" % new_value)
        self.mainPen.setWidth(new_value)

    def do_graph_mouse_move(self):
        self.labelStatusMessage.setText("Mouse move.")

    def do_graph_mouse_press(self):
        self.labelStatusMessage.setText("Mouse press.")

    def do_graph_mouse_release(self):
        self.labelStatusMessage.setText("Mouse release.")

    def do_color_selection(self):
        retval = self.color_dialog.exec_()
        if retval:
            self.mainColor.setColor(self.color_dialog.selectedColor())
            self.sceneMainColor.setBackgroundBrush(self.mainColor)

    def do_back_color_selection(self):
        retval = self.back_color_dialog.exec_()
        if retval:
            self.backColor.setColor(self.back_color_dialog.selectedColor())
            self.sceneBackColor.setBackgroundBrush(self.backColor)

    def do_clear(self):
        self.labelStatusMessage.setText("Tela limpa.")
        self.scene.setBackgroundBrush(self.backColor)
        self.scene.clear()

    def do_circle_selected(self):
        self.actual_tool = PAINT_TOOLS.CIRCLE
        self.labelStatusMessage.setText("Circulo Selecionado.")

    def do_rectangle_selected(self):
        self.actual_tool = PAINT_TOOLS.RECTANGLE
        self.labelStatusMessage.setText("Retangulo Selecionado.")

    def do_pencil_selected(self):
        self.actual_tool = PAINT_TOOLS.PENCIL
        self.labelStatusMessage.setText("Lapis Selecionado.")

    def show_error_msg(self, msg_to_show):
        msg = QMessageBox()
        msg.setIcon(QMessageBox.Warning)
        msg.setText(msg_to_show)
        msg.setWindowTitle("Erro")
        msg.exec_()

    def show_info_msg(self, msg_to_show):
        msg = QMessageBox()
        msg.setIcon(QMessageBox.Information)
        msg.setText(msg_to_show)
        msg.setWindowTitle("Mensagem Info")
        msg.exec_()


if __name__ == '__main__':
    import sys
    from PyQt4 import QtGui

    app = QtGui.QApplication(sys.argv)
    main = Main()
    main.show()
    sys.exit(app.exec_())
