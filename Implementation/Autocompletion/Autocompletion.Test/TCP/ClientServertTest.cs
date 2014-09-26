namespace Autocompletion.Test.TCP
{
	using System.IO;
	using System.Linq;
	using System.Diagnostics;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Autocompletion.Core;
	using Autocompletion.TCP;


	[TestClass]
	public class ClientServertTest
	{
		private const int port = 6501;

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

			using (var server = new Server(port, trie))
				using (var client = new Client("localhost", port))
				{
					Assert.AreEqual(4, client.GetStrings("k").Count());
					Assert.AreEqual(3, client.GetStrings("ka").Count());
					Assert.AreEqual(2, client.GetStrings("kar").Count());
				}
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

			using (var server = new Server(port, trie))
				using (var client1 = new Client("localhost", port))
				using (var client2 = new Client("localhost", port))
				using (var client3 = new Client("localhost", port))
				using (var client4 = new Client("localhost", port))
				{
					var timer = Stopwatch.StartNew();

					foreach (var a in testPrefixData)
					{
						client1.GetStrings(a);
						client2.GetStrings(a);
						client3.GetStrings(a);
						client4.GetStrings(a);
					}

					timer.Stop();

					Assert.IsTrue(timer.Elapsed.Milliseconds <= 500 * 4, "operation takes longer than half a second per client on the test data");
				}		
		}
	}
}
