﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using UnityEngine;

// TODO: add to the list when things are created instead of calling Save
public class SaveableHolder : MonoBehaviour
{
    public static SaveableHolder Instance
    {
        get { return s_Instance; }
        private set { s_Instance = value; }
    }

    static SaveableHolder s_Instance;

    protected List<ScriptableObject> m_LoadedSaveables = new List<ScriptableObject> ();

    public static bool IsLoaded (string saveableName)
    {
        for (int i = 0; i < Instance.m_LoadedSaveables.Count; i++)
        {
            if (Instance.m_LoadedSaveables[i].name == saveableName)
                return true;
        }
        return false;
    }

    public static TSaveable GetSaveable<TSaveable> (string saveableName)
        where TSaveable : Saveable<TSaveable>
    {
        for (int i = 0; i < Instance.m_LoadedSaveables.Count; i++)
        {
            TSaveable saveable = Instance.m_LoadedSaveables[i] as TSaveable;

            if (saveable != null)
            {
                if (saveable.name == saveableName)
                {
                    return saveable;
                }
            }
        }

        return null;
    }

    public static void AddSaveable<TSaveable> (TSaveable saveable)
        where TSaveable : Saveable<TSaveable>
    {
        Instance.m_LoadedSaveables.Add (saveable);
    }

    public static void Save ()
    {
        for (int i = 0; i < Instance.m_LoadedSaveables.Count; i++)
        {
            Type saveableType = Instance.m_LoadedSaveables[i].GetType ();
            MethodInfo saveMethod = saveableType.GetMethod ("Save", BindingFlags.Public | BindingFlags.Static);
            saveMethod.Invoke (null, new object[] { Instance.m_LoadedSaveables[i] });
        }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
