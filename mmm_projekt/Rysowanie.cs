using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace mmm_projekt
{
    class Rysowanie
    {
        //Number of points on the canvas
        private const int N_POINTS = 512;
        private const int N_X_AXIS_LINES = 40;
        private const int N_Y_AXIS_LINES = 20;

        private Canvas canvas;

        //Multipliers that will fit chart into canvas
        private double width_multiplier;
        private double height_multiplier;

        public Rysowanie(Canvas canvas)
        {
            this.canvas = canvas;
            width_multiplier = 1;
            height_multiplier = 1;
        }

        public double Height_multiplier { get => height_multiplier; set => height_multiplier = value; }
        public double Width_multiplier { get => width_multiplier; set => width_multiplier = value; }

        public void DrawChart(double[] x_points, double[] y_points, int x, int y, Color color)
        {
            //If x and y points have diffrent sizes: return
            if (x_points.Length != y_points.Length)
                return;

            //Omit few samples if arrays are larger than N_POINTS
            int n = 1;
            if (y_points.Length > N_POINTS)
                n = y_points.Length / N_POINTS;

            SolidColorBrush stroke = new SolidColorBrush(color);

            for (int i = 0; i < y_points.Length - n; i += n)
            {
                Line line = new Line()
                {
                    Stroke = stroke,
                    StrokeThickness = 2,
                    X1 = x_points[i] * width_multiplier + x,
                    X2 = x_points[i + n] * width_multiplier + x,
                    Y1 = canvas.ActualHeight - (y_points[i] * height_multiplier + y),
                    Y2 = canvas.ActualHeight - (y_points[i + n] * height_multiplier + y)
                };
                Canvas.SetZIndex(line, 1);
                canvas.Children.Add(line);
            }
        }

        public void DrawScale(int x, int y)
        {
            Line xAxis = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 3,
                X1 = x,
                X2 = canvas.ActualWidth,
                Y1 = canvas.ActualHeight - y,
                Y2 = canvas.ActualHeight - y
            };
            canvas.Children.Add(xAxis);

            double k = canvas.ActualWidth / N_X_AXIS_LINES;
            for(int i = 0; i < N_X_AXIS_LINES; i++)
            {
                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2,
                    X1 = k * (i + 1),
                    X2 = k * (i + 1),
                    Y1 = canvas.ActualHeight - (y - 10),
                    Y2 = canvas.ActualHeight - (y + 10)
                };
                canvas.Children.Add(line);
            }

            Line yAxis = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 3,
                X1 = 0,
                X2 = 0,
                Y1 = canvas.ActualHeight,
                Y2 = 0
            };
            canvas.Children.Add(yAxis);

            k = canvas.ActualHeight / N_Y_AXIS_LINES;
            for (int i = 0; i < N_Y_AXIS_LINES + 1; i++)
            {
                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2,
                    X1 = x - 10,
                    X2 = x + 10,
                    Y1 = canvas.ActualHeight - (k * i),
                    Y2 = canvas.ActualHeight - (k * i)
                };
                canvas.Children.Add(line);
            }
        }
    }
}
