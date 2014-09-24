namespace Autocompletion.Core
{
	using System.Collections.Generic;
	using System.Linq;


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

		public void Prepare(int count)
		{
			this.Prepare(count, Comparer<T>.Default);
		}
		public void Prepare(int count, Comparer<T> comparer)
		{
			Prepare(this.root, count, comparer);
		}

		public IEnumerable<T> Get(string prefix)
		{
			var current = this.root;

			foreach (char p in prefix)
			{
				if (!current.Children.ContainsKey(p))
					return Enumerable.Empty<T>();

				current = current.Children[p];
			}

			return current.Cache;
		}


		private static void Prepare(Node node, int count, Comparer<T> comparer)
		{
			foreach (var child in node.Children)
				Prepare(child.Value, count, comparer);

			node.Cache = node.Items.OrderBy(_ => _, comparer).Take(count).ToArray();
		}


		private class Node
		{
			public readonly IDictionary<char, Node> Children = new Dictionary<char, Node>();
	
			public readonly ICollection<T> Items = new List<T>();

			public T[] Cache;
		}
	}
}
