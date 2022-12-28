/*
Седёлкина Анастасия
Вдовкин Вадим
Михеев Сергей
Мифтахов Инсар
ПС-33
 */

using System.Runtime.CompilerServices;

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
        for (int i = 0; i < table.Count; i++)
        {
            for (column = 0; column < table[i].Count; column++)
            {
                Console.Write(table[i][column] + " ");
            }
            Console.WriteLine();
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
        bool result = Check(sr);

        Console.WriteLine(result_message);
    }

    private bool Check(StreamReader sr)
    {
        return true;
    }


    private string result_message = "Programm is correct";
    private List<List<string>> table = new List<List<string>>();
}
