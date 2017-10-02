using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PingPongV1 {
    public class goleiro {
        #region Propriedades
        public Point origem;
        public Point final;
        private bool visible;
        public Pen pen;
        public Graphics graphics;
        private Point dimensions;
        public Size tamanho_campo;
        #endregion

        #region Construtor
        public goleiro(Point _origem,Point _final,Pen _pen, Graphics _graphics, Size _tamanho_campo, bool _visible = false) {
            origem = _origem;
            final = _final;
            visible = _visible;
            pen = _pen;
            graphics = _graphics;
            dimensions = new Point(final.X - origem.X, final.Y - origem.Y);
            tamanho_campo = _tamanho_campo;
        }
        #endregion

        #region Aparecer na Interface
        public void show() {
            if (visible) {   
                graphics.FillRectangle(pen.Brush, origem.X, origem.Y, final.X - origem.X, final.Y - origem.Y);
            }
        }
        public void clear() {
            //this.graphics.Clear(Color.Green);
            this.graphics.FillRectangle(Brushes.Green, origem.X, origem.Y, final.X - origem.X, final.Y - origem.Y);
        }
        #endregion

        #region Movimentar
        public void move_relative(Point _rel_location) {
            //this.clear();
            origem.X = origem.X + _rel_location.X;
            origem.Y = origem.Y + _rel_location.Y;
            final.X = final.X + _rel_location.X;
            final.Y = final.Y + _rel_location.Y;
            //this.show();
        }
        public void move_absolute(Point _location) {
            //this.clear();
            origem.X = _location.X - dimensions.X/2;
            origem.Y = _location.Y - dimensions.Y/2;
            final.X = _location.X + dimensions.X/2;
            final.Y = _location.Y + dimensions.Y/2;
            //this.show();
        }

        public void move_absolute(int _y) {
            //this.clear();
            origem.Y = _y - dimensions.Y / 2;
            final.Y = _y + dimensions.Y / 2;

            if(origem.Y < 0) {
                origem.Y = 0;
                final.Y = dimensions.Y;
            }
            if (final.Y > tamanho_campo.Height) {
                origem.Y = tamanho_campo.Height - dimensions.Y;
                final.Y = tamanho_campo.Height;
            }
            //this.show();
        }

        public void move_time_step(double _time_step , int velocidade) {

            origem.Y = origem.Y + Convert.ToInt32(velocidade * _time_step);
            final.Y = origem.Y + dimensions.Y;

            if (origem.Y < 0) {
                origem.Y = 0;
                final.Y = dimensions.Y;
            }
            if (final.Y > tamanho_campo.Height) {
                origem.Y = tamanho_campo.Height - dimensions.Y;
                final.Y = tamanho_campo.Height;
            }
        }
        #endregion

    }
}
