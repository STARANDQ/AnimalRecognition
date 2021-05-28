namespace AnimalRecognition
{
    public class Vector
    {
        public double[] vector;
        public int length;

        public Vector(int n)
        {
            this.length = n;
            vector = new double[n];
        }

        public Vector(params double[] values)
        {
            length = values.Length;
            vector = new double[length];

            for (int i = 0; i < length; i++)
            {
                vector[i] = values[i];
            }
        }

        public double this[int index]
        {
            get { return vector[index]; }
            set { vector[index] = value; }
        }
    }
}
