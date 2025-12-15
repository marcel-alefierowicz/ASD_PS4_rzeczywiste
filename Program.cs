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
    static int max(int a, int b) { return (a > b) ? a : b; }

    static int Height(Node? n)
    {
        return n?.height ?? 0;
    }
    static int balanceFactor(Node? root)
    {
        return root == null ? 0 : Height(root.left) - Height(root.right);
    }
    static Node rotateLeft(Node? a)
    {
        //       a
        //     /  \
        //    /    \ 
        // (...)    b 
        //        /   \   
        //      sub   (...)

        Node? b = a!.right;
        Node? sub = b!.left;

        b.left = a;
        a.right = sub;


        a.height = 1 + max(Height(a.left), Height(a.right));
        b.height = 1 + max(Height(b.left), Height(b.right));
        //          b
        //        /   \
        //       a   (...)
        //     /  \
        // (...)  sub
        return b;
    }

    static Node rotateRight(Node? a)
    {
        //           a
        //         /   \ 
        //       b   (...)
        //     /   \ 
        //  (...)  sub
        // 

        Node? b = a!.left;
        Node? sub = b!.right;

        b.right = a;
        a.left = sub;

        a.height = 1 + max(Height(a.left), Height(a.right));
        b.height = 1 + max(Height(b.left), Height(b.right));

        //           b
        //         /   \ 
        //      (...)   a
        //            /   \
        //         sub   (...)
        return b;
    }

    static Node insert(Node? root, float val)
    {
        if (root == null)
            return new(val);
        if (val > root.value)
            root.right = insert(root.right, val);
        else if (val < root.value)
            root.left = insert(root.left, val);

        root.height = 1 + max(Height(root.left), Height(root.right));

        int bf = balanceFactor(root);
        int bfr = balanceFactor(root.right);
        int bfl = balanceFactor(root.left);
        // 1. RR: (bf(root) > 1 & bf(root.left) == 1) => rotateRight
        if (bf > 1 && bfl == 1)
            return rotateRight(root);

        // 2. LL: (bf(root) < -1 & bf(root.right) == -1) => rotateLeft
        if (bf < -1 && bfr == -1)
            return rotateLeft(root);

        // 3. LR: (bf(root) > 1 & bf(root.left) == -1) => rotateLeft => rotateRight
        if (bf > 1 && bfl == -1)
        {
            root.left = rotateLeft(root.left);
            return rotateRight(root);

        }
        // 4. RL: (bf(root) > 1 & bf(root.left) == -1) => rotateLeft => rotateRight
        if (bf < -1 && bfr == 1)
        {
            root.right = rotateRight(root.right);
            return rotateLeft(root);
        }
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
            // case 'U':
            //     {
            //         // root = delete(root, key);
            //         break;
            //     }
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
            // case 'L':
            //     {
            //         System.Console.WriteLine($"[{count}] starting with {key}: {countOccurrences(root, (int)key)}");
            //         // System.Console.WriteLine(countOccurrences(root, (int)key));
            //         break;
            //     }
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
        // printTree(root, 5);
        sw.Stop();
        System.Console.WriteLine($"DONE! execution time: {sw.ElapsedMilliseconds}ms");

    }
}