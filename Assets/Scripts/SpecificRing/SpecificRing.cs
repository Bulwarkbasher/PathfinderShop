﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificRing : SpecificItem<SpecificRing>
{
    // TODO: complete me

    public static SpecificRing CreateRandom()
    {
        return CreateInstance<SpecificRing>();
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
