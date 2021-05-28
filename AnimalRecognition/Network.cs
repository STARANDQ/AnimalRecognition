using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AnimalRecognition
{
    public class Network
    {
        struct LayerT
        {
            public Vector x;
            public Vector z;
            public Vector df;
        }

        Matrix[] weights;
        LayerT[] L;
        Vector[] deltas;

        int layersN;

        public Network(int[] sizes)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            layersN = sizes.Length - 1;

            weights = new Matrix[layersN];
            L = new LayerT[layersN];
            deltas = new Vector[layersN];

            for (int k = 1; k < sizes.Length; k++)
            {
                weights[k - 1] = new Matrix(sizes[k], sizes[k - 1], random);

                L[k - 1].x = new Vector(sizes[k - 1]);
                L[k - 1].z = new Vector(sizes[k]);
                L[k - 1].df = new Vector(sizes[k]);

                deltas[k - 1] = new Vector(sizes[k]);
            }
        }

        public Network()
        {

        }

        public Vector Forward(Vector input)
        {
            for (int k = 0; k < layersN; k++)
            {
                if (k == 0)
                {
                    for (int i = 0; i < input.length; i++)
                        L[k].x[i] = input[i];
                }
                else
                {
                    for (int i = 0; i < L[k - 1].z.length; i++)
                        L[k].x[i] = L[k - 1].z[i];
                }

                for (int i = 0; i < weights[k].n; i++)
                {
                    double y = 0;

                    for (int j = 0; j < weights[k].m; j++)
                        y += weights[k][i, j] * L[k].x[j];


                    L[k].z[i] = 1 / (1 + Math.Exp(-y));
                    L[k].df[i] = L[k].z[i] * (1 - L[k].z[i]);
                }
            }

            return L[layersN - 1].z;
        }
        void Backward(Vector output, ref double error)
        {
            int last = layersN - 1;

            error = 0;

            for (int i = 0; i < output.length; i++)
            {
                double e = L[last].z[i] - output[i];

                deltas[last][i] = e * L[last].df[i];
                error += e * e / 2;
            }


            for (int k = last; k > 0; k--)
            {
                for (int i = 0; i < weights[k].m; i++)
                {
                    deltas[k - 1][i] = 0;

                    for (int j = 0; j < weights[k].n; j++)
                        deltas[k - 1][i] += weights[k][j, i] * deltas[k][j];

                    deltas[k - 1][i] *= L[k - 1].df[i];
                }
            }
        }

        void UpdateWeights(double alpha)
        {
            for (int k = 0; k < layersN; k++)
            {
                for (int i = 0; i < weights[k].n; i++)
                {
                    for (int j = 0; j < weights[k].m; j++)
                    {
                        weights[k][i, j] -= alpha * deltas[k][i] * L[k].x[j];
                    }
                }
            }
        }
        public void Train(Vector[] X, Vector[] Y, double alpha, double eps, int epochs)
        {
            int epoch = 1;

            double error;

            do
            {
                error = 0;

                for (int i = 0; i < X.Length; i++)
                {
                    Forward(X[i]);
                    Backward(Y[i], ref error);
                    UpdateWeights(alpha);
                }

                epoch++;
            } while (epoch <= epochs && error > eps);
        }
    }

}
