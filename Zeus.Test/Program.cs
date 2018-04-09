using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Zeus.InternalLogger;
using Zeus.Log;

namespace Zeus.Test
{
    class Program
    {
        static void Main(string[] args)
        {
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
