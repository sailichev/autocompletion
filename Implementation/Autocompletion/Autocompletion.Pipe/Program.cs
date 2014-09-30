namespace Autocompletion.Pipe
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Autocompletion.Core;


	public class Program
	{
		public static void Main()
		{
			var testIn = ReadAllInput().ToArray();

			int dictionaryLength = int.Parse(testIn[0]);

			var dictionaryData = testIn.
				Skip(1).
				Take(dictionaryLength).
				Select(_ => _.Split(' ')).
				Select(_ => new
					{
						value = _[0],
						relevancy = int.Parse(_[1])
					}
				);

			int testPrefixCount = int.Parse(testIn[dictionaryLength + 1]);

			var testPrefixData = testIn.
				Skip(dictionaryLength + 1 + 1).
				Take(testPrefixCount)/*.ToArray()*/;

			var trie = new ItemTrie();

			foreach (var a in dictionaryData)
				trie.Add(a.value, a.relevancy);

			trie.Prepare(10);

			foreach (var a in testPrefixData)
			{
				foreach (var b in trie.Get(a))
					Console.Out.WriteLine(b.Value);

				Console.Out.WriteLine();
			}
		}

		private static IEnumerable<string> ReadAllInput()
		{
			string currentLine = Console.In.ReadLine();

			while (currentLine != null)
			{
				yield return currentLine;

				currentLine = Console.In.ReadLine();
			}
		}
	}
}
