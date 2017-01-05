/*
 * Author: Sean Hoey x11000759
 * Date: 23/02/2015
 */
using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System;


public class DataManager : MonoBehaviour 
{

public string StatsFileName;
private static bool created = false;
public bool Change = false;
string PlayerName;
public int version;
public float Level;
public int Exp;
public float CurrentHealth;
public int Coins;
public int Health;
public int Smoke;

byte[] Data;

	void Awake(){
        StatsFileName = Application.persistentDataPath + @"/Stats.xml";
		if (!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created = true;
		} else 
		{
			Destroy(this.gameObject);
		}
        ReadStats_FromDisk();
	}
	
    public float GetLevel()
    {
        return Level;
    }

    public int GetExp()
    {
        return Exp;
    }

    public int GetCoins()
    {
        return Coins;
    }

    public int GetInventory(int x)
    {
        if (x == 0)
        {
            return Health;
        }
        else if (x == 2)
        {
            return Smoke;
        }
        else
        {
            return 0;
        }
    }
 
	
	
	// Use this for initialization
	void Start () 
	{
        
	}

    public void reset()
    {
            Level = 1;
            Exp = 0;
            Coins = 0;
            Health = 5;
            Smoke = 5;
            version = 0;

    }
	void Update(){
        if (Change)
        {
            version++;
            SaveStats_ToDisk();
            Change = false;
        }
	}
	
	public void SaveStats_ToDisk()
	{
		try
		{
			
			using (Stream stream = File.Open(StatsFileName, FileMode.Create))
			{
				using (MemoryStream memStream = new MemoryStream())
				{
					
					using (XmlWriter memWriter = XmlWriter.Create(memStream))
					{
						memWriter.WriteStartDocument();
						memWriter.WriteStartElement("Player"); // open <player>
						memWriter.WriteAttributeString("PlayerName",PlayerName);
						memWriter.WriteAttributeString("Version", version.ToString());
                        memWriter.WriteAttributeString("Level", Level.ToString());
                        memWriter.WriteAttributeString("Exp", Exp.ToString());
                         memWriter.WriteAttributeString("Coins", Coins.ToString());
                         memWriter.WriteAttributeString("Health", Health.ToString());
                         memWriter.WriteAttributeString("Smoke", Smoke.ToString());
                         memWriter.WriteAttributeString("CurrentHealth", CurrentHealth.ToString());
						// write score
						
						memWriter.WriteEndElement(); // close <player>					
						memWriter.WriteEndDocument();
						memWriter.Flush(); // flushes the data into the mem stream
						
						memStream.Position = 0;
						
						// put the stream into an array so we can obsf it
						byte[] byteArray = memStream.ToArray();
						
						// Set up a new stream we can use to write into our save file
						using (BinaryWriter fileWriter = new BinaryWriter(stream))
						{
							fileWriter.Write(byteArray);
						}
					}
				}
			}
			
		}
		catch
		{
		}
	}
	
	public void ReadStats_FromDisk()
	{
		try
		{
			// check file exists
			if (!File.Exists(StatsFileName))
				return;
			
			// read the raw data fromt he file into a stream
			using( Stream stream = File.Open(StatsFileName, FileMode.Open) )
			{
				using (BinaryReader fileReader = new BinaryReader(stream))
				{
					
					// setup a file to read the data
					byte[] buffer = new byte[fileReader.BaseStream.Length]; 
					
					if (buffer.Length <= 0)
						return;
					
					// read the file into a buffer
					fileReader.Read(buffer, 0, buffer.Length);
					
					// read the buffer into a memory stream so we can send to global component
					using (MemoryStream memStream = new MemoryStream(buffer))
					{
						
						using (XmlReader memReader = XmlReader.Create(memStream))
						{
							// parse file and read each node
							// will continue as long as data available
							while (memReader.Read())
							{
								if (memReader.NodeType.ToString() == "Element")
								{
                                    if (memReader.Name == "Player")
									{
                                        PlayerName = memReader.GetAttribute("PlayerName");
                                        version = Convert.ToInt32(memReader.GetAttribute("Version"));
                                        Level = Convert.ToSingle(memReader.GetAttribute("Level"));
                                        Exp = Convert.ToInt32(memReader.GetAttribute("Exp"));
                                        Coins = Convert.ToInt32(memReader.GetAttribute("Coins"));
                                        Health = Convert.ToInt32(memReader.GetAttribute("Health"));
                                        Smoke = Convert.ToInt32(memReader.GetAttribute("Smoke"));
                                        CurrentHealth = Convert.ToInt32(memReader.GetAttribute("CurrentHealth"));
									}
								}
							}
						}
					}
				}
			}
		}
		catch
		{
		}
	}
}