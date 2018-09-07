using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControlClientside : MonoBehaviour
{
    public Action ReadyAction;


    //public Sounds heroSounds;
    public GameObject[] runes; 


    public CharacterAttributesClient charStatsClient { get; set; }
    public HeroGraphics heroGraphics { get; private set; }
    public ClientNetworkSender clientNetworkSender { get; private set; }
    public ClientNetworkReciever clientNetworkReciever { get; private set; }
    public PlayerConnection playerConnection;// { get; set; }
    public BulletManager bulletmanager { get; set; }
    public static string teamName { get; set; }
    public CharacterAim aim { get; set; }

    private InputCharacter input;

    private Sounds heroSounds;


    private Hashtable playerStatesHash = new Hashtable();
    public int lastStateChecked;
    private int currentStateNumber;
    private int biggestIdNumber;
    private int frameNumber;

    private int counter;

    private bool start;
    private bool firstRecieved;
    private bool waitingForRequest;
    public int playerId { get; private set; }
    // Use this for initialization
    void Awake()
    {
        clientNetworkReciever = ClientNetworkReciever.instance;
        charStatsClient = GetComponent<CharacterAttributesClient>();
        heroGraphics = GetComponent<HeroGraphics>();
        bulletmanager = GetComponent<BulletManager>();
        aim = GetComponent<CharacterAim>();
        input = GetComponent<InputCharacter>();
        heroSounds = GetComponent<Sounds>();
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
        ReadData();
    }

    public void SetNetworkComponents(PlayerConnection playerConnection, ClientNetworkSender clientNetworkSender, ServerNetwork serverNetworkReciever, int playerId)
    {
        this.playerConnection = playerConnection;
        this.clientNetworkSender = clientNetworkSender;
        this.playerId = playerId;
    }

    public void SetTeam(string teamName, string enemyTeamName)
    {
        if (IsLocalPlayer())
            PlayerControl.teamName = teamName;

        charStatsClient.teamName = teamName;
        charStatsClient.enemyTeamName = enemyTeamName;
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


            //Debug.Log(biggestIdNumber - currentStateNumber +"+"+ Time.frameCount);
            if (biggestIdNumber - currentStateNumber >= 6)
            {

                for (int i = lastStateChecked + 1; i <= biggestIdNumber - counter; i++)
                {
                    if (playerStatesHash.Contains(i))
                    {
                        GetData((string)playerStatesHash[i]);
                        playerStatesHash.Remove(i);
                    }
                }
                lastStateChecked = biggestIdNumber - counter;


                currentStateNumber = lastStateChecked + 1;

                Debug.Log(currentStateNumber);
                counter = (counter - 1) % 3;
                return;
            }

            if (IsLocalPlayer())
            {
                // Debug.Log(currentStateNumber + "+" + biggestIdNumber + "+" + lastStateChecked + " + " + Time.frameCount);
            }
            if (playerStatesHash.Contains(currentStateNumber))
            {

                for (int i = lastStateChecked + 1; i <= currentStateNumber; i++)
                {
                    if (playerStatesHash.Contains(i))
                    {
                        GetData((string)playerStatesHash[i]);
                        playerStatesHash.Remove(i);
                    }
                }
                lastStateChecked = currentStateNumber;
            }
            else if (currentStateNumber > biggestIdNumber && lastStateChecked < biggestIdNumber)
            {
                // Debug.Log("miss 1 fram :" + Time.frameCount);

                for (int i = lastStateChecked + 1; i <= biggestIdNumber; i++)
                {
                    if (playerStatesHash.Contains(i))
                    {
                        GetData((string)playerStatesHash[i]);
                        playerStatesHash.Remove(i);
                    }
                }
                lastStateChecked = biggestIdNumber;
            }
            if (currentStateNumber - lastStateChecked >= 3 && !waitingForRequest)
            {
                if (IsLocalPlayer())
                {
                    waitingForRequest = true;
                    clientNetworkSender.RequestWorldState(playerId, lastStateChecked);
                    Debug.Log("request from : "+ lastStateChecked +" +"+ currentStateNumber   +  "+" + Time.frameCount);
                }
            }
            currentStateNumber++;
            counter = (counter - 1) % 3;
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
        /*catch (UnassignedReferenceException e)
        {
            Debug.Log(data);
        }*/
    }

    // gets the code and value of datas
    private void Deserilize(char code, string value)
    {
        switch (code)
        {
            case 'A': heroGraphics.AbilityState(value); break;
            case 'a': heroGraphics.HeadState(value); break;
            case 'b': heroGraphics.BodyState(value); break;
            case 'c': heroGraphics.HandState(value); break;
            case 'd': heroGraphics.FeetState(value); break;
            case 'e': heroGraphics.SetSide(value); break;
            case 'g': heroGraphics.HpChange(value); break;
            case 'x': if (playerConnection.isLocalPlayer) heroGraphics.EnergyChange(value); break;
            case 'y': heroGraphics.AttackNumber(value); break;
            case 'C': aim.AimClinet(); break;
            case 'z': heroGraphics.SpeedRateChange(value); break;
            case 'f': heroGraphics.ArmorChange(value); break;
            case 'D': heroGraphics.Disarm(value); break;
            case 'E': heroGraphics.RootMark(value); break;
            case 'F': heroGraphics.Root(value); break;
            case 'G': heroGraphics.Aim(value); break;
            case 'B': heroGraphics.RageChange(value); break;
        }
        charStatsClient.SetAttribute(code, value);
        SoundHandle(code, value);
    }

    private void SoundHandle(char code, string value)
    {
        switch (code)
        {
            case 'c': heroSounds.HandState(value); break;
            case 'd': heroSounds.FeetState(value); break;
            case 'A': heroSounds.AbilityState(value); break;
            case 'a': heroSounds.HeadState(value); break;
            case 'b': heroSounds.BodyState(value); break;
        }
    }

    public void AddTOHashTable(int id, string state)
    {
        if (IsLocalPlayer())
        {
           // Debug.Log(id + "+" + Time.frameCount);
        }
        if (!start && (!firstRecieved || currentStateNumber <= id))
        {
            counter = 2;
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

    public void UpdateClient()
    {
        waitingForRequest = false;
        // start = false;
        //// if (id > currentStateNumber)
        //// {
        //     currentStateNumber = id + 1;
        //// }
        // GetData(state);
        // for (int i = lastStateChecked + 1; i <= id; i++)
        // {
        //     if (playerStatesHash.Contains(i))
        //     {
        //         playerStatesHash.Remove(i);
        //     }
        // }
        // lastStateChecked = id;
    }

    public void Die()
    {
        heroGraphics.Die();
        heroSounds.DieSound();
        Debug.Log("hey teacher don't leave these codes alone ");
        input.start = false;
        if (IsLocalPlayer())
        {
            Camera.main.GetComponent<SmoothCamera2D>().UnfollowTarget();
        }
    }

    public void Respawn()
    {
        Debug.Log("hey teache leave these codes alone ");
        input.start = true;
        GetComponent<SpriteRenderer>().enabled = true;
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
            float changeRange = float.Parse(deString[6]);
            bulletmanager.Shoot(attackSide, gravityAcc, id, range, number, startPos, changeRange);
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

    public void GetAdditionalData(string data)
    {
        string[] deString = data.Split('$');
        for (int i = 0; i < deString.Length; i++)
        {
            if (deString[i].Equals(""))
                continue;
            if (deString[i][0] == 'Z')
                Die();
            else if (deString[i][0] == 'Y')
                Respawn();
        }
    }

    public void GetAdditionalWorldData(string data)
    {
        string[] deString = data.Split('$');
        for(int i = 0; i < deString.Length - 1; i++)
        {
            if (deString[i][0] == 'R')
            {
                string[] dataS = deString[i].Split('&');
                int runeNum = int.Parse(dataS[1]);
                Vector2 spawnPos = Toolkit.DeserializeVector(dataS[2]);
                Instantiate(runes[runeNum], spawnPos, Quaternion.identity);
            }
        }
    }

    public void DisconnectedFromServer(){
        
    }
}
