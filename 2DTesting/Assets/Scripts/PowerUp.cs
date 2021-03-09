public abstract class PowerUp
{
    public static Rarity rarity;
    public static string name;
    public abstract void onPickup();
}

public enum Rarity
{
    Common,
    Rare,
    Legendary
}