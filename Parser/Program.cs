// Нужно добавить невосприимчивость к большому кличеству пробелов
// Думаю через sr.Peek() проверять если следующий символ пробел,
// то читать его и перемещать курсор, если не пробел,
// то продолжать проверку в соответствии с обрабатываемым "токеном"

// Если всю прогу можно написать в строку, то нужно много чего переделывать,
// тк я много где ориентировался, что некоторые токены начинаются с новой строки

// В idList на вход передаем символ конца списка

using System.Linq.Expressions;

Parser parser = new Parser();
parser.Parsing();
class Parser
{
    static void SpaceSkiper(StreamReader sr)
    {
        char space = (char)sr.Peek();
        while ((space == ' ') | (space == '\r') | (space == '\n'))
        {
            sr.Read();
            space = (char)sr.Peek();
        }
    }

    static bool LongRead(StreamReader sr, int count, ref string resultString)
    {
        int digit = 0;
        char workChar;
        char[] workString = new char[count];
        bool result;

        while((digit < count) & (sr.Peek() != -1))
        {
            workChar = (char)sr.Read();
            workString[digit] = workChar;
            digit++;
        }
        if (digit == count)
        {
            resultString = string.Concat(workString);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }
    public void Parsing()
    {
        Console.WriteLine("Укажите полный путь к проверяемому файлу с кодом:");
        string? pathToInpFile = Console.ReadLine();
        if (!File.Exists(pathToInpFile))
        {
            Console.WriteLine("Ошибка открытия файла");
            return;
        }
        using StreamReader sr = new(pathToInpFile);
        bool result = Prog(sr);

        Console.WriteLine(result_message);
    }

    private bool Prog(StreamReader sr)
    {
        bool result = false;
        string[] mas; 
        string lex = "";
        char[] workString = { };

        SpaceSkiper(sr);

        if ((LongRead(sr, 5, ref lex)) & (lex == "PROG "))
        {
            SpaceSkiper(sr);
            if ((LongRead(sr, 2, ref lex)) & (lex == "id"))
            {
                if (Var(sr))
                {
                    SpaceSkiper(sr);                   
                    if ((LongRead(sr, 6, ref lex)) & (lex == "BEGIN "))
                    {
                        if (ListSt(sr))
                        {
                            SpaceSkiper(sr);
                            if ((LongRead(sr, 3, ref lex)) & (lex == "END"))
                            {
                                result = true;
                            }
                            else
                            {
                                result = false;
                                result_message = "Ожидалось ключевое слово END";
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
                        result_message = "Ожидалось ключевое слово BEGIN";
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
                result_message = "Ожидалось название программы";
            }
        }
        else
        {
            result = false;
            result_message = "Ожидалось ключевое слово PROG";
        }

        return result;
    }

    private bool ListSt(StreamReader sr)
    {
        return true;
    }

    private bool Var(StreamReader sr)
    {
        int readResult;
        bool result = false;
        string lex = "";
        char endChar = ':';
        char[] workString = { };

        SpaceSkiper(sr);

        if ((LongRead(sr, 4, ref lex)) & (lex == "VAR "))
        {
            if (IdList(sr, endChar))
            {
                if (IdType(sr))
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
            result_message = "Ожидалось ключевое слово VAR";
        }

        return result;
    }

    private bool IdType(StreamReader sr)
    {
        bool result = false;
        string lex = "";

        SpaceSkiper(sr);
        if (LongRead(sr, 1, ref lex))
        {
            switch (lex)
            {
                case "i":
                    if ((LongRead(sr, 2, ref lex)) & (lex == "nt"))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        result_message = "Неизвестный тип переменной";
                    }
                    break;
                case "f":
                    if ((LongRead(sr, 4, ref lex)) & (lex == "loat"))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        result_message = "Неизвестный тип переменной";
                    }
                    break;
                case "b":
                    if ((LongRead(sr, 3, ref lex)) & (lex == "ool"))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        result_message = "Неизвестный тип переменной";
                    }
                    break;
                case "s":
                    if ((LongRead(sr, 5, ref lex)) & (lex == "tring"))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        result_message = "Неизвестный тип переменной";
                    }
                    break;
                default:
                    result = false;
                    result_message = "Ожидалcя тип объявленных переменных или тип указан неверно";
                    break;
            }
        }
        else
        {
            result = false;
            result_message = "Ожидалcя тип объявленных переменных";
        }

        return result;
    }

    private bool IdList(StreamReader sr, char ch)
    {
        char ch1 = (char)sr.Read();
        bool bol = false;
        bool bl = false;

        while (bl == false)
        {
            while (bol == false)
            {
                if (ch1 == 'i')
                {
                    ch1 = (char)sr.Read();
                    if (ch1 == 'd')
                    {
                        ch1 = (char)sr.Read();
                        while (ch1 == ' ')
                        {
                            ch1 = (char)sr.Read();
                        }
                        if (ch1 == ch)
                        {
                            bl = true;
                        }
                        if (ch1 != ',')
                        {
                            bol = true;
                            bl = true;
                        }

                    }
                }
                if (ch1 == ' ')
                {
                    bol = true;
                }
            }
            bol = false;
            while (ch1 == ' ')
            {
                ch1 = (char)sr.Read();
            }

        }
        if (ch1 == ',')
        {
            result_message = "Не встречена ',' между индетификаторами";
            return false;
        }
        else
            return true;
    }

    private string result_message = "Programm is correct";
}
