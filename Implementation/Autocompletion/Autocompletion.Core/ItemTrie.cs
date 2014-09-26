namespace Autocompletion.Core
{
	using System.Collections.Generic;

	using Autocompletion.Interfaces;

	
	public class ItemTrie : Trie<Item>, IAutocompletionSource
	{
		public void Add(string value, int relevancy)
		{
			this.Add(value, new Item(value, relevancy));
		}

		public IEnumerable<string> GetStrings(string prefix)
		{
			foreach (var a in this.Get(prefix))
				yield return a.Value;
		}
	}
}
