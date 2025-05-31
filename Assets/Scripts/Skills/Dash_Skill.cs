using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{
    public override void useSkill()
    {
        base.useSkill();
        Debug.Log("Dash_Skill");
    }
}