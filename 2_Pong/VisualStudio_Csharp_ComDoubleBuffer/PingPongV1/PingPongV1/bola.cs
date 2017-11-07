using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PingPongV1 {
    public class bola {
        #region Propriedades
        public enum lados {
            UP,
            DOWN,
            LEFT,
            RIGHT
        } //Usada para verificar colisoes
        public Point centro; //Centro da bola
        private int raio; //Raio da bola
        private bool visible; //Visibilidade
        public Pen pen; //Cor e espessura
        public Graphics graphics; //Onde sera plotada
        public PointF velocidade; //Velocidade de movimento em px/s
        #endregion

        #region Construtor
        public bola(Point _centro, int _raio, PointF _velocidade, Pen _pen, Graphics _graphics, bool _visible = false) {
            centro = _centro;
            raio = _raio;
            velocidade = _velocidade;
            pen = _pen;
            graphics = _graphics;
            visible = _visible;
        }
        #endregion

        #region Aparecer na Interface
        public void show() {
            if (visible) {
                this.graphics.FillEllipse(pen.Brush, centro.X - raio/2, centro.Y - raio/2, raio, raio);
            }
        }
        public void clear() {
            //this.graphics.Clear(Color.Green);
            this.graphics.FillEllipse(Brushes.Green, centro.X - raio / 2, centro.Y - raio / 2, raio, raio);
        }
        #endregion

        #region Movimentar
        public void setVelocidade(double _valor, double _theta) {
            velocidade.X = Convert.ToInt32(_valor * Math.Cos(_theta));
            velocidade.Y = Convert.ToInt32(_valor * Math.Sin(_theta));
        }

        public void move_relative(Point _rel_location) {
            //this.clear();
            centro.X = centro.X + _rel_location.X;
            centro.Y = centro.Y + _rel_location.Y;
            //this.show();
        }

        public void move_time_step(double _time_step) {
            //this.clear();
            centro.X = centro.X + Convert.ToInt32( velocidade.X * _time_step);
            centro.Y = centro.Y + Convert.ToInt32( velocidade.Y * _time_step);
            //this.show();
        }
        #endregion
        
        #region Colisões
        public void bounce(lados _onde_bateu) {
            if (velocidade.X - velocidade.Y > 200) {
                velocidade.X -= 30;
            }
            switch (_onde_bateu) {
                case lados.UP:
                velocidade.X = velocidade.X;
                velocidade.Y = -velocidade.Y;
                break;
                case lados.DOWN:
                velocidade.X = velocidade.X;
                velocidade.Y = -velocidade.Y;
                break;
                case lados.LEFT:
                velocidade.X = -velocidade.X;
                velocidade.Y = velocidade.Y;
                break;
                case lados.RIGHT:
                velocidade.X = velocidade.X;
                velocidade.Y = -velocidade.Y; 
                break;
            }
        }
        public void verify_collision_up_down(int _y, int _altura) {
            if((centro.Y - this.raio/2) < _y) {
                bounce(lados.UP);
            }
            if((centro.Y + this.raio/2) > _altura) {
                bounce(lados.DOWN);
            }
        }
        public void verify_collision_player_left(Point origem, Point final) {
            if (centro.Y < final.Y && centro.Y > origem.Y) { //Verifica altura
                if ((centro.X - this.raio/2) < final.X) {
                    velocidade.X = velocidade.X * (float)1.1;
                    velocidade.Y = velocidade.Y * (float)1.1;
                    bounce(lados.LEFT);
                    centro.X = final.X + this.raio / 2;
                }
            }
        }
        public void verify_collision_player_right(Point origem, Point final) {
            if (centro.Y < final.Y && centro.Y > origem.Y) { //Verifica altura
                if ((centro.X + this.raio / 2) > origem.X) {
                    velocidade.X = velocidade.X * (float)1.1;
                    velocidade.Y = velocidade.Y * (float)1.1;
                    bounce(lados.LEFT);
                    centro.X = origem.X - this.raio / 2;
                }
            }
        }
        public int verify_collision_left_right(int _x, int _largura) {
            if ((centro.X - this.raio / 2) < _x) {
                return 1;
            }
            if ((centro.X + this.raio / 2) > _largura) {
                return -1;
            }
            return 0;
        }
        #endregion

        #region Is Inside - Not Used
        public bool x_inside(int _x) {
            return (Math.Abs(_x - centro.X) < raio);
        }
        public bool y_inside(int _y) {
            return (Math.Abs(_y - centro.Y) < raio);
        }

        public bool is_inside(Point _p) {
            double distancia_do_centro;
            distancia_do_centro = Math.Pow((_p.X - centro.X), 2) + Math.Pow((_p.Y - centro.Y), 2);
            if(distancia_do_centro < Math.Pow(raio,2)) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
    }
}
