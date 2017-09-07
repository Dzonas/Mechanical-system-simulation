using System;

namespace mmm_projekt
{
    public class Model
    {
        // Wspolczynniki ukladu
        public struct Coefficients
        {
            public double k1;
            public double k2;
            public double l1;
            public double l2;
            public double b;
            public double mass;
            public double simulation_step;
            public double simulation_time;
            public double amplitude;
            public double period;
            public double constant;
            public double init_x1;
            public double init_x2;
            public double init_v;
            public int size;
            public INPUT_FUNC input_func;

            public void calculate_size()
            {
                size = (int)(simulation_time / simulation_step);
            }
        }

        public Coefficients wspolczynniki;
        public double[] wspolrzedne_x1;
        public double[] wspolrzedne_x2;
        public double[] t;
        public delegate double INPUT_FUNC(double t);

        public Model()
        {
            wspolczynniki = new Coefficients()
            {
                k1 = 1.0,
                k2 = 1.0,
                l1 = 1.0,
                l2 = 1.0,
                b = 1.0,
                mass = 1.0,
                simulation_step = 0.1,
                simulation_time = 100.0,
                amplitude = 1.0,
                period = 1.0,
                constant = 0.0,
                init_x1 = 0.0,
                init_x2 = 0.0
            };

            wspolczynniki.calculate_size();
        }

        public bool Calculate()
        {
            double czas_koncowy = wspolczynniki.simulation_time; // Koniec zakresu calkowania
            double krok = wspolczynniki.simulation_step; // Krok calkowania
            double a0, b0, c0, a1, b1, c1, a2, b2, c2, a3, b3, c3; // Wspolczynniki do metody calkowania
            wspolczynniki.calculate_size();
            int size = wspolczynniki.size;
            double k1 = wspolczynniki.k1;
            double k2 = wspolczynniki.k2;
            double l1 = wspolczynniki.l1;
            double l2 = wspolczynniki.l2;
            double b = wspolczynniki.b;
            double m = wspolczynniki.mass;
            double x1, x2, v;
            wspolrzedne_x1 = new double[size];
            wspolrzedne_x2 = new double[size];
            double[] wspolrzedne_v = new double[size];
            t = new double[size];


            // Warunki poczatkowe
            /*
            wspolrzedne_x1[0] = wspolczynniki.init_x1;
            wspolrzedne_x2[0] = wspolczynniki.init_x2;
            wspolrzedne_v[0] = wspolczynniki.init_v;*/

            wspolrzedne_x1[0] = 0;
            wspolrzedne_x2[0] = 0;
            wspolrzedne_v[0] = 0;

            // Inicjalizacja wektora X rozwiazania
            for (int i = 0; i < size; i++)
            {
                t[i] = krok * i;
            }
            // Algorytm całkowania
            for (int i = 0; i < size - 1; i++)
            {
                x1 = wspolrzedne_x1[i];
                x2 = wspolrzedne_x2[i];
                v = wspolrzedne_v[i];

                a0 = krok * x1_prim(t[i], x1, x2, v);
                b0 = krok * x2_prim(t[i], x1, x2, v);
                c0 = krok * v_prim(t[i], x1, x2, v);

                a1 = krok * x1_prim(t[i] + 0.5 * krok, x1 + 0.5 * a0, x2 + 0.5 * b0, v + 0.5 * c0);
                b1 = krok * x2_prim(t[i] + 0.5 * krok, x1 + 0.5 * a0, x2 + 0.5 * b0, v + 0.5 * c0);
                c1 = krok * v_prim(t[i] + 0.5 * krok, x1 + 0.5 * a0, x2 + 0.5 * b0, v + 0.5 * c0);

                a2 = krok * x1_prim(t[i] + 0.5 * krok, x1 + 0.5 * a1, x2 + 0.5 * b1, v + 0.5 * c1);
                b2 = krok * x2_prim(t[i] + 0.5 * krok, x1 + 0.5 * a1, x2 + 0.5 * b1, v + 0.5 * c1);
                c2 = krok * v_prim(t[i] + 0.5 * krok, x1 + 0.5 * a1, x2 + 0.5 * b1, v + 0.5 * c1);

                a3 = krok * x1_prim(t[i] + krok, x1 + a2, x2 + b2, v + c2);
                b3 = krok * x2_prim(t[i] + krok, x1 + a2, x2 + b2, v + c2);
                c3 = krok * v_prim(t[i] + krok, x1 + a2, x2 + b2, v + c2);

                wspolrzedne_x1[i + 1] = wspolrzedne_x1[i] + (1.0 / 6.0) * (a0 + 2 * a1 + 2 * a2 + a3);
                wspolrzedne_x2[i + 1] = wspolrzedne_x2[i] + (1.0 / 6.0) * (b0 + 2 * b1 + 2 * b2 + b3);
                wspolrzedne_v[i + 1] = wspolrzedne_v[i] + (1.0 / 6.0) * (c0 + 2 * c1 + 2 * c2 + c3);

                if (Double.IsInfinity(wspolrzedne_x1[i + 1]) || Double.IsInfinity(wspolrzedne_x2[i + 1])
                    || Double.IsInfinity(wspolrzedne_v[i + 1]) || Double.IsNaN(wspolrzedne_x1[i + 1])
                    || Double.IsNaN(wspolrzedne_v[i + 1]) || Double.IsNaN(wspolrzedne_x2[i + 1]))
                    return false;
            }

            for (int i = 0; i < size; i++)
            {
                wspolrzedne_x1[i] += wspolczynniki.init_x1;
                wspolrzedne_x2[i] += wspolczynniki.init_x2;
            }

            return true;
        }

