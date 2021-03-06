﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Tesseract_Online
{
    class TConfigFile
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;
        private List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

        public TConfigFile(string TConfigPath = null)
        {
            Path = new FileInfo(TConfigPath ?? EXE + ".tcfg").FullName.ToString();
            foreach (string line in File.ReadAllLines(Path))
            {
                string[] info = line.Split('=');
                data.Add(new KeyValuePair<string, string>(info[0], info[1]));
            }
        }

        public string Read(string Key)
        {
            return data.Where(k => k.Key == Key).First().Value;
        }
    }
}