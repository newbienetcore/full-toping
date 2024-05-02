using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SharedKernel.Libraries;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedKernel.Libraries
{
    public static class ExtensionMethod
    {
        #region Dictionary
        public static void RenameDictionaryKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey fromKey, TKey toKey)
        {
            TValue value = dict[fromKey];
            dict.Remove(fromKey);
            dict[toKey] = value;
        }
        #endregion

        #region Object
        /// <summary>
        /// Convert object to dictionary
        /// CreatedBy: nvcuong (16/05/2022)
        /// </summary>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            IEnumerable<PropertyInfo> properties = obj.GetType().GetProperties().Where(item => item.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop in properties)
            {
                dictionary.Add(prop.Name, prop.GetValue(obj));
            }
            return dictionary;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this object obj)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            IEnumerable<PropertyInfo> properties = obj.GetType().GetProperties().Where(item => item.GetIndexParameters().Length == 0);
            foreach (PropertyInfo prop in properties)
            {
                dictionary.Add((TKey)Convert.ChangeType(prop.Name, typeof(TKey)), (TValue)Convert.ChangeType(prop.GetValue(obj), typeof(TValue)));
            }
            return dictionary;
        }

        /// <summary>
        /// Downcast parent class to child class
        /// CreatedBy: nvcuong (02/05/2022)
        /// </summary>
        public static void Downcast<T>(this object parentInstance, out T childInstance)
        {
            var serializedParent = JsonConvert.SerializeObject(parentInstance);
            childInstance = JsonConvert.DeserializeObject<T>(serializedParent);
        }
        #endregion

        #region Type
        public static bool IsPrimitive(this Type type)
        {
            switch (type.Name)
            {
                case "Boolean":
                case "Byte":
                case "SByte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Char":
                case "Double":
                case "Single":
                    return true;

                default:
                    return false;
            }
        }
        #endregion

        #region Date
        public static string DateFullText(this DateTime date)
        {
            var ampm = date.ToString("tt", CultureInfo.InvariantCulture);
            var time = date.ToString("dd/MM/yyyy hh:mm:ss").Split(" ");
            return $"{time[0]} {time[1]}{ampm}";
        }

        public static int DiffDays(this DateTime date, DateTime date2)
        {
            var diffDays = Math.Ceiling(Math.Abs((date - date2).TotalDays));
            return Convert.ToInt32(diffDays);
        }
        #endregion

        #region T
        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialization method. NOTE: Private members are not cloned using this method.
        /// CreatedBy: nvcuong (02/05/2022)
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return default;

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var settings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), settings);
        }

        public static PropertyInfo HasAttribute<TEntity>(this TEntity entity, Type attribute)
        {
            return entity.GetType().GetProperties().FirstOrDefault(prop => prop.IsDefined(attribute, true));
        }

        public static IEnumerable<PropertyInfo> GetPropertyInfos<TEntity>(this TEntity entity)
        {
            return typeof(TEntity)
                    .GetProperties()
                    .Where(p => p.GetIndexParameters().Length == 0 && !p.IsDefined(typeof(IgnoreAttribute), true));
        }
        #endregion

        #region Enumeration + Constant
        /// <summary>
        /// Get description of enum
        /// </summary>
        public static string GetDescription(this Enum en)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = en.GetType().GetField(en.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return en.ToString();
        }

        public static List<object> GetEnumerationValues(this Type type)
        {
            var result = new List<object>();
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var field in fieldInfos)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    result.Add(field.GetValue(type)!);
                }
            }

            return result;
        }
        #endregion

        #region Attribute
        public static string GetDescription(this PropertyInfo property)
        {
            if (property == null)
                return "";

            if (!Attribute.IsDefined(property, typeof(DescriptionAttribute)))
                return "";

            return (Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description;
        }

        public static string GetDisplayText(this PropertyInfo property)
        {
            if (property == null)
                return "";

            if (!Attribute.IsDefined(property, typeof(DisplayTextAttribute)))
                return "";

            return (Attribute.GetCustomAttribute(property, typeof(DisplayTextAttribute)) as DisplayTextAttribute)?.Description;
        }
        #endregion

        #region IEnumerable
        public static IEnumerable<List<TSource>> ChunkList<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size", "size must be greater than 0");
            }

            var result = new List<List<TSource>>();
            for (int i = 0; i < source.Count(); i += size)
            {
                //yield return source.ToList().GetRange(i, Math.Min(size, source.Count() - i));
                result.Add(source.ToList().GetRange(i, Math.Min(size, source.Count() - i)));
            }
            return result;
        }
        #endregion

        #region HttpContext
        public static HttpContext Clone(this HttpContext httpContext, bool copyBody = true)
        {
            var existingRequestFeature = httpContext.Features.Get<IHttpRequestFeature>();
            var requestHeaders = new Dictionary<string, StringValues>(existingRequestFeature.Headers.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var header in existingRequestFeature.Headers)
            {
                requestHeaders[header.Key] = header.Value;
            }

            var requestFeature = new HttpRequestFeature
            {
                Protocol = existingRequestFeature.Protocol,
                Method = existingRequestFeature.Method,
                Scheme = existingRequestFeature.Scheme,
                Path = existingRequestFeature.Path,
                PathBase = existingRequestFeature.PathBase,
                QueryString = existingRequestFeature.QueryString,
                RawTarget = existingRequestFeature.RawTarget,
                Headers = new HeaderDictionary(requestHeaders),
            };

            if (copyBody)
            {
                // We need to buffer first, otherwise the body won't be copied
                // Won't work if the body stream was accessed already without calling EnableBuffering() first or without leaveOpen
                httpContext.Request.EnableBuffering();
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                requestFeature.Body = existingRequestFeature.Body;
            }

            var features = new FeatureCollection();
            features.Set<IHttpRequestFeature>(requestFeature);
            // Unless we need the response we can ignore it...
            //features.Set<IHttpResponseFeature>(new HttpResponseFeature());
            //features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(Stream.Null));

            var newContext = new DefaultHttpContext(features);
            if (copyBody)
            {
                // Rewind for any future use...
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            // Can happen if the body was not copied
            if (httpContext.Request.HasFormContentType && httpContext.Request.Form.Count != newContext.Request.Form.Count)
            {
                newContext.Request.Form = new FormCollection(httpContext.Request.Form.ToDictionary(f => f.Key, f => f.Value));
            }

            return newContext;
        }
        #endregion
    }
}
