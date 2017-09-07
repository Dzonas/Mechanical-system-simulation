using System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mmm_projekt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void symulacja_button_Click(object sender, RoutedEventArgs e)
        {
            bool error = await isError();

            if (!error)
                Frame.Navigate(typeof(ChartPage));
        }

        private async Task<bool> isError()
        {
            bool error = false;

            try
            {
                //Wspolczynniki modelu
                App.model.wspolczynniki.k1 = Double.Parse(k1_box.Text);
                App.model.wspolczynniki.k2 = Double.Parse(k2_box.Text);
                App.model.wspolczynniki.l1 = Double.Parse(l1_box.Text);
                App.model.wspolczynniki.l2 = Double.Parse(l2_box.Text);
                App.model.wspolczynniki.b = Double.Parse(b_box.Text);
                App.model.wspolczynniki.mass = Double.Parse(masa_box.Text);
                App.model.wspolczynniki.simulation_step = Double.Parse(krok_symulacji_box.Text);
                App.model.wspolczynniki.simulation_time = Double.Parse(czas_symulacji_box.Text);
                App.model.wspolczynniki.amplitude = Double.Parse(amplituda_box.Text);
                App.model.wspolczynniki.period = Double.Parse(okres_box.Text);
                App.model.wspolczynniki.constant = Double.Parse(skladowa_stala_box.Text);
                App.model.wspolczynniki.init_x1 = Double.Parse(x1_init_text_box.Text);
                App.model.wspolczynniki.init_x2 = Double.Parse(x2_init_text_box.Text);
                App.model.wspolczynniki.init_v = Double.Parse(v_init_text_box.Text);

                if ((bool)square_radio.IsChecked)
                {
                    App.model.wspolczynniki.input_func = new Model.INPUT_FUNC(App.model.Input_square);
                }
                else if ((bool)triangle_radio.IsChecked)
                {
                    App.model.wspolczynniki.input_func = new Model.INPUT_FUNC(App.model.Input_triangle);
                }
                else
                {
                    App.model.wspolczynniki.input_func = new Model.INPUT_FUNC(App.model.Input_sine);
                }
                if (App.model.wspolczynniki.k1 < 0 || App.model.wspolczynniki.k2 < 0 || App.model.wspolczynniki.l1 < 0 || App.model.wspolczynniki.l2 < 0)
                    throw new IOException("Parametry układu mechanicznego muszą być dodatnie.");
                if(App.model.wspolczynniki.b <= 0 || App.model.wspolczynniki.mass <= 0)
                    throw new IOException("Parametry \"m\" i \"b\" muszą być dodatnie i większe od 0.");
                if (App.model.wspolczynniki.simulation_step < 0 || App.model.wspolczynniki.simulation_step > 1)
                    throw new IOException("Krok musi być między 0, a 1.");
                if (App.model.wspolczynniki.simulation_time < 0)
                    throw new IOException("Czas symulacji nie może być ujemny.");
                if (App.model.wspolczynniki.amplitude < 0)
                    throw new IOException("Amplituda nie może być ujemna.");
                if (App.model.wspolczynniki.period < 0)
                    throw new IOException("Okres nie może być ujemny.");

            }
            catch (IOException e)
            {
                var messageDialog = new MessageDialog(e.Message);
                messageDialog.Commands.Add(new UICommand("Spróbuj ponownie."));
                messageDialog.DefaultCommandIndex = 0;
                error = true;

                await messageDialog.ShowAsync();
            }
            catch (FormatException)
            {
                var messageDialog = new MessageDialog("Niewłaściwe dane wejściowe.");
                messageDialog.Commands.Add(new UICommand("Spróbuj ponownie."));
                messageDialog.DefaultCommandIndex = 0;
                error = true;

                await messageDialog.ShowAsync();
            }

            return error;
        }

        private void mainPage_Loaded(object sender, RoutedEventArgs e)
        {
            k1_box.Text = App.model.wspolczynniki.k1.ToString();
            k2_box.Text = App.model.wspolczynniki.k2.ToString();
            l1_box.Text = App.model.wspolczynniki.l1.ToString();
            l2_box.Text = App.model.wspolczynniki.l2.ToString();
            b_box.Text = App.model.wspolczynniki.b.ToString();
            masa_box.Text = App.model.wspolczynniki.mass.ToString();
            krok_symulacji_box.Text = App.model.wspolczynniki.simulation_step.ToString();
            czas_symulacji_box.Text = App.model.wspolczynniki.simulation_time.ToString();
            amplituda_box.Text = App.model.wspolczynniki.amplitude.ToString();
            okres_box.Text = App.model.wspolczynniki.period.ToString();
            skladowa_stala_box.Text = App.model.wspolczynniki.constant.ToString();
            x1_init_text_box.Text = App.model.wspolczynniki.init_x1.ToString();
            x2_init_text_box.Text = App.model.wspolczynniki.init_x2.ToString();
            v_init_text_box.Text = App.model.wspolczynniki.init_v.ToString();

            if (App.model.wspolczynniki.input_func == App.model.Input_square)
            {
                square_radio.IsChecked = true;
                triangle_radio.IsChecked = false;
                sine_radio.IsChecked = false;
            }

            else if (App.model.wspolczynniki.input_func == App.model.Input_triangle)
            {
                square_radio.IsChecked = false;
                triangle_radio.IsChecked = true;
                sine_radio.IsChecked = false;
            }

            else if (App.model.wspolczynniki.input_func == App.model.Input_sine)
            {
                square_radio.IsChecked = false;
                triangle_radio.IsChecked = false;
                sine_radio.IsChecked = true;
            }
        }
    }
}
