using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace BluestackInfo
{
    public class clsBluestack
    {
        private static clsBluestack selfInstance = null;
        public static clsBluestack getInstance
        {
            get
            {
                if (selfInstance == null) selfInstance = new clsBluestack();
                return selfInstance;
            }
        }

        public clsBluestack()
        { 
        }

        ~clsBluestack()
        {
            bluestackItem.Clear();
        }

        private List<bluestackApp> bluestackItem = new List<bluestackApp>();
        public List<bluestackApp> BLUESTACK_ITEM
        {
            get { return bluestackItem; }
        }

        public int GetBluestackkey(string skey)
        {
            int nValue = 0;
            try
            {                
                RegistryKey Reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\BlueStacks\\Guests\\Android\\FrameBuffer\\0", true);
                if (Reg == null)
                    return nValue;
               
                nValue = Convert.ToInt32(Reg.GetValue(skey).ToString());
                Reg.Close();
            }
            catch
            {
                return nValue;
            }
            return nValue;  
        }

        public bool SetBluestackkey(string skey, int nValue)
        {           
            try
            {
                RegistryKey Reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\BlueStacks\\Guests\\Android\\FrameBuffer\\0", true);
                if (Reg == null)
                {
                    return false;
                }

                Reg.SetValue(skey, nValue);
                Reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
       
        public int GetBluestackMemory()
        {
            int nValue = 0;
            try
            {                
                RegistryKey Reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\BlueStacks\\Guests\\Android\\", true);
                if (Reg == null)
                    return nValue;

                nValue = Convert.ToInt32(Reg.GetValue("Memory").ToString());
                Reg.Close();
            }
            catch
            {
                return nValue;
            }
            return nValue;  
        }

        public bool SetBluestackMemory(int nValue)
        {
            try
            {
                RegistryKey Reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\BlueStacks\\Guests\\Android\\", true);
                if (Reg == null)
                {
                    return false;
                }

                Reg.SetValue("Memory", nValue);
                Reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetBluestackDataInfo(string skey)
        {
            string strValue = "";
            try
            {                
                RegistryKey Reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\BlueStacks", true);
                if (Reg == null)
                    return "";

                strValue = Reg.GetValue(skey).ToString();
                Reg.Close();
            }
            catch
            {
                return "";
            }
            return strValue;  
        }

        public bool HDRunAPP()
        {
            string sHDRunApp = string.Format("{0}\\HD-RunApp.exe", GetBluestackDataInfo("InstallDir"));
            if (!File.Exists(sHDRunApp))
                return false;

            Process.Start(sHDRunApp);            
            return true;
        }

        public bool HDRunAPP(string strpackage, string stractivity)
        {
            string sParam = string.Format(" -p {0} -a {1}", strpackage, stractivity);

            string sHDRunApp = string.Format("{0}\\HD-RunApp.exe", GetBluestackDataInfo("InstallDir"));
            if (!File.Exists(sHDRunApp))
                return false;

            Process.Start(sHDRunApp, sParam);
            return true;
        }

        public void GetBluestackGameInfo()
        {
            string sContent = "";

            string strPath = string.Format("{0}\\UserData\\Gadget\\apps.json", GetBluestackDataInfo("DataDir"));
            if (File.Exists(strPath))
            {
                StreamReader reader = new StreamReader(strPath);
                sContent = reader.ReadToEnd();
                reader.Close();
            }

            if(sContent == "")
                return; 
            
            string strParser = "";
            string[] blocks = sContent.Split('{', '}');
            for (int i = 0; i < blocks.Length; i++)
            {
                strParser = blocks[i].Trim();
                if (strParser.Length == 1 || strParser.Length == 0)
                    continue;

                string gamename = "";
                string package = "";
                string activity = "";
                string version = "";
                string appstore = "";
                string system = "";
                string img = "";

                string[] rows = strParser.Split(',');
                foreach (string line in rows)
                {
                    string[] value = line.Split(':');
                    string Name = value[0].Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    string key = value[1].Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();

                    if (Name == "name") gamename = key;                    
                    if (Name == "package") package = key;                     
                    if (Name == "activity") activity = key;                     
                    if (Name == "version") version = key;
                    if (Name == "appstore") appstore = key;
                    if (Name == "system") system = key;                    
                    if (Name == "img") img = key; 
                }

                bluestackApp Item = new bluestackApp(gamename, package, activity, version, appstore, system, img);
                bluestackItem.Add(Item);
            }
        }
    }
   
    public class bluestackApp
    { 
        public bluestackApp(string name, string package, string activity, string version, string appstore, string system, string img)
        {
            NAME = name;
            PACKAGE = package;
            ACTIVITY = activity;
            VERSION = version;
            APPSTORE = appstore;
            SYSTEM = system;
            IMG = img;
        }

        public string NAME { get; set; }
        public string PACKAGE { get; set; }
        public string ACTIVITY { get; set; }
        public string VERSION { get; set; }
        public string APPSTORE { get; set; }
        public string SYSTEM { get; set; }
        public string IMG { get; set; }
    }

}
