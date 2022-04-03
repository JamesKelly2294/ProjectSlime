using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTreeNode<D>
{
    public D data;
    public bool isLeaf;

    public BinaryTreeNode(D data, bool isLeaf)
    {
        this.data = data;
        this.isLeaf = isLeaf;
    }
}

public enum BinaryTreeTraversal
{
    BreadthFirst,
    DepthFirst
}

public class BinaryTree<T>
{
    private IList<BinaryTreeNode<T>> elements;

    public BinaryTree()
    {
        this.elements = new List<BinaryTreeNode<T>>();
    }

    public BinaryTree(List<BinaryTreeNode<T>> elements)
    {
        this.elements = elements;
    }

    public BinaryTree(BinaryTreeNode<T>[] elements)
    {
        this.elements = new List<BinaryTreeNode<T>>(elements);
    }

    public BinaryTreeNode<T> Root()
    {
        Debug.Assert(GuardClauses.IsNotEmpty(elements), "No elements in tree.");
        return elements[0];
    }

    public BinaryTreeNode<T> this[int index]
    {
        get
        {
            return elements[index];
        }
    }

    public int LeftIndex(int index)
    {
        return 2 * index + 1;
    }

    public int RightIndex(int index)
    {
        return 2 * index + 2;
    }

    public BinaryTreeNode<T> LeftNode(int nodeOffset)
    {
        Debug.Assert(GuardClauses.IsNotEmpty(elements), "Binary Tree is empty, nothing to Traverse");
        Debug.Assert(nodeOffset < elements.Count, "Attempted to go out of bounds during Binary Tree Traversal");
        int leftIndex = 2 * nodeOffset + 1;
        return this.elements[leftIndex];
    }

    public BinaryTreeNode<T> RightNode(int nodeOffset)
    {
        Debug.Assert(GuardClauses.IsNotEmpty(elements), "Binary Tree is empty, nothing to Traverse");
        Debug.Assert(nodeOffset < elements.Count, "Attempted to go out of bounds during Binary Tree Traversal");
        int rightIndex = 2 * nodeOffset + 2;
        return this.elements[rightIndex];
    }

    public void Traverse(BinaryTreeTraversal traversal, Action<BinaryTreeNode<T>> visit)
    {
        Debug.Assert(GuardClauses.IsNotEmpty(elements), "Binary Tree is Empty! Nothing to Traverse!");
        switch(traversal)
        {
            case BinaryTreeTraversal.BreadthFirst:
                var queue = new Queue<int>();
                queue.Enqueue(0);
                while(queue.Count > 0)
                {
                    int index = queue.Dequeue();
                    BinaryTreeNode<T> element = elements[index];
                    visit(element);
                    if (!element.isLeaf)
                    {
                        int left = 2 * index + 1;
                        int right = 2 * index + 2;
                        Debug.Assert(left < elements.Count, "Left child node in binary tree is out of bounds!");
                        Debug.Assert(right < elements.Count, "Right child node in binary tree is out of bounds!");
                        queue.Enqueue(left);
                        queue.Enqueue(right);
                    }
                }
                break;

            case BinaryTreeTraversal.DepthFirst:
                var stack = new Stack<int>();
                stack.Push(0);
                while (stack.Count > 0)
                {
                    int index = stack.Pop();
                    BinaryTreeNode<T> element = elements[index];
                    visit(element);
                    if (!element.isLeaf)
                    {
                        int left = 2 * index + 1;
                        int right = 2 * index + 2;
                        Debug.Assert(left < elements.Count, "Left child node in binary tree is out of bounds!");
                        Debug.Assert(right < elements.Count, "Right child node in binary tree is out of bounds!");
                        stack.Push(left);
                        stack.Push(right);
                    }
                }
                break;
        }
    }
}
