using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Globalization;

namespace Control
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public event ValueChangedHandler ValueChanged;
        public delegate void ValueChangedHandler(NumericUpDown m, double NewValue);

        public NumericUpDown()
        {
            InitializeComponent();
            upBtn.Click += upBtn_Click;
            downBtn.Click += downBtn_Click;
            valTxt.TextChanged += valTxt_TextChanged;
            _min = 0;
            _max = 100;
            Step = 1;
            _val = 50;
        }
        double _min, _max, _val;
        public double Value
        {
            get { return _val; }
            set
            {
                if (value < _min || value > _max) return;
                _val = value;
                if (ValueChanged != null)
                    ValueChanged(this, _val);
                valTxt.Text = value.ToString();
            }
        }
        public double Min
        {
            get { return _min; }
            set
            {
                if (value < _max) return;
                _min = value;
                if (Value < _min)
                    Value = _min;
            }
        }
        public double Max
        {
            get { return _max; }
            set
            {
                if (value < _min) return;
                _max = value;
                if (Value > _max)
                    Value = _max;
            }
        }
        public double Step { get; set; }
        void valTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            double tmp;
            if (!double.TryParse(valTxt.Text, out tmp)) return;
            if (tmp < _min)
                valTxt.Text = _min.ToString();
            if (tmp > _max)
                valTxt.Text = _max.ToString();
            _val = Convert.ToDouble(valTxt.Text);
        }

        void downBtn_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
        }

        void upBtn_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
        }

    }
}
namespace Psim.Converters
{
    public class PercentageConverter : MarkupExtension, IValueConverter
    {
        private static PercentageConverter _instance;

        public PercentageConverter()
        {

        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new PercentageConverter());
        }
    }
    public class SmartConverter : MarkupExtension, IValueConverter
    {
        private static SmartConverter _instance;

        public SmartConverter()
        {

        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Min(System.Convert.ToDouble(value) * 0.40, System.Convert.ToDouble(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new SmartConverter());
        }
    }
    public class SmartConverter2 : MarkupExtension, IValueConverter
    {
        private static SmartConverter2 _instance;

        public SmartConverter2()
        {

        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Max(System.Convert.ToDouble(value) * 0.60, System.Convert.ToDouble(value)-System.Convert.ToDouble(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new SmartConverter2());
        }
    }
}
