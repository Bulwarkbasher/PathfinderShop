﻿using UnityEngine;
using System;

public class SpecificArmourCollection : SpecificItemCollection<SpecificArmour, SpecificArmourCollection, ArmourQualityConstraintsMatrix>
{
    protected override SpecificArmour CreateRandomSpecificItem(SpecificItem.PowerLevel powerLevel, FloatRange budgetRange)
    {
        return SpecificArmour.CreateRandom(powerLevel, ingredient, budgetRange);
    }

    protected override ArmourQualityConstraintsMatrix GetIngredient(Shop shop)
    {
        return shop.ArmourQualityConstraintsMatrix;
    }

    protected override Shop.StockType GetStockType()
    {
        return Shop.StockType.Armour;
    }

    protected override void SetShopCollection(Shop shop)
    {
        shop.specificArmourCollection = this;
    }
}
