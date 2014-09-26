namespace Autocompletion.TCP.ServerEnvironment
{
	using System;
	using System.Linq;
	using System.IO;

	using Autocompletion.Core;


	public class Program
	{
		public static void Main(string[] args)
		{
			const int resultCount = 10;

			string file = args[0];
			int port = int.Parse(args[1]);

			var trie = new ItemTrie();

			Console.WriteLine("Initializing data from file: {0}", file);
			Init(trie, file, resultCount);

			using (var server = new Server(port, trie))
			{
				Console.WriteLine("Server started at port: {0}", port);
				Console.WriteLine("Press any key to exit");
				Console.ReadKey(true);
			}
		}

		private static void Init(ItemTrie trie, string file, int resultCount)
		{
			var testIn = File.ReadAllLines(file);

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

			foreach (var a in dictionaryData)
				trie.Add(a.value, a.relevancy);

			trie.Prepare(resultCount);
		}
	}
}
