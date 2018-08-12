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
    public CharacterPhysic physic { get; private set; }

    public Color color;

    private BuffManager buffManager;
    private AbilityManager abilityManager;
    // Use this for initialization
    void Awake()
    {
        physic = GetComponent<CharacterPhysic>();
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
        abilityManager = GetComponent<AbilityManager>();
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
            print(buffName);
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
        charStats.AimSide = new Vector2(charStats.AimSide.x,i);
        charStats.Side = new Vector2(charStats.Side.x, i);
    }

    public void MoveFinished(Vector3 position){
            
        characterMove.MoveReleasedServerside(position);
    }

    public void JumpPressed(){
        jump.JumpPressed();
    }

    public void JumpReleased()
    {
        jump.JumpReleased();
    }

    public void Ability1Pressed()
    {
        print("Ability 1");
        abilityManager.Ability1Pressed();
    }

    public void Ability1Hold()
    {
        
    }
    public void Ability1Released()
    {
        
    }
    public void Ability2Pressed()
    {
        
    }

    public void Ability2Hold()
    {
        
    }

    public void Ability2Released()
    {
        
    }

    public void AttackPressed()
    {
        attack.AttackPressed();
    }

    public void AttackReleased()
    {
        attack.AttackReleased();
    }
    public void DropDownPressed()
    {
        physic.ExcludeBridge();
    } //16

    public void DropDownReleased()
    {
        physic.IncludeBridge();
    }//17

    public void GetData(string data){
        PrintData(data);
    }

    private void PrintData(string data)
    {
        bool first = true;
        string[] dataSplit = data.Split('$');
        foreach (string dataS in dataSplit)
        {
            string[] deString = dataS.Split('&');
            if (first)
            {
                first = false;
                transform.position = Toolkit.DeserializeVector(deString[0]);
            }
            else
            {
                if (deString.Length > 1)
                {
                    Deserilize(deString[0].ToCharArray()[0], deString[1]);
                }
            }            
        }


    }

    // gets the code and value of datas
    private void Deserilize(char code,string value)
    {
        switch (code)
        {
            case 'b': heroGraphics.BodyState(value); break;
            case 'd': heroGraphics.FeetState(value); break;
            case 'e': heroGraphics.SetSide(value); break;
        }
    }

    

}


