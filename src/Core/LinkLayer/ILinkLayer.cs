using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core.LinkLayer
{
    public interface ILinkLayer
    {
        void Open();
        void Close();
        bool Opened { get; }
        object Configuration { get; set; }

        byte[] Transact(byte[] p);
    }
}
