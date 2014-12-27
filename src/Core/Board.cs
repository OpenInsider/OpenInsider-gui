using OpenInsider.Core.LinkLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Files;
using System.Windows.Forms;

namespace OpenInsider.Core
{
    public static class Board
    {
		public static ElfFile ProjectFile = null;

        public static ILinkLayer Link;
        public static List<WatchedVar> Watches = new List<WatchedVar>();
		public static event EventHandler<int> WatchesUpdated;

		static Board()
		{
			Application.Idle += IdleLoop;
		}		

        public static void Dispose()
        {
			Application.Idle -= IdleLoop;

            if ((Link != null) && (Link is IDisposable))
                (Link as IDisposable).Dispose();
        }

		static void IdleLoop(object sender, EventArgs e)
		{
			if ((Link == null) || (!Link.IsOpen))
				return;

			for (int i = 0 ; i < Watches.Count ; i++)
				if (Protocol.ReadWatch(Watches[i]))
					WatchUpdated(i);
		}

		public static void WatchAdd(WatchedVar var)
		{
			Watches.Add(var);

			WatchUpdated(-1);
		}

		public static void WatchRemove(int index)
		{
			Watches.RemoveAt(index);

			WatchUpdated(-1);
		}

		public static void WatchUpdated(int index)
		{
			if (WatchesUpdated != null)
				WatchesUpdated(null, index);
		}
	}
}
