using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using OxyPlot;
using System.IO;
using Microsoft.Win32;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DifferentialEquation equation;

        bool isSolution = false;
        int numberEducation;
        double x0 = 0;
        double xn = 0;
        double n = 0;
        List<Function> functionsLobatto = new List<Function>();
        List<double> y0 = new List<double>();
        string[] variables = new string[0];
        List<Function> functionsExact = new List<Function>();


        private List<double> xi;
        private List<List<double>> valueFunctions;
        private List<List<double>> turnValueFunctions;
        private List<List<double>> valueDerivativeFunctions;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            equation = new DifferentialEquation();
            functionsLobatto = new List<Function>();
            y0 = new List<double>();
            functionsExact = new List<Function>();
            numberEducation = 0;

            valueFunctions = new List<List<double>>();
            turnValueFunctions = new List<List<double>>();
            valueDerivativeFunctions = new List<List<double>>();
            xi = new List<double>();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            stackPanelLobattoEqution.Children.Add(SystemEducation.LobattoEquation(++numberEducation));
            stackPanelExactEquation.Children.Add(SystemEducation.ExactEquation(numberEducation));
        }

        private void ButtonAddEquation_Click(object sender, RoutedEventArgs e)
        {
            if (numberEducation < 10)
            {
                stackPanelLobattoEqution.Children.Add(SystemEducation.LobattoEquation(++numberEducation));
                stackPanelExactEquation.Children.Add(SystemEducation.ExactEquation(numberEducation));
            }
        }

        private void ButtonDeleteEquation_Click(object sender, RoutedEventArgs e)
        {
            if (numberEducation > 0)
            {
                stackPanelLobattoEqution.Children.RemoveAt(--numberEducation);
                stackPanelExactEquation.Children.RemoveAt(numberEducation);
            }



        }

        private void ButtonSolution_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isSolution = false;
                Graphics.Series.Clear();

                functionsLobatto = new List<Function>();
                y0 = new List<double>();
                functionsExact = new List<Function>();

                variables = new string[numberEducation + 1];
                variables[0] = "x";
                for (int i = 1; i <= numberEducation; i++)
                {
                    variables[i] = "y" + i;
                }

                List<StackPanel> stackPanels = stackPanelLobattoEqution.Children.OfType<StackPanel>().ToList();
                for (int i = 0; i < stackPanels.Count; i++)
                {
                    functionsLobatto.Add(new Function((stackPanels[i].Children[1] as ComboBox).Text, variables));
                }

                for (int i = 0; i < stackPanels.Count; i++)
                {
                    y0.Add(double.Parse((stackPanels[i].Children[3] as TextBox).Text));

                }

                if (double.TryParse(textBoxX0.Text, out double tryX0) == false || double.TryParse(textBoxXN.Text, out double tryXn) == false || double.TryParse(textBoxN.Text, out double tryN) == false)
                {
                    throw new Exception("Помилка при ініціалізації меж");
                }
                else
                {
                    x0 = double.Parse(textBoxX0.Text);
                    xn = double.Parse(textBoxXN.Text);
                    n = double.Parse(textBoxN.Text);
                }



                equation = new DifferentialEquation();

                equation.SolutionSystemDifferentialEquations<RungeKuttaLobatto>(x0, xn, n, functionsLobatto, y0, new RungeKuttaLobatto());
                valueFunctions = equation.ValueFunctions;
                valueDerivativeFunctions = equation.ValueDerivativeFunctions;
                xi = equation.Xi;

                turnValueFunctions = Turn(valueFunctions);
                if (turnValueFunctions.Count == 3)
                {
                    for (int i = 0; i < turnValueFunctions.Count; i++)
                    {
                        PlotFunctions.PlotFunction(Graphics, xi, turnValueFunctions[i], a1[i], Brushes.Red, 0.01);
                    }
                }
                else
                {
                    for (int i = 0; i < turnValueFunctions.Count; i++)
                    {
                        PlotFunctions.PlotFunction(Graphics, xi, turnValueFunctions[i], a[i], Brushes.Red, 0.01);
                    }
                }

                stackPanels = stackPanelExactEquation.Children.OfType<StackPanel>().ToList();

                functionsExact = new List<Function>();
                for (int i = 0; i < stackPanels.Count; i++)
                {
                    functionsExact.Add(new Function((stackPanels[i].Children[1] as ComboBox).Text, variables));
                    //PlotFunctions.PlotFunction(Graphics, functionsExact[i], x0, xn, "y" + (i + 1) + "  Exact", Brushes.Blue, 0.01);
                }

                PlotX.Minimum = Math.Min(x0, xn);
                PlotX.Maximum = Math.Max(x0, xn);

                isSolution = true;
                TextBoxNumber_TextChanged(textBoxNumber, null);
                TextBoxNumberFunctionForDataGrid_TextChanged(textBoxNumberFunctionForDataGrid, null);
                ButtonSolutionValueInPoint_Click(buttonSolutionErmit, null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
}
        static string[] a = new string[] { "Susceptible_Population", "Exposed_Population", "Infectious_Population", "Recovered_Population", "Death_Population" };
        static string[] a1 = new string[] { "Susceptible_Population", "Infectious_Population", "Recovered_Population" };
        private void ButtonPlotGraphicsTest_Click(object sender, RoutedEventArgs e)
        {
            double a = double.Parse(textBoxATest.Text);
            double b = double.Parse(textBoxBTest.Text);
            PlotXTest.Minimum = a;
            PlotXTest.Maximum = b;

            PlotFunctions.PlotFunction(GraphicsTest, new Function(textBoxFTest.Text, "x"), a, b, "f", Brushes.Blue, 0.1);
        }

       
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            GraphicsTest.Series.Clear();
            GraphicsTest.Series.Add(new LineSeries());
            GraphicsTest.Series[0].ItemsSource = new List<DataPoint>();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DataFile.ReadFile(openFileDialog.FileName, ref x0, ref xn, ref n, ref functionsLobatto, ref y0, ref variables, ref functionsExact, ref numberEducation);

                stackPanelLobattoEqution.Children.Clear();
                stackPanelExactEquation.Children.Clear();
                for (int i = 0; i < numberEducation; i++)
                {
                    stackPanelLobattoEqution.Children.Add(SystemEducation.LobattoEquation(i + 1, functionsLobatto[i].strFunction, y0[i].ToString()));
                    stackPanelExactEquation.Children.Add(SystemEducation.ExactEquation(i + 1, functionsExact[i].strFunction));

                }
                textBoxX0.Text = x0.ToString();
                textBoxXN.Text = xn.ToString();
                textBoxN.Text = n.ToString();
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                DataFile.SaveFile(saveFileDialog.FileName, x0, xn, n, functionsLobatto, y0, functionsExact, numberEducation);
        }

        private void ButtonSolutionValueInPoint_Click(object sender, RoutedEventArgs e)
        {
            if (isSolution)
            {
                List<TextBlock> textBlocks = Ermit.Children.OfType<TextBlock>().ToList();
                foreach (TextBlock block in textBlocks)
                    Ermit.Children.Remove(block);

                if (double.TryParse(textBoxPoint.Text, out double point) == false)
                {
                    textBoxPoint.Text = "0";
                    throw new Exception("Помилка при знаходження значення функції в точці");
                }

                if ((point < x0 && point < xn) || (point > x0 && point > xn)) throw new Exception("Помилка при знаходження значення функції в точці (точка поза межами)");

                double x1 = 0;
                double x2 = 0;
                Ermit.Children.Add(new TextBlock());
                for (int i = 0; i < xi.Count - 1; i++)
                {
                    x1 = xi[i];
                    x2 = xi[i + 1];
                    if (Math.Min(x1, x2) <= point && Math.Max(x1, x2) >= point)
                    {
                        for (int j = 0; j < numberEducation; j++)
                        {
                            double fPoint = HermitPolynomial(x1, x2, valueFunctions[i][j], valueFunctions[i + 1][j], valueDerivativeFunctions[i][j], valueDerivativeFunctions[i + 1][j], point);
                            double error = Math.Abs(fPoint - functionsExact[j].result(point));
                            Ermit.Children.Add(new TextBlock { Text = "y" + (j + 1) + " :  " + fPoint.ToString() + "\t" + error, Height = 30 });
                        }
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Виникла помилка при розв'язуванні. Задайте правильно всі дані, та натисніть кнопку \"Solution\"");
            }
        }
        static double HermitPolynomial(double x0, double x1, double fx0, double fx1, double dfx0, double dfx1, double x)
        {
            double h = x0 - x1;
            double P = fx0 + (x - x0) * (dfx0 + (x - x0) * (dfx0 - (fx0 - fx1) / h + (x - x1) * (dfx0 - 2 * (fx0 - fx1) / h + dfx1) / h) / h);
            return P;
        }

        private static List<List<double>> Turn(List<List<double>> list1)
        {
            List<List<double>> list2 = new List<List<double>>();
            for (int i = 0; i < list1[0].Count; i++)
            {
                list2.Add(new List<double>());
                for (int j = 0; j < list1.Count; j++)
                {
                    list2[i].Add(list1[j][i]);
                }
            }
            return list2;
        }

        static int[] exactx = new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68 };
        static int[] exacty = new int[] {0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,2,3,3,3,3,3,5,5,5,9,10,13,17,20,22,27,32,37,38,45,52,57,69,73,83,93,98,108,116,125,133,141,151,161,174,187,201,201,209,220,239,250,261,272,279,288,303,316,327,340,361,376,391};
       
        private void TextBoxNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(isSolution)
            {
                
                int number;
                if (int.TryParse(textBoxNumber.Text, out number) == true)
                {
                    GraphicsTest.Series.Clear();
                    PlotFunctions.PlotFunction(GraphicsTest, xi, turnValueFunctions[number-1], a[number-1], Brushes.Red, 0.01);
                    PlotFunctions.PlotFunction(GraphicsTest, exactx, exacty, "y" + (number) + "  Exact", Brushes.Blue, 0.01);
                    PlotXTest.Minimum = Math.Min(x0, xn);
                    PlotXTest.Maximum = Math.Max(x0, xn);
                }
                else
                {
                    try
                    {
                        GraphicsTest.Series.Clear();
                        List<int> list = (textBoxNumber.Text).Split(',').Select(i => int.Parse(i)).ToList();
                        foreach (int i in list)
                        {
                            PlotFunctions.PlotFunction(GraphicsTest, xi, turnValueFunctions[i - 1], "y" + (i) + "  Lobatto", Brushes.Red, 0.01);
                            PlotFunctions.PlotFunction(GraphicsTest, functionsExact[i - 1], x0, xn, "y" + (i) + "  Exact", Brushes.Blue, 0.01);
                            PlotXTest.Minimum = Math.Min(x0, xn);
                            PlotXTest.Maximum = Math.Max(x0, xn);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void TextBoxNumberFunctionForDataGrid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isSolution)
            {
                int number;
                if (int.TryParse(textBoxNumberFunctionForDataGrid.Text, out number) == true)
                {
                    List<object> list = new List<object>();
                    double x = 0;
                    for (int i = 0; i < turnValueFunctions[number-1].Count; i++)
                    {
                        x = xi[i];
                        list.Add(new { x = x, y = valueFunctions[i][number-1], dy = valueDerivativeFunctions[i][number-1], exactY = functionsExact[number-1].result(x), eps = Math.Abs(valueFunctions[i][number-1]- functionsExact[number-1].result(x)) });
                    }
                    datagrid.ItemsSource = list;
                }
            }
            
        }
    }
}
