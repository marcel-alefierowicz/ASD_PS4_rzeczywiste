public class AVL
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

    public static Node insert(Node? root, float val)
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

        // 1. RR rotation: (bf(root) > 1 & bf(root.left) == 1)
        if (bf > 1 && bfl == 1)
            return rotateRight(root);

        // 2. LL rotation: (bf(root) < -1 & bf(root.right) == -1)
        if (bf < -1 && bfr == -1)
            return rotateLeft(root);

        // 3. LR rotation: (bf(root) > 1 & bf(root.left) == -1)
        if (bf > 1 && bfl == -1)
        {
            root.left = rotateLeft(root.left);
            return rotateRight(root);

        }

        // 4. RL rotation: (bf(root) > 1 & bf(root.left) == -1)
        if (bf < -1 && bfr == 1)
        {
            root.right = rotateRight(root.right);
            return rotateLeft(root);
        }

        return root;
    }

    public static bool find(Node? root, float val)
    {
        if (root == null) return false;

        if (root.value == val) return true;

        if (root.value > val)
            return find(root.left, val);
        return find(root.right, val);
    }

    static Node nextInOrder(Node? root)
    {
        root = root?.right; // looking in the right subtree
        while (root != null && root.left != null)
        {
            root = root.left;
        }
        return root!;
    }

    public static Node delete(Node? root, float val)
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
        root.height = 1 + max(Height(root.left), Height(root.right));

        int bf = balanceFactor(root);
        int bfr = balanceFactor(root.right);
        int bfl = balanceFactor(root.left);

        // 1. RR rotation: (bf(root) > 1 & bf(root.left) == 1)
        if (bf > 1 && bfl == 1)
            return rotateRight(root);

        // 2. LL rotation: (bf(root) < -1 & bf(root.right) == -1)
        if (bf < -1 && bfr == -1)
            return rotateLeft(root);

        // 3. LR rotation: (bf(root) > 1 & bf(root.left) == -1)
        if (bf > 1 && bfl == -1)
        {
            root.left = rotateLeft(root.left);
            return rotateRight(root);

        }

        // 4. RL rotation: (bf(root) > 1 & bf(root.left) == -1)
        if (bf < -1 && bfr == 1)
        {
            root.right = rotateRight(root.right);
            return rotateLeft(root);
        }

        return root;
    }

    public static void printTree(Node? root, int indent)
    {
        if (root != null)
        {
            printTree(root.right, indent + 4);
            if (indent > 0)
                Console.Write(" ".PadLeft(indent));
            Console.WriteLine($"{root.value}");
            printTree(root.left, indent + 4);
        }
    }
    public static int countOccurrences(Node? root, int key)
    {
        if (root == null)
            return 0;

        int match = ((int)root.value == key) ? 1 : 0;

        return match
             + countOccurrences(root.left, key)
             + countOccurrences(root.right, key);
    }

}