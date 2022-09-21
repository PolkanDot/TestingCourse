using System;
using System.Diagnostics;
using System.ComponentModel;

namespace MyProcessSample
{
    class MyProcess
    {
        public static void Main()
        {
            Console.WriteLine("Type path to the file:");
            string? pathToInpFile = Console.ReadLine();
            if (!File.Exists(pathToInpFile))
            {
                Console.WriteLine("Error open file.");
                return;
            }
            using StreamReader sr = new(pathToInpFile);
            using StreamWriter sw = new("result.txt");

            string? inputArgs;
            string argsString;
            string expectedResult;
            while ((inputArgs = sr.ReadLine()) != null)
            {
                argsString = inputArgs.Substring(0, (inputArgs.IndexOf(':')));
                expectedResult = inputArgs.Substring(inputArgs.IndexOf(':') + 2);
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Study\Testing\Lab_1\TriangleClassificationProject\bin\Debug\net6.0\triangle.exe",
                        Arguments = argsString,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };
                    using Process? proc = Process.Start(startInfo);
                    string? result = proc.StandardOutput.ReadLine();
                    proc.WaitForExit();

                    string outputRes = (expectedResult == result) ? "success" : "error";
                    sw.WriteLine(outputRes);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
