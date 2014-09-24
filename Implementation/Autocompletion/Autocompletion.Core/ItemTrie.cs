namespace Autocompletion.Core
{
	public class ItemTrie : Trie<Item>
	{
		public void Add(string value, int relevancy)
		{
			this.Add(value, new Item(value, relevancy));
		}
	}
}
