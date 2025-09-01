﻿using System.Globalization;
using System.Windows.Data;

namespace ParameterViews.Converters
{
    public class ClampHeightConverter : IValueConverter
    {
        public double MaxHeight { get; set; } = 250;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualHeight)
            {
                return Math.Min(actualHeight, MaxHeight);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
