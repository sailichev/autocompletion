namespace Autocompletion.Interfaces
{
	using System.Collections.Generic;


	public interface IAutocompletionSource
	{
		IEnumerable<string> GetStrings(string prefix);
	}
}
