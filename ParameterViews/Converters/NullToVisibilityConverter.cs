using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ParameterViews.Converters
{
    public class NullToVisibilityConverter : NullConverter<Visibility>
    {
        public NullToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
}
