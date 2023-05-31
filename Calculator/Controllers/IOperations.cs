using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Controllers
{
    interface IOperations 
    {
        double Multiplication();
        double Division();
        double Sum();
        double Substraction();
        double Sqrt();
        double SqrtY();
        double Degree();
        double Square();
        double Factorial();
        double Percent();
        double Cos();
        double Sin();
        double Tan();
        void Memory_Sum();
        void Memory_Substraction();
        double Memory_Show();
        void Memory_Clear();
        void variables_Clear();
    }
}
