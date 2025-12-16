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