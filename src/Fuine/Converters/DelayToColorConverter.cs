namespace Fuine.Converters;
public class DelayToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int inValue)
        {
            return inValue switch
            {
                0 => "#758a99",
                < 300 => "#21a675",
                < 700 => "#d9b611",
                < 1200 => "#ff8936",
                _ => "#ff3300",
            };
        }

        return "#758a99";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
