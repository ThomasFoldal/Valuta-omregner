using System.Xml;
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
        Dictionary<string, double> units = new Dictionary<string, double>() { { "DDK", 100.0 } };
        public XmlDocument document = new XmlDocument();
        public MainWindow()
        {
            XmlTextReader textReader = new XmlTextReader("https://www.nationalbanken.dk/_vti_bin/DN/DataService.svc/CurrencyRatesXML?lang=da");
            while (textReader.Read())
            {
                string a;
                string b;
                double c;
                switch (textReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (textReader.Name == "currency")
                        {
                            textReader.MoveToNextAttribute();
                            a = textReader.Value;
                            textReader.MoveToNextAttribute();
                            b = textReader.Value;
                            textReader.MoveToNextAttribute();
                            if (textReader.Value == "-")
                            {
                                break;
                            }
                            else
                            {
                                c = Convert.ToDouble(textReader.Value);
                            }
                            units.Add(a, c);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        break;
                }
            }
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
            ShowResultText();
        }
        private void ShowResultText()
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

        private void CBLeft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowResultText();
        }

        private void CBRight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowResultText();
        }
    }
}
