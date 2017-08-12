using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Npcgen2CoordData
{
    public class CoordDataEntry
    {
        public string MapNumber = "";
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
    }

    class CoordData
    {
        private string Header = "";
        public Dictionary<string, List<CoordDataEntry>> Entrys = new Dictionary<string, List<CoordDataEntry>>();

        public event SetProgressMax ProgressMax;
        public event SetProgressValue ProgressValue;
        public event SetProgressNext ProgressNext;
        public event SetProgressText ProgressText;

        public void Read(string path)
        {
            ProgressMax?.Invoke(File.ReadAllLines(path).Length);
            ProgressText("Загружаем coord_data.txt");
            StreamReader sr = new StreamReader(path);
            Header = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                ProgressNext?.Invoke();
                string[] data = Regex.Replace(sr.ReadLine().Replace('"', ' ').Replace(",", " "), @"\s+", " ").Split(' ');
                if (data.Length > 3)
                {
                    if (!Entrys.ContainsKey(data[0]))
                        Entrys.Add(data[0], new List<CoordDataEntry>());
                    Entrys[data[0]].Add(new CoordDataEntry()
                    {
                        MapNumber = data[1],
                        X = float.Parse(data[2].Replace('.', ',')),
                        Y = float.Parse(data[3].Replace('.', ',')),
                        Z = float.Parse(data[4].Replace('.', ','))
                    });
                }
            }
            ProgressValue?.Invoke(0);
            ProgressText?.Invoke($"coord_data.txt загружен, {Entrys.Keys.Count} объектов");
            sr.Close();
        }

        public void Save(string path)
        {
            ProgressMax?.Invoke(Entrys.Keys.Count);
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(Header);
            ProgressValue?.Invoke(0);
            foreach (KeyValuePair<string, List<CoordDataEntry>> entry in Entrys)
            {
                ProgressNext?.Invoke();
                entry.Value.ForEach(x =>
                {
                    sw.WriteLine($"{entry.Key}\t{x.MapNumber}\t{string.Format("{0:F2}", x.X).Replace(",", ".")}\t{string.Format("{0:F2}", x.Y).Replace(",", ".")}\t{string.Format("{0:F2}", x.Z).Replace(",", ".")}");
                });
            }
            ProgressValue?.Invoke(0);
            ProgressText?.Invoke($"coord_data.txt успешно сохранен");
            sw.Close();
        }
    }
}
