using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerControl : MonoBehaviour
{
    public CharacterAttributes charStats { get; private set; }
    public CharacterMove characterMove { get; private set; }
    public PlayerJump jump { get; private set; }
    public Attack attack { get; private set; }
    public HeroGraphics heroGraphics { get; private set; }
    public ServerNetworkSender serverNetworkSender;
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
        clientNetworkReciever = ClientNetworkReciever.instance;
        serverNetworkSender = ServerNetworkSender.instance;
        serverNetwork = GetComponent<ServerNetwork>();
        charStats = GetComponent<CharacterAttributes>();
        heroGraphics = GetComponent<HeroGraphics>();
        characterMove = GetComponent<CharacterMove>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<Attack>();
        buffManager = GetComponent<BuffManager>();
    }

    void Start()
    {
        if (IsLocalPlayer())
        {
            print("local Player Camera");
            Camera.main.GetComponent<SmoothCamera2D>().target = this.transform;
        }
    }
    public bool IsLocalPlayer(){
        return clientNetworkSender.isLocalPlayer;

    }

    public bool IsServer()
    {
        return clientNetworkSender.isServer;
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

    public void MoveRight(){
        characterMove.MovePressed(1);
    }

    public void MoveLeft(){
        characterMove.MovePressed(-1);
    }

    public void SetVerticalDirection(int i)
    {
        charStats.side = new Vector2(charStats.side.x, i);
    }

    public void MoveFinished(Vector3 position){
        characterMove.MoveReleasedServerside(position);
    }

    public void JumpPressed(){
        jump.JumpPressed();
    }

    public void Meleeattack() { }
        

}


