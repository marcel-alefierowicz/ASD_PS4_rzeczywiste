using System.Diagnostics;
using System.Globalization;

class Program
{
    static Node? root = null;
    static int count = 0;
    static CultureInfo cul = new CultureInfo("de-DE");

    static void Main()
    {
        while (true)
        {
            printMenu();
            Console.Write("Choose option: ");
            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    insertCli();
                    break;
                case "2":
                    deleteCli();
                    break;
                case "3":
                    findCli();
                    break;
                case "4":
                    lProcedureCli();
                    break;
                case "5":
                    printTreeCli();
                    break;
                case "6":
                    readFromFileCli();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void printMenu()
    {
        Console.WriteLine("==== AVL Tree CLI ====");
        Console.WriteLine("1. Insert node");
        Console.WriteLine("2. Delete node");
        Console.WriteLine("3. Find node");
        Console.WriteLine("4. L procedure");
        Console.WriteLine("5. Print tree");
        Console.WriteLine("6. Read instructions from file");
        Console.WriteLine("0. Exit");
        Console.WriteLine("======================");
    }

    static bool tryReadFloat(out float value)
    {
        Console.Write("Enter value: ");
        return float.TryParse(Console.ReadLine(), NumberStyles.Float, cul, out value);
    }

    static void insertCli()
    {
        if (!tryReadFloat(out float value))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        count++;
        root = AVL.insert(root, value);
        Console.WriteLine($"[{count}] Inserted {value}");
    }

    static void deleteCli()
    {
        if (!tryReadFloat(out float value))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        count++;
        if (AVL.find(root, value))
        {
            root = AVL.delete(root, value);
            Console.WriteLine($"[{count}] Deleted {value}");

        }
        else
        {
            Console.WriteLine($"[{count}] did not find {value}");
        }
    }

    static void findCli()
    {
        if (!tryReadFloat(out float value))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        count++;
        if (AVL.find(root, value))
            Console.WriteLine($"[{count}] Found {value}");
        else
            Console.WriteLine($"[{count}] Did not find {value}");
    }

    static void lProcedureCli()
    {
        if (!tryReadFloat(out float value))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        count++;
        Console.WriteLine(
            $"[{count}] starting with {value}: {AVL.countOccurrences(root, (int)value)}"
        );
    }

    static void printTreeCli()
    {
        Console.WriteLine("AVL tree structure:");
        AVL.printTree(root, 5);
    }

    static void readFromFileCli()
    {
        Console.Write("Enter file path: ");
        string? path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        int n = int.Parse(lines[0]);

        Stopwatch sw = new();
        sw.Start();

        for (int i = 1; i <= n; i++)
        {
            string[] parts = lines[i].Split();
            char command = parts[0][0];
            float key = float.Parse(parts[1], cul);

            count++;

            switch (command)
            {
                case 'W':
                    root = AVL.insert(root, key);
                    break;
                case 'U':
                    root = AVL.delete(root, key);
                    break;
                case 'S':
                    if (AVL.find(root, key))
                        Console.WriteLine($"[{count}] Found {key}");
                    else
                        Console.WriteLine($"[{count}] Did not find {key}");
                    break;
                case 'L':
                    Console.WriteLine(
                        $"[{count}] starting with {key}: {AVL.countOccurrences(root, (int)key)}"
                    );
                    break;
            }
        }

        sw.Stop();
        Console.WriteLine($"execution time: {sw.ElapsedMilliseconds}ms");
    }
}
