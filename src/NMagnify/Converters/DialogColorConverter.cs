using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Caliburn.Micro.Contrib.Dialogs;

namespace NMagnify.Converters
{
    public class DialogColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dialogtype = (DialogType) value;
            switch (dialogtype)
            {
                case DialogType.None:
                    return Colors.Black;
                case DialogType.Question:
                    return Color.FromRgb(59,171,219);
                case DialogType.Warning:
                    return Colors.Yellow;
                case DialogType.Information:
                    return Color.FromRgb(59, 171, 219);
                case DialogType.Error:
                    return Colors.Red;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}