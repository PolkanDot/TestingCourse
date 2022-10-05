using System.Globalization;

namespace ConsoleApplication
{
    // Для обработки чисел с точкой и с запятой, пользоваться классом из видео и делать две проверки, меняя разделитель в классе
    // Отрицательные числа
    // Максимальные значения
    class MainProgram
    {
        public struct ParsingResult
        {
            public double[] side;
            public bool success;
        }
        public ParsingResult ParsingArgs(string[] args)
        {
            ParsingResult result = new ParsingResult();
            result.side = new double[args.Length];
            result.success = true;
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo()
            {
                NumberDecimalSeparator = "."
            };
            if (args.Length == 0)
            {
                result.success = false;
            }
            else
            {
                result.success = true;
            };
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    result.side[i] = Convert.ToDouble(args[i]);
                    if ((double.IsInfinity(result.side[i])) || (double.IsInfinity(-(result.side[i]))))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        result.side[i] = Convert.ToDouble(args[i], numberFormatInfo);
                        if ((double.IsInfinity(result.side[i])) || (double.IsInfinity(-(result.side[i]))))
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        result.success = false;
                    }
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
                if ((side[0].Equals(side[1])) && (side[0].Equals(side[2])))
                {
                    resultMessage = equilateralTriangle;
                }
                else
                {
                    if ((!(side[0].Equals(side[1]))) && (!(side[0].Equals(side[2]))) && (!(side[1].Equals(side[2]))))
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

        public string Start(string[] args)
        {
            // Output messages inatialisation
            string unknownError = "Неизвестная ошибка";
            string notATriangle = "Не треугольник";

            // Initialisation
            string resultMessage = unknownError;
            ParsingResult parsingResult = ParsingArgs(args);

            // Uncorrect convertation
            if (parsingResult.success)            
            {
                if ((parsingResult.side.Length != 3) 
                    || (parsingResult.side[0] < 0) || (parsingResult.side[1] < 0) || (parsingResult.side[2] < 0)
                    || (parsingResult.side[0].Equals(0)) || (parsingResult.side[1].Equals(0)) || (parsingResult.side[2].Equals(0)))
                {
                    resultMessage = notATriangle;
                }
                else
                {
                    if ((parsingResult.side[0] < (parsingResult.side[1] + parsingResult.side[2])) &&
                    (parsingResult.side[1] < (parsingResult.side[0] + parsingResult.side[2])) &&
                    (parsingResult.side[2] < (parsingResult.side[1] + parsingResult.side[0])))
                    {
                        resultMessage = TypeDetermining(parsingResult.side);
                    }
                    else
                    {
                        resultMessage = notATriangle;
                    }
                }
            }
            return resultMessage;
        }
        static void Main(string[] args)
        {
            MainProgram triangle = new MainProgram();
            Console.WriteLine(triangle.Start(args));
        }

    }
}

