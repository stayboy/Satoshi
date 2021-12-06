using System.ComponentModel;
using System.Reflection;

namespace Satoshi.Converters;
public class ListBinder<T>: List<T>
{
    public static bool TryParse(string value, IFormatProvider provider, out ListBinder<T> model)
    {
        if (string.IsNullOrEmpty(value))
        {
            model = null;
            return false;
        }

        var genericType = typeof(T).GetTypeInfo(); //.GenericTypeArguments[0];
        var converter = TypeDescriptor.GetConverter(genericType);

        var trimmedValue = value?.TrimStart('(').TrimEnd(')');
        var segments = trimmedValue?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => converter.ConvertFromString(x.Trim())).ToArray();
        if (segments.Length > 0)
        {
            var a_array = Array.CreateInstance(genericType, segments.Length);
            segments.CopyTo(a_array, 0);

            Type genericListType = typeof(List<>);
            Type concreteListType = genericListType.MakeGenericType(genericType);

            object list = Activator.CreateInstance(concreteListType, new object[] { a_array });
            model = new ListBinder<T>();
            model.AddRange((List<T>)list);

            //model = (List<T>)list;
            return true;
        }

        model = null;
        return false;
    }
}
 