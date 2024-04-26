internal class Item
{
    public bool itemEquip = false;
    public bool itemBuy { get; set; }
    public string itemName { get; set; }
    public int attack { get; set; }
    public string itemType { get; set; }
    public int itemStat { get; set; }
    public string itemDescript { get; set; }
    public int itemPrice { get; set; }


    public Item(bool _itemBuy, string _itemName, string _itemType, int _itemStat, string _itemDescript, int _itemPrice)
    {
        itemBuy = _itemBuy;
        itemName = _itemName;
        itemType = _itemType;
        itemStat = _itemStat;
        itemDescript = _itemDescript;
        itemPrice = _itemPrice;
    }

}