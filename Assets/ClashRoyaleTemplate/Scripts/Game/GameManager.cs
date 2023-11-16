using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    
    public static GameMetrics MT;
    
    [SerializeField] private UIGame uiGame;
    [SerializeField] private UIGameResults uiGameResults;
    [SerializeField] private Arena arena;
    [SerializeField] private HeroManager[] heroesPrefabs;
    [SerializeField] private Group[] groups;
    [SerializeField] private Material[] materials;
    
    [Header("Energy Parameters")]
    [Range(0, 99)] public float CurrentEnergy = 5;
    [Range(0, 99)] public float MaxEnergy = 30;
    [Range(0, 10)] public float SpeedEnergy = 1;
    
    public long GameTime { get; private set; }
    private float secondsPass;
    private float myGameTime;
    private float otherGameTime;
    private List<SimpleVector2> dynamicPositions;
    private List<SimpleVector2> staticPositions;
   
    public int GroupIndex { get; private set; }
    public Group MyGroup { get; private set; }
    public Group OtherGroup { get; private set; }
    public bool IsPaused { get; set; } = true;
    public bool IsInitialized { get; set; } 
    private bool gameIsEnd;
    
    

    public void Init()
    {
        if(IsInitialized) { return; }
        
        uiGame.OnPaused(false);
        dynamicPositions = new List<SimpleVector2>(0);
        staticPositions = new List<SimpleVector2>(0);
        //FillArrayStaticPosition
        for (int i = -arena.sizeHalf.x; i <= arena.sizeHalf.x; i++)
        {
            for (int j = -arena.sizeHalf.z; j <= arena.sizeHalf.z; j++)
            {
                SimpleVector2 position = new SimpleVector2(i, j);
                staticPositions.Add(position);
            }
        }

        
        GroupIndex = PunNetworkManager.NetworkManager.PlayerIndex;
        
        for (int i = 0; i < groups.Length; i++)
        {
            groups[i].Init2Group(arena, i);
            if (GroupIndex == i) { MyGroup = groups[i]; }
            else              { OtherGroup = groups[i]; }
            List<TowerManager> simpleTowers = groups[i].Towers;
            for (int x = 0; x < simpleTowers.Count; x++)
            {
                SimpleVector2 towerPosition = groups[i].Positions[0][(x+1) * arena.sizeHalf.x];
                simpleTowers[x].Init(i, towerPosition, null, materials[i]);
                simpleTowers[x].OnMustAttack+= UpdateHeroShooter;
                
                SimpleVector2 towerEnemyPosition =  groups[i].Positions[1][(x+1) * arena.sizeHalf.x];
                simpleTowers[x].transform.LookAt(new Vector3(towerEnemyPosition.x, 0.0f, towerEnemyPosition.z) , Vector3.up);
                
                dynamicPositions.Add(towerPosition);
            }
        }
        Debug.LogWarning("group index " + GroupIndex);

        foreach (HeroManager hero in heroesPrefabs)
        {   uiGame.previewMeshs.Add(hero.previewMeshObject);
            uiGame.PreviewMaterials.Add(hero.transparentMaterial);
            uiGame.CardEnergyCost.Add(hero.energyCost); }
        
        uiGame.SetGroupIndex(GroupIndex);         //Init UIbyIndex
        MT = new GameMetrics(); MT.InitMetrics(); //Init Metrics

        uiGame.OnCreateHero += AddMyHeroToWaitingList;
        PunNetworkManager.NetworkManager.Messenger.OnAddOtherHeroToWaitingList += AddOtherHeroToWaitingList;
        PunNetworkManager.NetworkManager.Messenger.OnStartGame += StartGame;
        PunNetworkManager.NetworkManager.Messenger.OnPing += OnPing;

        PunNetworkManager.NetworkManager.RPCToOthers("StartGame");
        IsInitialized = true;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            PunNetworkManager.NetworkManager.RPCToOthers("Ping", myGameTime);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void OnPing(float otherGameTime) {
        this.otherGameTime = otherGameTime;
    }
  
    private void AddMyHeroToWaitingList(int index, SimpleVector2 position, IEnumerator wait)
    {
        if (!MyGroup.HeroesWaitingList.ContainsKey(GameTime + 1) && !MyGroup.HeroesWaitingList.ContainsKey(GameTime + 2))
        {
            uiGame.HeroCreateTime = (2.0f - secondsPass); StartCoroutine(wait);
            AddMyHeroToWaitingList(index, GameTime + 2, position);
        }
        else
        {
            long addTime = 3; 
            while(MyGroup.HeroesWaitingList.ContainsKey(GameTime + addTime))
            {
                addTime++;
            }
            uiGame.HeroCreateTime = (addTime - secondsPass); StartCoroutine(wait);
            AddMyHeroToWaitingList(index, GameTime + addTime, position);
        }
        RestEnergy( uiGame.CardEnergyCost[index] );
    }
    private void AddMyHeroToWaitingList(int index, long addTime, SimpleVector2 position)
    {
        if (!MyGroup.HeroesWaitingList.ContainsKey(addTime))
        {
            MyGroup.AddHeroToWaitingList(index, addTime, position);
            PunNetworkManager.NetworkManager.RPCToOthers("AddOtherHeroToWaitingList", index, addTime, position.x, position.z);
        }
    }
    private void AddOtherHeroToWaitingList(int index, long addTime, SimpleVector2 position)
    {
        if(gameIsEnd)
        {
            return;
        }
        if (GameTime >= addTime)
        {
            Debug.LogWarning("Bed Network");
            PunNetworkManager.NetworkManager.ExitGame();
            return;
        }
        if (!OtherGroup.HeroesWaitingList.ContainsKey(addTime))
        {
            OtherGroup.AddHeroToWaitingList(index, addTime, position);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Game Time: " + GameTime);
        if (IsPaused)
            GUI.Label(new Rect(100, 100, 50, 30), "Game paused");
    }
    
    public void Update()
    {
        if(IsPaused || gameIsEnd)
        {
            return;
        }

        if(myGameTime - otherGameTime > 1.0f)
        {
            if(waitForOtherPlayer != null)
            {
                StopCoroutine(waitForOtherPlayer);
            }
            waitForOtherPlayer = StartCoroutine(WaitForOtherPlayer(myGameTime - otherGameTime));
        }

        AddEnergy(Time.deltaTime * SpeedEnergy);
        
        secondsPass += Time.deltaTime;
        myGameTime += Time.deltaTime;
        if (secondsPass >= 1f)
        {
            GameTime++;
            secondsPass = 0.0f;
            UpdateGame();
        }
    }
    private Coroutine waitForOtherPlayer;
    IEnumerator WaitForOtherPlayer(float waitingTime)
    {
        Pause(true);
        while (myGameTime - otherGameTime > 0.5f)
        {
            yield return null;
        }
        Pause(false);
    }

    public void Pause(bool pause)
    {
        IsPaused = pause;
        if (!pause || myGameTime - otherGameTime > 1.25f) { uiGame.OnPaused(pause); }
        
        if (IsInitialized)
        {
            foreach (BulletManager bullet in groups[0].Bullets) { bullet.IsPaused = pause; }
            foreach (BulletManager bullet in groups[1].Bullets) { bullet.IsPaused = pause; }
            foreach (HeroManager hero in groups[0].Heroes) { hero.IsPaused = pause; }
            foreach (HeroManager hero in groups[1].Heroes) { hero.IsPaused = pause; }
            foreach (TowerManager tower in groups[0].Towers) { tower.IsPaused = pause; }
            foreach (TowerManager tower in groups[1].Towers) { tower.IsPaused = pause; }
        }
        
    }

    private void StartGame()
    {
        ResetTo(0);
    }

    private void UpdateGame()
    {
        if (groups[0].HeroesWaitingList.ContainsKey(GameTime))
        {
            CreateHero(groups[0].HeroesWaitingList[GameTime], 0,  groups[0], groups[1]);
        }
        if (groups[1].HeroesWaitingList.ContainsKey(GameTime))
        {
            CreateHero(groups[1].HeroesWaitingList[GameTime], 1, groups[1], groups[0]);
        }
        UpdateHeroes(groups[0], groups[1]);
        UpdateHeroes(groups[1], groups[0]);
    }
    private void AddEnergy(float value)
    {
        if (CurrentEnergy < MaxEnergy) { CurrentEnergy += value;} // GameMng.MT.AddEnergyGenerated(value);
        else if (CurrentEnergy >= MaxEnergy) { CurrentEnergy = MaxEnergy;} //GameMng.MT.AddEnergyWasted(value); 
        uiGame.UpdateEnergyUI(CurrentEnergy, MaxEnergy);
    }
    public void RestEnergy(float value)
    {
        CurrentEnergy -= value; // GameMng.MT.AddEnergyUsed(value);
        uiGame.UpdateEnergyUI(CurrentEnergy, MaxEnergy);
    }

    private void CreateHero(HeroWaitingList heroWaitingList, int groupIndex, Group myGroup, Group otherGroup)
    {
        SimpleVector2 heroPosition = heroWaitingList.position;
        HeroManager newHero = CreateHero(heroWaitingList.index, groupIndex, heroPosition, otherGroup);
        myGroup.AddHero(newHero);
    }

    private HeroManager CreateHero(int index, int groupIndex, SimpleVector2 heroPosition, Group otherGroup)
    {
        HeroManager newHero = Instantiate(heroesPrefabs[index]);
        arena.AddHero(newHero);
        newHero.Init(groupIndex, heroPosition, GetTargetTowerOrHero(newHero, heroPosition, otherGroup), materials[groupIndex]);
        newHero.OnMustAttack += UpdateHeroShooter;
        
        if (!newHero.IsBomb)
        {
            dynamicPositions.Add(heroPosition);
        }
        return newHero;
    }
    private GameObjectManager GetTargetTowerOrHero(HeroManager newHero, SimpleVector2 heroPosition, Group otherGroup)
    {
        GameObjectManager target = GetTargetTower(heroPosition, otherGroup.Towers, out int distance);
        HeroManager otherHero = GetTargetHero(newHero, heroPosition, otherGroup.Heroes, out int heroDistance);
        if (otherHero && distance > heroDistance)
        {
            distance = heroDistance;
            target = otherHero;
        }
        return target;
    }

    private TowerManager GetTargetTower(SimpleVector2 heroPosition, List<TowerManager> towers, out int distance)
    {
        distance = int.MaxValue;
        if (towers.Count == 0)
        {
            return null;
        }
        TowerManager target = towers[0];
        distance = SimpleVector2.SqrDistance(heroPosition, target.Position);
        for (int i = 1; i < towers.Count; i++)
        {
            TowerManager newTarget = towers[i];
            if (newTarget != null)
            {
                int newDistance = SimpleVector2.SqrDistance(heroPosition, newTarget.Position);
                if (distance > newDistance)
                {
                    distance = newDistance;
                    target = newTarget;
                }
            }
        }
        return target;
    }

    private HeroManager GetTargetHero(TowerManager tower, List<HeroManager> heroes, out int distance)
    {
        distance = int.MaxValue;
        if (heroes.Count == 0)
        {
            return null;
        }
        HeroManager target = heroes[0].IsBomb ? null :heroes[0];
        if (target)
        {
            distance = SimpleVector2.SqrDistance(tower.Position, target.Position);
        }

        for (int i = 1; i < heroes.Count; i++)
        {
            HeroManager newTarget = heroes[i];
            if (newTarget != null && !newTarget.IsBomb)
            {
                int newDistance = SimpleVector2.SqrDistance(tower.Position, newTarget.Position);
                if (distance > newDistance)
                {
                    distance = newDistance;
                    target = newTarget;
                }
            }
        }
        return target;
    }

    private HeroManager GetTargetHero(HeroManager hero, SimpleVector2 heroPosition, List<HeroManager> heroes, out int distance)
    {
        distance = int.MaxValue;
        if (heroes.Count == 0)
        {
            return null;
        }
        HeroManager target = heroes[0].IsBomb? null : heroes[0];
        if (target)
        {
            distance = SimpleVector2.SqrDistance(heroPosition, target.Position);
        }
        for (int i = 1; i < heroes.Count; i++)
        {
            HeroManager newTarget = heroes[i];
            if (newTarget != null && !newTarget.IsBomb && hero.CanBeAsTarget(newTarget))
            {
                int newDistance = SimpleVector2.SqrDistance(heroPosition, newTarget.Position);
                if (distance > newDistance)
                {
                    distance = newDistance;
                    target = newTarget;
                }
            }
        }
        return target;
    }

    private void ResetTo(long timeToResset)
    {
        secondsPass = timeToResset;
        GameTime = timeToResset;
    }

    private void UpdateBulletOnImpact(BulletManager bullet)
    {
        Group bulletOwnerGroup = new Group(); Group bulletTargetGroup= new Group();;
        if (groups[0].Bullets.Contains(bullet)) { bulletOwnerGroup = groups[0]; bulletTargetGroup = groups[1]; }
        else if (groups[1].Bullets.Contains(bullet)) { bulletOwnerGroup = groups[1]; bulletTargetGroup = groups[0]; }
        else { Debug.Log("Error la bala no pertenece a ningún grupo"); }
        
        bulletOwnerGroup.Bullets.Remove(bullet);

        Debug.Log("Impacto la bala, tiene target?: "+bullet.TargetObject);
        if (bullet.TargetObject)
        {
            if (bullet.TargetObject.Damage(bullet.AttackDamage))
            {
                dynamicPositions.Remove(bullet.TargetObject.Position);
                bullet.TargetObject.RemoveFromGroup(bulletTargetGroup);
           
                bullet.TargetObject.Remove();
                Destroy(bullet.TargetObject);

                if(bulletTargetGroup.Towers.Count == 0)
                {
                    gameIsEnd = true;
                    StartCoroutine(OnWinOrLose(bulletTargetGroup.Index != GroupIndex));
                    
                }
            } 
        }
    }
    private void UpdateHeroShooter(GameObjectManager heroShooter)
    {
        HeroManager bomb = null;

        if (heroShooter.IsBomb) { heroShooter.OnFight(); }//bomb = heroShooter; }
        else if (heroShooter.CanShoot_IsInRange())
        {
            BulletManager bulletInstanced = heroShooter.OnFight();
            bulletInstanced.OnBulletImpact += UpdateBulletOnImpact;
            groups[heroShooter.MyOwnerGroup].Bullets.Add(bulletInstanced );
        }
        
        if(bomb)
        {
            bomb.RemoveFromGroup(groups[heroShooter.MyOwnerGroup]);
            bomb.Remove();
        }
    }
    
    private void UpdateHeroes(Group group1, Group group2)
    {
        foreach (HeroManager hero in group1.Heroes)
        {
            GameObjectManager target = GetTargetTowerOrHero(hero, hero.Position, group2);
            if (target) { hero.SetTargetObject(target); }
           
            if (hero.GetNewPosition(staticPositions, dynamicPositions, out SimpleVector2 newPosition))
            {
                dynamicPositions.Remove(hero.Position);
                dynamicPositions.Add(newPosition);
                hero.MoveTo(newPosition);
            }
            hero.UpdateLookAtPosition(newPosition);
        }
        foreach (TowerManager tower in group1.Towers)
        {
            HeroManager target = GetTargetHero(tower, group2.Heroes, out int distance);
            if(target) {tower.SetTargetObject(target);}
        }
    }
    private IEnumerator OnWinOrLose(bool isWin)
    {
        MT.CalculateLastMetrics(CurrentEnergy, SpeedEnergy);
        uiGame.OnWinOrLose(isWin);
        uiGameResults.SetGameOver(isWin);
        
        
        yield return new WaitForSeconds(2.5f);
        PunNetworkManager.NetworkManager.ExitGame();
    }
}

public class HeroWaitingList
{
    public int index;
    public SimpleVector2 position;
    public HeroWaitingList(int index, SimpleVector2 position)
    {
        this.index = index;
        this.position = position;
    }
}

[System.Serializable]
public class Group
{
    public List<BulletManager> Bullets { get; private set; }
    public List<HeroManager> Heroes { get; private set; }
    public Dictionary<long, HeroWaitingList> HeroesWaitingList { get; private set; }
    public List<TowerManager> Towers => towers;
    public SimpleVector2[][] Positions => positions;
    public int Index { get; private set; }

    [SerializeField] private List<TowerManager> towers;
    [SerializeField] private SimpleVector2[][] positions;

    public void AddHero(HeroManager hero) { Heroes.Add(hero); }
    public void AddTower(TowerManager tower) { Towers.Add(tower); }
    public void AddBullet(BulletManager bullet) { Bullets.Add(bullet); }
    public void RemoveHero(HeroManager hero) { Heroes.Remove(hero); }
    public void RemoveTower(TowerManager tower) { Towers.Remove(tower); }
    public void RemoveBullet(BulletManager bullet) { Bullets.Remove(bullet); }

    public void AddHeroToWaitingList(int heroId, long addTime, SimpleVector2 position)
    {
        HeroesWaitingList.Add(addTime, new HeroWaitingList(heroId, position));
    }
    public void RemoveHeroFromWaitingList(long addTime)
    {
        HeroesWaitingList.Remove(addTime);
    }


    public void Init2Group(Arena arena, int groupIndex)
    {
        Index = groupIndex;
        Heroes = new List<HeroManager>(0);
        Bullets = new List<BulletManager>(0);
        HeroesWaitingList = new Dictionary<long, HeroWaitingList>(0);
        positions = new SimpleVector2[arena.sizeHalf.z][];

        int z = 0;
        int x = 0;

        if (groupIndex == 0)
        {
            for (int j = -arena.sizeHalf.z; j < 0; j++)
            {
                positions[z] = new SimpleVector2[2 * arena.sizeHalf.x + 1];
                x = 0;
                for (int i = -arena.sizeHalf.x; i <= arena.sizeHalf.x; i++)
                {
                    positions[z][x] = new SimpleVector2(i, j);
                    x++;
                }
                z++;
            }
        }
        else
        {
            for (int j = arena.sizeHalf.z; j > 0; j--)
            {
                positions[z] = new SimpleVector2[2 * arena.sizeHalf.x + 1];
                x = 0;
                for (int i = arena.sizeHalf.x; i >= -arena.sizeHalf.x; i--)
                {
                    positions[z][x] = new SimpleVector2(i, j);
                    x++;
                }
                z++;
            }
        }
    }
}

