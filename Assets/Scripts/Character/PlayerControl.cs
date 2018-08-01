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

    private BuffManager buffManager;

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
        buffManager = GetComponent<BuffManager>();
        Camera.main.GetComponent<Camera_Follow>().player_ = gameObject;
    }

    void Start()
    {
        
    }
    public bool IsLocalPlayer(){
        return serverNetwork.isLocalPlayer;

    }

    public bool IsServer(){
        return serverNetwork.isServer;
    }

    // Some Damage has been done
    public void TakeAttack(float damage, string buffName)
    {
        if (buffName != "")
        {
            buffManager.ActivateBuff(buffName);
        }
        TakeDamage(damage);

    }

    private void TakeDamage(float damage)
    {
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


    public void Meleeattack() { }
        

}


