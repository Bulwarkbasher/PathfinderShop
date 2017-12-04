﻿using UnityEngine;

public abstract class ItemCollection<TItemCollectionFilter, TItemCollection, TItem> : Saveable<TItemCollection>
    where TItemCollectionFilter : ItemCollectionFilter<TItemCollectionFilter, TItemCollection, TItem>
    where TItemCollection : ItemCollection<TItemCollectionFilter, TItemCollection, TItem>
    where TItem : Item<TItem>
{
    public EnumSetting rarities;
    public EnumSetting books;
    public TItemCollectionFilter itemCollectionFilter;
    public TItem[] items = new TItem[0];
    public bool[] doesItemPassFilter = new bool[0];     // Do not serialized, call apply filter when loading instead.

    public TItem this [int index]
    {
        get { return items[index]; }
    }

    public int Length
    {
        get { return items.Length; }
    }

    public static TItemCollection Create(string name, EnumSetting books)
    {
        TItemCollection newArmourCollection = CreateInstance<TItemCollection>();

        if (CheckName(name) == NameCheckResult.Bad)
            throw new UnityException("Collection name invalid, contains invalid characters.");
        if (CheckName(name) == NameCheckResult.IsDefault)
            throw new UnityException("Collection name invalid, name cannot start with Default");

        newArmourCollection.name = name;
        newArmourCollection.items = new TItem[0];
        newArmourCollection.books = books;

        SaveableHolder.AddSaveable(newArmourCollection);

        return newArmourCollection;
    }

    public void ApplyFilter ()
    {
        itemCollectionFilter.ApplyFilter(this as TItemCollection);
    }

    public bool AddBlankItem ()
    {
        return AddItem(Item<TItem>.CreateBlank(rarities, books));
    }

    public bool AddItem (TItem newItem)
    {
        if (!NameIsUnique (newItem.name))
            return false;

        TItem[] newItems = new TItem[items.Length + 1];
        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }
        newItems[items.Length] = newItem;

        return true;
    }

    public bool RemoveItemAt(int index)
    {
        if (index >= items.Length || index < 0)
            return false;

        TItem[] newItems = new TItem[items.Length - 1];
        for (int i = 0; i < newItems.Length; i++)
        {
            int oldItemIndex = i < index ? i : i + 1;
            newItems[i] = items[oldItemIndex];
        }

        return true;
    }

    public bool RemoveItem (TItem itemToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                return RemoveItemAt(i);
            }
        }

        return false;
    }

    public bool NameIsUnique (string itemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].name == itemName)
                return false;
        }
        return true;
    }

    protected override string ConvertToJsonString(string[] jsonSplitter)
    {
        string jsonString = "";

        jsonString += name + jsonSplitter[0];
        jsonString += rarities.name + jsonSplitter[0];
        jsonString += books.name + jsonSplitter[0];
        jsonString += ItemCollectionFilter<TItemCollectionFilter, TItemCollection, TItem>.GetJsonString(itemCollectionFilter) + jsonSplitter[0];

        for (int i = 0; i < items.Length; i++)
        {
            jsonString += Item<TItem>.GetJsonString(items[i]) + jsonSplitter[0];
        }

        return jsonString;
    }

    protected override void SetupFromSplitJsonString(string[] splitJsonString)
    {
        name = splitJsonString[0];
        rarities = EnumSetting.Load(splitJsonString[1]);
        books = EnumSetting.Load(splitJsonString[2]);
        itemCollectionFilter = ItemCollectionFilter<TItemCollectionFilter, TItemCollection, TItem>.CreateFromJsonString(splitJsonString[3]);

        items = new TItem[splitJsonString.Length - 4];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = Item<TItem>.CreateFromJsonString(splitJsonString[i + 4]);
        }
        
        itemCollectionFilter.ApplyFilter(this as TItemCollection);
    }
}
