//NFT unit class
public class NFTsUnit : NFTsCard
{
    public override string KeyId
    {
        get => $"{GlobalManager.NFTsPrefix[EntType]}_{Faction[..3].ToUpper()}_{LocalID}";
        set => base.KeyId = value;
    }

    public int HitPoints { get; set; }

    public int Shield { get; set; }

    public int Dammage { get; set; }

    public float Speed { get; set; }
}
