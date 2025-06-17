class Program
{
    static void Main()
    {
        string[] roles = {"Super Administrator", "System Administrator", "Service Engineer"};
        int selection = 0;
        ConsoleKey key = ConsoleKey.None;

        while (key != ConsoleKey.Enter)
        {
            Console.Clear();
            Console.WriteLine("=== Role Selection Menu ===");

            for (int i = 0; i < roles.Length; i++)
            {
                if (i == selection)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"> {roles[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(roles[i]);
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if(key == ConsoleKey.UpArrow)
            {
                selection = (selection - 1 + roles.Length) % roles.Length;
            }
            else if(key == ConsoleKey.DownArrow)
            {
                selection = (selection + 1) % roles.Length;
            }
        }
        Console.Clear();
        Console.WriteLine($"You selected: {roles[selection]}");
    }
}