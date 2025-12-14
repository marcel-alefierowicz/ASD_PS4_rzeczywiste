using System.Diagnostics;
using System.Globalization;

public class Node
{
    public Node? left;
    public Node? right;
    public float value;
    public int height { get; set; }
    public Node(float value)
    {
        left = null;
        right = null;
        this.value = value;
        height = 1;
    }
}


class Program
{
    static Node insert(Node? root, float val)
    {
        if (root == null)
            return new(val);

        if (val > root.value)
        {
            root.right = insert(root.right, val);
        }
        else if (val < root.value)
            root.left = insert(root.left, val);
        return root;
    }

    static bool find(Node? root, float val)
    {
        if (root == null) return false;

        if (root.value == val) return true;

        if (root.value > val)
            return find(root.left, val);
        return find(root.right, val);
    }

    static Node nextInOrder(Node? curr)
    {
        curr = curr?.right; // looking in the right subtree
        while (curr != null && curr.left != null)
        {
            curr = curr.left;
        }
        return curr!;
    }

    static Node delete(Node? root, float val)
    {
        if (root == null)
            return root!;

        if (root.value > val)
        {
            root.left = delete(root.left, val);
        }
        else if (root.value < val)
        {
            root.right = delete(root.right, val);
        }
        else // if we're here, that means that we've found the value we're looking to delete
        {
            if (root.left == null) // either 1 or no children
                return root.right!;
            if (root.right == null)
                return root.left;
            // if two children, replace tgt node with next in line
            Node next = nextInOrder(root);
            root.value = next.value;
            root.right = delete(root.right, next.value);
        }
        return root;
    }

    static void printTree(Node? root, int indent)
    {
        if (root != null)
        {
            printTree(root.right, indent + 4);
            if (indent > 0)
                Console.Write(" ".PadLeft(indent));
            Console.WriteLine($"{(root.value)}");
            printTree(root.left, indent + 4);
        }
    }
    static int countOccurrences(Node? root, int key)
    {
        if (root == null)
            return 0;

        int match = ((int)root.value == key) ? 1 : 0;

        return match
             + countOccurrences(root.left, key)
             + countOccurrences(root.right, key);
    }

    static void parseCommand((char command, float value) operation, ref Node? root, ref int count)
    {
        count++; // count of operations for speed tracking purposes
        (char c, float key) = operation;
        switch (c)
        {
            case 'W':
                {
                    root = insert(root, key);
                    break;
                }
            case 'U':
                {
                    root = delete(root, key);
                    break;
                }
            case 'S':
                {
                    if (find(root, key))
                        System.Console.WriteLine($"[{count}] Found {key}");
                    // System.Console.WriteLine($"TAK");
                    else
                        System.Console.WriteLine($"[{count}] Did not find {key}");
                    // System.Console.WriteLine($"NIE");
                    break;
                }
            case 'L':
                {
                    System.Console.WriteLine($"[{count}] starting with {key}: {countOccurrences(root, (int)key)}");
                    // System.Console.WriteLine(countOccurrences(root, (int)key));
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

        Stopwatch sw = new();
        sw.Start();

        CultureInfo cul = new CultureInfo("de-DE"); // formatting floats properly (eu format)
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
        sw.Stop();
        System.Console.WriteLine($"DONE! execution time: {sw.ElapsedMilliseconds}ms");
        // if (find(root, 0))
        // {
        //     System.Console.WriteLine("found 0");
        // }
        // else System.Console.WriteLine("nope");


    }
}