#region Using namespaces.
using System;
using Newtonsoft.Json;
#endregion

namespace WebApi.Helpers
{
    public static class JSONHelper
    {
         #region Public extension methods.
        /// <summary>
        /// Extened method of object class
        /// Converts an object to a json string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch(Exception)
            {
                return "";
            }
        }

        public static object ToObject(this string value)
        {
            try
            {
                return JsonConvert.DeserializeObject(value);
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion
    }
}