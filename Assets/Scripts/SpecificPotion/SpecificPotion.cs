﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificPotion : SpecificItem<SpecificPotion>
{
    // TODO: complete me

    public static SpecificPotion CreateRandom()
    {
        return CreateInstance<SpecificPotion>();
    }

    protected override void SetupFromSplitJsonString (string[] splitJsonString)
    {
        throw new System.NotImplementedException ();
    }

    protected override string ConvertToJsonString (string[] jsonSplitter)
    {
        throw new System.NotImplementedException ();
    }
}
