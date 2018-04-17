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
        private static Section[] Sections;
        private static Question[] Questions;
        public static int NumSec { get; set; }
        public static string NumQus { get; set; }
        public static string DBSource { get; set; }
        public static string InitCat { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        
        public Environment()
        {
            bool update = 0<(DateTime.Compare(File.GetLastWriteTime(HostingEnvironment.ApplicationPhysicalPath + "/Settings.csv"),
                 File.GetLastWriteTime(HostingEnvironment.ApplicationPhysicalPath + "ConnectionStrings.config")));
            
            StreamReader file = new StreamReader(HostingEnvironment.ApplicationPhysicalPath + "/Settings.csv");
            string line, qtext = "", qY = "", qN = "", qR = "", sf = "", sl = "", sn = "";
            int scount = 0, qcount = 0;

            while ((line = file.ReadLine())!=null)
            {
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
                    else if (line.Contains("of Questions:"))
                    {
                        NumQus = line.Split(',')[1];
                        Questions = new Question[Convert.ToInt64(NumQus)];
                    }
                    else if (line.Contains("Sections:"))
                    {
                        NumSec = Convert.ToInt32(line.Split(',')[1]);
                        Sections = new Section[NumSec];
                    }
                    else if (line.Contains("Text:"))
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
                        Sections[scount++] = new Section(sf, sl, sn);
                    }
            }
            file.Close();
            if(update)
            { 
                StreamWriter fileO = new StreamWriter(HostingEnvironment.ApplicationPhysicalPath + "ConnectionStrings.config",false);
            
                line += "<connectionStrings>\n";
                line += "\t<add name=\"DBAContext\" connectionString=\"Data Source = " + DBSource + "; Initial Catalog = " + InitCat + "; Integrated Security = False; User Id = "+ User +"; Password = "+ Password +"; Encrypt = True; TrustServerCertificate = False; MultipleActiveResultSets = True\" providerName=\"System.Data.SqlClient\" />\n";
                line += "\t<add name=\"DBUContext\" connectionString=\"Data Source = " + DBSource + "; Initial Catalog = " + InitCat + "; Integrated Security = False; User Id = " + User + "; Password = " + Password + "; Encrypt = True; TrustServerCertificate = False; MultipleActiveResultSets = True\" providerName=\"System.Data.SqlClient\" />\n";
                line += "</connectionStrings>";
                fileO.Write(line);
                fileO.Flush();
                fileO.Close();
            }

        }
        public string GetSectionName(int i)
        {
            return Sections[i].name;
        }
        public string GetSectionFirst(int i)
        {
            return Sections[i].first;
        }
        public string GetSectionLast(int i)
        {
            return Sections[i].last;
        }
        public string GetQuestionText(int i)
        {
            return Questions[i].text;
        }
        public string GetQuestionYV(int i)
        {
            return Questions[i].YesVal;
        }
        public string GetQuestionNV(int i)
        {
            return Questions[i].NoVal;
        }
        public string GetQuestionRO(int i)
        {
            return Questions[i].ReliesOn;
        }
    }
}