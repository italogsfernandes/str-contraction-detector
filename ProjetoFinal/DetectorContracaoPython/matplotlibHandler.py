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
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.animation as animation
from matplotlib.lines import Line2D

from ThreadHandler import InfiniteTimer
from Queue import Queue
# ------------------------------------------------------------------------------


class PyPlotHandler:
    def __init__(self, qnt_pontos=10, axis_lim = [0,10,0,5]):
        self.qnt_pontos = qnt_pontos
        self.x_axis_lim = axis_lim[:2]
        self.y_axis_lim = axis_lim[2:]
        # self.x_values = [float(n)/float(self.x_axis_lim[1]) for n in range(self.qnt_pontos)]
        self.x_values = range(self.qnt_pontos)
        self.y_values = [0] * self.qnt_pontos
        self.plot_buffer = Queue(qnt_pontos)
        self.fig = plt.figure()
        self.ax = self.fig.gca()
        self.line = Line2D(self.x_values, self.y_values)
        self.ax.add_line(self.line)
        self.configure_plot()
        self.ani = animation.FuncAnimation(self.fig, self.update_y_points,
         init_func=self.init_animation, interval=25, blit=True)

    def configure_plot(self, title="Plot",x_label="Points",y_label="Values"):
        self.ax.set_xlim(self.x_axis_lim)
        self.ax.set_ylim(self.y_axis_lim)
        self.ax.set_title(title)
        self.ax.set_xlabel(x_label)
        self.ax.set_ylabel(y_label)
        self.ax.grid()
        for item in [self.fig, self.ax]:
            item.patch.set_visible(True) #fundo

    def init_animation(self):
        # Init only required for blitting to give a clean slate.
        self.line.set_ydata(np.ma.array(self.x_values, mask=True))
        return self.line,

    def put(self, item):
        self.plot_buffer.put(item)

    def update_y_points(self, frame_counter):
        points_to_add = self.plot_buffer.qsize()
        if points_to_add > 0:
            for n in range(points_to_add): # obtains the new values
                num = self.plot_buffer.get()
                self.y_values.append(num)
                if len(self.y_values) > self.qnt_pontos:
                    self.y_values.pop(0)
            self.line.set_ydata(self.y_values)  # update the data
        return self.line,

    def update_points_as_osciloscope(self, frame_counter):
        raise NotImplementedError
        points_to_add = self.plot_buffer.qsize()
        if points_to_add > 0:
            for n in range(points_to_add): # obtains the new values
                par = self.plot_buffer.get()
                self.x_values.append(par[0])
                self.y_values.append(par[1])
                if len(self.x_values) > self.qnt_pontos:
                    self.x_values.pop(0)
                if len(self.y_values) > self.qnt_pontos:
                    self.y_values.pop(0)
            self.line.set_ydata(self.y_values)  # update the data
            self.line.set_xdata(self.x_values)  # update the data
            # Sometimes you just don't know
            '''
            lastx = self.x_values[-1]
            if lastx > self.x_values[0] + self.axis_lim[1]:  # reset the arrays
                self.x_values = [lastx + n/self.qnt_pontos for n in range(self.qnt_pontos)]
                self.ax.set_xlim(self.x_values[0], self.x_values[-1])
                self.ax.figure.canvas.draw()
            '''

        return self.line,

    def update_xy_points(self, frame_counter):
        raise NotImplementedError
        '''
        points_to_add = self.plot_buffer.qsize()
        if points_to_add > 0:
            for n in range(points_to_add): # obtains the new values
                par = self.plot_buffer.get()
                self.x_values.append(par[0])
                self.y_values.append(par[1])
                if len(self.x_values) > self.qnt_pontos:
                    self.x_values.pop(0)
                if len(self.y_values) > self.qnt_pontos:
                    self.y_values.pop(0)

            self.line.set_xdata(self.x_values)  # update the x data
            self.line.set_ydata(self.y_values)  # update the data

            # new_xlim = [math.floor(self.x_values[1]*2)/2.0,
            #  math.ceil(self.x_values[len(self.x_values)-1]*2)/2.0]
            # if new_xlim[1] > self.ax.get_xlim()[1]:
            #    self.ax.set_xlim(new_xlim)
            #    self.ax.figure.canvas.draw()
            # print 'Limits: ' + str([self.x_values[0], self.x_values[len(self.x_values)-1]])

        return self.line,
        '''

    def show(self):
        plt.show()

    def get_buffers_status(self):
        return "Plot: %4d" % (self.plot_buffer.qsize()) + '/' + str(self.plot_buffer.maxsize)

if __name__ == '__main__':
    my_plot = PyPlotHandler(50, [0, 50, 0, 1])

    from datetime import datetime

    def generate_point():
        agr = datetime.now()
        y_value = agr.microsecond / 1000000.0
        my_plot.put(y_value)
        print y_value

    timer = InfiniteTimer(0.1, generate_point)
    timer.start()

    plt.show()
    timer.stop()
