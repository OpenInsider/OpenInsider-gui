using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Files
{
    public enum SymbolType
    {
        Object = 1,
        Function = 2,
        Section = 3,
        File = 4,
        Proc0 = 13,
        Proc1 = 14,
        Proc2 = 15
    }

    public class Symbol
    {
        public string Name;
        public UInt32 Value;
        public UInt32 Size;
        public string Bind;
        public SymbolType Typ;
        public UInt32 SecBase;

        public override string ToString()
        {
            return string.Format("{0} {1} {2:D4} {3:X8} {4}", Bind, Typ, Size, Value, Name);
        }
    }

    public class ElfFile
    {
        public static T ReadStruct<T>(BinaryReader stream) where T : struct
        {
            var sz = Marshal.SizeOf(typeof(T));
            var buffer = stream.ReadBytes(sz);
            var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var structure = (T)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(T));
            pinnedBuffer.Free();
            return structure;
        }

        enum sh_type : uint
        {
            NULL = 0,
            PROGBITS = 1,
            SYMTAB = 2,
            STRTAB = 3,
            RELA = 4,
            HASH = 5,
            DYNAMIC = 6,
            NOTE = 7,
            NOBITS = 8,
            REL = 9,
            SHLIB = 10,
            DYNSYM = 11,
            LOPROC = 0x70000000U,
            HIPROC = 0x7FFFFFFFU,
            LOUSER = 0x80000000U,
            HIUSER = 0xFFFFFFFFU,

            ARM_EXIDX = 0x70000001,
            ARM_PREEMPTMAP = 0x70000002,
            ARM_ATTRIBUTES = 0x70000003,
            ARM_DEBUGOVERLAY = 0x70000004,
            ARM_OVERLAYSECTION = 0x70000005,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct e
        {
            public UInt32 e_magic;     // 0..1..2..3
            public byte e_cls;           // 4
            public byte e_endian;        // 5
            public byte e_ver;           // 6

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)] 
            public byte[] e_pad;       // 7 ... 15

            public UInt16 e_type;
            public UInt16 e_machine;
            public UInt32 e_version;

            public UInt32 e_entry;
            public UInt32 e_phoff;
            public UInt32 e_shoff;
            public UInt32 e_flags;

            public UInt16 e_ehsize;
            public UInt16 e_phentsize;
            public UInt16 e_phnum;
            public UInt16 e_shentsize;
            public UInt16 e_shnum;
            public UInt16 e_shstrndx;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct sh
        {
            public UInt32 sh_name;
            public sh_type sh_type;
            public UInt32 sh_flags;
            public UInt32 sh_addr;
            public UInt32 sh_off;
            public UInt32 sh_size;
            public UInt32 sh_link;
            public UInt32 sh_info;
            public UInt32 sh_adralign;
            public UInt32 sh_entsize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct st
        {
            public UInt32 st_name;
            public UInt32 st_value;
            public UInt32 st_size;
            public byte st_info;
            public byte st_other;
            public UInt16 st_shndx;
        }

        sh[] secs = new sh[0];
        byte[][] sdatas = new byte[0][];

        public List<Symbol> Symbols = new List<Symbol>();


        public ElfFile(string p)
        {
            using (Stream s = new FileStream(p, FileMode.Open, FileAccess.Read))
            using (BinaryReader sr = new BinaryReader(s))
            {
                e ee = ReadStruct<e>(sr);

                secs = new sh[ee.e_shnum];
                sdatas = new byte[ee.e_shnum][];

				/* preload all sections */
                for (int i = 0; i < ee.e_shnum; i++)
                {
                    sr.BaseStream.Seek(ee.e_shoff + i * ee.e_shentsize, SeekOrigin.Begin);
                    secs[i] = ReadStruct<sh>(sr);

                    sr.BaseStream.Seek(secs[i].sh_off, SeekOrigin.Begin);
                    sdatas[i] = sr.ReadBytes((int)secs[i].sh_size);
                }

				/* parse data in symbol table sections */
                for (int i = 0; i < ee.e_shnum; i++)
                {
					if (secs[i].sh_type != sh_type.SYMTAB)
						continue;
                    
                    for (int j = 0; j < secs[i].sh_size; j += (int)secs[i].sh_entsize)
                    {
                        sr.BaseStream.Seek(secs[i].sh_off + j, SeekOrigin.Begin);
                        st sym = ReadStruct<st>(sr);

						if ((sym.st_info & 0x0F) == 0)
							continue;

                        Symbol sm = new Symbol()
                        {
                            Name = GetStringFromTable((int)secs[i].sh_link, (int)sym.st_name),
                            Value = sym.st_value,
                            Size = sym.st_size,
                            SecBase = secs[i].sh_addr,
                            Typ = (SymbolType)(sym.st_info & 0x0F),
                        };

                        switch (sym.st_info >> 4)
                        {
                            case 0: sm.Bind = "L"; break;
                            case 1: sm.Bind = "G"; break;
                            case 2: sm.Bind = "W"; break;
                            case 13: sm.Bind = "P0"; break;
                            case 14: sm.Bind = "P1"; break;
                            case 15: sm.Bind = "P2"; break;
                            default: sm.Bind = (sym.st_info >> 4).ToString(); break;
                        }

                        Symbols.Add(sm);
                    }

					//string nam = GetStringFromTable((int)ee.e_shstrndx, (int)secs[i].sh_name);
                    //Debug.WriteLine("SECTION {0}\tBASE: {1:X8} SIZE: {2:X8} ALIGN: {3:X4} TYPE: {4}", nam, secs[i].sh_addr, secs[i].sh_size, secs[i].sh_adralign, secs[i].sh_type);
                }

               
            }
            

        }

        public string GetStringFromTable(int section, int index)
        {            
            Debug.Assert(secs[section].sh_type == sh_type.STRTAB);

            StringBuilder sb = new StringBuilder();

            byte[] data = sdatas[section];

            for (int i = index; i < data.Length; i++)
            {
                byte c = data[i];
                if (c == 0)
                    break;

                sb.Append((char)c);
            }

            return sb.ToString();
        }
    }
}
