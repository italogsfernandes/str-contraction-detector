using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPongV1 {
    public partial class Form1 : Form {

        #region Public Properties

        #region Itens da interface
        public Graphics drawingGraphic; //Area de desenho
        public goleiro[] players; //Paddles
        public bola main_bola; //Bola
        public Graphics double_buffer; //Graphics q manipula o bitmap de buffer
        public Bitmap aux_buffer; //Bitmap utilizado como buffer
        #endregion

        #region Controles dos players
        public bool mouse_is_down;
        public bool controlling_player_1;
        public Point mouse_position;
        public bool w_pressed, s_pressed, up_pressed, down_pressed;
        #endregion

        #region Regras de jogo
        public int end_id;
        public int[] pontos;
        public Random random;
        public int ultimo_ganhador;
        #endregion

        #endregion

        #region Construtor e Inicializadores

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

            #region Iniciando variaveis de controle e interface
            mouse_is_down = false;
            controlling_player_1 = true;
            w_pressed = false;
            s_pressed = false;
            up_pressed = false;
            down_pressed = false;
            mouse_position = new Point(0, 0);

            random = new Random(DateTime.Now.Millisecond);
            pontos = new int[2];

            label1.Visible = false;
            drawingGraphic = panel1.CreateGraphics();
            aux_buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            double_buffer = Graphics.FromImage(aux_buffer);
            #endregion


            #region Instanciando os Itens do jogo
            players = new goleiro[2];
            players[0] = new goleiro(
                new Point(18, panel1.Size.Height/2 - 100),
                new Point(38, panel1.Size.Height/2 + 100),
                new Pen(Color.Red,10),
                double_buffer, panel1.Size,true);
            players[1] = new goleiro(
                new Point(panel1.Size.Width - 40, panel1.Size.Height / 2 - 100),
                new Point(panel1.Size.Width - 20, panel1.Size.Height / 2 + 100),
                new Pen(Color.Blue, 10),
                double_buffer, panel1.Size, true);
            main_bola = new bola(new Point(panel1.Size.Width / 2, panel1.Size.Height / 2),
                50,
                new PointF(100,10),
                new Pen(Color.Yellow, 10),
                double_buffer,
                true);
            #endregion

            ultimo_ganhador = random.Next(0, 1);
            resetar_jogo();
            show_items();
        }

        private void resetar_jogo() {
            double angulo_inicio = Convert.ToDouble(random.Next(30,150)); //Angulo randomico entre 30 e 150
            angulo_inicio = angulo_inicio * Math.PI / 180; //Convert para Rad
            angulo_inicio = angulo_inicio + Math.PI / 2;
            angulo_inicio += ultimo_ganhador == 1? 0 : Math.PI;

            main_bola.setVelocidade(300, angulo_inicio);
            main_bola.centro = new Point(panel1.Size.Width / 2, panel1.Size.Height / 2);
        }

        void resetar_players() {
            players[0].origem = new Point(25, panel1.Size.Height / 2 - 50);
            players[0].final = new Point(50, panel1.Size.Height / 2 + 50);
            players[1].origem = new Point(panel1.Size.Width - 50, panel1.Size.Height / 2 - 50);
            players[1].final = new Point(panel1.Size.Width - 25, panel1.Size.Height / 2 + 50);
        }

        #endregion

        #region Event Handlers

        #region Menu Strip

        private void startToolStripMenuItem_Click(object sender, EventArgs e) {
            timerUpdateBall.Enabled = true;
            timerUpdateBall.Start();
            label1.Visible = false;
            label1.Text = "Ponto!";
            resetar_jogo();
        }
        private void sobreToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            try {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "https://github.com/italogfernandes/str";
                myProcess.Start();
            } catch (Exception exc) {
                Console.WriteLine(exc.Message);
            }
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            try {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "http://www.google.com/";
                myProcess.Start();
            } catch (Exception exc) {
                Console.WriteLine(exc.Message);
            }
        }
        #endregion

        #region Timers
        private void timerUpdateBall_Tick(object sender, EventArgs e) {
            mover_elementos();
            verificar_colisoes();
            verificar_pontuacao();
            show_items();
        }

        private void timerEspera_Tick(object sender, EventArgs e) {
            label1.Visible = false;
            timerUpdateBall.Start();
            timerEspera.Stop();
        }

        #endregion

        #region Controles

        private void panel1_MouseDown(object sender, MouseEventArgs e) {
            mouse_is_down = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e) {
            mouse_position.X = e.Location.X;
            mouse_position.Y = e.Location.Y;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e) {
            mouse_is_down = false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.W:
                w_pressed = false;
                break;
                case Keys.S:
                s_pressed = false;
                break;
                case Keys.Up:
                up_pressed = false;
                break;
                case Keys.Down:
                down_pressed = false;
                break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            //lbPlacar.Text = e.KeyCode.ToString();
            switch (e.KeyCode) {
                case Keys.W:
                w_pressed = true;
                break;
                case Keys.S:
                s_pressed = true;
                break;
                case Keys.Up:
                up_pressed = true;
                break;
                case Keys.Down:
                down_pressed = true;
                break;
                case Keys.Enter:
                timerUpdateBall.Enabled = true;
                timerUpdateBall.Start();
                label1.Visible = false;
                label1.Text = "Ponto!";
                resetar_jogo();
                break;
            }
        }
        #endregion

        #endregion

        #region Metodos Auxiliares

        private void show_items() {
            double_buffer.Clear(Color.Green);
            double_buffer.DrawImage(Properties.Resources.fundo2, new Point(0, 0));
            players[0].show();
            players[1].show();
            main_bola.show();
            pictureBox1.Image = aux_buffer;
        }

        private void verificar_colisoes() {
            main_bola.verify_collision_up_down(0, panel1.Size.Height);
            main_bola.verify_collision_player_left(players[0].origem, players[0].final);
            main_bola.verify_collision_player_right(players[1].origem, players[1].final);
        }

        private void verificar_pontuacao() {
            end_id = main_bola.verify_collision_left_right(0, panel1.Size.Width);
            if (end_id != 0) {
                if (end_id == 1) {
                    pontos[1] += 1;
                    ultimo_ganhador = 1;
                    label1.ForeColor = Color.Blue;
                } else if (end_id == -1) {
                    pontos[0] += 1;
                    ultimo_ganhador = 0;
                    label1.ForeColor = Color.Red;
                }

                if (pontos[0] >= 5 || pontos[1] >= 5) {
                    label1.Text = (pontos[0] >= 10) ? "Azul Venceu!" : "Vermelho Venceu!";
                    label1.Visible = true;
                    timerUpdateBall.Stop();
                } else {
                    label1.Text = pontos[0].ToString() + " vs " + pontos[1].ToString();
                    label1.Visible = true;
                    timerEspera.Start();
                    timerUpdateBall.Stop();
                    update_label_pontos();
                    resetar_jogo();
                }
            }
        }

        private void move_players_with_keyboard() {
            if (w_pressed) {
                players[0].move_time_step(timerUpdateBall.Interval / 1000.0, -500);
            }
            if (s_pressed) {
                players[0].move_time_step(timerUpdateBall.Interval / 1000.0, 500);
            }
            if (up_pressed) {
                players[1].move_time_step(timerUpdateBall.Interval / 1000.0, -500);
            }
            if (down_pressed) {
                players[1].move_time_step(timerUpdateBall.Interval / 1000.0, 500);
            }
        }

        private void move_players() {
            controlling_player_1 = (mouse_position.X < panel1.Size.Width / 2);
            players[controlling_player_1 ? 0 : 1].move_absolute(mouse_position.Y);
        }

        private void mover_elementos() {
            if (mouse_is_down) {
                move_players();
            }
            move_players_with_keyboard();
            main_bola.move_time_step(timerUpdateBall.Interval / 1000.0);
        }

        private void update_label_pontos() {
            lbPontos1.Text = pontos[0].ToString();
            lbPontos2.Text = pontos[1].ToString();
        }

        #endregion
    }
}