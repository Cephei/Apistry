namespace Apistry
{
    using System;
    using System.Text.RegularExpressions;

    public static class StringHelper
    {
        public static String RemoveMultipleSpaces(String value)
        {
            return Regex.Replace(value.Trim(), @"\s+", " ");
        }
    }
}