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
    public PlayerConnection playerConnection { get; set; }

    public Color color;
    private Hashtable playerStatesHash = new Hashtable();
    private int lastStateChecked ;
    private int currentStateNumber ;
    private int biggestIdNumber;
    private int  framCount;
    private bool start;
    private bool firstRecieved;

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
        /*foreach(GameObject obj in GameObject.FindGameObjectsWithTag("PlayerConnection")){
            PlayerConnection p = obj.GetComponent<PlayerConnection>();
            if(p.isLocalPlayer){
                playerConnection = p;
                break;
            }
        }*/
    }

    void Start()
    {
        if (IsLocalPlayer())
        {
            Camera.main.GetComponent<SmoothCamera2D>().target = this.transform;
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(playerConnection + "   " + playerConnection.clientId + "   " + gameObject.GetInstanceID());
        counter++;
        ReadData();
    }

    private void ReadData()
    {
        if (start)
        {
            if (playerStatesHash.Contains(currentStateNumber))
            {
                for (int i = lastStateChecked + 1; i <= currentStateNumber; i++)
                {
                    GetData((string)playerStatesHash[i]);
                    playerStatesHash.Remove(i);
                }
                lastStateChecked = currentStateNumber;
            }
            else if (currentStateNumber > biggestIdNumber && lastStateChecked < biggestIdNumber)
            {
                for (int i = lastStateChecked + 1; i <= biggestIdNumber; i++)
                {
                    GetData((string)playerStatesHash[i]);
                    playerStatesHash.Remove(i);
                }
                lastStateChecked = biggestIdNumber;
            }
            if(currentStateNumber - lastStateChecked >= 3)
            {
                serverNetwork.CmdSendWorldStateToClient(clientNetworkSender.PlayerID);
            }
            currentStateNumber++;
        }

    }

    public bool IsLocalPlayer()
    {
        return playerConnection.isLocalPlayer;

    }

    public bool IsServer()
    {
        return playerConnection.isServer;
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
            if (clientNetworkSender.isServer)
            {
                serverNetwork.CmdKillPlayer();
            }
        }
    }

    public void MoveRight()
    {
        characterMove.MovePressed(1);
    }

    public void MoveLeft()
    {
        characterMove.MovePressed(-1);
    }

    public void SetVerticalDirection(int i)
    {
        charStats.AimSide = new Vector2(charStats.AimSide.x, i);
        charStats.Side = new Vector2(charStats.Side.x, i);
    }

    public void MoveFinished(Vector3 position)
    {

        characterMove.MoveReleasedServerside(position);
    }

    public void JumpPressed()
    {
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

    public void GetData(string data)
    {

        try{
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
        catch(MissingReferenceException e){
            Debug.Log(data);
        }
        catch(UnassignedReferenceException e){
            Debug.Log(data);
        }
    }

    // gets the code and value of datas
    private void Deserilize(char code, string value)
    {
        switch (code)
        {
            case 'A': heroGraphics.AbilityState(value); break;
            case 'b': heroGraphics.BodyState(value); break;
            case 'c': heroGraphics.HandState(value); break;
            case 'd': heroGraphics.FeetState(value); break;
            case 'e': heroGraphics.SetSide(value); break;
        }
    }


    int counter;
    public void AddTOHashTable(int id, string state)
    {
        if (!start &&(!firstRecieved || currentStateNumber <= id))
        {
            counter = id;
            currentStateNumber = id;
            lastStateChecked = id - 1;
            start = true;
            firstRecieved = true;
        }
        playerStatesHash.Add(id, state);
        if(id > biggestIdNumber)
        {
            biggestIdNumber = id;
        }
    }

    public void UpdateClient(int id,string state)
    {
        start = false;
        currentStateNumber = id+1;
        GetData(state);
        for(int i = lastStateChecked+1;i <= id; i++)
        {
            if (playerStatesHash.Contains(i))
            {
                playerStatesHash.Remove(i);
            }
        }
        lastStateChecked = id;
    }

}



