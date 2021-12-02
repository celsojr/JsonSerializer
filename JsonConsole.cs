using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace JsonSerializer
{
    static class JsonConsole
    {
        private static bool IsRec = false;
        private static bool IsLast = false;
        private static readonly StringBuilder json = new("{");

        public static string Serialize(object model)
        {
            var myType = model.GetType();
	        var props = myType.GetProperties();

            foreach (var prop in props)
            {
                if (IsRec)
                {
                    var content = json.ToString();
                    json.Clear();
                    json.Append($"{content.TrimEnd(',')}}},");
                    IsRec = false;
                }

                var propValue = prop.GetValue(model, null);

                if (propValue.GetType() == typeof(string))
                {
                    AppendProp(prop.Name, $"\"{GetString(propValue)}\"");
                }
                else if (propValue.GetType() == typeof(int))
                {
                    AppendProp(prop.Name, $"{GetInt(propValue)}");
                }
                else if (propValue.GetType() == typeof(string[]))
                {
                    var items = string.Join("\",\"", GetStringArray(propValue as string[]));
                    AppendProp(prop.Name, $"[\"{items}\"]");
                }
                else if (propValue.GetType().IsAnonymousType())
                {
                    AppendProp(prop.Name, '{', false);
                    Serialize(propValue);
                    IsLast = props.Last().Name == prop.Name;
                    IsRec = true;
                }

                if (IsLast && props.Last().Name == prop.Name)
                {
                    var content = json.ToString();
                    json.Clear();
                    json.Append($"{content.TrimEnd(',')}}},");
                    IsLast = false;
                }
            }

            return $"{json.ToString().TrimEnd(',')}}}";
        }

        private static string GetString(object value)
        {
            return value.ToString();
        }

        private static int GetInt(object value)
        {
            return Convert.ToInt32(value);
        }

        private static IEnumerable<string> GetStringArray(string[] arr)
        {
            foreach (var str in arr)
            {
                yield return GetString(str);
            }
        }

        private static void AppendProp(string propName, object propValue, bool hasComa = true)
        {
            json.Append($"\"{propName}");
            json.Append("\":");
            json.Append(propValue);

            if (hasComa)
            {
                json.Append(',');
            }
        }

        private static bool IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            var isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }

    }
}
