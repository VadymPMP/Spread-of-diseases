using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МетодЕйлераРунгеКутта
{
    class Methods
    {
        protected List<double> valueFunctions;
        protected List<double> valueDerivativeFunctions;


        public List<double> ValueFunctions { get => valueFunctions; }
        public List<double> ValueDerivativeFunctions { get => valueDerivativeFunctions; }

        public Methods()
        {
            valueFunctions = new List<double>();
            valueDerivativeFunctions = new List<double>();
        }

        public virtual void Solution(double x0, double h, List<Function> functions, List<double> y0) { }
    }
}
