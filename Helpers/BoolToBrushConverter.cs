/*
 * @Author: Arian Sjöström Shaafi
 * B.Sc Computer Science & Mobile IT
 */
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace Formula1Retro.Helpers
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isOn = (bool)value;
            string color = parameter?.ToString() ?? "Gray";

            if (isOn)
                return (SolidColorBrush)new BrushConverter().ConvertFromString(color)!;
            else
                return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

