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
    class DifferentialEquation
    {
        private List<double> xi;
        private List<List<double>> valueFunctions;
        private List<List<double>> valueDerivativeFunctions;

        public List<double> Xi { get => xi; }
        public List<List<double>> ValueFunctions { get => valueFunctions; }
        public List<List<double>> ValueDerivativeFunctions { get => valueDerivativeFunctions; }

        public DifferentialEquation()
        {
            xi = new List<double>();
            valueFunctions = new List<List<double>>();
            valueDerivativeFunctions = new List<List<double>>();
        }

        public void SolutionSystemDifferentialEquations<T>(double x0, double xn, double n, List<Function> functions, List<double> y0, T method) where T : Methods
        {
            if (functions.Count != y0.Count || n<0) throw new Exception("Кількість функцій не збігається з кількістю початкових умов або відємне n");

            int N = functions.Count;

            valueFunctions = new List<List<double>>();
            valueDerivativeFunctions = new List<List<double>>();

            double h = (xn - x0) / n;

            xi.Add(x0);
            valueFunctions.Add(y0);
            int i = 0;

            while (i < n)
            {
                method.Solution(xi[i], h, functions, valueFunctions[i]);
                valueFunctions.Add(method.ValueFunctions);
                valueDerivativeFunctions.Add(method.ValueDerivativeFunctions);

                xi.Add(Math.Round(xi[i] + h, 6));
                i++;
            }
            method.Solution(xi[i], h, functions, valueFunctions[i]);
            valueDerivativeFunctions.Add(method.ValueDerivativeFunctions);
        }

        //public List<List<DataPoint>> MethodsLobatto2()
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    // the code that you want to measure comes here
            
        //    List<List<DataPoint>> dataPoints = new List<List<DataPoint>>();
            
        //    for (int k = 0; k < N; k++)
        //    {
        //        dataPoints.Add(new List<DataPoint>());
        //        dataPoints[k].Add(new DataPoint(x0, y0[k]));
        //    }

        //    double h = (xn - x0) / n;

        //    double xi = x0;

        //    List<double> yi = Copy(y0);
        //    List<double> aa = new List<double>();
        //    while (xi < xn)
        //    {
                
        //        yi = LobattoMethod(xi, h, yi, functions, ref aa);
        //        lis.Add(aa);
        //        xi = xi + h;
        //        xi = Math.Round(xi, 6);
        //        for (int k = 0; k < N; k++)
        //        {
        //            dataPoints[k].Add(new DataPoint(xi, yi[k]));
        //        }
        //    }
        //    yi = LobattoMethod(xi, h, yi, functions, ref aa);
        //    lis.Add(aa);
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    MessageBox.Show(elapsedMs.ToString());
        //    return dataPoints;
        //}

        //public List<List<DataPoint>> MethodsLobattoEPS(double eps)
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    List<List<DataPoint>> dataPoints = new List<List<DataPoint>>();
        //    for (int k = 0; k < N; k++)
        //    {
        //        dataPoints.Add(new List<DataPoint>());
        //        dataPoints[k].Add(new DataPoint(x0, y0[k]));
        //    }

        //    double hi = (xn - x0) / n;

        //    double xi = x0;

        //    List<double> yi = Copy(y0);

        //    List<double> yih1 = Copy(y0);
        //    List<double> yih2 = Copy(y0);
        //    List<double> yi2h = Copy(y0);

        //    while (xi < xn)
        //    {
        //        yi2h = Copy(yi);

        //        //yih1 = LobattoMethod(xi, hi, yi, functions, ref new List<double>());
        //        //yih2 = LobattoMethod(xi + hi, hi, yih1, functions);

        //        //yi2h = LobattoMethod(xi, 2 * hi, yi, functions);
        //        // / Max(yih2, yi2h, 1)
        //        //(1 / (Math.Pow(2, 6) - 1)) *
        //        double err = (Norma(yih2, yi2h) );
        //        double h = hi * Math.Max(0.1, 0.9*Math.Pow((eps/err), 1/(double)7));
        //        if(err<=eps)
        //        {
                    
                    
        //            for (int k = 0; k < N; k++)
        //            {
        //                dataPoints[k].Add(new DataPoint(xi+hi, yih1[k]));
        //                dataPoints[k].Add(new DataPoint(xi+2*hi, yih2[k]));
        //            }
        //            yi = Copy(yih2);
        //            xi = xi + 2 * hi;
        //            hi = Math.Min(hi*2, h);
        //        }
        //        else
        //        {
        //            hi = h;
        //        }
        //    }
        //    watch.Stop();
        //    var elapsedMs = watch.Elapsed.Seconds;
        //    MessageBox.Show(elapsedMs.ToString());
        //    return dataPoints;
        //}

        //public List<List<DataPoint>> MethodsLobatto()
        //{
        //    List<List<DataPoint>> dataPoints = new List<List<DataPoint>>();
        //    for (int k = 0; k < N; k++)
        //    {
        //        dataPoints.Add(new List<DataPoint>());
        //        dataPoints[k].Add(new DataPoint(x0, y0[k]));
        //    }

        //    double[] b = new double[4] { 1 / (double)12, 5 / (double)12, 5 / (double)12, 1 / (double)12 };
        //    double[] c = new double[4] { 0, (5 - Math.Sqrt(5)) / (double)10, (5 + Math.Sqrt(5)) / (double)10, 1 };
        //    double[,] a = new double[4, 4] { { 0, 0, 0, 0 }, { (5 + Math.Sqrt(5)) / (double)60, 1 / (double)6, (15 - 7 * Math.Sqrt(5)) / (double)60, 0 }, { (5 - Math.Sqrt(5)) / (double)60, (15 + 7 * Math.Sqrt(5)) / (double)60, 1 / (double)6, 0 }, { 1 / (double)6, (5 - Math.Sqrt(5)) / (double)12, (5 + Math.Sqrt(5)) / (double)12, 0 } };

        //    double s = 4;
        //    double h = (xn - x0) / n;

        //    double xi = x0;
        //    List<double> yi = Copy(y0);

        //    double[] k1 = new double[N];
        //    double[] k2 = new double[N];
        //    double[] k3 = new double[N];
        //    double[] k4 = new double[N];

        //    while (xi < xn)
        //    {


        //        for (int k = 0; k < N; k++)
        //        {

        //            k1[k] = functions[k].result(ToArray(xi, yi));
        //            k2[k] = functions[k].result(ToArray(xi, yi));
        //            k3[k] = functions[k].result(ToArray(xi, yi));
        //            k4[k] = functions[k].result(ToArray(xi, yi));
        //        }

        //        double[][] K = new double[4][] { k1, k2, k3, k4 };

        //        double[,] KPlusOne = new double[4, N];

        //        double x;
        //        while (true)
        //        {
        //            for (int j = 0; j < s; j++)
        //            {
                        
        //                x = xi + h * c[j];

        //                List<double> y = new List<double>();
        //                for (int k = 0; k < N; k++)
        //                {
        //                    y.Add(new double());
        //                    y[k] = yi[k];
        //                    for (int i = 0; i < s; i++)
        //                    {
        //                        y[k] += h * a[j, i] * K[i][k];
        //                    }
        //                }

        //                for (int k = 0; k < N; k++)
        //                {
        //                    KPlusOne[j, k] = functions[k].result(ToArray(x, y));
                            
        //                }
                        
        //            }

        //            bool isEps = true;
        //            double eps = 0.000001;

        //            for (int k = 0; k < N; k++)
        //            {
        //                for (int j = 0; j < s; j++)
        //                {
        //                    if (Math.Abs((KPlusOne[j, k] - K[j][k]) / K[j][k]) > eps) isEps = false;
        //                }
        //            }
                    
        //            if (isEps)
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                for (int k = 0; k < N; k++)
        //                {
        //                    for (int j = 0; j < s; j++)
        //                    {
        //                        K[j][k] = KPlusOne[j, k];
        //                    }
        //                }
        //            }
        //        }

        //        xi = xi + h;
        //        for (int k = 0; k < N; k++)
        //        {
        //            for (int j = 0; j < s; j++)
        //            {
        //                yi[k] += h * b[j] * K[j][k];
        //            }
        //            dataPoints[k].Add(new DataPoint(xi, yi[k]));
        //        }
        //    }
        //    return dataPoints;
        //}
        //public static List<double> Copy(List<double> list)
        //{
        //    List<double> listCopy = new List<double>();
        //    foreach(double digit in list)
        //    {
        //        listCopy.Add(digit);
        //    }
        //    return listCopy;
        //}
        //public static double[] ToArray(double xi, List<double> yi)
        //{
        //    double[] variableValues = new double[yi.Count + 1];
        //    variableValues[0] = xi;
        //    for (int i = 0; i < variableValues.Length-1; i++)
        //    {
        //        variableValues[i + 1] = yi[i];
        //    }
        //    return variableValues;
        //}
        //public static double Norma(List<double> yih, List<double> yi2h)
        //{
        //    double norma = 0;
        //    for(int i=0; i<yih.Count; i++)
        //    {
        //        if(Math.Abs(yih[i]-yi2h[i])>norma)
        //        {
        //            norma = Math.Abs(yih[i] - yi2h[i]);
        //        }
        //    }
        //    return norma;
        //}
        //public static double Max(List<double> yih, List<double> yi2h, double digit)
        //{
        //    double max = 0;
        //    for (int i = 0; i < yih.Count; i++)
        //    {
        //        if (Math.Abs(Math.Max(Math.Abs(yih[i]), Math.Abs(yi2h[i]))) > max)
        //        {
        //            max = Math.Abs(Math.Max(Math.Abs(yih[i]), Math.Abs(yi2h[i])));
        //        }
        //    }
        //    return Math.Max(max,1);
        //}
    }
}
