using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorContracao
{
    public class Janela
    {
        int tamanho;
        public double[] dados;

        public Janela(int qnt_de_pontos = 512) //construtor
        {
            tamanho = qnt_de_pontos;
            dados = new double[tamanho];
        }

        public void FillWith(double item)
        {
            dados = Enumerable.Repeat(item, tamanho).ToArray();
        }
        
        public void AddPoints(double[] points){
            //Desloca os Pontos atuais para a esquerda - em y e x
            for (int i = 0; i < (tamanho - points.Length); i++)
            {
                dados[i] = dados[i + points.Length];
            }

            //Adiciona os novos pontos no novo espaço a direita - em y e x
            for (int i = (tamanho - points.Length); i < tamanho; i++)
            {
                dados[i] = points[i  - (tamanho - points.Length)];
            }
        }
        public void AddPoint(double point)
        {
            AddPoints(new double[] { point });
        }
    }
}
