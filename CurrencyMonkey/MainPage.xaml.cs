using System;
using System.Linq;
using System.Windows;
using CurrencyMonkey.CurrencyService;
using Microsoft.Phone.Controls;

namespace CurrencyMonkey
{
    public partial class MainPage : PhoneApplicationPage
    {
        String[] Currency = CurrencyTypeData.Currencies.Keys.ToArray();
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.lpkSourceCurrency.ItemsSource = Currency;
            this.lpkTargetCurrency.ItemsSource = Currency;
        }

        private void ConvertCurrency_Click(object sender, RoutedEventArgs e)
        {
            double parsed;
            var wasParsed = double.TryParse(SourceCurrencyValue.Text, out parsed);
            if (! wasParsed)
            {
                Result.Text = "Enter a valid value";
                return;
            }
            var currencySource = (Currency) Enum.Parse(typeof (Currency), lpkSourceCurrency.SelectedItem.ToString());
            var currencyTarget = (Currency) Enum.Parse(typeof (Currency), lpkTargetCurrency.SelectedItem.ToString());
            var service = new CurrencyConvertorSoapClient();
            service.ConversionRateCompleted += service_ConversionRateCompleted;
            service.ConversionRateAsync(currencySource, currencyTarget);
        }

        private void service_ConversionRateCompleted(object sender, ConversionRateCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // try to resort to local data or issue error 
                // for now local error
                Result.Text = "Error Calling Service";
            }
            else
            {
                // save result to local storage in case of going offline - save date in case of emergency
                Result.Text = ( double.Parse(SourceCurrencyValue.Text) * e.Result )+ " " + lpkTargetCurrency.SelectedItem;
            }
        }
    }
}