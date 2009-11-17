using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.StringTemplate;

namespace MVCContrib.Export.ST_Renderer
{
    /// <summary>
    /// string renderer - 
    ///     can be used with TOLOWER (TOUPPER)or HTML or any other format
    /// from http://www.codeplex.com/Exporter
    /// </summary>
    public class AdvancedStringRenderer : IAttributeRenderer
    {

        #region IAttributeRenderer Members

        /// <summary>
        /// IAttributeRenderer implementation
        /// </summary>
        /// <param name="o">The value object - string </param>
        /// <returns></returns>
        public string ToString(object o)
        {
            return ToString(o, null);
        }

        /// <summary>
        /// formats the string.
        /// </summary>
        /// <param name="o">The value object - string</param>
        /// <param name="formatName">Name of the format - TOUPPER, TOLOWER, HTML or any other .NET format</param>
        /// <returns></returns>
        public string ToString(object o, string formatName)
        {
            if (o == null)
                return null;

            if (formatName == null)
                return o.ToString();

            switch (formatName.ToUpper())
            {
                case "XML":
                    return o.ToString().Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;");
                case "HTML":
                    return o.ToString().Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;");
                case "TOUPPER":
                    return o.ToString().ToUpper();
                case "TOLOWER":
                    return o.ToString().ToLower();
                case "HTML_A_NAME":
                    return o.ToString().Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace(@"\", "_");

                default:

                    return string.Format("{0:" + formatName + "}", o.ToString());
            }
        }

        #endregion
    }
    /// <summary>
    /// date time renderer - 
    ///     can be used with any other format implemented by .NET
    /// </summary>
    public class AdvancedDateTimeRenderer : IAttributeRenderer
    {
        #region IAttributeRenderer Members
        /// <summary>
        /// IAttributeRenderer implementation
        /// </summary>
        /// <param name="o">The value object - string </param>
        /// <returns></returns>
        public string ToString(object o)
        {
            return ToString(o, null);
        }

        /// <summary>
        /// formats the string.
        /// </summary>
        /// <param name="o">The value object - string</param>
        /// <param name="formatName">Name of the format - TOUPPER, TOLOWER, HTML or any other .NET format</param>
        /// <returns></returns>
        public string ToString(object o, string formatName)
        {
            if (o == null)
                return null;
            if (formatName == null)
                return o.ToString();
            DateTime dt = Convert.ToDateTime(o);
            return string.Format("{0:" + formatName + "}", dt);
        }

        #endregion
    }
    public class AdvancedDecimalRenderer : IAttributeRenderer
    {
        #region IAttributeRenderer Members
        /// <summary>
        /// IAttributeRenderer implementation
        /// </summary>
        /// <param name="o">The value object - string </param>
        /// <returns></returns>
        public string ToString(object o)
        {
            return ToString(o, null);
        }

        /// <summary>
        /// formats the string.
        /// </summary>
        /// <param name="o">The value object - string</param>
        /// <param name="formatName">Name of the format - TOUPPER, TOLOWER, HTML or any other .NET format</param>
        /// <returns></returns>
        public string ToString(object o, string formatName)
        {
            if (o == null)
                return null;
            if (formatName == null)
                return o.ToString();
            decimal dt = Convert.ToDecimal(o);
            return string.Format("{0:" + formatName + "}", dt);
        }

        #endregion
    }
}
