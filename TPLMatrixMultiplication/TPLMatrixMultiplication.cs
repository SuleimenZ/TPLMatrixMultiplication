using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;

namespace TPLMatrixMultiplication
{
    internal class TPLMatrixMultiplication
    {
        private int _row1;
        private int _row2;
        private int _col1;
        private int _col2;

        public int Row1 
        { 
            get { return _row1; } 
            set { _row1 = value >= 0 ? value : _row1; }
        }
        public int Col1
        {
            get { return _col1; }
            set { _col1 = value >= 0 ? value : _col1; }
        }
        public int Row2
        {
            get { return _row2; }
            set { _row2 = value >= 0 ? value : _row2; }
        }
        public int Col2
        {
            get { return _col2; }
            set { _col2 = value >= 0 ? value : _col2; }
        }

        private int defaultSize = 500;

        //These variable represent range of values when filling up matrices.
        private int upperBound = 100;
        private int lowerBound = -100;

        public Matrix<double>? Matrix1 { get; set; }
        public Matrix<double>? Matrix2 { get; set; }
        public Matrix<double>? ResultMatrix { get; private set; }

        private double[,]? matrixArray1;
        private double[,]? matrixArray2;
        private double[,]? resultArray;

        public TPLMatrixMultiplication()
        {
            //setting up matrices dimensions
            Row1 = defaultSize;
            Row2 = defaultSize;
            Col1 = defaultSize;
            Col2 = defaultSize;

            RandomizeMatricies();
        }

        public TPLMatrixMultiplication(int row1, int col1, int col2)
        {
            //setting up matrices dimensions
            Row1 = row1;
            Row2 = col1;
            Col1 = col1;
            Col2 = col2;

            RandomizeMatricies();
        }

        public void SerialForMultiplication()
        {
            if (Col1 != Row2)
            {
                Console.WriteLine("Incorrect input. Make sure col1 = row2");
                return;
            }

            //calculating
            for (int row1 = 0; row1 < this.Row1; row1++)
            {
                for (int col2 = 0; col2 < this.Col2; col2++)
                {
                    //temp value will further increase speed as calling array takes more time
                    double temp = 0;
                    for (int row2 = 0; row2 < this.Row2; row2++)
                    {
                        temp += matrixArray1[row1, row2] * matrixArray2[row2, col2];
                    }
                    resultArray[row1, col2] = temp;
                }
            }

            //storing result to separate variable
            ResultMatrix = Matrix<double>.Build.DenseOfArray(resultArray);
        }

        public void SerialWhileMultiplication()
        {
            if (Col1 != Row2)
            {
                Console.WriteLine("Incorrect input. Make sure col1 = row2");
                return;
            }

            //calculating
            int row1 = 0;
            while (row1 < this.Row1)
            {
                int col2 = 0;
                while(col2 < this.Col2)
                {
                    int row2 = 0;
                    //temp value will further increase speed as calling array takes more time
                    double temp = 0;
                    while(row2 < this.Row2)
                    {
                        temp += matrixArray1[row1, row2] * matrixArray2[row2, col2];
                        row2++;
                    }
                    resultArray[row1, col2] = temp;
                    col2++;
                }
                row1++;
            }

            //storing result to separate variable
            ResultMatrix = Matrix<double>.Build.DenseOfArray(resultArray);
        }

        public void MathNetMultiplication()
        {
            ResultMatrix = Matrix1.Multiply(Matrix2);
        }

        public void ParallelForMultiplication(int threads)
        {
            if (Col1 != Row2)
            {
                Console.WriteLine("Incorrect input. Make sure col1 = row2");
                return;
            }

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = threads
            };

            Parallel.For(0, Row1, options, row1 =>
            {
                for (int col2 = 0; col2 < Col2; col2++)
                {
                    double temp = 0;
                    for (int row2 = 0; row2 < Row2; row2++)
                    {
                        temp += matrixArray1[row1, row2] * matrixArray2[row2, col2];
                    }
                    resultArray[row1, col2] = temp;
                }
            });

            ResultMatrix = Matrix<double>.Build.DenseOfArray(resultArray);
        }

        public void ParallelWhileMultiplication(int threads)
        {
            if (Col1 != Row2)
            {
                Console.WriteLine("Incorrect input. Make sure col1 = row2");
                return;
            }

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = threads
            };

            Parallel.For(0, Row1, options, row1 =>
            {
                int col2 = 0;
                while (col2 < Col2)
                {
                    double temp = 0;
                    int row2 = 0;
                    while (row2 < Row2)
                    {
                        temp += matrixArray1[row1, row2] * matrixArray2[row2, col2];
                        row2++;
                    }
                    resultArray[row1, col2] = temp;
                    col2++;
                }
            });

            ResultMatrix = Matrix<double>.Build.DenseOfArray(resultArray);
        }

        public void RandomizeMatricies()
        {
            Matrix1 = Matrix<double>.Build.Random(Row1, Col1,
                new ContinuousUniform(lowerBound, upperBound));
            Matrix2 = Matrix<double>.Build.Random(Row2, Col2, 
                new ContinuousUniform(lowerBound, upperBound));

            UpdateMatrixArrays();
        }

        private void UpdateMatrixArrays()
        {
            //Converting matrices to arrays, as working with arrays is much faster.
            matrixArray1 = ConvertToArray(Matrix1);
            matrixArray2 = ConvertToArray(Matrix2);
            resultArray = new double[Row1, Col2];
        }

        private double[,] ConvertToArray(Matrix<double> matrix)
        {
            return matrix.ToArray();
        }
    }
}
