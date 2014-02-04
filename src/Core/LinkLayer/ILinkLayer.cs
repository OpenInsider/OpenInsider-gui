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
        bool IsOpen { get; }
        object Configuration { get; set; }

        int Read(byte[] data);
        int Write(byte[] data);
        void DiscardInput();
        void DiscardOutput();

        event EventHandler ConnectionChanged;
    }
}
