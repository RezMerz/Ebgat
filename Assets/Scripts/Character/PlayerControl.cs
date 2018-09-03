using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class PlayerControl : MonoBehaviour
{
    public CharacterAttributes charStats { get; private set; }
    public CharacterMove characterMove { get; private set; }
    public PlayerJump jump { get; private set; }
    public CharacterDash dash { get; private set; }
    public Attack attack { get; private set; }
    public ServerNetworkSender serverNetworkSender { get; private set; }
    public ClientNetworkSender clientNetworkSender { get; private set; }
    public ClientNetworkReciever clientNetworkReciever { get; private set; }
    public ServerNetwork serverNetworkReciever { get; private set; }
    public WorldState worldState;
    public CharacterPhysic physic { get; private set; }
    public PlayerConnection playerConnection;// { get; set; }
    public Vector2 deathPoint { get; set; }
    public static string teamName { get; set; }
    public CharacterAim aim { get; set; }

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
        characterMove = GetComponent<CharacterMove>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<Attack>();
        buffManager = GetComponent<BuffManager>();
        abilityManager = GetComponent<AbilityManager>();
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

        charStats.teamName = teamName;
        charStats.enemyTeamName = enemyTeamName;
        gameObject.layer = LayerMask.NameToLayer(teamName);
    }

    public void SetReady()
    {
        ReadyAction();
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
                Die();
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

    public void Die()
    {
        Debug.Log("Warning INCOMING POKH");
        transform.position = deathPoint;
        charStats.ResetStats();
        buffManager.DebuffAllCharacter();
    }

    public void Respawn()
    {
        Debug.Log("Warning INCOMING POKH");
        transform.position = playerConnection.spawnPoint;
    }
}



