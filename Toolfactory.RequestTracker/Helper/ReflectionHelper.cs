using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Toolfactory.RequestTracker.Helper
{
    public static class ReflectionHelper
    {
        public static void CopyObject(ref object dest, object src)
        {
            if (null == src) throw new ArgumentNullException("src");
            if (null == dest) throw new ArgumentNullException("dest");
            var srcType = src.GetType();
            var destType = dest.GetType();
            var srcInfo = srcType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var destInfo = destType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var array = srcInfo;
            foreach (var t in array)
            {
                var name = t.Name;
                var val = t.GetValue(src);
                var array2 = destInfo;
                foreach (var t2 in from t2 in array2 let targetName = t2.Name where name.Equals(targetName) select t2)
                {
                    t2.SetValue(dest, val);
                    break;
                }
            }
        }
        public static void PrintMethod(ref object obj)
        {
            var t = obj.GetType();
            var mif = t.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var array = mif;
            foreach (var t2 in array)
            {
                Trace.WriteLine("method name: {0} ", t2.Name);
                Trace.WriteLine("from: {0}", t2.Module.Name);
                var p = t2.GetParameters();
                foreach (var t1 in p)
                {
                    Trace.WriteLine("parameter name: " + t1.Name);
                    Trace.WriteLine("parameter type: " + t1.ParameterType);
                }
                Trace.WriteLine("******************************************");
            }
        }

        public static void CopyProperty(ref object dest, object src, string fieldName)
        {
            if (null == src) throw new ArgumentNullException("src");
            if (null == dest) throw new ArgumentNullException("dest");
            if (null == dest) throw new ArgumentNullException("fieldName");
            var srcType = src.GetType();
            var destType = dest.GetType();
            var srcInfo = srcType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var destInfo = destType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (srcInfo == null || destInfo == null) return;
            var val = srcInfo.GetValue(src);
            destInfo.SetValue(dest, val);
        }
        public static void SetProperty(object instance, string propery, object value)
        {
            var properties = propery.Split('.');
            var type = instance.GetType();
            object[] index = null;
            PropertyInfo property = null;
            for (var i = 0; i < properties.Length; i++)
            {
                var indexValue = Regex.Match(properties[i], "(?<=\\[)(.*?)(?=\\])").Value;
                if (string.IsNullOrEmpty(indexValue))
                {
                    property = type.GetProperty(properties[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    index = null;
                }
                else
                {
                    property = type.GetProperty(properties[i].Replace(string.Format("[{0}]", indexValue), string.Empty), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    index = GetIndex(indexValue, property);
                }
                if (i < properties.Length - 1)
                {
                    instance = property.GetValue(instance, index);
                }
                type = instance.GetType();
            }
            if (property != null)
            {
                property.SetValue(instance, value, index);
            }
        }
        public static object GetProperty(object instance, string propery)
        {
            var properties = propery.Split('.');
            var type = instance.GetType();
            var array = properties;
            foreach (var p in array)
            {
                var indexValue = Regex.Match(p, "(?<=\\[)(.*?)(?=\\])").Value;
                PropertyInfo property;
                object[] index;
                if (string.IsNullOrEmpty(indexValue))
                {
                    property = type.GetProperty(p, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    index = null;
                }
                else
                {
                    property = type.GetProperty(p.Replace(string.Format("[{0}]", indexValue), string.Empty), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    index = GetIndex(indexValue, property);
                }
                instance = property.GetValue(instance, index);
                type = instance.GetType();
            }
            return instance;
        }
        private static object[] GetIndex(string indicesValue, PropertyInfo property)
        {
            var parameters = indicesValue.Split(',');
            var parameterTypes = property.GetIndexParameters();
            var index = new object[parameterTypes.Length];
            for (var i = 0; i < parameterTypes.Length; i++)
            {
                index[i] = (parameterTypes[i].ParameterType.IsEnum ? Enum.Parse(parameterTypes[i].ParameterType, parameters[i]) : Convert.ChangeType(parameters[i], parameterTypes[i].ParameterType));
            }
            return index;
        }
    }
}