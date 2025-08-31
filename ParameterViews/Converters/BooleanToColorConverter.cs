using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ParameterViews.Converters
{
    public class BooleanToColorConverter : BooleanConverter<Brush>
    {
        public BooleanToColorConverter() :
            base(Brushes.Red, Brushes.Transparent)
        { }
    }
}
