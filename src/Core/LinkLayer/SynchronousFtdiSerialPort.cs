using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenInsider.Core.LinkLayer
{
    #region Enums
    internal enum FtdiStatus : uint
    {
        Ok,
        InvalidHandle,
        DeviceNotFound,
        DeviceNotOpened,
        IoError,
        InsufficientResources,
        InvalidParameter,
        InvalidBaudRate,
        DeviceNotOpenedForErase,
        DeviceNotOpenedForWrite,
        FailedToWriteDevice,
        EepromReadFailed,
        EepromWriteFailed,
        EepromEraseFailed,
        EepromNotPresent,
        EepromNotProgrammed,
        InvalidArgs,
        NotSupported,
        OtherError,
        DeviceListNotReady,
    }

    public enum FtdiDevType : uint
    {
        DeviceBM,
        DeviceAM,
        Device100AX,
        DeviceUNKNOWN,
        Device2232C,
        Device232R,
        Device2232H,
        Device4232H,
        Device232H,
    }

    [Flags]
    internal enum FtdiPurgeMode : uint
    {
        Rx = 1,
        Tx = 2,
    }

    [Flags]
    internal enum FtdiEvtMode : uint
    {
        None = 0,
        RxChar = 1,
        ModemStatus = 2,
        LineStatus = 4,
        All = 7,
    }

    internal enum FtdiOpenExFlags : uint
    {
        None = 0,
        BySerial = 1,
        ByDescription = 2,
        ByLocation = 4
    }

    internal enum FtdiFlowControl : ushort
    {
        None = 0,
        RtsCts = 0x100,
        DtrDsr = 0x200,
        XonXoff = 0x400,
    }

    internal enum FtdiParity : byte
    {
        None = 0,
        Odd = 1,
        Even = 2,
        Mark = 3,
        Space = 4,
    }

    [Flags]
    internal enum FtdiModemStatus : uint
    {
        Cts = 0x0010,
        Dsr = 0x0020,
        Ri = 0x0040,
        Dcd = 0x0080,

        OverrunError = 0x0200,
        ParityError = 0x0400,
        FramingError = 0x0800,
        BreakInput = 0x1000,
    }
    #endregion

    #region Structures
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct ft_program_data
    {
        public uint Signature1;
        public uint Signature2;
        public uint Version;
        public ushort VendorId;
        public ushort ProductId;
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string Manufacturer;
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string ManufacturerId;
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string Description;
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string SerialNumber;
        public ushort MaxPower;
        public ushort PnP;
        public ushort SelfPowered;
        public ushort RemoteWakeup;
        public byte Rev4;
        public byte IsoIn;
        public byte IsoOut;
        public byte PullDownEnable;
        public byte SerNumEnable;
        public byte USBVersionEnable;
        public ushort USBVersion;
        public byte Rev5;
        public byte IsoInA;
        public byte IsoInB;
        public byte IsoOutA;
        public byte IsoOutB;
        public byte PullDownEnable5;
        public byte SerNumEnable5;
        public byte USBVersionEnable5;
        public ushort USBVersion5;
        public byte AIsHighCurrent;
        public byte BIsHighCurrent;
        public byte IFAIsFifo;
        public byte IFAIsFifoTar;
        public byte IFAIsFastSer;
        public byte AIsVCP;
        public byte IFBIsFifo;
        public byte IFBIsFifoTar;
        public byte IFBIsFastSer;
        public byte BIsVCP;
        public byte UseExtOsc;
        public byte HighDriveIOs;
        public byte EndpointSize;
        public byte PullDownEnableR;
        public byte SerNumEnableR;
        public byte InvertTXD;
        public byte InvertRXD;
        public byte InvertRTS;
        public byte InvertCTS;
        public byte InvertDTR;
        public byte InvertDSR;
        public byte InvertDCD;
        public byte InvertRI;
        public byte Cbus0;
        public byte Cbus1;
        public byte Cbus2;
        public byte Cbus3;
        public byte Cbus4;
        public byte RIsD2XX;
        public byte PullDownEnable7;
        public byte SerNumEnable7;
        public byte ALSlowSlew;
        public byte ALSchmittInput;
        public byte ALDriveCurrent;
        public byte AHSlowSlew;
        public byte AHSchmittInput;
        public byte AHDriveCurrent;
        public byte BLSlowSlew;
        public byte BLSchmittInput;
        public byte BLDriveCurrent;
        public byte BHSlowSlew;
        public byte BHSchmittInput;
        public byte BHDriveCurrent;
        public byte IFAIsFifo7;
        public byte IFAIsFifoTar7;
        public byte IFAIsFastSer7;
        public byte AIsVCP7;
        public byte IFBIsFifo7;
        public byte IFBIsFifoTar7;
        public byte IFBIsFastSer7;
        public byte BIsVCP7;
        public byte PowerSaveEnable;
        public byte PullDownEnable8;
        public byte SerNumEnable8;
        public byte ASlowSlew;
        public byte ASchmittInput;
        public byte ADriveCurrent;
        public byte BSlowSlew;
        public byte BSchmittInput;
        public byte BDriveCurrent;
        public byte CSlowSlew;
        public byte CSchmittInput;
        public byte CDriveCurrent;
        public byte DSlowSlew;
        public byte DSchmittInput;
        public byte DDriveCurrent;
        public byte ARIIsTXDEN;
        public byte BRIIsTXDEN;
        public byte CRIIsTXDEN;
        public byte DRIIsTXDEN;
        public byte AIsVCP8;
        public byte BIsVCP8;
        public byte CIsVCP8;
        public byte DIsVCP8;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi), Serializable]
    internal struct FtdiDevNode
    {
        public uint Flags;
        public FtdiDevType Type;
        public uint ID;
        public uint LocId;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SerialNumber;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Description;
        public IntPtr ftHandle;
    }
    #endregion


    internal static class FtdiNative
    {
        [DllImport("FTD2XX.dll", EntryPoint = "FT_Open", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus Open(Int32 devNum, IntPtr pHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_OpenEx", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus OpenEx([MarshalAs(UnmanagedType.LPStr)] string pArg1, FtdiOpenExFlags Flags, out IntPtr pHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_OpenEx", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus OpenEx(IntPtr pArg1, FtdiOpenExFlags Flags, out IntPtr pHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_Close", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus Close(IntPtr pHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_Read", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus Read(IntPtr ftHandle, [Out] byte[] lpBuffer, uint dwBytesToRead, out uint lpBytesReturned);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_Write", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus Write(IntPtr ftHandle, [In] byte[] lpBuffer, uint dwBytesToWrite, out uint lpBytesWritten);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_CreateDeviceInfoList", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus CreateDeviceInfoList(out UInt32 numdevs);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetDeviceInfoList", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus GetDeviceInfoList([Out] FtdiDevNode[] devicelist, ref UInt32 numdevs);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetDeviceInfoDetail", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern FtdiStatus GetDeviceInfoDetail(UInt32 dwIndex, out UInt32 lpdwFlags, out FtdiDevType lpdwType, out UInt32 lpdwID, out UInt32 lpdwLocId, [Out]  StringBuilder lpSerialNumber, [Out] StringBuilder lpDescription, out IntPtr pftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetComPortNumber", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern FtdiStatus GetComPortNumber(IntPtr ftHandle, out int lpdwComPortNumber);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_Purge", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus Purge(IntPtr ftHandle, FtdiPurgeMode Mask);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetTimeouts", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetTimeouts(IntPtr ftHandle, uint readTimeout, uint writeTimeout);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetBaudRate", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetBaudRate(IntPtr ftHandle, uint baudRate);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetDivisor", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetDivisor(IntPtr ftHandle, uint divisor);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetDataCharacteristics", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetDataCharacteristics(IntPtr ftHandle, byte wordLength, byte stopBits, FtdiParity parity);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetFlowControl", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetFlowControl(IntPtr ftHandle, FtdiFlowControl flowControl, byte xonChar, byte xoffChar);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetModemStatus", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetModemStatus(IntPtr ftHandle, out FtdiModemStatus pModemStatus);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetQueueStatus", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetQueueStatus(IntPtr ftHandle, out uint dwRxBytes);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetEventStatus", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetEventStatus(IntPtr ftHandle, out FtdiEvtMode dwEventDWord);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetStatus", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetStatus(IntPtr ftHandle, out uint dwRxBytes, out uint dwTxBytes, out FtdiEvtMode dwEventDWord);        

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetDriverVersion", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetDriverVersion(IntPtr ftHandle, ref uint lpdwVersion);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetLibraryVersion", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetLibraryVersion(ref uint lpdwVersion);        

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetEventNotification", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetEventNotification(IntPtr ftHandle, FtdiEvtMode Mask, SafeHandle Param);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_StopInTask", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus StopInTask(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_RestartInTask", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus RestartInTask(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_ResetPort", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus ResetPort(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_CyclePort", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus CyclePort(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_ResetDevice", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus ResetDevice(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_Rescan", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus Rescan();

        [DllImport("FTD2XX.dll", EntryPoint = "FT_Reload", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus Reload(ushort wVid, ushort wPid);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetLatencyTimer", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetLatencyTimer(IntPtr ftHandle, byte ucLatency);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetLatencyTimer", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetLatencyTimer(IntPtr ftHandle, out byte pucLatency);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetBitMode", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetBitMode(IntPtr ftHandle, byte ucMask, byte ucEnable);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetBitMode", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetBitMode(IntPtr ftHandle, out byte pucMode);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetUSBParameters", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetUSBParameters(IntPtr ftHandle, uint ulInTransferSize, uint ulOutTransferSize);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetDeadmanTimeout", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetDeadmanTimeout(IntPtr ftHandle, uint ulDeadmanTimeout);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetResetPipeRetryCount", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetResetPipeRetryCount(IntPtr ftHandle, uint dwCount);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_GetQueueStatusEx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus GetQueueStatusEx(IntPtr ftHandle, ref uint dwRxBytes);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetChars", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetChars(IntPtr ftHandle, byte EventChar, byte EventCharEnabled, byte ErrorChar, byte ErrorCharEnabled);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetDtr", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetDtr(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_ClrDtr", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus ClrDtr(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetRts", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetRts(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_ClrRts", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus ClrRts(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetBreakOn", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetBreakOn(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetBreakOff", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetBreakOff(IntPtr ftHandle);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_SetWaitOnMask", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus SetWaitMask(IntPtr ftHandle, uint Mask);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_WaitOnMask", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus WaitOnMask(IntPtr ftHandle, ref uint Mask);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_ReadEE", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus ReadEE(IntPtr ftHandle, uint dwWordOffset, ref ushort lpwValue);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_WriteEE", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus WriteEE(IntPtr ftHandle, uint dwWordOffset, ushort wValue);

        [DllImport("FTD2XX.dll", EntryPoint = "FT_EraseEE", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern FtdiStatus EraseEE(IntPtr ftHandle);


        /*
         *
        public static extern uint FT_ListDevices(IntPtr pArg1, IntPtr pArg2, uint Flags);
        
        public static extern uint FT_EE_Program(IntPtr ftHandle, ref ft_program_data pData);
        public static extern uint FT_EE_ProgramEx(IntPtr ftHandle, ref ft_program_data pData, IntPtr Manufacturer, IntPtr ManufacturerId, IntPtr Description, IntPtr SerialNumber);
        public static extern uint FT_EE_Read(IntPtr ftHandle, ref ft_program_data pData);
        public static extern uint FT_EE_ReadEx(IntPtr ftHandle, ref ft_program_data pData, IntPtr Manufacturer, IntPtr ManufacturerId, IntPtr Description, IntPtr SerialNumber);
        public static extern uint FT_EE_UASize(IntPtr ftHandle, ref uint lpdwSize);
        public static extern uint FT_EE_UAWrite(IntPtr ftHandle, IntPtr pucData, uint dwDataLen);
        public static extern uint FT_EE_UARead(IntPtr ftHandle, IntPtr pucData, uint dwDataLen, ref uint lpdwBytesRead);
        public static extern uint FT_EE_ReadConfig(IntPtr ftHandle, byte ucAddress, IntPtr pucValue);
        public static extern uint FT_EE_WriteConfig(IntPtr ftHandle, byte ucAddress, byte ucValue);
        public static extern uint FT_EE_ReadECC(IntPtr ftHandle, byte ucOption, ref ushort lpwValue);

        public static extern uint FT_GetDeviceInfo(IntPtr ftHandle, ref uint lpftDevice, ref uint lpdwID, IntPtr SerialNumber, IntPtr Description, IntPtr Dummy);
         * */
    }

    public class FtdiSerialPortConfig_IndexConverter : UInt32Converter
    {
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
            UInt32 n;
            if (FtdiNative.CreateDeviceInfoList(out n) != FtdiStatus.Ok)
                throw new IOException();

            List<UInt32> lst = new List<uint>();
            for (UInt32 i = 0; i < n; i++)
                lst.Add(i);
            
            return new StandardValuesCollection(lst);            
        }
    }

    public class FtdiSerialPortConfig : INotifyPropertyChanged
    {
        private UInt32 aPortIndex = unchecked ((UInt32)(-1));

        [Category("Connection")]
        [TypeConverter(typeof(FtdiSerialPortConfig_IndexConverter))]
        public uint Index { get { return aPortIndex; } set { SetSerial(value); } }

        [Category("Connection")]
        public string SerialNumber { get; private set; }

        [Category("Connection")]
        public string Description { get; private set; }

        [Category("Connection")]
        public string Location { get; private set; }

        [Category("Connection")]
        public FtdiDevType Type { get; private set; }

        [Category("Connection")]
        public string Id { get; private set; }

        [Category("Connection")]
        public string Port { get; private set; }

        [Category("Port settings")]
        public uint BaudRate { get; set; }

        [Category("Port settings")]
        public byte DataBits { get; set; }

        [Category("Port settings")]
        public byte StopBits { get; set; }

        [Category("Timeouts")]
        public uint ReadTimeout { get; set; }

        [Category("Timeouts")]
        public uint WriteTimeout { get; set; }

        public static FtdiSerialPortConfig DefaultConfig
        {
            get
            {
                return new FtdiSerialPortConfig()
                {
                    Index = 0,
                    BaudRate = 115200,
                    DataBits = 8,
                    StopBits = 1,
                    ReadTimeout = 40,
                    WriteTimeout = 40,
                };
            }
        }
            
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChange([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetSerial(UInt32 idx)
        {
            UInt32 n;
            if (FtdiNative.CreateDeviceInfoList(out n) != FtdiStatus.Ok)
                throw new IOException();

            if (idx >= n)
                throw new IOException();

            aPortIndex = idx;

            uint flags;
            FtdiDevType typ;
            uint id;
            uint locid;
            IntPtr handle;
            StringBuilder serno = new StringBuilder(16);
            StringBuilder desc = new StringBuilder(64);

            FtdiStatus stat = FtdiNative.GetDeviceInfoDetail(idx, out flags, out typ, out id, out locid, serno, desc, out handle);            


            SerialNumber = serno.ToString();
            Description = desc.ToString();
            Id = "VID=0x"+(id >> 16).ToString("X4") + "/PID=0x" + (id & 0xFFFF).ToString("X4");
            Location = "0x"+locid.ToString("X8");
            Type = typ;
            Port = "COMx";
            NotifyChange("Id");
        }
    }

    public sealed class SynchronousFtdiSerialPort : IDisposable, ILinkLayer
    {
        private FtdiSerialPortConfig _config = FtdiSerialPortConfig.DefaultConfig;
        private IntPtr _hFile;

        public bool IsOpen { get { return _hFile != IntPtr.Zero; } }

        public object Configuration
        {
            get { return _config; }
            set
            {
                FtdiSerialPortConfig spc = value as FtdiSerialPortConfig;

                if (spc == null)
                    throw new InvalidOperationException("Invalid config");

                _config = spc;
                UpdateConfig();
            }
        }

        public event EventHandler ConnectionChanged;

        #region Constructor/destructor
        ~SynchronousFtdiSerialPort()
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
            if (!IsOpen)
                return;

            if (isDisposing)
            {
                // free managed resources
            }

            // free unmanaged resources


            if (FtdiNative.Close(_hFile) != FtdiStatus.Ok)
                throw new IOException();

            _hFile = IntPtr.Zero;
        }
        #endregion

        public void Open()
        {
           // FtdiNative.OpenEx("", FtdiOpenExFlags.ByDescription, out _hFile);
            UInt32 n;
            if (FtdiNative.CreateDeviceInfoList(out n) != FtdiStatus.Ok)
                throw new IOException();

            if (n == 0)
                throw new IOException();

            FtdiDevNode[] devs = new FtdiDevNode[n];
            if (FtdiNative.GetDeviceInfoList(devs, ref n) != FtdiStatus.Ok)
                throw new IOException();

            if (FtdiNative.OpenEx(devs[0].SerialNumber, FtdiOpenExFlags.BySerial, out _hFile) != FtdiStatus.Ok)
                throw new IOException();

            UpdateConfig();

            if (ConnectionChanged != null)
                ConnectionChanged(this, EventArgs.Empty);
        }

        private void UpdateConfig()
        {
            if (FtdiNative.SetBaudRate(_hFile, 115200) != FtdiStatus.Ok)
                throw new IOException();

            if (FtdiNative.SetTimeouts(_hFile, _config.ReadTimeout, _config.WriteTimeout) != FtdiStatus.Ok)
                throw new IOException();

            if (FtdiNative.SetDataCharacteristics(_hFile, _config.DataBits, _config.StopBits, FtdiParity.None) != FtdiStatus.Ok)
                throw new IOException();

            if (FtdiNative.SetFlowControl(_hFile, FtdiFlowControl.None, 0, 0) != FtdiStatus.Ok)
                throw new IOException();
        }

        public void Close()
        {
            if (!IsOpen)
                return;

            if (FtdiNative.Close(_hFile) != FtdiStatus.Ok)
                throw new IOException();

            _hFile = IntPtr.Zero;

            if (ConnectionChanged != null)
                ConnectionChanged(this, EventArgs.Empty);
        }

        public int Write(byte[] data)
        {
            uint written;

            if (FtdiNative.Write(_hFile, data, (uint)data.Length, out written) != FtdiStatus.Ok)
                throw new IOException();

            return (int)written;
        }

        public int Read(byte[] data)
        {
            uint readed;

            if (FtdiNative.Read(_hFile, data, (uint)data.Length, out readed) != FtdiStatus.Ok)
                throw new IOException();

            return (int)readed;
        }

        public void DiscardInput()
        {
            if (FtdiNative.Purge(_hFile, FtdiPurgeMode.Rx) != FtdiStatus.Ok)
                throw new IOException();
        }

        public void DiscardOutput()
        {
            if (FtdiNative.Purge(_hFile, FtdiPurgeMode.Tx) != FtdiStatus.Ok)
                throw new IOException();
        }
    }
}
