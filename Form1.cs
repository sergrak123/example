using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodRungeKutta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string func = "x ^ 2 / (x ^ 4 + 4 + Sin(x)) * Sin(x / 2)";
            double T = 3;
            double x0 = 2;
            double N = 100;
            textBox1.Text = func;
            textBox2.Text = T.ToString();
            textBox3.Text = x0.ToString();
            textBox4.Text = N.ToString();
            label11.Text = "";

        }
        private void CalculateAll()
        {
            button1.Enabled = false;
            //string func = "x ^ 2 / (x ^ 4 + 4 + Sin(x)) * Sin(x / 2)";
            //string func = "((x + 2) ^ 2 / (5 + Sin(x))) ^ Sin(x)";
            string func = textBox1.Text;
            DifferentialEquations equation = new DifferentialEquations();
            double t0 = 0;
            if (!equation.SetFunction(func))
            {
                Error("Функция введена некорректно");
                return;
            }
            if (!double.TryParse(textBox2.Text, out double T))
            {

                Error("Неверный ввод параметра T");
                return;
            }
            else if (T <= t0)
            {
                Error("T должно быть больше to");
                return;
            }
            if (!double.TryParse(textBox3.Text, out double x0))
            {
                Error("Неверный ввод x(0)");
                return;
            }
            else if (x0 < t0 || x0 > T)
            {
                Error(string.Format("x(0) должнен принадлежать отрезку интегрирования [{0}, {1}]", t0, T));
                return;
            }
            if (!int.TryParse(textBox4.Text, out int N))
            {
                Error("Неверный ввод N");
                return;
            }
            else if (N <= 0)
            {
                Error("N должно быть больше нуля");
                return;
            }
            equation = new DifferentialEquations(func, T, x0, N);

            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();

            double errorMax = 0;
            double errorMin = 0;

            equation.FindLocalErrors(out errorMax, out errorMin);
            for (int i = 0; i < N; i++)
            {
                chart2.Series[0].Points.AddXY(i, equation.errors[i]);
            }

            equation.Decision(N);
            for (int k = 0; k < N; k++)
            {
                chart1.Series[0].Points.AddXY(equation.arrtn[k], equation.arrXn[k]);
            }
            label11.Visible = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            label11.Text = "загрузка...";
            CalculateAll();
            button1.Enabled = true;
            label11.Text = "";

        }
        public void Error(string m)
        {
            MessageBox.Show(m, "Ошибка");
            button1.Enabled = true;

        }
    }
}
