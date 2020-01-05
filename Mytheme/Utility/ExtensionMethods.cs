using System;
using System.ComponentModel;
using System.Reflection;
using Mytheme.Data.Dto;
using Serilog;

namespace Mytheme.Utility
{
    public static class Logger
    {
        public static void LogException(Exception ex, string message)
        {
            Log.Error($"{message} ex: {ex.Message}");
            Log.Debug(ex.StackTrace);
        }
    }

    public static class ExtensionMethods
    {
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        public static SectionType GetSubSectionType(this SectionType sectionType)
        {
            switch (sectionType)
            {
                case SectionType.Campaign:
                    return SectionType.Adventure;
                case SectionType.Adventure:
                    return SectionType.Section;
                case SectionType.Section:
                    return SectionType.Chapter;
                default:
                    return SectionType.None;
            }
        }

    }
}
