using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Zeus;
using Zeus.Config;
using Zeus.Data;
using Zeus.Extensions;
using Zeus.InternalLogger;
using Zeus.Log;
using Zeus.Log.Channels;
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
            CircularBuffer<double> buff = new CircularBuffer<double>(5);
            buff.Add(1);
            buff.Add(2);
            buff.ChangeCapacity(3);
            buff.Add(3);
            buff.Add(4);
            buff.Add(5);
            buff.ChangeCapacity(4);
            buff.Add(6);
            buff.Add(7);
            buff.Add(8);
            foreach (double d in buff)
            {
                Console.WriteLine(d);
            }
            DataStore store = new DataStore();
            store.Create<bool>("test1", false);
            store.Set<string>("test1", "true");
            bool test1 = store.Get<bool>("test1");
            store.Create<string>("test2", "true");
            bool test = store.Get<bool>("test2");
            foreach (EnumTest et in EnumTest.GetValues())
            {
                Console.WriteLine(et);
            }
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
