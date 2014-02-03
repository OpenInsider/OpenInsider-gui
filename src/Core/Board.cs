using OpenInsider.Core.LinkLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core
{
    public static class Board
    {
        public static ILinkLayer Link;

        public static void Dispose()
        {
            if ((Link != null) && (Link is IDisposable))
                (Link as IDisposable).Dispose();
        }
    }
}
