using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireWall Emulator", menuName = "ScriptableObjects/FireWall Emulator", order = 1)]
public class FireWallEmulator: ScriptableObject
{
    public List<string> filters = new List<string>();
}