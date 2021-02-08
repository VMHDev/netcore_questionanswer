using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionAnswer.Utilities
{
    public static class UtilFunctions
    {
        /// <summary>
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>bool</returns>
        /// <remarks>
        /// <para>
        /// Check if the type of value is ...
        /// </para>
        /// <list type="bullet">
        ///         <item>
        ///             <description>byte</description>
        ///         </item>
        ///         <item>
        ///             <description>short</description>
        ///         </item>
        ///         <item>
        ///             <description>ushort</description>
        ///         </item>
        ///         <item>
        ///             <description>int</description>
        ///         </item>
        ///         <item>
        ///             <description>uint</description>
        ///         </item>
        ///         <item>
        ///             <description>long</description>
        ///         </item>
        ///         <item>
        ///             <description>ulong</description>
        ///         </item>
        ///         <item>
        ///             <description>float</description>
        ///         </item>
        ///         <item>
        ///             <description>double</description>
        ///         </item>
        ///         <item>
        ///             <description>decimal</description>
        ///         </item>
        /// </list>
        /// </remarks>
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }

        /// <summary>
        /// Check if the value is object null, string empty, or string whitespace.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>bool</returns>
        public static bool IsNull(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value.IsNumber())
            {
                return false;
            }

            Type t = value.GetType();

            if (t == typeof(string))
            {
                return string.IsNullOrWhiteSpace(value.ToString());
            }

            return false;
        }

        /// <summary>
        /// Convert object to boolean
        /// </summary>
        /// <param name="argOrg">Object</param>
        /// <returns>Boolean</returns>
        public static bool ObjectToBoolean(object argOrg)
        {
            try
            {
                return System.Convert.ToBoolean(argOrg);
            }
            catch (Exception ex)
            {
                GetDetailsException(ex);
                return false;
            }
        }

        /// <summary>
        /// Get information details exception
        /// </summary>
        /// <param name="_Ex">Exception</param>
        /// <returns>Information details exception</returns>
        public static string GetDetailsException(Exception _Ex)
        {
            string sException = string.Empty;
            try
            {
                StackTrace stackTrace = new StackTrace();
                string sNameMethod = stackTrace.GetFrame(1).GetMethod().Name;
                string sNameFile = stackTrace.GetFrame(1).GetMethod().ReflectedType.Name;
                sException = "Error Function " + sNameFile + "." + sNameMethod + ": " + _Ex.Message + "\n";
            }
            catch (Exception exc)
            {
                sException = "Error Function Common.UtilFunctions.GetDetailsException().\nDetails: " + exc.Message;
            }
            return sException;
        }
    }
}
