using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Helpers
{
    public static class Mapper
    {
        /// <summary>
        /// Copy all not null properties values of object source in object target if the properties are present.
        /// Use this method to copy only simple type properties, not collections.
        /// </summary>
        /// <param name="source">source object</param>
        /// <param name="target">target object</param>
        public static T SimpleCopy<T>(object source, T target = null, bool nullableTarget = true) where T : class, new()
        {
            if (source == null)
            {
                if (nullableTarget)
                {
                    return default!;

                }
                else
                {
                    return new T();
                }
            }
            if (target == null)
            {
                target = new T();
            }
            foreach (PropertyInfo pi in source.GetType().GetProperties())
            {
                object propValue = pi?.GetGetMethod()?.Invoke(source, null);
                if (propValue != null)
                {
                    try
                    {
                        PropertyInfo pit = GetTargetProperty(pi?.Name ?? "", target);
                        if (pit != null) pit.GetSetMethod()?.Invoke(target, new object[] { propValue });
                    }
                    catch (Exception) { /* do nothing */ }
                }
            }
            return target;
        }

        private static PropertyInfo GetTargetProperty(string name, object target)
        {
            foreach (PropertyInfo pi in target.GetType().GetProperties())
            {
                if (pi.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) return pi;
            }
            return default!;
        }
    }
}
