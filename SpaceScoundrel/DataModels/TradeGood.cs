using UnityEngine;
using System.Collections;

public class TradeGood {

    public enum GoodType
    {
        Food
    }

    public enum GoodTier
    {
        Tier0,
        Tier1,
        Tier2,
        Tier3
    }

    private int buyPrice;
    private int buyPriceModifier;
    private int sellPrice;
    private int sellpriceModifier;
    private int goodQuantity;
    private string goodName;
    private GoodTier goodTier;
    private GoodType goodType;

   TradeGood(int bPrice , int buyMod ,int sPrice ,int sellMod, int gQuantity , string name , GoodTier tier, GoodType type)
    {
        buyPrice = bPrice;
        buyPriceModifier = buyMod;
        sellPrice = sPrice;
        sellpriceModifier = sellMod;
        goodQuantity = gQuantity;
        goodName = name;
        goodTier = tier;
        goodType = type;
    }
    

    public int getBuyPrice()
    {
        return buyPrice;
    }

    public int getSellPrice()
    {
        return sellPrice;
    }

    public int getBuyPriceModifier()
    {
        return buyPrice;
    }

    public int getModifiedPrice()
    {
        return buyPrice + buyPriceModifier;
    }

    public int getGoodQuantity()
    {
        return goodQuantity;
    }

    public void setBasePrice (int price)
    {
        buyPrice = price;
    }

    public void setPriceModifier(int modifier)
    {
        buyPriceModifier = modifier;
    }

    public void setGoodQuantity(int initial)
    {
        goodQuantity = initial;
    }

    public void addGoodQuantity(int add)
    {
        goodQuantity += add;
    }

    public string getGoodName()
    {
        return goodName;
    }

    public void setGoodName(string name)
    {
        goodName = name;
    }

    public GoodType getGoodType()
    {
        return goodType;
    }

    public void setGoodType(GoodType type)
    {
        goodType = type;
    }

    public GoodTier getGoodTier()
    {
        return goodTier;
    }
    
    public void setGoodTier(GoodTier tier)
    {
        goodTier = tier;
    }




   
}
