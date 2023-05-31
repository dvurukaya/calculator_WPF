using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Controllers
{
    public class Operations : IOperations
    {
        public double X_variable = 0;
        public double Y_variable = 0;
        public double result = 0;
        public double memory = 0;
        public void setVariable_x(double x)
        {
            X_variable = x;
        }
        public void setVariable_y(double y)
        {
            Y_variable = y;
        }

        #region classic operations
        public double Multiplication()
        {
            return X_variable * Y_variable; 
        }
        public double Division()
        {
            return X_variable / Y_variable;
        }
        public double Sum()
        {
            return X_variable + Y_variable;
        }
        public double Substraction()
        {
            return X_variable - Y_variable;
        }
        #endregion

        #region maths operations
        public double Sqrt()
        {
            return Math.Sqrt(X_variable);
        }
        public double SqrtY()
        {
            return Math.Pow(X_variable, 1/Y_variable);
        }
        public double Degree()
        {
            return Math.Pow(X_variable, Y_variable); 
        }
        public double Square()
        {
            return Math.Pow(X_variable, 2.0);
        }
        public double Factorial()
        {
            double c = 1;
            for (int i = 1; i <= X_variable; i++)
            {
                c *= (double)i;
            }
            return c;
        }
        public double Percent()
        {
            return Y_variable * 100 / X_variable;
        }
        #endregion

        #region trigonometry
        public double Cos()
        {
            return Math.Cos(X_variable);
        }
        public double Sin()
        {
            return Math.Sin(X_variable);
        }
        public double Tan()
        {
            return Math.Tan(X_variable);
        }
        #endregion

        #region Memory
        public void Memory_Sum()
        {
            memory += X_variable;
        }

        public void Memory_Substraction()
        {
            memory -= X_variable;
        }

        //one press - show, double press - clean
        public double Memory_Show()
        {
            return memory;
        }

        public void Memory_Clear()
        {
            memory = 0;
        }
         
        public void variables_Clear()
        {
            X_variable = 0;
            Y_variable = 0;
        }
        #endregion
    }
}
