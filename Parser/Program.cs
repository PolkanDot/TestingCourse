﻿namespace MyProcessSample
{
    class MyProcess
    {
        //<PROG>
        bool Prog(StreamReader sr)
        {
            bool result = false;
            string[] mas; 
            string lex = "";
            lex = sr.ReadLine();
            mas = lex.Split(" ");
            if ((mas[0] == "PROG") & (mas.Length == 2))
            {
                if (Var(sr))
                {
                    lex = sr.ReadLine();
                    if (lex == "BEGIN")
                    {
                        if (ListSt(sr))
                        {
                            lex = sr.ReadLine();
                            if (lex == "END")
                            {
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        //<ListSt>
        bool ListSt(StreamReader sr)
        {
            return true;
        }

        //<Var>
        bool Var(StreamReader sr)
        {
            return true;
        }

        //<IdList>
        bool IdList(StreamReader sr)
        {
            return true;
        }
        
        public void Main()
        {
            Console.WriteLine("Type path to the file:");
            string? pathToInpFile = Console.ReadLine();
            if (!File.Exists(pathToInpFile))
            {
                Console.WriteLine("Error open file.");
                return;
            }
            using StreamReader sr = new(pathToInpFile);
            bool result = Prog(sr);

            if (result)
            {
                Console.WriteLine("Program is correct");
            }
            else
            {
                Console.WriteLine("Program is not correct");
            }
        }
    }
}