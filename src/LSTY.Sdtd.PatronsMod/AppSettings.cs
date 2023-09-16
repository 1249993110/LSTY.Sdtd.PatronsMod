﻿using System.Configuration;
using System.Reflection;

namespace LSTY.Sdtd.PatronsMod
{
    internal static class AppSettings
    {
        public static string AccessToken { get; private set; }

        public static string SignalrUrl { get; private set; }

        static AppSettings()
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings;

            var properties = typeof(AppSettings).GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (var item in properties)
            {
                var element = config[item.Name];
                if(element != null)
                {
                    item.SetValue(null, element.Value);
                }
            }
        }
    }
}