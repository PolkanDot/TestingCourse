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
            while ((inputArgs = sr.ReadLine()) != null)
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Study\Testing\Lab_1\TriangleClassificationProject\bin\Debug\net6.0\triangle.exe",
                        Arguments = "1 2 3",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };
                    using Process? proc = Process.Start(startInfo);
                    string? result = proc.StandardOutput.ReadLine();
                    proc.WaitForExit();

                    string outputRes = (testCase.result == result) ? "success" : "error";
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
