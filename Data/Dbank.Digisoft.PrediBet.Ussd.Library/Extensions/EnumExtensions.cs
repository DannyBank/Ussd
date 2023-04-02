using System;
using System.ComponentModel;
using System.Reflection;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Extensions {
    public static class EnumExtensions {
        public static string ToDescriptionString(this Enum value) {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),
                    false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        public static string GetPropertyDescription<T>(this T parent, string fieldName) {
            string result;
            var fi = typeof(T).GetProperty(fieldName);
            if (fi != null) {
                try {
                    object[] descriptionAttrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    DescriptionAttribute description = (DescriptionAttribute)descriptionAttrs[0];
                    result = (description.Description);
                }
                catch {
                    result = null;
                }
            }
            else {
                result = null;
            }

            return result;
        }

        public static T GetEnumDefaultValue<T>() where T : struct {
            DefaultValueAttribute[] attributes = typeof(T).GetCustomAttributes(typeof(DefaultValueAttribute), false) as DefaultValueAttribute[];

            if (attributes != null && attributes.Length > 0) {
                return (T)attributes[0].Value;
            }
            return default(T);
        }
    }
}
