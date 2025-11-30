using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Eaship.page.Admin
{
    public class ContractStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString() ?? "";

            return status switch
            {
                "Pending" => new SolidColorBrush(Color.FromRgb(255, 243, 205)),
                "Approved" => new SolidColorBrush(Color.FromRgb(204, 229, 255)),
                "Completed" => new SolidColorBrush(Color.FromRgb(212, 237, 218)),
                "Cancelled" or "Rejected"
                             => new SolidColorBrush(Color.FromRgb(248, 215, 218)),
                _ => Brushes.LightGray,
            };
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
