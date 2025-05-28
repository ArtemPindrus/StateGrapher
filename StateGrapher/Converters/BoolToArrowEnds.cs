using Nodify;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StateGrapher.Converters
{
    /// <summary>
    /// Converts bool to <see cref="ArrowHeadEnds"/>. true -> BothEnds; false -> End
    /// </summary>
    [ValueConversion(typeof(bool), typeof(ArrowHeadEnds))]
    public class BoolToArrowEnds : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool b = (bool)value;

            return b ? ArrowHeadEnds.Both : ArrowHeadEnds.End;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
