using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace OpenInsider.Core.LinkLayer
{
    #region Structures
    internal struct COMMTIMEOUTS
    {
        public int ReadIntervalTimeout;
        public int ReadTotalTimeoutMultiplier;
        public int ReadTotalTimeoutConstant;
        public int WriteTotalTimeoutMultiplier;
        public int WriteTotalTimeoutConstant;
    }

    internal struct DCB
    {
        public uint DCBlength;
        public uint BaudRate;
        public uint Flags;
        public ushort wReserved;
        public ushort XonLim;
        public ushort XoffLim;
        public byte ByteSize;
        public byte Parity;
        public byte StopBits;
        public byte XonChar;
        public byte XoffChar;
        public byte ErrorChar;
        public byte EofChar;
        public byte EvtChar;
        public ushort wReserved1;
    }

    internal struct COMMPROP
    {
        public ushort wPacketLength;
        public ushort wPacketVersion;
        public int dwServiceMask;
        public int dwReserved1;
        public int dwMaxTxQueue;
        public int dwMaxRxQueue;
        public int dwMaxBaud;
        public int dwProvSubType;
        public int dwProvCapabilities;
        public int dwSettableParams;
        public int dwSettableBaud;
        public ushort wSettableData;
        public ushort wSettableStopParity;
        public int dwCurrentTxQueue;
        public int dwCurrentRxQueue;
        public int dwProvSpec1;
        public int dwProvSpec2;
        public char wcProvChar;
    }

    internal struct COMMSTAT
    {
        public uint Flags;
        public uint cbInQue;
        public uint cbOutQue;
    }

    [StructLayout( LayoutKind.Sequential )] 
    internal struct COMMCONFIG 
    {
       public uint dwSize;
       public ushort wVersion;
       public ushort wReserved;
       public DCB dcb;
       public uint dwProviderSubType;
       public uint dwProviderOffsert;
       public uint dwProviderSize;
    }
    #endregion

    #region Enums
    [Flags]
    public enum ExtendedFunctions : uint
    {
        CLRBREAK = 9, //Restores character transmission and places the transmission line in a nonbreak state.
        CLRDTR = 6, //Clears the DTR (data-terminal-ready) signal.
        CLRRTS = 4, //Clears the RTS (request-to-send) signal.
        SETBREAK = 8, //Suspends character transmission and places the transmission line in a break state until the ClearCommBreak function is called
        SETDTR = 5, //Sends the DTR (data-terminal-ready) signal.
        SETRTS = 3, //Sends the RTS (request-to-send) signal.
        SETXOFF = 1, //Causes transmission to act as if an XOFF character has been received.
        SETXON = 2 //Causes transmission to act as if an XON character has been received.
    }

    [Flags]
    public enum AccessMask : uint
    {
        GENERIC_READ = (1U << 31),
        GENERIC_WRITE = (1U << 30),
        GENERIC_READWRITE = GENERIC_READ | GENERIC_WRITE,
        GENERIC_EXECUTE = (1U << 29),
        GENERIC_ALL = (1U << 28),
        MAXIMUM_ALLOWED = (1U << 25),
        ACCESS_SYSTEM_SECURITY = (1U << 24),
    }

    public enum Disposition : uint
    {
        CREATE_NEW = 1,
        CREATE_ALWAYS = 2,
        OPEN_EXISTING = 3,
        OPEN_ALWAYS = 4,
        TRUNCATE_EXISTING = 5,
    }

    [Flags]
    public enum FileFlags : uint
    {
        FILE_ATTRIBUTE_READONLY = (1U << 0),
        FILE_ATTRIBUTE_HIDDEN = (1U << 1),
        FILE_ATTRIBUTE_SYSTEM = (1U << 2),
        FILE_ATTRIBUTE_ARCHIVE = (1U << 5),
        FILE_ATTRIBUTE_NORMAL = (1U << 7),
        FILE_ATTRIBUTE_TEMPORARY = (1U << 8),
        FILE_ATTRIBUTE_OFFLINE = (1U << 12),
        FILE_ATTRIBUTE_ENCRYPTED = (1U << 14),

        SECURITY_SQOS_PRESENT = 0x00100000, 
        SECURITY_ANONYMOUS = 0x00010000,
        SECURITY_IDENTIFICATION = 0x00020000,
        SECURITY_IMPERSONATION = 0x00030000,
        SECURITY_DELEGATION = 0x00040000,
        SECURITY_CONTEXT_TRACKING = 0x00040000,
        SECURITY_EFFECTIVE_ONLY = 0x00080000,

        FILE_FLAG_OPEN_NO_RECALL = (1U << 20),
        FILE_FLAG_OPEN_REPARSE_POINT = (1U << 21),
        FILE_FLAG_SESSION_AWARE = (1U << 23),

        FILE_FLAG_POSIX_SEMANTICS = (1U << 24),
        FILE_FLAG_BACKUP_SEMANTICS = (1U << 25),
        FILE_FLAG_DELETE_ON_CLOSE = (1U << 26),
        FILE_FLAG_SEQUENTIAL_SCAN = (1U << 27),
        FILE_FLAG_RANDOM_ACCESS = (1U << 28),
        FILE_FLAG_NO_BUFFERING = (1U << 29),
        FILE_FLAG_OVERLAPPED = (1U << 30),
        FILE_FLAG_WRITE_THROUGH = (1U << 31),
    }

    [Flags]
    public enum SerialEvents : uint
    {
        EV_RXCHAR = 0x00001,
        EV_RXFLAG = 0x00002,
        EV_TXEMPTY = 0x00004,
        EV_CTS = 0x00008,
        EV_DSR = 0x00010,
        EV_RLSD = 0x00020,
        EV_BREAK = 0x00040,
        EV_ERR = 0x00080,
        EV_RING = 0x00100,
        EV_PERR = 0x00200,
        EV_RX80FULL = 0x00400,
        EV_EVENT1 = 0x00800,
        EV_EVENT2 = 0x01000,
    }

    [Flags]
    public enum PurgeFlags : uint
    {
        PURGE_TXABORT = 0x00001,
        PURGE_RXABORT = 0x00002,
        PURGE_TXCLEAR = 0x00004,
        PURGE_RXCLEAR = 0x00008,
    }

    [Flags]
    public enum CommError : uint
    {
        CE_RXOVER = 0x00001,
        CE_OVERRUN = 0x00002,
        CE_RXPARITY = 0x00004,
        CE_FRAME = 0x00008,
        CE_BREAK = 0x00010,
        CE_TXFULL = 0x00100,
        CE_PTO = 0x00200,
        CE_IOE = 0x00400,
        CE_DNS = 0x00800,
        CE_OOP = 0x01000,
        CE_MODE = 0x08000,
    }

    [Flags]
    public enum ModemStatus : uint
    {
        MS_CTS_ON = 0x00010,
        MS_DSR_ON = 0x00020,
        MS_RING_ON = 0x00040,
        MS_RLSD_ON = 0x00080,
    }
    #endregion

    #region Functions
    internal static class Native
    {
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, AccessMask dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, Disposition dwCreationDisposition, FileFlags dwFlagsAndAttributes, IntPtr hTemplateFile);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommTimeouts(SafeFileHandle hFile, ref COMMTIMEOUTS lpCommTimeouts);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetCommTimeouts(SafeFileHandle hFile, ref COMMTIMEOUTS lpCommTimeouts);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommMask(SafeFileHandle hFile, out SerialEvents lpEvtMask);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetCommMask(SafeFileHandle hFile, SerialEvents dwEvtMask);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommState(SafeFileHandle hFile, ref DCB lpDCB);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetCommState(SafeFileHandle hFile, ref DCB lpDCB);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GetCommConfig(SafeFileHandle hCommDev, out COMMCONFIG lpCC, ref uint lpdwSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetCommConfig(SafeFileHandle hCommDev, ref COMMCONFIG lpCC, uint dwSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDefaultCommConfig(string lpszName, ref COMMCONFIG lpCC, uint dwSize);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetupComm(SafeFileHandle hFile, uint dwInQueue, uint dwOutQueue);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GetCommModemStatus(SafeFileHandle hFile, out ModemStatus lpModemStat);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommProperties(SafeFileHandle hFile, out COMMPROP lpCommProp);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool PurgeComm(SafeFileHandle hFile, PurgeFlags dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ClearCommError(SafeFileHandle hFile, out CommError lpErrors, out COMMSTAT lpStat);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetCommBreak(SafeFileHandle fileHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool ClearCommBreak(SafeFileHandle hFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool EscapeCommFunction(SafeFileHandle hFile, ExtendedFunctions dwFunc);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool TransmitCommChar(IntPtr hFile, char cChar);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool WaitCommEvent(SafeFileHandle hFile, out SerialEvents lpEvtMask, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal unsafe static extern bool WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal unsafe static extern bool ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr overlapped);
    }
    #endregion

    #region Converters
    public class SerialPortConfig_PortNameConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection
               GetStandardValues(ITypeDescriptorContext context)
        {
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM", false))
                return new StandardValuesCollection(rk.GetValueNames().Select(x => (string)rk.GetValue(x)).ToArray());
        }
    }

    public class SerialPortConfig_BaudRateConverter : UInt32Converter
    {
        static readonly UInt32[] baudrates = new UInt32[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection
               GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(baudrates);
        }
    }

    public class SerialPortConfig_StopBitsConverter : ByteConverter
    {
        static readonly Byte[] stopbits = new Byte[] { 1, 2 };

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection
               GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(stopbits);
        }
    }

    public class SerialPortConfig_DataBitsConverter : ByteConverter
    {
        static readonly Byte[] databits = new Byte[] { 7, 8 };

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection
               GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(databits);
        }
    }
    #endregion

    public class SerialPortConfig 
    {
        [Category("Connection")]
        [TypeConverter(typeof(SerialPortConfig_PortNameConverter))]
        public string PortName { get; set; }

        [Category("Port settings")]
        [TypeConverter(typeof(SerialPortConfig_BaudRateConverter))]
        public uint BaudRate { get; set; }

        [Category("Port settings")]
        [TypeConverter(typeof(SerialPortConfig_DataBitsConverter))]
        public byte DataBits { get; set; }

        [Category("Port settings")]
        [TypeConverter(typeof(SerialPortConfig_StopBitsConverter))]
        public byte StopBits { get; set; }

        [Category("Timeouts")]
        public int ReadIntervalTimeout { get; set; }

        [Category("Timeouts")]
        public int ReadTimeoutConstant { get; set; }

        [Category("Timeouts")]
        public int ReadTimeoutMultiplier { get; set; }

        [Category("Timeouts")]
        public int WriteTimeoutConstant { get; set; }

        [Category("Timeouts")]
        public int WriteTimeoutMultiplier { get; set; }

        public static SerialPortConfig DefaultConfig
        {
            get { return new SerialPortConfig() {
                    PortName = "COM19",
                    BaudRate = 115200,
                    DataBits = 8,
                    StopBits = 1,
                    ReadIntervalTimeout = 40,
                    ReadTimeoutMultiplier = 40,
                    ReadTimeoutConstant = 50,
                    WriteTimeoutMultiplier = 40,
                    WriteTimeoutConstant = 50,
                };
            }
        }
    }

    public sealed class SynchronousSerialPort : IDisposable, ILinkLayer
    {
        private SafeFileHandle _hFile;
        private SerialPortConfig _config = SerialPortConfig.DefaultConfig;

        #region Constructor / Destructor
        public SynchronousSerialPort()
        {
        }

        ~SynchronousSerialPort()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        private void Dispose(bool isDisposing)
        {
            if (_hFile == null || _hFile.IsInvalid)
                return;

            if (isDisposing)
            {
                // free managed resources
            }

            // free unmanaged resources

            _hFile.Close();
            _hFile = null;
        }
        #endregion

        public bool IsOpen
        {
            get { return _hFile != null && !_hFile.IsInvalid; }
        }

        public object Configuration
        {
            get { return _config; }
            set {
                SerialPortConfig ls = value as SerialPortConfig;

                if (ls == null)
                    throw new InvalidOperationException("Bad configuration set ");

                if (IsOpen && ls.PortName != _config.PortName)
                    Close();

                _config = ls;
                UpdateConfig();
            }
        }

        public event EventHandler ConnectionChanged;

        public void Open()
        {            
            _hFile = Native.CreateFile(@"\\.\" + _config.PortName,AccessMask.GENERIC_READWRITE, 0, IntPtr.Zero, Disposition.OPEN_EXISTING, FileFlags.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

            if (_hFile.IsInvalid)
                throw new IOException();

            UpdateConfig();

            if (ConnectionChanged != null)
                ConnectionChanged(this, EventArgs.Empty);
        }

        private void UpdateConfig()
        {
            if (!IsOpen)
                return;

            DCB dcb = default(DCB);
            COMMTIMEOUTS touts = default(COMMTIMEOUTS);


            if (!Native.GetCommState(_hFile, ref dcb))
                throw new IOException();

            dcb.DCBlength = (uint)Marshal.SizeOf(dcb);
            dcb.BaudRate = _config.BaudRate;
            dcb.ByteSize = _config.DataBits;
            dcb.Parity = 0;
            dcb.StopBits = (byte)((_config.StopBits == 1) ? 0 : 2);

            if (!Native.SetCommState(_hFile, ref dcb))
                throw new IOException();

            

            if (!Native.GetCommTimeouts(_hFile, ref touts))
                throw new IOException();

            touts.ReadIntervalTimeout = _config.ReadIntervalTimeout;
            touts.ReadTotalTimeoutConstant = _config.ReadTimeoutConstant;
            touts.ReadTotalTimeoutMultiplier = _config.ReadTimeoutMultiplier;

            touts.WriteTotalTimeoutConstant = _config.WriteTimeoutConstant;
            touts.WriteTotalTimeoutMultiplier = _config.WriteTimeoutMultiplier;

            if (!Native.SetCommTimeouts(_hFile, ref touts))
                throw new IOException();
        }

        public void Close()
        {
            _hFile.Close();
            _hFile = null;

            if (ConnectionChanged != null)
                ConnectionChanged(this, EventArgs.Empty);
        }

        public unsafe int Write(byte[] array)
        {
            if (!IsOpen)
                return 0;

            int result = 0;

            fixed (byte* ptr = array)
                if (Native.WriteFile(_hFile, ptr, array.Length, out result, IntPtr.Zero))
                    return result;

            switch (Marshal.GetLastWin32Error())
            {
                case 6: // invalid handle
                    _hFile.SetHandleAsInvalid();
                    break;

                case 1121: // ERROR_COUNTER_TIMEOUT
                    break;
            }

            return result;            
        }

        public unsafe int Read(byte[] array)
        {
            if (!IsOpen)
                return 0;

            int result = 0;

            fixed (byte* ptr = array)
                if (Native.ReadFile(_hFile, ptr, array.Length, out result, IntPtr.Zero))
                    return result;

            switch (Marshal.GetLastWin32Error())
            {
                case 6: // invalid handle
                    _hFile.SetHandleAsInvalid();
                    break;

                case 1121: // ERROR_COUNTER_TIMEOUT
                    break;
            }

            return result;
        }

        public void DiscardInput()
        {
            if (!Native.PurgeComm(_hFile, PurgeFlags.PURGE_RXCLEAR | PurgeFlags.PURGE_RXABORT))
                throw new IOException();
        }

        public void DiscardOutput()
        {
            if (!Native.PurgeComm(_hFile, PurgeFlags.PURGE_TXCLEAR | PurgeFlags.PURGE_TXABORT))
                throw new IOException();
        }
    }
}
