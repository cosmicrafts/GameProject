using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public SimpleVector2 sizeHalf;
    [SerializeField] private Transform heroesContainer;

    public void AddHero(HeroManager hero)
    {
        hero.transform.SetParent(heroesContainer);
    }
}
