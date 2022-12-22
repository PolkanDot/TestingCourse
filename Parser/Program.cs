// Нужно добавить невосприимчивость к большому кличеству пробелов
// Думаю через sr.Peek() проверять если следующий символ пробел,
// то читать его и перемещать курсор, если не пробел,
// то продолжать проверку в соответствии с обрабатываемым "токеном"

// Возможно при выводе сообщения об ошибке нам нужно еще писать строку и столбец

// Если всю прогу можно написать в строку, то нужно много чего переделывать,
// тк я много где ориентировался, что некоторые токены начинаются с новой строки

// В idList на вход передаем символ конца списка

Parser parser = new Parser();
parser.Parsing();
class Parser
{
    static void SpaceSkiper(StreamReader sr)
    {
        char space = (char)sr.Peek();
        while (space == ' ')
        {
            sr.Read();
            space = (char)sr.Peek();
        }
    }

    static bool LongRead(StreamReader sr, int count, string resultString)
    {
        int digit = 0;
        char workChar;
        char[] workString = { };
        bool result;

        while((digit < count) &(sr.Peek() != -1))
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

        if ((LongRead(sr, 5, lex)) & (lex == "PROG "))
        {
            SpaceSkiper(sr);
            if ((LongRead(sr, 2, lex)) & (lex == "id"))
            {
                if (Var(sr))
                {
                    SpaceSkiper(sr);                   
                    if ((LongRead(sr, 6, lex)) & (lex == "BEGIN "))
                    {
                        if (ListSt(sr))
                        {
                            SpaceSkiper(sr);
                            if ((LongRead(sr, 3, lex)) & (lex == "END"))
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
        char workChar;
        char[] workString = { };

        readResult = sr.Read(workString, 0, 4);
        if (readResult == -1)
        {
            result_message = "Ожидалось ключевое слово VAR";
            result = false;
        }

        lex = string.Concat(workString);

        if (lex == "VAR ")
        {
            if (IdList(sr))
            {
                //доделать проверку ":"
                sr.Peek()
                if (ListSt(sr))
                {
                    if (IdType(sr))
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
            }
        }
        else
        {
            result = false;
            result_message = "Ожидалось ключевое слово VAR";
        }

        return true;
    }

    private bool IdType(StreamReader sr)
    {
        bool result = false;
        string lex = "";

        lex = sr.ReadLine();
        if (lex == null)
        {
            result_message = "Ожидался тип объявленных параметров";
            return false;
        }

        if ((lex == "int") &(lex == "float") & (lex == "bool") & (lex == "string"))
        {
            result = true;
        }
        else
        {
            result = false;
            result_message = "Неверно указан тип параметра";
        }

        return result;
    }

    private bool IdList(StreamReader sr)
    {
        return true;
    }

    private string result_message = "Programm is correct";
}
