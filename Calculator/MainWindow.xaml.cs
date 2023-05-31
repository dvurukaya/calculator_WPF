using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Calculator.Controllers;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        Operations operations;
        public MainWindow()
        {
            InitializeComponent();
            operations = new Operations();
        }

        #region check variables

        int checkInput = 0;
        int checkDot = 0;
        int checkOperation = 0; //iterator for double clicked operations(+,-,*,/ etc.,) which uses instead of "=" 

        string buttonContent = ""; //to save the symbol of the operation (+,-,*,/ etc.,)
        
        public enum operations_Condition 
        {
            None,
            FirstNumber,
            FirstOperation,
            DoubledOperation,
            TrigonometryOperation,
            Equal,
            EqualBySign,
            Memory
        }
        operations_Condition operations_condition = operations_Condition.None;

        public enum memory_Indicator
        {
            memoryCan,
            memoryCannot
        }
        memory_Indicator memory_indicator = memory_Indicator.memoryCan;

        public enum operations_twoOperand
        {
            plus,
            subtraction,
            multiplication,
            division,
            percent,
            sqrtX,
            degree,
            NULL //no operation is used
        }
        operations_twoOperand currentOperation = operations_twoOperand.NULL;

        #endregion

        #region functions
        private void close_Button(object sender, RoutedEventArgs e)
        {
            Close();
        }
     
        private void set_Condition(string action) //check which function is clicked, for two operands
        {
            switch(action)
            {
                case "plus":
                    currentOperation = operations_twoOperand.plus;
                    break;
                case "subtraction":
                    currentOperation = operations_twoOperand.subtraction;
                    break;
                case "multiplication":
                    currentOperation = operations_twoOperand.multiplication;
                    break;
                case "division":
                    currentOperation = operations_twoOperand.division;
                    break;
                case "percent":
                    currentOperation = operations_twoOperand.percent;
                    break;
                case "sqrtX":
                    currentOperation = operations_twoOperand.sqrtX;
                    break;
                case "degree":
                    currentOperation = operations_twoOperand.degree;
                    break;
            }
        }

        private void operation_InizializationX() // inizialization of first input
        {
            operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
        }
        private void operation_InizializationY() // inizialization of second input
        {
            operations.setVariable_y(Convert.ToDouble(TextBox_Screen.Text));
        }
       
        private void doubledScreen_clean()
        {
            doubled_Screen.Text = "";
        }

        private void writeSymbol() //write symbol of operation when it doubled
        {
            if (checkOperation >= 2 && operations_condition == operations_Condition.FirstNumber)
            {
                doubled_Screen.Text += $" {buttonContent} ";
            }
        }
        private void checkZero()
        {
            if (TextBox_Screen.Text == "0") //to delete zero when input new number
            {
                checkInput++;
                TextBox_Screen.Text = TextBox_Screen.Text.Remove(0, 1);
                operations_condition = operations_Condition.FirstNumber;
            }
            else if (TextBox_Screen.Text == "0" && NumberZero.IsEnabled) //to block attempts to write zero more than 1 time, if dot isn't used
            {
                TextBox_Screen.Text = TextBox_Screen.Text.Substring(0, TextBox_Screen.Text.Length - 1);
            }
            else if (TextBox_Screen.Text.Length > 0 && checkInput == 0)
            {
                TextBox_Screen.Text = TextBox_Screen.Text.Remove(0, TextBox_Screen.Text.Length);
                checkInput++;
            }
        }
        private void checkDot_check()
        {
            if (checkDot > 1) { TextBox_Screen.Text = TextBox_Screen.Text.Substring(0, TextBox_Screen.Text.Length - 1); }
        }

        private void _checkOperation() // working with TextBox_Screen/doubledScreen content, depending on condition the app has at the moment
        {
            if (operations_condition == operations_Condition.None) { TextBox_Screen.Text = "0"; doubled_Screen.Text = ""; operations_condition = operations_Condition.FirstNumber; }
            else if(operations_condition == operations_Condition.TrigonometryOperation) { cleaning(); }
            else if(operations_condition == operations_Condition.EqualBySign) { TextBox_Screen.Text = "0"; doubledScreen_clean(); doubled_Screen.Text = $"{operations.result} ";
                                                                                operations_condition = operations_Condition.FirstNumber; }
            else if(operations_condition == operations_Condition.DoubledOperation) { doubledScreen_clean(); doubled_Screen.Text = $"{operations.X_variable} {buttonContent}"; 
                                                                                     memory_indicator = memory_Indicator.memoryCannot; }  
            else if (operations_condition == operations_Condition.Memory) { TextBox_Screen.Text = "0"; doubled_Screen.Text = ""; operations_condition = operations_Condition.None; 
                                                                            memory_indicator = memory_Indicator.memoryCan; }
        }

        #endregion

        #region panel for buttons
        private void number_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;

            _checkOperation();
            checkZero();

            chooseNumber(name);
        }

        private void chooseNumber(string name)
        {
            switch (name)
            {
                case "NumberOne":
                    TextBox_Screen.Text += "1";
                    break;
                case "NumberTwo":
                    TextBox_Screen.Text += "2";
                    break;
                case "NumberThree":
                    TextBox_Screen.Text += "3";
                    break;
                case "NumberFour":
                    TextBox_Screen.Text += "4";
                    break;
                case "NumberFive":
                    TextBox_Screen.Text += "5";
                    break;
                case "NumberSix":
                    TextBox_Screen.Text += "6";
                    break;
                case "NumberSeven":
                    TextBox_Screen.Text += "7";
                    break;
                case "NumberEight":
                    TextBox_Screen.Text += "8";
                    break;
                case "NumberNine":
                    TextBox_Screen.Text += "9";
                    break;
                case "NumberZero":
                    TextBox_Screen.Text += "0";
                    break;
            }
        }

        private void dot_Click(object sender, RoutedEventArgs e)
        {
            checkDot_check();
            if (TextBox_Screen.Text == "0") { checkInput++; checkDot++; TextBox_Screen.Text = "0,"; } 
            else if(TextBox_Screen.Text != "0" && checkDot < 1) { TextBox_Screen.Text += ","; checkDot++; }
        }

        private void mark_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_Screen.Text[0] == '-') { TextBox_Screen.Text = TextBox_Screen.Text.Remove(0, 1); }
            else { TextBox_Screen.Text = '-' + TextBox_Screen.Text; }
        }

        #endregion

        #region panel for  operations
        //for operations: !n, ^2, √
        private void oneOperandOperations_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;

            switch(buttonName)
            {
                case ("factorial"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    TextBox_Screen.Text = Convert.ToString(operations.Factorial());
                    doubled_Screen.Text += $"!{operations.X_variable}";
                    break;
                case ("square"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    TextBox_Screen.Text = Convert.ToString(operations.Sqrt());
                    doubled_Screen.Text += $"{operations.X_variable}^2";
                    break;
                case ("sqrt"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    TextBox_Screen.Text = Convert.ToString(operations.Sqrt());
                    doubled_Screen.Text += $"√{operations.X_variable}";
                    break;
            }
            memory_indicator = memory_Indicator.memoryCan;
            memoryOperations_Check();
        }

        //for operations: +. -, /, x, x^y, % 
        private void twoOperandOperations_Click(object sender, RoutedEventArgs e)
        {
            checkOperation++;
            buttonContent = ((Button)sender).Content.ToString();
            set_Condition(((Button)sender).Name.ToString());

            if(operations_condition == operations_Condition.Memory)
            {
                _checkOperation();
                checkOperation = 0;
                checkInput++;
            }
            
            if (operations_condition == operations_Condition.Equal)
            {
                operations.setVariable_x(operations.result);
                operations_condition = operations_Condition.DoubledOperation;
                _checkOperation();
                operations.result = 0;
            }
            
            if (operations_condition == operations_Condition.FirstNumber)
            {
                operation_InizializationX();
                doubled_Screen.Text += $"{operations.X_variable} {buttonContent} ";
                operations_condition = operations_Condition.FirstOperation;  
                memory_indicator = memory_Indicator.memoryCannot;
            }

            if (checkOperation > 1 || operations_condition == operations_Condition.EqualBySign) 
            {
                    equality();
                    operations.X_variable = operations.result;
                    writeSymbol();
                    memory_indicator = memory_Indicator.memoryCan;
                    operations_condition = operations_Condition.DoubledOperation;
                    _checkOperation();
            }
            memoryOperations_Check();
            checkInput = 0;   
        }

        //for operations: log, cos, tan
        private void trigonometry_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            operations_condition = operations_Condition.TrigonometryOperation;

            switch (buttonName)
            {
                case ("tan"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    TextBox_Screen.Text = Convert.ToString(operations.Tan());
                    doubled_Screen.Text += $"{operations.X_variable}";
                    break;
                case ("cos"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    TextBox_Screen.Text = Convert.ToString(operations.Cos());
                    doubled_Screen.Text += $"{operations.X_variable}";
                    break;
                case ("sin"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    TextBox_Screen.Text = Convert.ToString(operations.Sin());
                    doubled_Screen.Text += $"{operations.X_variable}";
                    break;
            }
            memory_indicator = memory_Indicator.memoryCan;
            memoryOperations_Check();
        }

        #endregion

        #region equals
        private void equality() //separated function of button operation Equals_Click, for realization of doubledClicked operation (+,-,x,/ etc.)
        {
            double result = 0;
            operation_InizializationY();

            switch (currentOperation)
            {
                case operations_twoOperand.plus:
                    result = operations.Sum();
                    break;
                case operations_twoOperand.subtraction:
                    result = operations.Substraction();
                    break;
                case operations_twoOperand.multiplication:
                    result = operations.Multiplication();
                    break;
                case operations_twoOperand.division:
                    result = operations.Division();
                    break;
                case operations_twoOperand.sqrtX:
                    result = operations.SqrtY();
                    break;
                case operations_twoOperand.degree:
                    result = operations.Degree();
                    break;
                case operations_twoOperand.percent:
                    result = operations.Percent();
                    break;
            }

            TextBox_Screen.Text = result.ToString();
            doubled_Screen.Text += $"{operations.Y_variable} = ";
            operations.result = result;
            checkDot = 0;

            currentOperation = operations_twoOperand.NULL;
            operations_condition = operations_Condition.EqualBySign;
            memory_indicator = memory_Indicator.memoryCan;
            memoryOperations_Check();
        }
        private void equals_Click(object sender, RoutedEventArgs e)
        {
            if(operations_condition == operations_Condition.Equal || operations_condition == operations_Condition.EqualBySign 
                                                                  || operations_condition == operations_Condition.None
                                                                  || operations_condition == operations_Condition.DoubledOperation) 
                                                             { cleaning(); }
            else
            { 
                equality();
                checkOperation = 0;
                operations_condition = operations_Condition.Equal;
                memory_indicator = memory_Indicator.memoryCan;
                memoryOperations_Check();
            }
        }

        #endregion

        #region panel for memory operations
        private void memoryOperations_Check() //check the statement to use memoryFunctions
        {
            if(memory_indicator == memory_Indicator.memoryCannot)
            {
                memory_plus.IsEnabled = false;
                memory_subtraction.IsEnabled = false;
                memory_show.IsEnabled = false;
            }
            else 
            {
                memory_plus.IsEnabled = true;
                memory_subtraction.IsEnabled = true;
                memory_show.IsEnabled = true;
            }
        }

        private void memory_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;

            switch (buttonName)
            {
                case ("memory_plus"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    operations.Memory_Sum();
                    doubled_Screen.Text = $"You plused to memory {operations.X_variable}";
                    operations_condition = operations_Condition.Memory;
                    break;
                case ("memory_subtraction"):
                    operations.setVariable_x(Convert.ToDouble(TextBox_Screen.Text));
                    operations.Memory_Substraction();
                    doubled_Screen.Text = $"You subtracted from memory {operations.X_variable}";
                    operations_condition = operations_Condition.Memory;
                    break;
                case ("memory_show"):
                    checkOperation++;
                    if (checkOperation == 1) { TextBox_Screen.Text = operations.Memory_Show().ToString(); doubled_Screen.Text = "Memory content is:";
                        operations_condition = operations_Condition.Memory; }
                    if (checkOperation == 2) { operations.Memory_Clear(); TextBox_Screen.Text = "0"; operations_condition = operations_Condition.Memory; }

                    break;
            }
        }
        #endregion

        #region cleaning operation
        private void cleaning()
        {
            TextBox_Screen.Clear();
            TextBox_Screen.Text = "0";
            doubled_Screen.Text = "";
            checkOperation = 0;
            operations_condition = operations_Condition.None;
            operations.variables_Clear();
            memory_indicator = memory_Indicator.memoryCan;
            memoryOperations_Check();
            currentOperation = operations_twoOperand.NULL;
            checkDot = 0;
        }
        private void clean_Click(object sender, RoutedEventArgs e)
        {
            cleaning();
        }
        #endregion
    }
}
