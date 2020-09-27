using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using OxyPlot;
using OxyPlot.Wpf;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace МетодЕйлераРунгеКутта
{
    class RungeKuttaLobatto: Methods
    {
        private const int s = 4;
        private const double eps = 0.00001;
        private double[] b = new double[4] { 1 / (double)12, 5 / (double)12, 5 / (double)12, 1 / (double)12 };
        private double[] c = new double[4] { 0, (5 - Math.Sqrt(5)) / (double)10, (5 + Math.Sqrt(5)) / (double)10, 1 };
        private double[,] a = new double[4, 4] { { 0, 0, 0, 0 }, { (5 + Math.Sqrt(5)) / (double)60, 1 / (double)6, (15 - 7 * Math.Sqrt(5)) / (double)60, 0 }, { (5 - Math.Sqrt(5)) / (double)60, (15 + 7 * Math.Sqrt(5)) / (double)60, 1 / (double)6, 0 }, { 1 / (double)6, (5 - Math.Sqrt(5)) / (double)12, (5 + Math.Sqrt(5)) / (double)12, 0 } };

        public RungeKuttaLobatto() : base() { }

        public override void Solution(double x0, double h, List<Function> functions, List<double> y0)
        {
            if (functions.Count != y0.Count) throw new Exception("Кількість функцій не збігається з кількістю початкових умов");

            valueFunctions = Copy(y0);
            valueDerivativeFunctions = new List<double>();

            int N = functions.Count;
            double[] k1 = new double[N];
            double[] k2 = new double[N];
            double[] k3 = new double[N];
            double[] k4 = new double[N];

            for (int k = 0; k < N; k++)
            {
                valueDerivativeFunctions.Add(functions[k].result(ToArray(x0, y0)));
                k1[k] = functions[k].result(ToArray(x0, y0));
                k2[k] = functions[k].result(ToArray(x0, y0));
                k3[k] = functions[k].result(ToArray(x0, y0));
                k4[k] = functions[k].result(ToArray(x0, y0));
            }

            double[][] K = new double[4][] { k1, k2, k3, k4 };

            double[,] KPlusOne = new double[4, N];

            List<double> y;
            double x;

            while (true)
            {
                for (int j = 0; j < s; j++)
                {

                    x = x0 + h * c[j];

                    y = new List<double>();
                    for (int k = 0; k < N; k++)
                    {
                        y.Add(new double());
                        y[k] = y0[k];
                        for (int i = 0; i < s; i++)
                        {
                            y[k] += h * a[j, i] * K[i][k];
                        }
                    }

                    for (int k = 0; k < N; k++)
                    {
                        KPlusOne[j, k] = functions[k].result(ToArray(x, y));

                    }

                }

                bool isEps = true;


                for (int k = 0; k < N; k++)
                {
                    for (int j = 0; j < s; j++)
                    {
                        if (Math.Abs((KPlusOne[j, k] - K[j][k]) / K[j][k]) > eps) isEps = false;
                    }
                }

                if (isEps)
                {
                    break;
                }
                else
                {
                    for (int k = 0; k < N; k++)
                    {
                        for (int j = 0; j < s; j++)
                        {
                            K[j][k] = KPlusOne[j, k];
                        }
                    }
                }
            }

            for (int k = 0; k < N; k++)
            {
                for (int j = 0; j < s; j++)
                {
                    valueFunctions[k] += h * b[j] * K[j][k];
                }
            }
        }

        public static List<double> Copy(List<double> list)
        {
            List<double> listCopy = new List<double>();
            foreach (double digit in list)
            {
                listCopy.Add(digit);
            }
            return listCopy;
        }

        public static double[] ToArray(double x, List<double> y)
        {
            double[] variableValues = new double[y.Count + 1];
            variableValues[0] = x;
            for (int i = 0; i < variableValues.Length - 1; i++)
            {
                variableValues[i + 1] = y[i];
            }
            return variableValues;
        }
    }
}
