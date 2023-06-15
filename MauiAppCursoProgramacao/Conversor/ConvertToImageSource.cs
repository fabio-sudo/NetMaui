using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppCursoProgramacao.Conversor
{
    public class ConvertToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return (value == null || ((byte[])value).Length == 0) ? "noimagen.jpg" :
            ImageSource.FromStream(() => new MemoryStream((byte[])value));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
