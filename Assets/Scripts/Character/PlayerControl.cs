using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public CharacterAttributes charStats { get; private set; }
    public CharacterMove characterMove { get; private set; }
    public PlayerJump jump { get; private set; }
    public Attack attack { get; private set; }
    public HeroGraphics heroGraphics { get; private set; }
    public ClientNetworkSender clientNetworkSender;
    public ClientNetworkReciever clientNetworkReciever;
    public ServerNetwork serverNetwork;
    public GameObject bulletPrefab;

    public Color color; 

    // Use this for initialization
    void Awake()
    {
        clientNetworkSender = GetComponent<ClientNetworkSender>();
        clientNetworkReciever = GetComponent<ClientNetworkReciever>();
        serverNetwork = GetComponent<ServerNetwork>();
        charStats = GetComponent<CharacterAttributes>();
        heroGraphics = GetComponent<HeroGraphics>();
        characterMove = GetComponent<CharacterMove>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<Attack>();
        Camera.main.GetComponent<Camera_Follow>().player_ = gameObject;
    }

    void Start()
    {
        
    }

    public bool IsLocalPlayer(){
        return serverNetwork.isLocalPlayer;
    }

    // Some Damage has been done
    public void TakeAttack(float damage, Buff buff)
    {
        if (buff != null)
        {
            // buff code here
        }
        TakeDamage(damage);

    }

    private void TakeDamage(float damage)
    {
        Debug.Log("here");
        heroGraphics.TakeDamage();
        print("Took Damage");
        charStats.hitPoints -= damage;
        if (charStats.hitPoints <= 0)
        {
            print("Dead");
            if(clientNetworkSender.isServer){
                serverNetwork.CmdKillPlayer();
            }
        }
    }
}


