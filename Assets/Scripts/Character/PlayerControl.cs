using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class PlayerControl : MonoBehaviour
{
    public CharacterAttributes charStats { get; private set; }
    public CharacterAttributesClient charStatsClient { get; set; }
    public CharacterMove characterMove { get; private set; }
    public PlayerJump jump { get; private set; }
    public CharacterDash dash { get; private set; }
    public Attack attack { get; private set; }
    public HeroGraphics heroGraphics { get; private set; }
    public ServerNetworkSender serverNetworkSender { get; private set; }
    public ClientNetworkSender clientNetworkSender { get; private set; }
    public ClientNetworkReciever clientNetworkReciever { get; private set; }
    public ServerNetwork serverNetworkReciever { get; private set; }
    public WorldState worldState;
    public GameObject bulletPrefab;
    public CharacterPhysic physic { get; private set; }
    public PlayerConnection playerConnection;// { get; set; }

    public BulletManager bulletmanager { get; set; }

    private InputCharacter input;
    public Vector2 deathPoint { get; set; }

    public static string teamName { get; set; }
    public CharacterAim aim { get; set; }
    public Color color;
    private Hashtable playerStatesHash = new Hashtable();
    private int lastStateChecked;
    private int currentStateNumber;
    private int biggestIdNumber;
    private int framCount;
    private bool start;
    private bool firstRecieved;
    private bool waitingForRequest;


    private BuffManager buffManager;
    private AbilityManager abilityManager;

    public Action ReadyAction;
    public int playerId { get; private set; }
    // Use this for initialization
    void Awake()
    {
        physic = GetComponent<CharacterPhysic>();
        clientNetworkReciever = ClientNetworkReciever.instance;
        serverNetworkSender = ServerNetworkSender.instance;
        charStats = GetComponent<CharacterAttributes>();
        charStatsClient = GetComponent<CharacterAttributesClient>();
        heroGraphics = GetComponent<HeroGraphics>();
        characterMove = GetComponent<CharacterMove>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<Attack>();
        buffManager = GetComponent<BuffManager>();
        abilityManager = GetComponent<AbilityManager>();
        input = GetComponent<InputCharacter>();
        bulletmanager = GetComponent<BulletManager>();
        dash = GetComponent<CharacterDash>();
        aim = GetComponent<CharacterAim>();
        deathPoint = new Vector2(16, -48);
    }

    void Start()
    {
        if (playerConnection.isServer)
        {
            ServerManager.instance.UpdatePlayers();
            StartCoroutine(EnergyCycle());
        }
        if (IsLocalPlayer())
        {
            Camera.main.GetComponent<SmoothCamera2D>().target = this.transform;
        }

       
    }

    private void FixedUpdate()
    {
        //worldState.print();
        //Debug.Log(playerConnection + "   " + playerConnection.clientId + "   " + gameObject.GetInstanceID());
        ReadData();
    }

    public void SetNetworkComponents(PlayerConnection playerConnection, ClientNetworkSender clientNetworkSender, ServerNetwork serverNetworkReciever, int playerId)
    {
        this.playerConnection = playerConnection;
        this.clientNetworkSender = clientNetworkSender;
        this.serverNetworkReciever = serverNetworkReciever;
        this.playerId = playerId;
    }

    public void SetTeam(string teamName, string enemyTeamName)
    {
        if (IsLocalPlayer())
            PlayerControl.teamName = teamName;

        if (playerConnection.isServer)
        {
            charStats.teamName = teamName;
            charStats.enemyTeamName = enemyTeamName;
        }
        else
        {
            charStatsClient.teamName = teamName;
            charStatsClient.enemyTeamName = enemyTeamName;
        }
        gameObject.layer = LayerMask.NameToLayer(teamName);
    }

    public void SetReady()
    {
        heroGraphics.CreateHpBar();
        ReadyAction();
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
            if (currentStateNumber - lastStateChecked >= 3 && !waitingForRequest)
            {
                waitingForRequest = true;
               clientNetworkSender.RequestWorldState(playerId);
            }
            currentStateNumber++;
        }

    }

    public bool IsLocalPlayer()
    {
        try
        {
            return playerConnection.isLocalPlayer;
        }
        catch
        {
            return false;
        }
    }

    public bool IsServer()
    {
        try
        {
            return playerConnection.isServer;
        }
        catch
        {
            return false;
        }
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



    private IEnumerator EnergyCycle()
    {
        yield return new WaitForSeconds(0.1f);
        FillEnergy();
        StartCoroutine(EnergyCycle());
    }

    private void FillEnergy()
    {
        if (charStats.Energy < charStats.energyBase)
        {
            if (charStats.Energy < charStats.energyBase / 3)
            {
                charStats.Energy += (int)(charStats.energyRegenRate * 1);
            }
            else if (charStats.Energy < charStats.energyBase * 2 / 3)
            {
                charStats.Energy += (int)(charStats.energyRegenRate * 1.5);
            }
            else
            {
                charStats.Energy += (int)(charStats.energyRegenRate * 2);
            }
        }
        else if (charStats.Energy > charStats.energyBase)
            charStats.Energy = charStats.energyBase;
    }
    public void TakeStun(float time)
    {
        charStats.HeadState = EHeadState.Stunned;
    }
    private void TakeDamage(float damage)
    {
        //heroGraphics.TakeDamage();
        charStats.HitPoints -= damage;
        if (charStats.HitPoints <= 0)
        {
            if (playerConnection.isServer)
            {
                ServerManager.instance.KillHero(playerConnection.clientId);
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

    public void DashPressed()
    {
        dash.DashPressed();
    }

    public void Ability1Pressed()
    {
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
        abilityManager.Ability2Pressed();
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

    public void deltaYAim(float deltaY)
    {
        aim.yChange(deltaY);
    }

    public void deltaXAim(float deltaX)
    {
        aim.XChange(deltaX);
    }

    public void AimPressed()
    {
        aim.AimPressed();
    }

    public void AimReleased()
    {
        aim.AimReleased();
    }

    public void AimController(Vector2 aimAxis)
    {
        aim.ControllerAim(aimAxis);
    }

    public void GetData(string data)
    {

        try
        {
            bool first = true;
            string[] dataSplit = data.Split('$');
            foreach (string dataS in dataSplit)
            {
                string[] deString = dataS.Split('&');
                if (first)
                {
                    first = false;
                    heroGraphics.ChangePosition(Toolkit.DeserializeVector(deString[0]));
                   
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
        catch (MissingReferenceException e)
        {
            Debug.Log(data);
        }
        catch (UnassignedReferenceException e)
        {
            Debug.Log(data);
        }
    }

    // gets the code and value of datas
    private void Deserilize(char code, string value)
    {
        if (!playerConnection.isServer)
            charStatsClient.SetAttribute(code, value);
        switch (code)
        {
            case 'A': heroGraphics.AbilityState(value); break;
            case 'a': heroGraphics.HeadState(value); break;
            case 'b': heroGraphics.BodyState(value); break;
            case 'c': heroGraphics.HandState(value); break;
            case 'd': heroGraphics.FeetState(value); break;
            case 'e': heroGraphics.SetSide(value); break;
            case 'g': heroGraphics.HpChange(value); break;
            case 'x':if(playerConnection.isLocalPlayer) heroGraphics.EnergyChange(value); break;
            case 'y': heroGraphics.AttackNumber(value); break;
        }
    }

    public void AddTOHashTable(int id, string state)
    {
        if (!start && (!firstRecieved || currentStateNumber <= id))
        {
            currentStateNumber = id;
            lastStateChecked = id - 1;
            start = true;
            firstRecieved = true;
        }
        playerStatesHash.Add(id, state);
        if (id > biggestIdNumber)
        {
            biggestIdNumber = id;
        }
    }

    public void UpdateClient(int id, string state)
    {
        waitingForRequest = false;
        start = false;
        if(id > currentStateNumber)
        {
            currentStateNumber = id + 1;
        }
        GetData(state);
        for (int i = lastStateChecked + 1; i <= id; i++)
        {
            if (playerStatesHash.Contains(i))
            {
                playerStatesHash.Remove(i);
            }
        }
        lastStateChecked = id;
    }

    public void Die()
    {
        input.start = false;
        transform.position = deathPoint;
        GetComponent<SpriteRenderer>().enabled = false;
        if (IsServer())
        {
            physic.virtualPosition = transform.position;
        }
        if (IsLocalPlayer())
        {
            Camera.main.GetComponent<SmoothCamera2D>().UnfollowTarget();
        }
    }

    public void Respawn()
    {
        input.start = true;
        GetComponent<SpriteRenderer>().enabled = true;
        transform.position = playerConnection.spawnPoint;
        if (IsServer())
        {
            physic.virtualPosition = transform.position;
            charStats.ResetHP();
        }
        if (IsLocalPlayer())
        {
            Camera.main.GetComponent<SmoothCamera2D>().FollowTarget();
        }
    }

    public void Shoot(string data)
    {
        string[] dataSplit = data.Split('$');
        foreach (string dataS in dataSplit)
        {
            if (dataS.Equals(""))
                continue;
            string[] deString = dataS.Split('&');
            int id = Convert.ToInt32(deString[0]);
            Vector2 attackSide = Toolkit.DeserializeVector(deString[1]);
            float gravityAcc = float.Parse(deString[2]);
            float range = float.Parse(deString[3]);
            int number = int.Parse(deString[4]);
            Vector2 startPos = Toolkit.DeserializeVector(deString[5]);
            bulletmanager.Shoot(attackSide, gravityAcc, id,range,number,startPos);
        }
    }
    public void DestroyBullet(string data)
    {
        string[] dataSplit = data.Split('$');
        foreach (string dataS in dataSplit)
        {
            if (dataS.Equals(""))
                continue;
            int id = Convert.ToInt32(dataS);
            bulletmanager.DestroyBullet(id);
        }
    }
}



