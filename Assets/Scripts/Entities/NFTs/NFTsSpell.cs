//NFT Spell class

public class NFTsSpell : NFTsCard
{
    public override string KeyId { 
        get => $"H_{Faction[..3].ToUpper()}_{LocalID}"; 
        set => base.KeyId = value; }
}
