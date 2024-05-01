using System.Diagnostics.Contracts;

class Util
{
    static public void Print(
        int x,
        int y,
        string s,
        ConsoleColor color = ConsoleColor.White,
        ConsoleColor back = ConsoleColor.Black
    )
    {
        if (y >= Console.WindowHeight || y < 0)
            return;

        Console.CursorLeft = x;
        Console.CursorTop = y;
        var oldfore = Console.ForegroundColor;
        var oldback = Console.BackgroundColor;
        Console.ForegroundColor = color;
        Console.BackgroundColor = back;
        Console.Write(s);
        Console.ForegroundColor = oldfore;
        Console.BackgroundColor = oldback;
    }

    static public int Menu(
        int offsetx,
        int offsety,
        string[] items
    )
    {
        int option = 0;
        while (true)
        {
            //нарисовать меню
            PrintMenu(offsetx, offsety, items, option);

            //обрабтать ввод (передвинуть курсор)
            var ki = Console.ReadKey(true);
            if (ki.Key == ConsoleKey.Enter) break;
            if (ki.Key == ConsoleKey.UpArrow) option--;
            if (ki.Key == ConsoleKey.DownArrow) option++;

            if (option < 0) option = items.Length - 1;
            if (option >= items.Length) option = 0;
        }
        return option;
    }

    static void PrintMenuItem(int x, int y, string s, int wd, bool selected = false)
    {
        ConsoleColor fore = selected ? ConsoleColor.Black : ConsoleColor.White;
        ConsoleColor back = selected ? ConsoleColor.White : ConsoleColor.Black;
        for (int i = 0; i < wd; i++)
            Print(x + i, y, " ", fore, back);
        Print(x, y, s, fore, back);
    }

    static void PrintMenu(int x, int y, string[] items, int option)
    {
        int wd = items[0].Length;
        for (int i = 0; i < items.Length; i++)
            if (items[i].Length > wd)
                wd = items[i].Length;

        for (int i = 0; i < items.Length; i++)
            PrintMenuItem(x, y + i, items[i], wd, i == option);
    }

    public static string Input(string msg)
    {
        Console.Write(msg);
        return Console.ReadLine();
    }

    public static int InputInt(string msg)
    {
        Console.Write(msg);
        while (true)
        {
            string s = Console.ReadLine();
            if (int.TryParse(s, out int i))
                return i;
            Console.Write(msg);
        }
    }
    public static int InputIntWithMinMax(string msg, int min, int max)
    {
        int res;
        do res = InputInt(msg);
        while (res < min || res > max);
        return res;
    }

    public static DateTime InputDateTime(string msg)
    {
        Console.Write(msg);
        while (true)
        {
            string s = Console.ReadLine();
            if (DateTime.TryParse(s, out DateTime dt))
                return dt;
            Console.Write(msg);
        }
    }
}

