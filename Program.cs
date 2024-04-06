using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Application
    {
        String name { get; }
        int byteSize { get; }

        public Application(String name, int byteSize)
        {
            this.name = name;
            this.byteSize = byteSize;
        }

        public override string ToString()
        {
            return "Название: " + name + " Размер (в байтах): " + byteSize + " ";
        }

        public string export()
        {
            return name+"."+byteSize;
        }
    }

    class ComputerClass
    {
        public DateTime manufacteredDate { get; set; }
        public List<Application> apps { get; set; }
        public String CPU { get; set; }
        public String GPU { get; set; }
        public String RAM { get; set; }
        private int id;

        public int getID()
        {
            return id;
        }

        public ComputerClass(List<Application> apps, String CPU, String GPU, String RAM, DateTime manufactoredDate)
        {
            id = Program.getUniqueId();
            this.apps = apps;
            this.CPU = CPU;
            this.GPU = GPU;
            this.RAM = RAM;
            this.manufacteredDate = manufacteredDate;
        }

        public ComputerClass()
        {
            id = Program.getUniqueId();
            apps = new List<Application>();
            CPU = "None";
            GPU = "None";
            RAM = "None";
            manufacteredDate = new DateTime();
        }

        public ComputerClass(string specsString)
        {
            string[] specs = specsString.Split(';');
            try { id = Int32.Parse(specs[0]); }
            catch { id = Program.getUniqueId(); }

            try
            {
                List<Application> newApps = new List<Application>();
                string[] appsFromString = specs[1].Split('|');
                for (int i = 0; i < appsFromString.Length; i++)
                {
                    string[] appProperties = appsFromString[i].Split('.');
                    if (appProperties.Length < 2) continue;
                    newApps.Add(new Application(appProperties[0], int.Parse(appProperties[1])));
                }
                apps = newApps;
            }
            catch { apps = new List<Application>(); }

            try { CPU = specs[2]; }
            catch { CPU = "None"; }

            try { GPU = specs[3];}
            catch { GPU = "None"; }

            try { RAM = specs[4]; }
            catch { RAM = "None"; }

            try { manufacteredDate = DateTime.Parse(specs[5]); }
            catch { manufacteredDate = new DateTime(); }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ID: " + id);
            builder.Append(" | Приложения: ");
            if (apps.Count != 0)
            {
                builder.Append('[');
                foreach (Application app in apps)
                {
                    builder.Append(app.ToString());
                }
                builder.Append("]");
            }
            
            builder.Append(" | Процессор: " + CPU);
            builder.Append(" | Видеокарта: " + GPU);
            builder.Append(" | Оперативная память: " + RAM);
            return builder.ToString();
        }

        public void printInfo()
        {
            Console.WriteLine(this.ToString());
        }

        public string export()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(id);
            if (apps.Count != 0)
            {
                builder.Append(";");
                foreach (Application application in apps)
                {
                    builder.Append(application.export()+"|");
                }
            }
            
            builder.Append(";" + CPU);
            builder.Append(";" + GPU);
            builder.Append(";" + RAM);
            builder.Append(";" + manufacteredDate);
            return builder.ToString();
        }
    }

    class Program
    {
        private static int lastId = 0;
        public static int getUniqueId()
        {
            lastId++;
            return lastId;
        }

        static void Main(string[] args)
        {
            ComputerClass[] computerClasses = { 
                new ComputerClass(new List<Application> { new Application("Paint", 1024*100), new Application("MS Word", 1024*1024) }, "Ryzen 5 3600", "RTX 3060", "DDR4 32GB", new DateTime()), 
                new ComputerClass(new List<Application> { new Application("Browser", 1024*1024*100) }, "CPU", "GPU", "RAM", new DateTime())
            };

            Console.WriteLine("Перед записью в файл");

            foreach (ComputerClass computerClass in computerClasses)
            {
                computerClass.printInfo();
                System.IO.File.AppendAllText("comps.txt", computerClass.export() + "\n", System.Text.Encoding.GetEncoding("utf-8"));
            }



            List<ComputerClass> newComputers = new List<ComputerClass>();
            String s = System.IO.File.ReadAllText("comps.txt");
            String[] strings = s.Split('\n');
            foreach (String s2 in strings)
            {
                if (s2.Length < 3) continue;
                newComputers.Add(new ComputerClass(s2));
            }
            Console.WriteLine("Из файла");
            foreach (ComputerClass computer in newComputers)
            {
                computer.printInfo();
            }
            Console.ReadLine();
        }
    }
}
