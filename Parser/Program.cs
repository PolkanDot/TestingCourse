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
            resultString = resultString.ToLower();
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

        if ((LongRead(sr, 5, ref lex)) & (lex == "prog "))
        {
            SpaceSkiper(sr);
            if ((LongRead(sr, 2, ref lex)) & (lex == "id"))
            {
                if (Var(sr))
                {
                    SpaceSkiper(sr);                   
                    if ((LongRead(sr, 6, ref lex)) & (lex == "prog "))
                    {
                        if (ListSt(sr))
                        {
                            SpaceSkiper(sr);
                            if ((LongRead(sr, 3, ref lex)) & (lex == "end"))
                            {
                                result = true;
                            }
                            else
                            {
                                result = false;
                                result_message = "Ожидалось ключевое слово end";
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
                        result_message = "Ожидалось ключевое слово begin";
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
            result_message = "Ожидалось ключевое слово prog";
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
        string endChar = ":";
        char[] workString = { };

        SpaceSkiper(sr);

        if ((LongRead(sr, 4, ref lex)) & (lex == "var "))
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
            result_message = "Ожидалось ключевое слово var";
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

    private bool IdList(StreamReader sr, string ch)
    {
        int count = 2;
        bool end = false;
        bool result = false;
        string ch1 = "";
        SpaceSkiper(sr);
        while (!end)
        {
            if ((LongRead(sr, 1, ref ch1)) & (ch1 == "i"))
            {
                if ((LongRead(sr, 1, ref ch1)) & (ch1 == "d"))
                {
                    count -= 1;
                    SpaceSkiper(sr);
                    if ((LongRead(sr, 1, ref ch1)) & (ch1 == ","))
                    {
                        SpaceSkiper(sr);
                        count *= 2;
                    }
                    else if (ch1 == ch)
                    {
                        end = true;
                    }
                    else
                    {
                        end = true;
                        result = false;
                        result_message = $"Ожидался служебный символ {ch}";
                    }
                }
                else
                {
                    end = true;
                    result = false;
                    result_message = "Ожидался идентификатор id";
                }
            }
            else
            {
                end = true;
                result = false;
                result_message = "Ожидался идентификатор id";
            }
        }
        if (count == 1)
        {
            result = true;
        }
        return result;
    }

    private string result_message = "Programm is correct";
}
