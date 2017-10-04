using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeSinais
{

    class SignalGenerator
    {
        public enum WaveForm
        {
            Senoidal,
            Quadrada,
            Rampa
        }
        public WaveForm forma_da_onda;
        public double frequencia;
        public double amplitude;
        public UInt64 counter;

        public SignalGenerator(double _frequencia = 1, double _amplitude = 1, WaveForm _forma_da_onda = WaveForm.Senoidal)
        {
            this.forma_da_onda = _forma_da_onda;
            this.frequencia = _frequencia;
            this.amplitude = _amplitude;
            counter = 0;
        }

        #region Setters

        public void setQuadrada()
        {
            forma_da_onda = WaveForm.Quadrada;
        }

        public void setSenoidal()
        {
            forma_da_onda = WaveForm.Senoidal;
        }

        public void setRampa()
        {
            forma_da_onda = WaveForm.Rampa;
        }
        #endregion

        #region Get value by delta time
        public double getValue(double delta_time)
        {
            switch (forma_da_onda)
            {
                case WaveForm.Senoidal:
                    return getSenoidalValue(delta_time);
                case WaveForm.Quadrada:
                    return getSquareValue(delta_time);
                case WaveForm.Rampa:
                    return getRampaValue(delta_time);
                default:
                    return getSenoidalValue(delta_time);
            }
        }

        public double getSenoidalValue(double delta_time)
        {
            double tempo = delta_time * counter;
            counter += 1;

            if(tempo >= 100.0/frequencia)
            {
                counter = 0;
            }
            return amplitude * Math.Sin(2*Math.PI*frequencia * tempo);
        }
        public double getSquareValue(double delta_time)
        {
            double tempo = delta_time * counter;
            counter += 1;

            if (tempo >= 1.0 / frequencia)
            {
                counter = 0;
            }

            return (tempo >= 0.5 / frequencia) ? amplitude : -amplitude;
        }
        public double getRampaValue(double delta_time)
        {
            double tempo = delta_time * counter;
            counter += 1;

            if (tempo >= 1.0 / frequencia)
            {
                counter = 0;
            }
            //se t = 0 -> y = 0
            //se t = T -> y = A

            return tempo*frequencia * (double) amplitude;
        }
        #endregion

        #region Get value by delta angle
        public double getValue_angle(double delta_rad)
        {
            switch (forma_da_onda)
            {
                case WaveForm.Senoidal:
                    return getSenoidalValue_angle(delta_rad);
                case WaveForm.Quadrada:
                    return getSquareValue_angle(delta_rad);
                case WaveForm.Rampa:
                    return getRampaValue_angle(delta_rad);
                default:
                    return getSenoidalValue_angle(delta_rad);
            }
        }

        public double getSenoidalValue_angle(double delta_rad)
        {
            double angulo;
            angulo = counter * delta_rad;
            counter += 1;

            //Qnt de pontos por ciclo = Fsample / Fsignal
            if (angulo >= 2*Math.PI)
            {
                counter = 0;
                angulo = 0;
            }
            return amplitude * Math.Sin(angulo);
        }
        public double getSquareValue_angle(double delta_rad)
        {
            double angulo;
            angulo = counter * delta_rad;
            counter += 1;

            //Qnt de pontos por ciclo = Fsample / Fsignal
            if (angulo >= 2 * Math.PI)
            {
                counter = 0;
                angulo = 0;
            }

            return (angulo >= Math.PI) ? amplitude : -amplitude;
        }
        public double getRampaValue_angle(double delta_rad)
        {
            double angulo;
            angulo = counter * delta_rad;
            counter += 1;

            //Qnt de pontos por ciclo = Fsample / Fsignal
            if (angulo >= 2 * Math.PI)
            {
                counter = 0;
                angulo = 0;
            }


            return (angulo / (2 * Math.PI)) * (double) amplitude;
        }
        #endregion
    }
   
}
