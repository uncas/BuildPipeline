using System.Configuration;

namespace Uncas.BuildPipeline.Configuration
{
    public static class ConfigurationAppSetting
    {
        public static int Int32(string name, int defaultValue)
        {
            int result;
            string appSetting = ConfigurationManager.AppSettings[name];
            if (int.TryParse(appSetting, out result))
                return result;
            return defaultValue;
        }

        public static string String(string name, string defaultValue)
        {
            return ConfigurationManager.AppSettings[name] ?? defaultValue;
        }
    }
}