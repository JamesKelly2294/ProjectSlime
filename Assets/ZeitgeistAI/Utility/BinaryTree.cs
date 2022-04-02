using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree<T>
{
    public enum BinaryTreeTraversal
    {
        BreadthFirst,
        DepthFirst
    }

    private IList<(T, bool)> elements;

    public BinaryTree()
    {
        this.elements = new List<(T, bool)>();
    }

    public BinaryTree(IList<(T, bool)> elements)
    {
        this.elements = elements;
    }

    public (T, bool) LeftNode(int nodeOffset)
    {
        Debug.Assert(GuardClauses.IsNotEmpty(elements), "Binary Tree is empty, nothing to Traverse");
        Debug.Assert(nodeOffset < elements.Count, "Attempted to go out of bounds during Binary Tree Traversal");
        int leftIndex = 2 * nodeOffset + 1;
        return this.elements[leftIndex];
    }

    public (T, bool) RightNode(int nodeOffset)
    {
        Debug.Assert(GuardClauses.IsNotEmpty(elements), "Binary Tree is empty, nothing to Traverse");
        Debug.Assert(nodeOffset < elements.Count, "Attempted to go out of bounds during Binary Tree Traversal");
        int rightIndex = 2 * nodeOffset + 2;
        return this.elements[rightIndex];
    }

    public void Traverse(BinaryTreeTraversal traversal, Action<(T, bool)> visit)
    {
        Debug.Assert(GuardClauses.IsNotEmpty(elements), "Binary T ree is Empty! Nothing to Traverse!");
        switch(traversal)
        {
            case BinaryTreeTraversal.BreadthFirst:
                var queue = new Queue<int>();
                queue.Enqueue(0);
                while(queue.Count > 0)
                {
                    int index = queue.Dequeue();
                    (T value, bool isLeaf) element = elements[index];
                    visit(element);
                    if (!element.isLeaf)
                    {
                        queue.Enqueue(2 * index + 1);
                        queue.Enqueue(2 * index + 2);
                    }
                }
                break;

            case BinaryTreeTraversal.DepthFirst:
                var stack = new Stack<int>();
                stack.Push(0);
                while (stack.Count > 0)
                {
                    int index = stack.Pop();
                    (T value, bool isLeaf) element = elements[index];
                    visit(element);
                    if (!element.isLeaf)
                    {
                        stack.Push(2 * index + 1);
                        stack.Push(2 * index + 2);
                    }
                }
                break;
        }
    }
}
