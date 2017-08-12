using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Npcgen2CoordData
{
    public delegate void SetProgressMax(int value);
    public delegate void SetProgressNext();
    public delegate void SetProgressValue(int value);
    public delegate void SetProgressText(string value);

    public partial class Form1 : Form
    {
        NpcGen npcgen = new NpcGen();
        CoordData coord = new CoordData();

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            npcgen.ProgressMax += ProgressMax;
            npcgen.ProgressNext += ProgressNext;
            npcgen.ProgressText += ProgressText;
            npcgen.ProgressValue += ProgressValue;
            coord.ProgressMax += ProgressMax;
            coord.ProgressNext += ProgressNext;
            coord.ProgressText += ProgressText;
            coord.ProgressValue += ProgressValue;
        }

        private void ProgressValue(int value)
        {
            Progress.Value = value;
        }

        private void ProgressText(string value)
        {
            Status.Text = value;
        }

        private void ProgressNext()
        {
            ++Progress.Value;
        }

        private void ProgressMax(int value)
        {
            Progress.Maximum = value;
        }

        private void load_coord_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Coord_data|*.txt|All Files|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() => coord.Read(ofd.FileName)).Start();
            }
        }

        private void save_coord_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Coord_data|*.txt|All Files|*.*"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() => coord.Save(sfd.FileName)).Start();
            }
        }

        private void import_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Npcgen|Npcgen.data|All Files|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                new Thread(() => 
                {
                    ProgressMax(npcgen.NpcMobList.Count + npcgen.ResourcesList.Count);
                    List<int> cleared = new List<int>();
                    npcgen.ReadNpcgen(new BinaryReader(File.OpenRead(ofd.FileName)));
                    ProgressText("Импортируем");
                    string map = Microsoft.VisualBasic.Interaction.InputBox("Название локации в которой находятся мобы, например, world, a78, a64", "Название локации", "world");
                    npcgen.NpcMobList.ForEach(x =>
                    {
                        ProgressNext();
                        x.MobDops.ForEach(y =>
                        {
                            if (coord.Entrys.ContainsKey(y.Id.ToString()))
                            {
                                if (!cleared.Contains(y.Id))
                                {
                                    coord.Entrys[y.Id.ToString()].Clear();
                                    cleared.Add(y.Id);
                                }
                                coord.Entrys[y.Id.ToString()].Add(new CoordDataEntry()
                                {
                                    MapNumber = map,
                                    X = x.X_position,
                                    Y = x.Y_position,
                                    Z = x.Z_position
                                });
                            }
                            else
                            {
                                cleared.Add(y.Id);
                                coord.Entrys[y.Id.ToString()] = new List<CoordDataEntry>
                                {
                                    new CoordDataEntry()
                                    {
                                        MapNumber = map,
                                        X = x.X_position,
                                        Y = x.Y_position,
                                        Z = x.Z_position
                                    }
                                };
                            }
                        });
                    });
                    npcgen.ResourcesList.ForEach(x =>
                    {
                        ProgressNext();
                        x.ResExtra.ForEach(y =>
                        {
                            if (coord.Entrys.ContainsKey(y.Id.ToString()))
                            {
                                if (!cleared.Contains(y.Id))
                                {
                                    coord.Entrys[y.Id.ToString()].Clear();
                                    cleared.Add(y.Id);
                                }
                                coord.Entrys[y.Id.ToString()].Add(new CoordDataEntry()
                                {
                                    MapNumber = map,
                                    X = x.X_position,
                                    Y = x.Y_position,
                                    Z = x.Z_position
                                });
                            }
                            else
                            {
                                cleared.Add(y.Id);
                                coord.Entrys[y.Id.ToString()] = new List<CoordDataEntry>
                                {
                                    new CoordDataEntry()
                                    {
                                        MapNumber = map,
                                        X = x.X_position,
                                        Y = x.Y_position,
                                        Z = x.Z_position
                                    }
                                };
                            }
                        });
                    });
                    ProgressValue(0);
                    ProgressText("Готово");
                }).Start();
            }
        }
    }
}
