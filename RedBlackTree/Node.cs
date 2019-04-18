using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    /// <summary>
    /// Represents a node of the tree
    /// </summary>
    /// <typeparam name="T">A generic data type</typeparam>
    public class Node<T>
    {
        /// <summary>
        /// Gets or sets the nodes value
        /// </summary>
        public T value;

        /// <summary>
        /// Gets or sets the right child of the node
        /// </summary>
        public Node<T> right;

        /// <summary>
        /// Gets or sets the left child of the node
        /// </summary>
        public Node<T> left;

        /// <summary>
        /// Gets or sets the nodes color
        /// </summary>
        /// <remarks>
        /// Defaulted to true because all new nodes are red at creation
        /// </remarks>
        public bool red = true;

        /// <summary>
        /// Initializes a new instance of the Node class
        /// </summary>
        /// <param name="value">The value of the new node</param>
        public Node(T value)
        {
            this.value = value;
        }

    }
}
