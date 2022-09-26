using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Valuta_omregner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, double> units = new Dictionary<string, double>()
        {
            {"m/s", 1.0}, {"km/h", 3.6}, {"ft/s", 3.28084}, {"knots", 1.94384}, {"mph", 2.236936}
        };
        public MainWindow()
        {
            InitializeComponent();
            CBLeft.ItemsSource = units.Keys;
            CBRight.ItemsSource = units.Keys;
            CBLeft.Text = "m/s";
            CBRight.Text = "km/h";
        }
        private double Calculate(double input, double from, double to)
        {
            double factor = to / from;
            double result =  input * factor;
            return Math.Round(result, 5);
        }

        private void TextLeft_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = TextLeft.Text;
            if (text == "")
            {
                TextRight.Text = "";
            }
            else
            {
                double input = Convert.ToDouble(text);
                string unitIn = CBLeft.Text;
                string unitOut = CBRight.Text;
                double from = units[unitIn];
                double to = units[unitOut];
                TextRight.Text = Convert.ToString(Calculate(input, from, to));
            }
        }
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }

        private bool AreAllValidNumericChars(string str)
        {
            bool deci = TextLeft.Text.Contains(',');
            foreach (char c in str)
            {
                if (Char.IsNumber(c) || c == ',')
                {
                    if (c == ',' && (TextLeft.Text == "" || deci))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private void TextLeft_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnPreviewTextInput(e);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string temp = CBLeft.Text;
            CBLeft.Text = CBRight.Text;
            CBRight.Text = temp;
        }
    }
}
