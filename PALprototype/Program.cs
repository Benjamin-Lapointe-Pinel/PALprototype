using paPrototype;
using System;

namespace PALprototype
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new PaPrototype())
				game.Run();
		}
	}
}
