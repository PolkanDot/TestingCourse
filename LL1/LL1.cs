/*
 * ПС-33
 * Мифтахов Инсар
 * Михеев Сергей
 * Вдовкин Вадим
 */

Parser parser = new Parser();
parser.Start();
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

        while ((digit < count) & (sr.Peek() != -1))
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
    public void Start()
    {
        Console.WriteLine("Укажите полный путь к файлу с таблицей LL1:");
        string? pathToInpFile = Console.ReadLine();
        if (!File.Exists(pathToInpFile))
        {
            Console.WriteLine("Ошибка открытия файла");
            return;
        }
        using StreamReader sr = new(pathToInpFile);
        ReadTable(sr);
        Parsing();
        Console.WriteLine(result_message);
    }
    private void ReadTable(StreamReader sr)
    {
        int column;
        bool result = true;
        string str = "";
        string[] mas = { };
        List<string> tableString = new List<string>();
        while (!sr.EndOfStream)
        {
            tableString = new List<string>();
            mas = (sr.ReadLine()).Split(" ");
            for (column = 0; column < mas.Length; column++)
            {
                tableString.Add(mas[column]);
            }
            table.Add(tableString);
        }
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

        Check(sr);
    }

    private void Check(StreamReader sr)
    {
        bool error = false;
        bool end = false;
        int currentLine = 0;
        string[] mas = { };
        SpaceSkiper(sr);
        string currentChar = "";
        LongRead(sr, 1, ref currentChar);
        while ((!error) & (!end)) 
        {
            mas = (table[currentLine][1]).Split(",");
            // Направляющее множество
            if (Array.IndexOf(mas, currentChar) != -1)
            {             
                // Конечное состояние
                if ((table[currentLine][6] == "да") & (stack.Count == 0))
                {
                    end = true;
                }
                else
                {
                    // Стек
                    if (table[currentLine][4] != "нет")
                    {
                        stack.Push(Convert.ToInt32(table[currentLine][4]));
                    }
                    // Сдвиг по фазе
                    if (table[currentLine][2] == "да")
                    {
                        SpaceSkiper(sr);
                        if (!LongRead(sr, 1, ref currentChar))
                        {
                            currentChar = "/n";
                        }
                    }
                    // Переход
                    if (table[currentLine][3] != "-")
                    {
                        currentLine = Convert.ToInt32(table[currentLine][3]);
                    }
                    else if (stack.Count != 0)
                    {
                        currentLine = stack.Pop();
                        SpaceSkiper(sr);
                        if ((!LongRead(sr, 1, ref currentChar)) & (currentChar != "/n"))
                        {
                            currentChar = "/n";
                        }
                        else if (currentChar == "/n")
                        {
                            error= true;
                            result_message = $"Ошибка в строке {currentLine}, ожидалось {table[currentLine][1]}";
                        }
                    }
                    else
                    {
                        end = true;
                    }
                }           
            }
            else
            {
                // Ошибка
                if (table[currentLine][5] == "да")
                {
                    error = true;
                    result_message = $"Ошибка в строке {currentLine}, ожидалось {table[currentLine][1]}";
                }
                else
                {
                    currentLine++;
                }
            }
        }
    }

    private string result_message = "Expression is correct";
    private List<List<string>> table = new List<List<string>>();
    private Stack<int> stack = new Stack<int>();
}
