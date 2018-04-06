using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace StateTemplateV5Beta.Models
{
    public class Section
    {
        public string first { get; set; }
        public string last { get; set; }
        public string name { get; set; }
        public Section(string f, string l, string n)
        {
            first = f;
            last = l;
            name = n;
        }
    }
    public class Question
    {
        public string text { get; set; }
        public string YesVal { get; set; }
        public string NoVal { get; set; }
        public string ReliesOn { get; set; }

        public Question(string t, string y, string n, string r)
        {
            text = t;
            YesVal = y;
            NoVal = n;
            ReliesOn = r;
        }
    }

    public class Environment
    {
        public static Section[] Sections { get; set;}
        public static Question[] Questions { get; set; }
        public static int NumSec { get; set; }
        public static int NumQus { get; set; }
        public static string DBSource { get; set; }
        public static string InitCat { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        
        public Environment()
        {
            using (FileStream fs = File.Open(HostingEnvironment.ApplicationPhysicalPath + "/Settings.csv", FileMode.Open))
            {
                string line;
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    line = temp.GetString(b);
                    if (line.Contains("DBSource:"))
                    {
                        DBSource = line.Split(',')[1];
                    }
                    else if (line.Contains("Initial Catalog:"))
                    {
                        InitCat = line.Split(',')[2];
                    }
                    else if (line.Contains("User:"))
                    {
                        User = line.Split(',')[2];
                    }
                    else if (line.Contains("Password:"))
                    {
                        Password = line.Split(',')[2];
                    }
                    else if (line.Contains("Questions:"))
                    {
                        NumQus = Convert.ToInt32(line.Split(',')[1]);
                        Questions = new Question[NumQus];
                    }
                    else if (line.Contains("Sections:"))
                    {
                        NumSec = Convert.ToInt32(line.Split(',')[1]);
                        Sections = new Section[NumSec];
                    }
                }
            }
            string qtext="",qY="",qN="",qR="", sf="",sl="",sn="";
            using (FileStream fs = File.Open(HostingEnvironment.ApplicationPhysicalPath + "/Settings.csv", FileMode.Open))
            {
                int scount = 0, qcount=0;
                string line;
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    line = temp.GetString(b);
                    if (line.Contains("Text:"))
                    {
                        qtext = line.Split(Convert.ToChar(34))[1];
                    }
                    else if (line.Contains("YesVal:"))
                    {
                        qY = line.Split(',')[3];
                    }
                    else if (line.Contains("NoVal:"))
                    {
                        qN = line.Split(',')[3];
                    }
                    else if (line.Contains("ReliesOn:"))
                    {
                        qR = line.Split(',')[3];
                        Questions[qcount++] = new Question(qtext, qY, qN, qR);
                    }
                    else if (line.Contains("Name:"))
                    {
                        sn = line.Split(',')[2];
                    }
                    else if (line.Contains("First:"))
                    {
                        sf = line.Split(',')[3];
                    }
                    else if (line.Contains("Last:"))
                    {
                        sl = line.Split(',')[3];
                        Sections[scount++] = new Section(sf,sl,sn);
                    }
                }
            }
        }
    }
}