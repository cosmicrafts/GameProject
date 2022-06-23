//NFT master card for deck (can be unit or spell)

public abstract class NFTsCard : NFTs
{
    public override string KeyId
    {
        get => $"{TypePrefix}_{FactionPrefix}_{LocalID}";
        set => base.KeyId = value;
    }

    public int EnergyCost { get; set; }
}
