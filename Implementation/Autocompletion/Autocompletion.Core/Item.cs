namespace Autocompletion.Core
{
	using System;

	
	public struct Item : IComparable<Item>
    {
		public readonly string Value;
		public readonly int Relevancy;

		public Item(string value, int relevancy)
		{
			this.Value = value;
			this.Relevancy = relevancy;
		}

		public int CompareTo(Item other)
		{
			var res = -this.Relevancy.CompareTo(other.Relevancy);
			
			if (res == 0)
				res = string.CompareOrdinal(this.Value, other.Value); //this.Value.CompareTo(other.Value);

			return res;
		}
	}
}
