using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalRecognition
{
    class Matrix
    {
        double[][] v;
        public int n, m;

        public Matrix(int n, int m, Random random)
        {
            this.n = n;
            this.m = m;

            v = new double[n][];

            for (int i = 0; i < n; i++)
            {
                v[i] = new double[m];

                for (int j = 0; j < m; j++)
                {
                    v[i][j] = random.NextDouble() - 0.5;
                }
            }
        }

        public double this[int i, int j]
        {
            get { return v[i][j]; }
            set { v[i][j] = value; }
        }
    }
}
