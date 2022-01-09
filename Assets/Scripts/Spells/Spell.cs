using UnityEngine;

public class Spell : MonoBehaviour
{
    protected bool IsFake = false;
    protected string Key;
    public Team MyTeam;
    public int PlayerId = 1;

    [Range(0, 300)]
    public float Duration = 1;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (Duration > 0)
        {
            Destroy(gameObject, Duration);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void setHasFake()
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
}
