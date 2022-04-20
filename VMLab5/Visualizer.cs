using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace VMLab5
{
    internal class Visualizer
    {
        private static Color analiticColor = Color.IndianRed;
        private static Color methodColor = Color.DeepSkyBlue;

        public static void InitGraph(ref Chart chart)
        {
            String[] names = { "Analitic Solution", "Analitic Approximation", "Method Solution", "Method Approximation" };

            for (int i = 0; i < names.Length; i++)
            {
                chart.Series.Add(names[i]);
                chart.Legends.Add(names[i]);
                chart.Legends[i].Position.Auto = false;
                chart.Legends[i].Position = new ElementPosition(5, 95, 90, 5);
                chart.Legends[i].Enabled = false;
            }

            for (int i = 0; i < names.Length; i += 2)
            {
                chart.Series[i].ChartType = SeriesChartType.Point;
                chart.Series[i].MarkerStyle = MarkerStyle.Circle;
                chart.Series[i].Color = analiticColor;

                chart.Series[i + 1].ChartType = SeriesChartType.Spline;
                chart.Series[i + 1].Color = analiticColor;
            }

            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "{0.00}";

            chart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

            chart.Series[0].Color = analiticColor; chart.Series[2].Color = methodColor;
            chart.Series[1].Color = analiticColor; chart.Series[3].Color = methodColor;
        }

        public static void PrintPoints(ref Chart chart, Point[] points, int seriesNumber)
        {
            chart.Series[seriesNumber].Points.Clear();
            chart.Legends[seriesNumber].Enabled = true;

            foreach (Point point in points)
                chart.Series[seriesNumber].Points.AddXY(point.X, point.Y);
        }
    }
}
