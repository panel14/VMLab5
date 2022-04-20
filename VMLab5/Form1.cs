using System;
using System.Drawing;
using System.Windows.Forms;

namespace VMLab5
{
    public partial class Form1 : Form
    {
        private TextBox[] initValues = new TextBox[2];

        private int equationNumber = 0;

        public Form1()
        {
            InitializeComponent();
            Visualizer.InitGraph(ref chart1);
        }

        private void setBadStatus(String status)
        {
            statusLabel.Text = status;
            statusLabel.ForeColor = Color.Red;
        }

        private void setGoodStatus(String status)
        {
            statusLabel.Text = status;
            statusLabel.ForeColor = Color.Green;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double currentX = Double.Parse(xInitTBox.Text);
            double currentY = Double.Parse(yInitTBox.Text);

            yInitTBox.Text = currentY.ToString();

            Point initPoint = new Point(currentX, currentY);

            double[] borders = { initPoint.X, trackBar1.Value };

            Point[] solutionPoints = Solver.SolveByImprovedEulerMethod(borders, equationNumber, (int)numericUpDown1.Value, initPoint);
            Point[] analiticPoints = Solver.GetAnaliiticSolution(borders, equationNumber, (int)numericUpDown1.Value, initPoint);

            Point[] approxSolutionPoints = Solver.Approximate(solutionPoints);
            Point[] approxAnaliticPoints = Solver.Approximate(analiticPoints);

            Visualizer.PrintPoints(ref chart1, analiticPoints, 0);
            Visualizer.PrintPoints(ref chart1, approxAnaliticPoints, 1);

            Visualizer.PrintPoints(ref chart1, solutionPoints, 2);
            Visualizer.PrintPoints(ref chart1, approxSolutionPoints, 3);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initValues[0] = xInitTBox; initValues[1] = yInitTBox;
        }

        private void xInitTBox_TextChanged(object sender, EventArgs e)
        {
            double x, y;

            if (!Double.TryParse(xInitTBox.Text, out x))
            {
                setBadStatus("Неверный формат ввода X.");
                button1.Enabled = false;
                return;
            }

            if (!Double.TryParse(xInitTBox.Text, out y))
            {
                setBadStatus("Неверный формат ввода Y.");
                button1.Enabled = false;
                return;
            }

            setGoodStatus("ok.");

            button1.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            equationNumber = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            equationNumber = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            equationNumber = 2;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Maximum = (int)numericUpDown2.Value;
        }
    }
}
