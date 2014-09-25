namespace Autocompletion.Test.Core
{
	using System.IO;
	using System.Linq;
	using System.Diagnostics;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Autocompletion.Core;


	[TestClass]
	public class TrieTest
	{
		[TestMethod]
		public void GetTest()
		{
			var trie = new ItemTrie();

			trie.Add("kare", 10);
			trie.Add("kanojo", 20);
			trie.Add("karetachi", 1);
			trie.Add("korosu", 7);
			trie.Add("sakura", 3);

			trie.Prepare(10);

			Assert.AreEqual(4, trie.Get("k"  ).Count());
			Assert.AreEqual(3, trie.Get("ka" ).Count());
			Assert.AreEqual(2, trie.Get("kar").Count());
		}

		[TestMethod]
		public void GetLoadTest()
		{
			#region getting test data from file

			var testIn = File.ReadAllLines("test.in");
			
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
				Take(testPrefixCount).ToArray();

			#endregion

			var trie = new ItemTrie();

			foreach (var a in dictionaryData)
				trie.Add(a.value, a.relevancy);

			trie.Prepare(10);

			
			var timer = Stopwatch.StartNew();

			foreach (var a in testPrefixData)
				trie.Get(a);

			timer.Stop();

			Assert.IsTrue(timer.Elapsed.Milliseconds <= 500, "operation takes longer than half a second on the test data");
		}
	}
}
