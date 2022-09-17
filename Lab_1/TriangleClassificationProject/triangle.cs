﻿namespace ConsoleApplication
{
    // Для обработки чисел с точкой и с запятой, пользоваться классом из видео и делать две проверки, меняя разделитель в классе
    // Отрицательные числа
    // Максимальные значения
    class MainProgram
    {
        public struct ParsingResult
        {
            public double[] side;
            public bool sucsess;
        }
        public ParsingResult ParsingArgs(string[] args)
        {
            ParsingResult result = new ParsingResult();
            result.side = new double[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    result.side[i] = Convert.ToDouble(args[i]);
                }
                catch (Exception)
                {
                    result.sucsess = false;
                }
            }
            return result;
        }

        public string TypeDetermining(double[] side)
        {
            string usualTriangle = "Обычный";
            string isoscelesTriangle = "Равнобедренный";
            string equilateralTriangle = "Равносторонний";
            string resultMessage = usualTriangle;
                if ((side[0] == side[1]) && (side[0] == side[2]) && (side[1] == side[2]))
                {
                    resultMessage = equilateralTriangle;
                }
                else
                {
                    if ((side[0] != side[1]) && (side[0] != side[2]) && (side[1] != side[2]))
                    {
                        resultMessage = usualTriangle;
                    }
                    else
                    {
                        resultMessage = isoscelesTriangle;
                    }
                }
            return resultMessage;
        }

        public string Op(string[] args)
        {
            // Output messages inatialisation
            string unknownError = "Неизвестная ошибка";
            string notATriangle = "Не треугольник";

            // Initialisation
            string resultMessage = unknownError;
            ParsingResult parsingResult = ParsingArgs(args);

            // Uncorrect convertation
            if (ParsingArgs(args).sucsess)            
            {
                if ((parsingResult.side.Length != 3) || (parsingResult.side[0] < 0) || (parsingResult.side[1] < 0) || (parsingResult.side[2] < 0))
                {
                    resultMessage = notATriangle;
                }
                if ((parsingResult.side[0] > (parsingResult.side[1] + parsingResult.side[2])) &&
                    (parsingResult.side[1] > (parsingResult.side[0] + parsingResult.side[2])) && 
                    (parsingResult.side[2] > (parsingResult.side[1] + parsingResult.side[0])))
                {
                    resultMessage = TypeDetermining(parsingResult.side);
                }
            }
            return resultMessage;
        }
        static void Main(string[] args)
        {
            MainProgram haha = new MainProgram();
            Console.WriteLine(haha.Op(args));
        }

    }
}

