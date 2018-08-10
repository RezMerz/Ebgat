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
    public ServerNetworkSender serverNetworkSender { get; private set; }
    public ClientNetworkSender clientNetworkSender { get; private set; }
    public ClientNetworkReciever clientNetworkReciever { get; private set; }
    public ServerNetwork serverNetwork { get; private set; }
    public WorldState worldState;
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
    public void TakeStun(float time)
    {
        charStats.HeadState = EHeadState.Stunned;
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
        charStats.Side = new Vector2(charStats.Side.x, i);
    }

    public void MoveFinished(Vector3 position){
        characterMove.MoveReleasedServerside(position);
    }

    public void JumpPressed(){
        jump.JumpPressed();
    }

    public void RangedAttack(Vector2 attackDir){
        attack.AttackServerside(attackDir);
    }

    public void Meleeattack(Vector2 attackDir) {
        attack.AttackServerside(attackDir);
    }
        

    public void GetData(string data){
        PrintData(data);
    }

    private void PrintData(string data)
    {
        string[] dataSplit = data.Split('$');
        foreach (string dataS in dataSplit)
        {
            print(dataS);
        }
    }

}


