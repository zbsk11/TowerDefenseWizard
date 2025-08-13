using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    private Turret builtTurret = null;
    private bool occupied = false;

    public bool Occupied { get => occupied; set => occupied = value; }
    public Turret BuiltTurret { get => builtTurret; set => builtTurret = value; }
}
