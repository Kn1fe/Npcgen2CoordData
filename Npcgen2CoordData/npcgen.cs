using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Npcgen2CoordData
{
    public class ClassDefaultMonsters
    {
        public int Location;
        public int Amount_in_group;
        public float X_position;
        public float Y_position;
        public float Z_position;
        public float X_direction;
        public float Y_direction;
        public float Z_direction;
        public float X_random;
        public float Y_random;
        public float Z_random;
        public int Type;
        public int iGroupType;
        public byte BInitGen;
        public byte bAutoRevive;
        public byte BValicOnce;
        public int dwGenId;
        public int Trigger_id;
        public int Life_time;
        public int MaxRespawnTime;
        public List<ClassExtraMonsters> MobDops;
    }

    public class ClassExtraMonsters
    {
        public int Id;
        public int Amount;
        public int Respawn;
        public int Dead_amount;
        public int Agression;
        public float fOffsetWater;
        public float fOffsetTrn;
        public int Group;
        public int Group_help_sender;
        public int Group_help_Needer;
        public byte bNeedHelp;
        public byte bFaction;
        public byte bFac_Helper;
        public byte bFac_Accept;
        public int Path;
        public int Path_type;
        public int Speed;
        public int Dead_time;
        public int RefreshLower;
    }

    public class ClassDefaultResources
    {
        public float X_position;
        public float Y_position;
        public float Z_position;
        public float X_Random;
        public float Z_Random;
        public int Amount_in_group;
        public byte bInitGen;
        public byte bAutoRevive;
        public byte bValidOnce;
        public int dwGenID;
        public byte InCline1;
        public byte InCline2;
        public byte Rotation;
        public int Trigger_id;
        public int IMaxNum;
        public List<ClassExtraResources> ResExtra;
    }

    public class ClassExtraResources
    {
        public int ResourceType;
        public int Id;
        public int Respawntime;
        public int Amount;
        public float fHeiOff;
    }

    public class ClassDynamicObject
    {
        public int Id;
        public float X_position;
        public float Y_position;
        public float Z_position;
        public byte InCline1;
        public byte InCline2;
        public byte Rotation;
        public int TriggerId;
        public byte Scale;
    }

    public class ClassTrigger
    {
        public int Id;
        public int GmID;
        public string TriggerName;
        public byte AutoStart;
        public int WaitWhileStart;
        public int WaitWhileStop;
        public byte DontStartOnSchedule;
        public byte DontStopOnSchedule;
        public int StartYear;
        public int StartMonth;
        public int StartWeekDay;
        public int StartDay;
        public int StartHour;
        public int StartMinute;
        public int StopYear;
        public int StopMonth;
        public int StopWeekDay;
        public int StopDay;
        public int StopHour;
        public int StopMinute;
        public int Duration;
    }

    class NpcGen
    {
        public int File_version;
        public int NpcMobsAmount = 0;
        public int ResourcesAmount = 0;
        public int DynobjectAmount = 0;
        public int TriggersAmount = 0;
        public List<ClassDefaultMonsters> NpcMobList = new List<ClassDefaultMonsters>();
        public List<ClassDefaultResources> ResourcesList = new List<ClassDefaultResources>();
        public List<ClassDynamicObject> DynamicsList = new List<ClassDynamicObject>();
        public List<ClassTrigger> TriggersList = new List<ClassTrigger>();

        public event SetProgressMax ProgressMax;
        public event SetProgressValue ProgressValue;
        public event SetProgressNext ProgressNext;
        public event SetProgressText ProgressText;

        public void ReadNpcgen(BinaryReader br)
        {
            ProgressText?.Invoke("Загружаем npcgen.data");
            File_version = br.ReadInt32();
            NpcMobsAmount = br.ReadInt32();
            ResourcesAmount = br.ReadInt32();
            DynobjectAmount = br.ReadInt32();
            if (File_version > 6)
            {
                TriggersAmount = br.ReadInt32();
            }
            ProgressMax.Invoke(NpcMobsAmount + ResourcesAmount);
            for (int i = 0; i < NpcMobsAmount; i++)
            {
                NpcMobList.Add(ReadExistence(br, File_version));
                ProgressNext?.Invoke();
            }
            for (int i = 0; i < ResourcesAmount; i++)
            {
                ResourcesList.Add(ReadResource(br, File_version));
                ProgressNext?.Invoke();
            }
            ProgressText?.Invoke($"npcgen.data загружен, {NpcMobsAmount + ResourcesAmount} объектов");
            ProgressValue(0);
            br.Close();
        }

        public void WriteNpcgen(BinaryWriter bw, int Version)
        {
            bw.Write(Version);
            bw.Write(NpcMobsAmount);
            bw.Write(ResourcesAmount);
            bw.Write(DynobjectAmount);
            if (Version > 6)
            {
                bw.Write(TriggersAmount);
            }
            for (int i = 0; i < NpcMobsAmount; i++)
            {
                WriteExistence(bw, Version, i);
            }
            for (int i = 0; i < ResourcesAmount; i++)
            {
                WriteResource(bw, Version, i);
            }
            for (int i = 0; i < DynobjectAmount; i++)
            {
                WriteDynamic(bw, Version, i);
            }
            if (Version > 6)
            {
                #region Triggers
                for (int i = 0; i < TriggersAmount; i++)
                {
                    WriteTrigger(bw, Version, i);
                }
                #endregion
            }
        }

        public ClassDefaultMonsters ReadExistence(BinaryReader br, int Version)
        {
            ClassDefaultMonsters mn = new ClassDefaultMonsters();
            mn.Location = br.ReadInt32();
            mn.Amount_in_group = br.ReadInt32();
            mn.X_position = br.ReadSingle();
            mn.Y_position = br.ReadSingle();
            mn.Z_position = br.ReadSingle();
            mn.X_direction = br.ReadSingle();
            mn.Y_direction = br.ReadSingle();
            mn.Z_direction = br.ReadSingle();
            mn.X_random = br.ReadSingle();
            mn.Y_random = br.ReadSingle();
            mn.Z_random = br.ReadSingle();
            mn.Type = br.ReadInt32();
            mn.iGroupType = br.ReadInt32();
            mn.BInitGen = br.ReadByte();
            mn.bAutoRevive = br.ReadByte();
            mn.BValicOnce = br.ReadByte();
            mn.dwGenId = br.ReadInt32();
            if (Version > 6)
            {
                mn.Trigger_id = br.ReadInt32();
                mn.Life_time = br.ReadInt32();
                mn.MaxRespawnTime = br.ReadInt32();
            }
            mn.MobDops = new List<ClassExtraMonsters>(mn.Amount_in_group);
            for (int z = 0; z < mn.Amount_in_group; z++)
            {
                ClassExtraMonsters mne = new ClassExtraMonsters();
                mne.Id = br.ReadInt32();
                mne.Amount = br.ReadInt32();
                mne.Respawn = br.ReadInt32();
                mne.Dead_amount = br.ReadInt32();
                mne.Agression = br.ReadInt32();
                mne.fOffsetWater = br.ReadSingle();
                mne.fOffsetTrn = br.ReadSingle();
                mne.Group = br.ReadInt32();
                mne.Group_help_sender = br.ReadInt32();
                mne.Group_help_Needer = br.ReadInt32();
                mne.bNeedHelp = br.ReadByte();
                mne.bFaction = br.ReadByte();
                mne.bFac_Helper = br.ReadByte();
                mne.bFac_Accept = br.ReadByte();
                mne.Path = br.ReadInt32();
                mne.Path_type = br.ReadInt32();
                mne.Speed = br.ReadInt32();
                mne.Dead_time = br.ReadInt32();
                if (Version >= 11)
                {
                    mne.RefreshLower = br.ReadInt32();
                }
                mn.MobDops.Add(mne);
            }
            return mn;
        }

        public ClassDefaultResources ReadResource(BinaryReader br, int Version)
        {
            ClassDefaultResources rs = new ClassDefaultResources();
            rs.X_position = br.ReadSingle();
            rs.Y_position = br.ReadSingle();
            rs.Z_position = br.ReadSingle();
            rs.X_Random = br.ReadSingle();
            rs.Z_Random = br.ReadSingle();
            rs.Amount_in_group = br.ReadInt32();
            rs.bInitGen = br.ReadByte();
            rs.bAutoRevive = br.ReadByte();
            rs.bValidOnce = br.ReadByte();
            rs.dwGenID = br.ReadInt32();
            rs.InCline1 = br.ReadByte();
            rs.InCline2 = br.ReadByte();
            rs.Rotation = br.ReadByte();
            rs.Trigger_id = br.ReadInt32();
            rs.IMaxNum = br.ReadInt32();
            rs.ResExtra = new List<ClassExtraResources>();
            for (int j = 0; j < rs.Amount_in_group; j++)
            {
                ClassExtraResources drs = new ClassExtraResources();
                drs.ResourceType = br.ReadInt32();
                drs.Id = br.ReadInt32();
                drs.Respawntime = br.ReadInt32();
                drs.Amount = br.ReadInt32();
                drs.fHeiOff = br.ReadSingle();
                rs.ResExtra.Add(drs);
            }
            return rs;
        }

        public ClassDynamicObject ReadDynObjects(BinaryReader br, int Version)
        {
            ClassDynamicObject ddo = new ClassDynamicObject();
            ddo.Id = br.ReadInt32();
            ddo.X_position = br.ReadSingle();
            ddo.Y_position = br.ReadSingle();
            ddo.Z_position = br.ReadSingle();
            ddo.InCline1 = br.ReadByte();
            ddo.InCline2 = br.ReadByte();
            ddo.Rotation = br.ReadByte();
            ddo.TriggerId = br.ReadInt32();
            ddo.Scale = br.ReadByte();
            return ddo;
        }

        public ClassTrigger ReadTrigger(BinaryReader br, int Version)
        {
            ClassTrigger ct = new ClassTrigger();
            ct.Id = br.ReadInt32();
            ct.GmID = br.ReadInt32();
            ct.TriggerName = Encoding.GetEncoding(936).GetString(br.ReadBytes(128)).TrimEnd('\0');
            ct.AutoStart = br.ReadByte();
            ct.WaitWhileStart = br.ReadInt32();
            ct.WaitWhileStop = br.ReadInt32();
            ct.DontStartOnSchedule = br.ReadByte();
            if (ct.DontStartOnSchedule == 0)
                ct.DontStartOnSchedule = 1;
            else if (ct.DontStartOnSchedule == 1)
                ct.DontStartOnSchedule = 0;
            ct.DontStopOnSchedule = br.ReadByte();
            if (ct.DontStopOnSchedule == 1)
                ct.DontStopOnSchedule = 0;
            else if (ct.DontStopOnSchedule == 0)
                ct.DontStopOnSchedule = 1;
            ct.StartYear = br.ReadInt32();
            ct.StartMonth = br.ReadInt32();
            ct.StartWeekDay = br.ReadInt32();
            ct.StartDay = br.ReadInt32();
            ct.StartHour = br.ReadInt32();
            ct.StartMinute = br.ReadInt32();
            ct.StopYear = br.ReadInt32();
            ct.StopMonth = br.ReadInt32();
            ct.StopWeekDay = br.ReadInt32();
            ct.StopDay = br.ReadInt32();
            ct.StopHour = br.ReadInt32();
            ct.StopMinute = br.ReadInt32();
            if (File_version > 7)
            {
                ct.Duration = br.ReadInt32();
            }
            return ct;
        }

        public void WriteExistence(BinaryWriter bw, int Version, int i)
        {
            bw.Write(NpcMobList[i].Location);
            bw.Write(NpcMobList[i].Amount_in_group);
            bw.Write(NpcMobList[i].X_position);
            bw.Write(NpcMobList[i].Y_position);
            bw.Write(NpcMobList[i].Z_position);
            bw.Write(NpcMobList[i].X_direction);
            bw.Write(NpcMobList[i].Y_direction);
            bw.Write(NpcMobList[i].Z_direction);
            bw.Write(NpcMobList[i].X_random);
            bw.Write(NpcMobList[i].Y_random);
            bw.Write(NpcMobList[i].Z_random);
            bw.Write(NpcMobList[i].Type);
            bw.Write(NpcMobList[i].iGroupType);
            bw.Write(NpcMobList[i].BInitGen);
            bw.Write(NpcMobList[i].bAutoRevive);
            bw.Write(NpcMobList[i].BValicOnce);
            bw.Write(NpcMobList[i].dwGenId);
            if (Version > 6)
            {
                bw.Write(NpcMobList[i].Trigger_id);
                bw.Write(NpcMobList[i].Life_time);
                bw.Write(NpcMobList[i].MaxRespawnTime);
            }
            for (int k = 0; k < NpcMobList[i].Amount_in_group; k++)
            {
                bw.Write(NpcMobList[i].MobDops[k].Id);
                bw.Write(NpcMobList[i].MobDops[k].Amount);
                bw.Write(NpcMobList[i].MobDops[k].Respawn);
                bw.Write(NpcMobList[i].MobDops[k].Dead_amount);
                bw.Write(NpcMobList[i].MobDops[k].Agression);
                bw.Write(NpcMobList[i].MobDops[k].fOffsetWater);
                bw.Write(NpcMobList[i].MobDops[k].fOffsetTrn);
                bw.Write(NpcMobList[i].MobDops[k].Group);
                bw.Write(NpcMobList[i].MobDops[k].Group_help_sender);
                bw.Write(NpcMobList[i].MobDops[k].Group_help_Needer);
                bw.Write(NpcMobList[i].MobDops[k].bNeedHelp);
                bw.Write(NpcMobList[i].MobDops[k].bFaction);
                bw.Write(NpcMobList[i].MobDops[k].bFac_Helper);
                bw.Write(NpcMobList[i].MobDops[k].bFac_Accept);
                bw.Write(NpcMobList[i].MobDops[k].Path);
                bw.Write(NpcMobList[i].MobDops[k].Path_type);
                bw.Write(NpcMobList[i].MobDops[k].Speed);
                bw.Write(NpcMobList[i].MobDops[k].Dead_time);
                if (Version >= 11)
                {
                    bw.Write(NpcMobList[i].MobDops[k].RefreshLower);
                }
            }
        }

        public void WriteResource(BinaryWriter bw, int Version, int i)
        {
            bw.Write(ResourcesList[i].X_position);
            bw.Write(ResourcesList[i].Y_position);
            bw.Write(ResourcesList[i].Z_position);
            bw.Write(ResourcesList[i].X_Random);
            bw.Write(ResourcesList[i].Z_Random);
            bw.Write(ResourcesList[i].Amount_in_group);
            bw.Write(ResourcesList[i].bInitGen);
            bw.Write(ResourcesList[i].bAutoRevive);
            bw.Write(ResourcesList[i].bValidOnce);
            bw.Write(ResourcesList[i].dwGenID);
            bw.Write(ResourcesList[i].InCline1);
            bw.Write(ResourcesList[i].InCline2);
            bw.Write(ResourcesList[i].Rotation);
            bw.Write(ResourcesList[i].Trigger_id);
            bw.Write(ResourcesList[i].IMaxNum);
            for (int z = 0; z < ResourcesList[i].Amount_in_group; z++)
            {
                bw.Write(ResourcesList[i].ResExtra[z].ResourceType);
                bw.Write(ResourcesList[i].ResExtra[z].Id);
                bw.Write(ResourcesList[i].ResExtra[z].Respawntime);
                bw.Write(ResourcesList[i].ResExtra[z].Amount);
                bw.Write(ResourcesList[i].ResExtra[z].fHeiOff);
            }
        }

        public void WriteDynamic(BinaryWriter bw, int Version, int i)
        {
            bw.Write(DynamicsList[i].Id);
            bw.Write(DynamicsList[i].X_position);
            bw.Write(DynamicsList[i].Y_position);
            bw.Write(DynamicsList[i].Z_position);
            bw.Write(DynamicsList[i].InCline1);
            bw.Write(DynamicsList[i].InCline2);
            bw.Write(DynamicsList[i].Rotation);
            bw.Write(DynamicsList[i].TriggerId);
            bw.Write(DynamicsList[i].Scale);
        }

        public void WriteTrigger(BinaryWriter bw, int Version, int i)
        {
            bw.Write(TriggersList[i].Id);
            bw.Write(TriggersList[i].GmID);
            bw.Write(GetBytes(TriggersList[i].TriggerName, 128, Encoding.GetEncoding(936)));
            bw.Write(TriggersList[i].AutoStart);
            bw.Write(TriggersList[i].WaitWhileStart);
            bw.Write(TriggersList[i].WaitWhileStop);
            if (TriggersList[i].DontStartOnSchedule == 1) bw.Write((byte)0);
            else bw.Write((byte)1);
            if (TriggersList[i].DontStopOnSchedule == 1) bw.Write((byte)0);
            else bw.Write((byte)1);
            bw.Write(TriggersList[i].StartYear);
            bw.Write(TriggersList[i].StartMonth);
            bw.Write(TriggersList[i].StartWeekDay);
            bw.Write(TriggersList[i].StartDay);
            bw.Write(TriggersList[i].StartHour);
            bw.Write(TriggersList[i].StartMinute);
            bw.Write(TriggersList[i].StopYear);
            bw.Write(TriggersList[i].StopMonth);
            bw.Write(TriggersList[i].StopWeekDay);
            bw.Write(TriggersList[i].StopDay);
            bw.Write(TriggersList[i].StopHour);
            bw.Write(TriggersList[i].StopMinute);
            if (Version > 7)
            {
                bw.Write(TriggersList[i].Duration);
            }
        }

        public byte[] GetBytes(string Name, int NameLength, Encoding e)
        {
            Name = Name.Split('\0')[0];
            byte[] data = new byte[NameLength];
            if (e.GetByteCount(Name) > NameLength)
            {
                Array.Copy(e.GetBytes(Name), 0, data, 0, NameLength);
            }
            else
            {
                Array.Copy(e.GetBytes(Name), data, e.GetByteCount(Name));
            }
            return data;
        }
    }
}