        private double x1_prim(double t, double x1, double x2, double v)
        {
            double wartosc = 0.0;
            double b = wspolczynniki.b;
            double k2 = wspolczynniki.k2;
            double l2 = wspolczynniki.l2;

            wartosc = (-k2 / b) * x1 - (l2 / b) * Math.Pow(x1, 3) + (k2 / b) * x2
                + (l2 / b) * Math.Pow(x2, 3);

            return wartosc;
        }

        private double v_prim(double t, double x1, double x2, double v)
        {
            double wartosc = 0.0;
            double b = wspolczynniki.b;
            double m = wspolczynniki.mass;
            double k1 = wspolczynniki.k1;
            double k2 = wspolczynniki.k2;
            double l1 = wspolczynniki.l1;
            double l2 = wspolczynniki.l2;

            wartosc = (1 / m) * wspolczynniki.input_func(t) - (k2 / m) * x2 - (l2 / m) * Math.Pow(x2, 3) + (k2 / m) * x1
                + (l2 / m) * Math.Pow(x1, 3) - (k1 / m) * x2 - (l1 / m) * Math.Pow(x2, 3);

            return wartosc;
        }

        private double x2_prim(double t, double x1, double x2, double v)
        {
            return v;
        }

        public double Input_square(double t)
        {
            double wartosc = 0.0;
            double amplituda = wspolczynniki.amplitude;
            double okres = wspolczynniki.period;
            double stala = wspolczynniki.constant;

            while (t > okres)
                t -= okres;

            if (t <= okres / 2)
                wartosc = amplituda;

            return wartosc + stala;
        }

        public double Input_triangle(double t)
        {
            double wartosc = 0.0;
            double amplituda = wspolczynniki.amplitude;
            double okres = wspolczynniki.period;
            double stala = wspolczynniki.constant;

            if (okres > 0)
            {
                while (t > okres)
                    t -= okres;

                if (t <= okres / 2)
                {
                    wartosc = amplituda * (2 / okres) * t;
                }
                else if (t > okres / 2 && t < okres)
                {
                    wartosc = amplituda * ((-2 / okres) * t + 2);
                }
            }

            return wartosc + stala;
        }

        public double Input_sine(double t)
        {
            double A = wspolczynniki.amplitude;
            double okres = wspolczynniki.period;
            double stala = wspolczynniki.constant;
            return A * (Math.Sin((2 * Math.PI / okres) * t - Math.PI / 2) + 1) + stala;
        }

        public double Input_step(double t)
        {
            return 1.0;
        }

        public double[] get_input_func()
        {
            double czas_koncowy = wspolczynniki.simulation_time;
            double krok = wspolczynniki.simulation_step;
            double t = 0.0;
            wspolczynniki.calculate_size();
            double[] points = new double[wspolczynniki.size];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = wspolczynniki.input_func(t);
                t += krok;
            }

            return points;
        }
    }
}