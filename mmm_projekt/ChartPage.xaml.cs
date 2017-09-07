using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace mmm_projekt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChartPage : Page
    {
        public double[] u_points;
        private bool drawX1, drawX2, drawU, autoSize;
        private double uMax, x1Max, x2Max;
        private bool loaded;
        Rysowanie rysowanie;

        public ChartPage()
        {
            this.InitializeComponent();
        }

        private void chartPage_Loaded(object sender, RoutedEventArgs e)
        {
            u_points = App.model.get_input_func();

            //Integrate function
            if(!App.model.Calculate())
            {
                checkBoxAutoSize.IsEnabled = false;
                checkBoxU.IsEnabled = false;
                checkBoxX1.IsEnabled = false;
                checkBoxX2.IsEnabled = false;
            }
            else
            {
                //Get max from arrays
                uMax = u_points.Max();
                x1Max = App.model.wspolrzedne_x1.Max();
                x2Max = App.model.wspolrzedne_x2.Max();

                rysowanie = new Rysowanie(chart_canvas);
                rysowanie.Width_multiplier = chart_canvas.ActualWidth / App.model.t.Max();
                double max = uMax;
                if (x1Max > max)
                    max = x1Max;
                if (x2Max > max)
                    max = x2Max;
                rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                DrawCharts();
                loaded = true;
            }
        }

        public void DrawCharts()
        {
            chart_canvas.Children.Clear();

            AutoSize();

            if (drawU)
                rysowanie.DrawChart(App.model.t, u_points, 0, (int)(chart_canvas.ActualHeight / 2), Colors.Red);
            if (drawX1)
                rysowanie.DrawChart(App.model.t, App.model.wspolrzedne_x1, 0, (int)(chart_canvas.ActualHeight / 2), Colors.Blue);
            if (drawX2)
                rysowanie.DrawChart(App.model.t, App.model.wspolrzedne_x2, 0, (int)(chart_canvas.ActualHeight / 2), Colors.Green);

            rysowanie.DrawScale(0, (int)(chart_canvas.ActualHeight / 2));
        }

        private void AutoSize()
        {
            if (autoSize)
            {
                if (drawU && drawX1 && drawX2)
                {
                    double max = uMax;
                    if (x1Max > max)
                        max = x1Max;
                    if (x2Max > max)
                        max = x2Max;
                    rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                }
                else if(drawU && drawX1)
                {
                    double max = uMax;
                    if (x1Max > max)
                        max = x1Max;
                    rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                }
                else if(drawU && drawX2)
                {
                    double max = uMax;
                    if (x2Max > max)
                        max = x2Max;
                    rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                }
                else if (drawX1 && drawX2)
                {
                    double max = x1Max;
                    if (x2Max > max)
                        max = x2Max;
                    rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                }
                else if (drawU)
                {
                    double max = uMax;
                    rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                }
                else if (drawX1)
                {
                    double max = x1Max;
                    rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                }
                else if (drawX2)
                {
                    double max = x2Max;
                    rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
                }
            }
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void checkBoxU_Checked(object sender, RoutedEventArgs e)
        {
            drawU = true;
            if (loaded)
                DrawCharts();
        }

        private void checkBoxX1_Checked(object sender, RoutedEventArgs e)
        {
            drawX1 = true;
            if (loaded)
                DrawCharts();
        }

        private void checkBoxX2_Checked(object sender, RoutedEventArgs e)
        {
            drawX2 = true;
            if (loaded)
                DrawCharts();
        }

        private void checkBoxU_Unchecked(object sender, RoutedEventArgs e)
        {
            drawU = false;
            if (loaded)
                DrawCharts();
        }

        private void checkBoxAutoSize_Checked(object sender, RoutedEventArgs e)
        {
            autoSize = true;
            if (loaded)
                DrawCharts();
        }

        private void checkBoxAutoSize_Unchecked(object sender, RoutedEventArgs e)
        {
            autoSize = false;
            double max = u_points.Max();
            rysowanie.Height_multiplier = (chart_canvas.ActualHeight / 2) / max;
            if (loaded)
                DrawCharts();
        }

        private void checkBoxX1_Unchecked(object sender, RoutedEventArgs e)
        {
            drawX1 = false;
            if (loaded)
                DrawCharts();
        }

        private void checkBoxX2_Unchecked(object sender, RoutedEventArgs e)
        {
            drawX2 = false;
            if (loaded)
                DrawCharts();
        }
    }
}
