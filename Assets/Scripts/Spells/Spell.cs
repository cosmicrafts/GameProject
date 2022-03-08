using UnityEngine;

public class Spell : MonoBehaviour
{
    protected bool IsFake = false;
    protected string Key;
    public Team MyTeam;
    public int PlayerId = 1;
    protected int Id;

    [Range(0, 300)]
    public float Duration = 1;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameMng.GM.AddSpell(this);
        if (Duration > 0)
        {
            Destroy(gameObject, Duration);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void setHasFake()
    {
        IsFake = true;
    }

    public void setKey(string key)
    {
        Key = key;
    }

    public string getKey()
    {
        return Key;
    }

    public void setId(int id)
    {
        Id = id;
    }

    public int getId()
    {
        return Id;
    }

    private void OnDestroy()
    {
        if (GameMng.GM != null)
        {
            GameMng.GM.DeleteSpell(this);
        }
    }
}
