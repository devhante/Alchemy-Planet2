using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Length { get; set; }
    public string[] Monsters { get; set; }

    public StageInfo(int id, string name, int length, params string[] monsters)
    {
        Id = id;
        Name = name;
        Length = length;
        Monsters = monsters;
    }
}
