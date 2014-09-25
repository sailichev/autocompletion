namespace Autocompletion.Core
{
	using System;
	using System.Collections.Generic;


	public class Trie<T>
	{
		private readonly Node root = new Node();

		public void Add(string prefix, T item)
		{
			var current = this.root;
			current.Items.Add(item);

			foreach (char p in prefix)
			{
				if (!current.Children.ContainsKey(p))
					current.Children.Add(p, new Node());

				current = current.Children[p];
				
				current.Items.Add(item);
			}
		}

		public void Prepare(int resultCount = int.MaxValue, Comparer<T> comparer = null)
		{
			Prepare(this.root, resultCount > 0 ? resultCount : int.MaxValue, comparer ?? Comparer<T>.Default);
		}

		public IEnumerable<T> Get(string prefix)
		{
			var current = this.root;

			foreach (char p in prefix)
			{
				if (!current.Children.ContainsKey(p))
					return new T[0];

				current = current.Children[p];
			}

			return current.Cache;
		}

		private static void Prepare(Node node, int count, Comparer<T> comparer)
		{
			foreach (var child in node.Children)
				Prepare(child.Value, count, comparer);

			node.Cache = new T[Math.Min(count, node.Items.Count)];
			node.Items.Sort(comparer);
			node.Items.CopyTo(0, node.Cache, 0, node.Cache.Length);
		}


		private class Node
		{
			public readonly IDictionary<char, Node> Children = new Dictionary<char, Node>();

			public readonly List<T> Items = new List<T>();

			public T[] Cache = new T[0];
		}
	}
}
