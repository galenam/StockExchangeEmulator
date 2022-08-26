using System.ComponentModel.DataAnnotations;

public static class Helper {
    public static T GetEnumValueByDisplayName<T>(this string attributeName)
        where T : struct
    {
        var fInfos = typeof(T).GetFields();

        foreach (var fInfo in fInfos)
        {
            var attributes = (DisplayAttribute[])fInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                if (attributeName == attributes[0].Name)
                {
                    Enum.TryParse<T>(fInfo.Name, out T value);
                    return (T)value;
                }
            }
        }
        return default(T);
    }
 }