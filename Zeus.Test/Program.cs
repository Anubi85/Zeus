using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Zeus.Config;
using Zeus.InternalLogger;
using Zeus.Log;
using Zeus.Patterns;
using Zeus.Plugin;
using Zeus.Plugin.Repositories;

namespace Zeus.Test
{
    class Program
    {
        class EnumTest : TypeSafeEnumBase<EnumTest>
        {
            public static readonly EnumTest value1 = new EnumTest();
            public static readonly EnumTest value2 = new EnumTest();
        }

        static void Main(string[] args)
        {
            foreach (EnumTest et in EnumTest.GetValues())
            {
                Console.WriteLine(et);
            }
            LogSettings settings = ConfigManager.LoadSection<LogSettings>();
            ConfigManager.IsLocalSourceReadOnly = true;
            LogChannelSettings lcs = new LogChannelSettings();
            lcs.ChannelName = "DynamicFile";
            lcs.ChannelType = "FileChannel";
            lcs.CustomSettings.SetValue<string>("FileName", "dynamic.csv");
            settings.ChannelSettings.Add(lcs);
            ConfigManager.SaveSection<LogSettings>(settings);
            Console.ReadLine();
            LogManager.Start();
            Console.ReadLine();
            LogManager.WriteTraceMessage("Test trace message");
            LogManager.WriteDebugMessage("Test debug message");
            LogManager.WriteInfoMessage("Test info message");
            Console.ReadLine();
            LogManager.WriteSuccessMessage("Test success message");
            LogManager.WriteWarningMessage("Test warning message");
            LogManager.WriteErrorMessage("Test error message");
            LogManager.WriteFatalMessage("Test fatal message");
            LogManager.Stop();
            Console.ReadLine();
        }
    }
}
