[System.Serializable]
public struct SimpleVector2
{
    public int x;
    public int z;

    public SimpleVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public readonly static SimpleVector2 zero = new SimpleVector2(0, 0);


    public readonly static SimpleVector2 one = new SimpleVector2(1, 1);

    public readonly static SimpleVector2 right = new SimpleVector2(1, 0);

    public readonly static SimpleVector2 left = new SimpleVector2(-1, 0);

    public readonly static SimpleVector2 forward = new SimpleVector2(0, 1);

    public readonly static SimpleVector2 back = new SimpleVector2(0, -1);


    public static SimpleVector2 operator +(SimpleVector2 v1, SimpleVector2 v2)
    {
        return new SimpleVector2(v1.x + v2.x, v1.z + v2.z);
    }

    public static SimpleVector2 operator -(SimpleVector2 v1, SimpleVector2 v2)
    {
        return new SimpleVector2(v1.x - v2.x, v1.z - v2.z);
    }

    public static SimpleVector2 operator * (SimpleVector2 v, int l)
    {
        return new SimpleVector2(v.x * l, v.z * l);
    }

    public static SimpleVector2 operator / (SimpleVector2 v, int l)
    {
        return new SimpleVector2(v.x / l, v.z / l);
    }

    public int SqrMagnitude
    {
        get
        {
            return (x * x + z * z);
        }
    }

    public static int SqrDistance(SimpleVector2 v1, SimpleVector2 v2)
    {
        SimpleVector2 dV = v1 - v2;
        return dV.x * dV.x + dV.z * dV.z;
    }

    public void ClampMagnitude(int magnitude)
    {
        if(x > magnitude)
        {
            x = magnitude;
        }
        else if(x < -magnitude)
        {
            x = -magnitude;
        }
        if (z > magnitude)
        {
            z = magnitude;
        }
        else if (z < -magnitude)
        {
            z = -magnitude;
        }
    }


    public override string ToString()
    {
        if (x == 0 && z == 0)
        {
            return "z";
        }
        else if (x == 1 && z == 0)
        {
            return "r";
        }
        else if (x == 0 && z == 1)
        {
            return "f";
        }
        else if (x == -1 && z == 0)
        {
            return "l";
        }
        else if (x == 0 && z == -1)
        {
            return "b";
        }
        else if (x == 1 && z == 1)
        {
            return "o";
        }
        else
        {
            return "(" + x + ", " + z + ")";
        }
    }
    public static SimpleVector2 FromString(string s)
    {
        if (s == "" || s == "z")
        {
            return zero;
        }
        else if (s == "r")
        {
            return right;
        }
        else if (s == "u")
        {
            return forward;
        }
        else if (s == "l")
        {
            return left;
        }
        else if (s == "d")
        {
            return back;
        }
        else if (s == "o")
        {
            return one;
        }
       
        string strX = "";
        string strZ = "";

        int step = 1;
        foreach (char c in s)
        {
            if (c == ',')
            {
                step++;
                continue;
            }
            else if (c == '(' || c == ' ' || c == ')')
            {
                continue;
            }
            if (step == 1)
            {
                strX += c.ToString();
            }
            else if (step == 2)
            {
                strZ += c.ToString();
            }
        }

        int.TryParse(strX, out int x);
        int.TryParse(strZ, out int z);

        return new SimpleVector2(x, z);
    }
}
