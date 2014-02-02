using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core.LinkLayer
{
    class LinkSerialConfiguration
    {
        [Category("Port settings")]
        public string PortName {get; set; }

        [Category("Port settings")]
        public int BaudRate {get; set; }
    }

    class LinkSerial : ILinkLayer, IDisposable
    {
        SerialPort port = new SerialPort();

        public LinkSerial()
        {
            port.BaudRate = 115200;
            port.PortName = "COM19";
        }

        public void Dispose()
        {
            port.Dispose();
        }

        public void Open()
        {
            port.Open();
        }

        public void Close()
        {
            port.Close();
        }

        public bool Opened
        {
            get { return port.IsOpen; }
        }

        public object Configuration
        {
            get
            {
                LinkSerialConfiguration ls = new LinkSerialConfiguration();
                ls.PortName = port.PortName;
                ls.BaudRate = port.BaudRate;
                return ls;
            }
            set
            {
                LinkSerialConfiguration ls = value as LinkSerialConfiguration;

                if (ls == null)
                    throw new InvalidOperationException("Bad configuration set ");

                if (port.IsOpen)
                    port.Close();

                port.PortName = ls.PortName;
                port.BaudRate = ls.BaudRate;                
            }
        }
       
    }
}
