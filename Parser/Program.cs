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

        if ((LongRead(sr, 4, ref lex)) & (lex == "prog"))
        {
            SpaceSkiper(sr);
            if ((LongRead(sr, 2, ref lex)) & (lex == "id"))
            {
                if (Var(sr))
                {
                    SpaceSkiper(sr);                   
                    if ((LongRead(sr, 5, ref lex)) & (lex == "begin"))
                    {
                        if (ListSt(sr))
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
                    SpaceSkiper(sr);
                    if ((LongRead(sr, 1, ref lex)) & (lex == ";"))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        result_message = "Ожидалось ;";
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
        bool result = false;
        SpaceSkiper(sr);
        IdListRecursive(sr, ch, ref count);
        if (count == 1)
        {
            result = true;
        }
        return result;
    }

    private void IdListRecursive(StreamReader sr, string ch, ref int count)
    {
        string ch1 = "";
        SpaceSkiper(sr);
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
                    IdListRecursive(sr, ch, ref count);
                }
                else if (ch1 == ch)
                {
                }
                else
                {
                    result_message = $"Ожидался служебный символ {ch}";
                    count = 0;
                }
            }
            else
            {
                result_message = "Ожидался идентификатор id";
            }
        }
        else
        {
            result_message = "Ожидался идентификатор id";
        }
    }

    private bool Write(StreamReader sr)
    {
        string potentialOperator = "";
        LongRead(sr, 1, ref potentialOperator);
        if (potentialOperator == "(")
        {
            if (IdList(sr, ")"))
            {
                SpaceSkiper(sr);
                if (LongRead(sr, 1, ref potentialOperator))
                {
                    return (potentialOperator == ";");
                }
                result_message = "Ожидалось ;";
                return false;
            }
            return false;
        }
        result_message = "Ожидался список идентификаторов";
        return false;
    }
    private bool Read(StreamReader sr)
    {
        string potentialOperator = "";
        LongRead(sr, 1, ref potentialOperator);
        if (potentialOperator == "(")
        {
            if (IdList(sr, ")"))
            {
                SpaceSkiper(sr);
                if (LongRead(sr, 1, ref potentialOperator))
                {
                    return (potentialOperator == ";");
                }
                result_message = "Ожидалось ;";
                return false;
            }
            return false;
        }
        result_message = "Ожидался список идентификаторов";
        return false;
    }
    private bool Assign(StreamReader sr)
    {
        string potentialRavno = "", potentialSign = "";
        if (LongRead(sr, 2, ref potentialRavno))
        {
            if (potentialRavno == ":=")
            {
                if (Exp(sr))
                {
                    SpaceSkiper(sr);
                    if (LongRead(sr, 1, ref potentialSign))
                    {
                        return (potentialSign == ";");
                    }
                    result_message = "Ожидалось ;";
                    return false;
                }
                return false;
            }
            result_message = "Ожидалось :=";
            return false;
        }
        result_message = "Ожидалось :=";
        return false;
    }
    private bool ST(StreamReader sr)
    {
        string potentialOperator = "";
        SpaceSkiper(sr);
        if (!LongRead(sr, 2, ref potentialOperator))
        {
            result_message = "Ожидался оператор";
            return false;
        }
        switch (potentialOperator)
        {
            case ("wr"):
                {
                    if ((LongRead(sr, 3, ref potentialOperator)) & (potentialOperator == "ite"))
                    {
                        SpaceSkiper(sr);
                        return Write(sr);
                    }
                    result_message = "Ожидался оператор";
                    return false;
                }
            case ("re"):
                {
                    if ((LongRead(sr, 2, ref potentialOperator)) & (potentialOperator == "ad"))
                    {
                        SpaceSkiper(sr);
                        return Read(sr);
                    }
                    result_message = "Ожидался оператор";
                    return false;
                }
            case ("id"):
                {
                    SpaceSkiper(sr);
                    return Assign(sr);
                }

            default:
                result_message = "Ожидался оператор";
                return false;
        }
    }
    // <LISTST> -> <ST> | <LISTST> <ST>
    // переделываем в:
    // <ListST> -> <ST> <B>
    // <B> -> Empty | <ST> <B>
    private bool B(StreamReader sr)
    {
        SpaceSkiper(sr);
        int symbol = sr.Peek();
        if ((char)symbol == 'e' || (char)symbol == 'E')
        {
            string check = "";
            LongRead(sr, 3, ref check);
            if (check == "end")
            {
                return true;
            }
            result_message = "Ожидалось ключевое слово end";
            return false;
        }
        if (ST(sr))
        {
            return B(sr);
        }
        return false;
    }

    private bool F(StreamReader sr)
    {
        SpaceSkiper(sr);
        string symbol = "";
        if (LongRead(sr, 1, ref symbol))
        {
            switch (symbol)
            {
                case ("-"):
                    return F(sr);
                case ("("):
                    if (Exp(sr))
                    {
                        SpaceSkiper(sr);
                        if (LongRead(sr, 1, ref symbol))
                        {
                            return (symbol == ")");
                        }
                        result_message = "Ожидалось )";
                        return false;
                    }
                    return false;
                case ("i"):
                    if (LongRead(sr, 1, ref symbol))
                    {
                        if (symbol == "d")
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                case ("n"):
                    if (LongRead(sr, 2, ref symbol))
                    {
                        if (symbol == "um")
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
            }
        }
        return true;
    }
    // <D> -> Empty | * <F> <D>  
    private bool D(StreamReader sr)
    {
        int potentialMulty;
        SpaceSkiper(sr);
        potentialMulty = sr.Peek();
        if ((char)potentialMulty == '*')
        {
            potentialMulty = sr.Read();
            if (F(sr))
            {
                return D(sr);
            }
            result_message = "Некорректный множитель";
            return false;
        }
        return true;
    }
    private bool T(StreamReader sr)
    {
        if (!F(sr))
        {
            return false;
        }
        return D(sr);
    }
    // <C> -> Empty | + <T> <C>
    private bool C(StreamReader sr)
    {
        int potentialPlus;
        SpaceSkiper(sr);
        potentialPlus = sr.Peek();
        if ((char)potentialPlus == '+')
        {
            potentialPlus = sr.Read();
            if (T(sr))
            {
                return C(sr);
            }
            result_message = "Некорректное слагаемое";
            return false;
        }
        return true;
    }
    // <EXP> -> <T> <C>
    private bool Exp(StreamReader sr)
    {
        if (!T(sr))
        {
            return false;
        }
        return C(sr);
    }
    private bool ListSt(StreamReader sr)
    {
        if (!ST(sr))
        {
            return false;
        }
        return B(sr);
    }

    private string result_message = "Programm is correct";
}
