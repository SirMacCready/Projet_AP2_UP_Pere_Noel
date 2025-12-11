namespace BigDaddySanta
{
    public class DataStructure
    {
        public class LinkedList<T>
        {
            private Node<T>? _head;
            private Node<T>? _tail;
            private int _count;

            public LinkedList()
            {
                this._head = null;
                this._tail = null;
                this._count = 0;
            }

            public bool IsEmpty()
            {
                return this._count == 0;

            }

            public void InsertAtFirst(T value)
            {
                if (this.IsEmpty())
                {
                    this._head = new Node<T>(value, null);
                    this._tail = this._head;
                }
                else
                {
                    this._head = new Node<T>(value, this._head);
                }
                this._count++;
            }

            public void InsertAtLast(T value)
{
    if (this.IsEmpty())
    {
        InsertAtFirst(value);
        // À CLOÉ : Suppression du this._count++ ici, car déjà fait dans InsertAtFirst
    }
    else
    {
        Node<T> newNode = new Node<T>(value, null);
        this._tail.Next = newNode;
        this._tail = newNode;
        this._count++;
    }
}

            public Node<T> RemoveAtFirst()
            {
                if (this.IsEmpty())
                {
                    throw new ArgumentOutOfRangeException("REMOVE AT FIRST : La liste est vide");
                }
                Node<T> nodeToRemove = this._head;
                if (this._head.Next == null)
                {
                    this._head = null;
                    this._tail = null;
                }
                else
                {
                    this._head = this._head.Next;
                }
                this._count--;
                return nodeToRemove;
            }

            public void ShowList()
            {
                if (this.IsEmpty())
                {
                    Console.WriteLine("SHOW LIST : La liste est vide");
                    return;
                }
                Node<T> pointer = this._head;
                while (pointer != null)
                {
                    Console.Write($"{pointer.Value}" + (pointer.Next == null ? "" : " -> "));
                    pointer = pointer.Next;
                }
                Console.WriteLine();
            }

        }
        public class Node<T>
        {
            public T Value { get; set; }
            internal Node<T>? Next { get; set; }

            public Node(T value, Node<T>? next)
            {
                this.Value = value;
                this.Next = next;
            }
        }

        public class Queue<T>
        {
            private LinkedList<T> LinkedListQueue;

            public Queue()
            {
                this.LinkedListQueue = new LinkedList<T>();
            }

            public bool IsEmpty()
            {
                return this.LinkedListQueue.IsEmpty();
            }

            public void Enqueue(T value)
            {
                this.LinkedListQueue.InsertAtLast(value);
            }

            public Node<T> Dequeue()
            {
                return this.LinkedListQueue.RemoveAtFirst();
            }

        }

        public class Stack<T>
        {
            private LinkedList<T> LinkedListStack;

            public Stack()
            {
                this.LinkedListStack = new LinkedList<T>();
            }
            public bool IsEmpty()
            {
                return this.LinkedListStack.IsEmpty();
            }

            public void Push(T value)
            {
                this.LinkedListStack.InsertAtFirst(value);
            }

            public Node<T> Pop()
            {
                return this.LinkedListStack.RemoveAtFirst();
            }
        }
    }
}