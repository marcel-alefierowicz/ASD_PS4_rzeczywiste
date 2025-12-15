using System.Diagnostics;
using System.Globalization;
class Program
{
    static void parseCommand((char command, float value) operation, ref Node? root, ref int count)
    {
        count++; // count of operations for speed tracking purposes
        (char c, float key) = operation;
        switch (c)
        {
            case 'W':
                {
                    root = AVL.insert(root, key);
                    break;
                }
            case 'U':
                {
                    root = AVL.delete(root, key);
                    break;
                }
            case 'S':
                {
                    if (AVL.find(root, key))
                        System.Console.WriteLine($"[{count}] Found {key}");
                    // System.Console.WriteLine($"TAK");
                    else
                        System.Console.WriteLine($"[{count}] Did not find {key}");
                    // System.Console.WriteLine($"NIE");
                    break;
                }
            case 'L':
                {
                    System.Console.WriteLine($"[{count}] starting with {key}: {AVL.countOccurrences(root, (int)key)}");
                    // System.Console.WriteLine(AVL.countOccurrences(root, (int)key));
                    break;
                }
            default:
                break;
        }
    }
    static void Main()
    {
        Node? root = null;
        int count = 0;
        string[] lines = File.ReadAllLines("./in/duzy1.txt");
        int n = int.Parse(lines[0]);
        CultureInfo cul = new CultureInfo("de-DE"); // formatting floats properly (eu format)

        Stopwatch sw = new();
        sw.Start();

        (char command, float value)[] commands = new (char, float)[n];
        for (int i = 1; i <= n; i++)
        {
            string[] parts = lines[i].Split();
            char operation = parts[0][0];
            float key = float.Parse(parts[1], cul);
            commands[i - 1] = (operation, key);
        }

        // proper execution
        for (int i = 0; i < n; i++)
        {
            parseCommand(commands[i], ref root, ref count);
        }
        // printTree(root, 5);
        sw.Stop();
        System.Console.WriteLine($"\x1b[1;35mexecution time: {sw.ElapsedMilliseconds}ms\x1b[0m");
    }
}