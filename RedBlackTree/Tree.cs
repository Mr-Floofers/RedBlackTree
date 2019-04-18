using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public class Tree<T> 
    {
        /// <summary>
        /// A costom comparer that is declared in the "Program" class to properly comapare strings(words) and int, float, double, and long types(numbers)
        /// </summary>
        IComparer<T> comparer;

        /// <summary>
        /// The top of the tree that is used as the begining to traverse the tree
        /// </summary>
        Node<T> Root;

        /// <summary>
        /// Creates a clone of the Root node that can be read but not changed
        /// </summary>
        public Node<T> RootNode
        {
            get
            {
                return Root;
            }
            set
            {
                RootNode = Root;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Left Leaning Red Black Tree
        /// </summary>
        /// <param name="comparer">A comparer so that nubers and words can be comapred to eachother</param>
        public Tree(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        /// <summary>
        /// Flip the colors of the specified node and its direct children.
        /// </summary>
        /// <param name="current">Specified node.</param>
        private void flipColor(Node<T> current)
        {

            current.red = !current.red;
            if (current.left != null) current.left.red = !current.left.red;
            if (current.right != null) current.right.red = !current.right.red;

        }
        /// <summary>
        /// Rotate the specified node "left" and perform recoloring.
        /// </summary>
        /// <param name="current">Specified node.</param>
        /// <returns>New root node.</returns>
        public Node<T> rotateLeft(Node<T> current)
        {
            Node<T> node = current.right;
            current.right = node.left;
            node.left = current;

            node.red = current.red;
            current.red = true;
            return node;
        }
        /// <summary>
        /// Rotate the specified node "right" and perform recoloring.
        /// </summary>
        /// <param name="current">Specified node.</param>
        /// <returns>New root node.</returns>
        public Node<T> rotateRight(Node<T> current)
        {
            Node<T> node = current.left;
            current.left = node.right;
            node.right = current;

            node.red = current.red;
            current.red = true;
            return node;
        }
        /// <summary>
        /// Checks if the given node is red
        /// </summary>
        /// <param name="current"></param>
        /// <returns>True if red, False if black</returns>
        public bool isRed(Node<T> current)
        {
            return current == null ? false : current.red;
        }
        /// <summary>
        /// Adds a value to the tree.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(T value)
        {
            Root = Add(Root, value);
            Root.red = false;
        }

        /// <summary>
        /// Adds the specified value below the specified root node.
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <param name="value">Value to add.</param>
        /// <returns>New root node.</returns>
        private Node<T> Add(Node<T> current, T value)
        {
            if (current == null)
            {
                //Insert a new node
                return new Node<T>(value);
            }

            if (isRed(current.left) && isRed(current.right))
            {
                // Split (4-node) node with two red children
                flipColor(current);
            }

            //Finding the right place for the new node recursively
            if (comparer.Compare(value, current.value) < 0)
            {
                current.left = Add(current.left, value);
            }
            else if (comparer.Compare(value, current.value) > 0)
            {
                current.right = Add(current.right, value);
            }
            else
            {
                // There are 3 options when dealing with duplicate values
                // 1) Throw an excaption.
                // 2) Allow duplicate values on right (or left, just be consistent).
                // 3) Add a Count variable to the node class and increment it when a duplicate values arrives.
                throw new ArgumentException("A node with the same value arleady exists");
            }


            if (isRed(current.right))
            {
                //Rotating the node to prevent a red node as a right child
                current = rotateLeft(current);
            }

            if (isRed(current.left) && isRed(current.left.left))
            {
                //Rotate to prevent two consecutive red nodes
                current = rotateRight(current);
            }

            return current;
        }
        /// <summary>
        /// Moves a red node form the left child to the right child.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <returns>New root node.</returns>
        Node<T> RightMove(Node<T> node)
        {
            flipColor(node);
            if (isRed(node.right.left))
            {
                node.right = rotateRight(node.right);
                node = rotateRight(node);
                flipColor(node);

                if (isRed(node.right.right))
                {
                    node.right = rotateLeft(node.right);
                }
            }
            return node;
        }
        /// <summary>
        /// Moves a red node from the right child to the left child.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <returns>New root node.</returns>
        Node<T> LeftMove(Node<T> node)
        {
            flipColor(node);
            if (isRed(node.left.left))
            {
                node = rotateRight(node);
                flipColor(node);
            }
            return node;
        }

        /// <summary>
        /// Removes a value from the tree.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        public void Remove(T value)
        {
            Root = Remove(Root, value);
            if (Root != null)
            {
                Root = Remove(Root, value);
                if (Root != null)
                {
                    Root.red = false;
                }
            }
        }

        /// <summary>
        /// Removes the specified value from below the specified node.
        /// </summary>
        /// <remarks>
        /// The end goal of remove is to ensure that the value we want to remove is within a 3-node or a 4-node.
        /// The Fixup function will recover the invariants of the tree as the recursion unwinds.
        /// </remarks>
        /// <param name="node">Specified node.</param>
        /// <param name="value">Value to remove.</param>
        /// <returns></returns>
        private Node<T> Remove(Node<T> current, T value)
        {
            if (comparer.Compare(value, current.value) < 0)
            {
                // Continue search if left is present
                if (current.left != null)
                {
                    // As we travel down the left, carry a red node with us if necessary
                    if (!isRed(current.left) && !isRed(current.left.left))
                    {
                        // Move a red node over
                        current = LeftMove(current);
                    }
                    // Remove from left recursive call
                    current.left = Remove(current.left, value);
                }
            }
            else
            {
                if (isRed(current.left))
                {
                    // Flip a node with 1 red child (3-node) or unbalance a node with 2 red children (4-node)
                    current = rotateRight(current);
                }
                if (current.right == null)
                {
                    // Remove a leaf node
                    return null;
                }
                // Continue search if right is present
                if (current.right != null)
                {
                    // As we travel down the right, carry a red node with us if necessary
                    if (!isRed(current.right) && !isRed(current.right.left))
                    {
                        current = RightMove(current);
                    }
                    if (comparer.Compare(value, current.value) == 0)
                    {
                        // Remove a node with 2 children
                        // Find the smallest node on the right sub-tree, swap, and remove it
                        // (binary tree removal when node has 2 children)
                        Node<T> small = min(current.right);
                        current.value = small.value;
                        current.right = Remove(current.right, small.value);
                    }
                    else
                    {
                        // Remove from right recursive call
                        current.right = Remove(current.right, value);
                    }
                }
            }
            // Maintains invariants
            return fix(current);
        }
        /// <summary>
        /// Maintains rules/invariants by adjusting the specified nodes children.
        /// </summary>
        /// <param name="current">Specified node.</param>
        /// <returns>New root node.</returns>
        public Node<T> fix(Node<T> current)
        {
            if (isRed(current.right))
            {
                // Enforce left leaning policy
                current = rotateLeft(current);
            }
            if (isRed(current.left) && isRed(current.left.left))
            {
                // balance a (4-node) node with 2 reds on the left
                current = rotateRight(current);
            }
            if (isRed(current.left) && isRed(current.right))
            {
                // Push Red Up (break up 4-node)
                flipColor(current);
            }
            // Avoid leaving behind right-leaning nodes
            if (current.left != null && isRed(current.left.right) && !isRed(current.left.left))
            {
                current.left = rotateLeft(current.left);
                // Avoid red touching red
                if (isRed(current.left))
                {
                    //Balance a (4-node) node with 2 red children
                    current = rotateRight(current);
                }
            }
            return current;
        }

        public Node<T> max(Node<T> node)
        {
            if (node.right != null)
            {
                node = node.right;
                max(node);
            }
            return node;
        }
        public Node<T> min(Node<T> node)
        {
            if (node.left != null)
            {
                node = node.left;
                min(node);
            }
            return node;
        }
        public string ShowRed()
        {
            int count = 0;
            List<Node<T>> nodes = new List<Node<T>>();
            string final;
            Search(Root);
            void Search(Node<T> node)
            {
                if (isRed(node))
                {
                    count++;
                    nodes.Add(node);
                }
                if (node.left != null)
                {
                    Search(node.left);
                }
                if (node.right != null)
                {
                    Search(node.right);
                }
            }
            final = $"{count} : ";
            for (int i = 0; i < count; i++)
            {
                final = $"{final} {nodes[i].value}, ";
            }

            return final;
        }
        public string ShowBlack()
        {
            int count = 0;
            List<Node<T>> nodes = new List<Node<T>>();
            string final;
            Search(Root);
            void Search(Node<T> node)
            {
                if (node.red == false)
                {
                    count++;
                    nodes.Add(node);
                }
                if (node.left != null)
                {
                    Search(node.left);
                }
                if (node.right != null)
                {
                    Search(node.right);
                }
            }
            final = $"{count} : ";
            for (int i = 0; i < count; i++)
            {
                final = $"{final}, {nodes[i].value}";
            }

            return final;
        }

        public IEnumerable<Node<T>> BreadthFirst()
        {
            Queue<Node<T>> tempNodes = new Queue<Node<T>>();
            tempNodes.Enqueue(Root);
            Queue<Node<T>> allNodes = new Queue<Node<T>>();

            while (tempNodes.Count != 0)
            {
                Node<T> removedNode = tempNodes.Dequeue();
                allNodes.Enqueue(removedNode);
                if (removedNode.right != null)
                {
                    tempNodes.Enqueue(removedNode.right);
                }
                if (removedNode.left != null)
                {
                    tempNodes.Enqueue(removedNode.left);
                }
            }
            return allNodes;
        }


        /// <summary>
        /// The fuction that calls the other display function so that the node that is being displayed can be chaged
        /// </summary>
        public void Display()
        {
            Display(Root, Console.BufferWidth / 2, Console.BufferWidth / 2, 0, 4);
        }
        /// <summary>
        /// Displays the values and line linking the values in a tree structure
        /// </summary>
        /// <param name="node">The current node that is being diplayed</param>
        /// <param name="offset">The distance away from the left side of the Console to the node being displayed</param>
        /// <param name="x">The X position of the node being diplayed</param>
        /// <param name="y">The Y position of the node being diplayed</param>
        /// <param name="lineoffset">The line difference between each level of nodes</param>
        private void Display(Node<T> node, int offset = 10, int x = 20, int y = 0, int lineoffset = 2)
        {
            Console.SetCursorPosition(x, y + 1);
            Console.ForegroundColor = ConsoleColor.Blue;
            if (node.right != null && node.left != null)
            {
                Console.WriteLine("│");
                Console.SetCursorPosition(x, y + 2);
                Console.WriteLine("┴");
            }
            if (node.right != null && node.left == null)
            {
                Console.WriteLine("│");
                Console.SetCursorPosition(x, y + 2);
                Console.WriteLine("└");
            }
            if (node.right == null && node.left != null)
            {
                Console.WriteLine("│");
                Console.SetCursorPosition(x, y + 2);
                Console.WriteLine("┘");
            }

            for (int i = 0; i < offset / 2 - 1; i++)
            {
                if (node.right == null && node.left != null)
                {
                    Console.SetCursorPosition(x - i - 1, y + 2);
                    Console.WriteLine("─");
                    if (i == offset / 2 - 2)
                    {
                        Console.SetCursorPosition(x - offset / 2 + 1, y + 2);
                        Console.WriteLine("┌");
                        Console.SetCursorPosition(x - offset / 2 + 1, y + 3);
                        Console.WriteLine("│");
                    }
                }
                if (node.right != null && node.left == null)
                {
                    Console.SetCursorPosition(x + i + 1, y + 2);
                    Console.WriteLine("─");
                    if (i == offset / 2 - 2)
                    {
                        Console.SetCursorPosition(x + offset / 2, y + 2);
                        Console.WriteLine("┐");
                        Console.SetCursorPosition(x + offset / 2, y + 3);
                        Console.WriteLine("│");
                    }
                }
                if (node.right != null && node.left != null)
                {
                    Console.SetCursorPosition(x - i - 1, y + 2);
                    Console.WriteLine("─");
                    if (i == offset / 2 - 2)
                    {
                        Console.SetCursorPosition(x - offset / 2 + 1, y + 2);
                        Console.WriteLine("┌");
                        Console.SetCursorPosition(x - offset / 2 + 1, y + 3);
                        Console.WriteLine("│");

                    }
                    Console.SetCursorPosition(x + i + 1, y + 2);
                    Console.WriteLine("─");
                    if (i == offset / 2 - 2)
                    {
                        Console.SetCursorPosition(x + offset / 2, y + 2);
                        Console.WriteLine("┐");
                        Console.SetCursorPosition(x + offset / 2, y + 3);
                        Console.WriteLine("│");

                    }
                }
            }

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = node.red ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(node.value);
            offset /= 2;

            if (node.left != null)
            {
                Display(node.left, offset, x - offset, y + lineoffset, lineoffset);
            }
            if (node.right != null)
            {
                Display(node.right, offset, x + offset, y + lineoffset, lineoffset);
            }
        }




    }
}
