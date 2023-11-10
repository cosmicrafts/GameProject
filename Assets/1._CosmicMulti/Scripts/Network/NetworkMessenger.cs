using UnityEngine;
using System;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

#if PHOTON_UNITY_NETWORKING
public class NetworkMessenger : MonoBehaviour, IPunObservable
#else
public class NetworkMessenger : MonoBehaviour
#endif
{
    public Action OnStartGame;
    public Action<float> OnPing;
    public Action<int, long, SimpleVector2> OnAddOtherHeroToWaitingList;

    [NetworkRPC]
    public void AddOtherHeroToWaitingList(int index, long addTime, int x, int z)
    {
        OnAddOtherHeroToWaitingList?.Invoke(index, addTime, new SimpleVector2(x, z));
    }
    [NetworkRPC]
    public void StartGame()
    {
        OnStartGame?.Invoke();
    }
    [NetworkRPC]
    public void Ping(float gameTime)
    {
        OnPing?.Invoke(gameTime);
    }

    public void ClearActions()
    {
        OnStartGame = null;
        OnAddOtherHeroToWaitingList = null;
    }
    
#if PHOTON_UNITY_NETWORKING
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
#endif
}