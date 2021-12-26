using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodRungeKutta
{
    public class DifferentialEquations
    {
        public double T;
        public double x0;
        public int N;

        public double[] arrXn;
        public double[] arrtn;
        public double[] errors;

        //Добавляем парсер математических выражений
        Sprache.Calc.XtensibleCalculator calc;

        public DifferentialEquations(string function, double T, double x0, int N)
        {
            calc = new Sprache.Calc.XtensibleCalculator();
            SetFunction(function);
            this.T = T;
            this.x0 = x0;
            this.N = N;
        }

        public DifferentialEquations()
        {
            calc = new Sprache.Calc.XtensibleCalculator();
        }


        public void FindLocalErrors(out double errorMax, out double errorMin)
        {
            errors = new double[N];

            //2N интервалов
            Decision(2 * N);
            double[] arrxn = new double[2 * N];
            arrXn.CopyTo(arrxn, 0);

            //N интервалов(увеличиваем шаг)
            Decision(N);
            errorMax = 0;
            errorMin = 0;
            for (int k = 0; k < N; k++)
            {
                //Ошибка по правилу Рунге
                errors[k] = (arrxn[2 * k] - arrXn[k]) * Math.Pow(2, 4) / (Math.Pow(2, 4) - 1);
                if (errors[k] > errorMax)
                    errorMax = errors[k];
                else if (errors[k] < errorMin)
                    errorMin = errors[k];
            }
        }

        public void Decision(int N)
        {
            double t = 0;
            double h = (T - t) / N;
            arrXn = new double[N];
            arrtn = new double[N];

            for (int k = 0; k < N; k++)
            {
                arrtn[k] = t + k * h;
            }

            double K1, K2, K3, K4;
            arrXn[0] = x0;
            for (int k = 0; k < N - 1; k++)
            {
                K1 = h * F(arrXn[k]);
                K2 = h * F(arrXn[k] + 1 / 4.0 * K1);
                K3 = h * F(arrXn[k] - 189 / 800.0 * K1 + 729 / 800.0 * K2);
                K4 = h * F(arrXn[k] + 214 / 891.0 * K1 + 1 / 33.0 * K2 + 650 / 891.0 * K3);

                arrXn[k + 1] = arrXn[k] + 533 / 2106.0 * K1 + 800 / 1053.0 * K3 - 1 / 78.0 * K4;
            }

        }

        private double F(double сurrentX)
        {
            try
            {
                var expr = calc.ParseExpression("f(x)", x => сurrentX);
                var func = expr.Compile();
                return func();
            }
            catch 
            {
                return double.NaN;
            }
        }

        //Задание функции вручную
        private double funcM(double сurrentX)
        {
            double x = сurrentX;
            double func = Math.Cos(x) + x*3;
            return func;

        }

        public void DecisionM(int N)
        {
            double t = 0;
            double h = (T - t) / N;
            arrXn = new double[N];
            arrtn = new double[N];

            for (int k = 0; k < N; k++)
            {
                arrtn[k] = t + k * h;
            }

            double K1, K2, K3, K4;
            arrXn[0] = x0;

            for (int k = 0; k < N - 1; k++)
            {
                K1 = h * funcM(arrXn[k]);
                K2 = h * funcM(arrXn[k] + 1 / 4.0 * K1);
                K3 = h * funcM(arrXn[k] - 189 / 800.0 * K1 + 729 / 800.0 * K2);
                K4 = h * funcM(arrXn[k] + 214 / 891.0 * K1 + 1 / 33.0 * K2 + 650 / 891.0 * K3);

                arrXn[k + 1] = arrXn[k] + 533 / 2106.0 * K1 + 800 / 1053.0 * K3 - 1 / 78.0 * K4;
            }


        }

        public void FindLocalErrorsM(out double errorMax, out double errorMin)
        {
            errors = new double[N];

            //Расчет значений для 2N интервалов
            DecisionM(2 * N);
            double[] arrxn = new double[2 * N];
            arrXn.CopyTo(arrxn, 0);

            //Расчет значений для N интервалов(уменьшаем шаг)
            DecisionM(N);
            errorMax = 0;
            errorMin = 0;
            for (int k = 0; k < N; k++)
            {
                //Расчет ошибки по правилу Рунге
                errors[k] = (arrxn[2 * k] - arrXn[k]) * Math.Pow(2, 4) / (Math.Pow(2, 4) - 1);
                //Находим мин и макс ошибки
                if (errors[k] > errorMax)
                    errorMax = errors[k];
                else if (errors[k] < errorMin)
                    errorMin = errors[k];
            }
        }

        public bool SetFunction(string function)
        {
            try
            {
                calc.RegisterFunction("f", function, "x");
                return true;
            }
            catch 
            {
                return false;
            }
        }
        


    }
}
