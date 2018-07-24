using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    public CharacterAttributes charStats { get; private set; }
    public CharacterMove characterMove { get; private set; }
    public PlayerJump jump { get; private set; }
    public Attack attack { get; private set; }

    public HeroGraphics heroGraphics { get; private set; }

    public GameObject bulletPrefab;

    public Color color;

    private BuffManager buffManager;

    // Use this for initialization
    void Awake()
    {
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
        if (isLocalPlayer)
        {
            charStats.teamName = "Team 1";
            charStats.enemyTeamName = "Team 2";
            gameObject.layer = LayerMask.NameToLayer("Team 1");

            //change color for localm player
            color = Color.green;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            charStats.teamName = "Team 2";
            charStats.enemyTeamName = "Team 1";
            gameObject.layer = LayerMask.NameToLayer("Team 2");
            color = Color.white;
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

    private void TakeDamage(float damage)
    {
        heroGraphics.TakeDamage();
        print("Took Damage");
        charStats.hitPoints -= damage;
        if (charStats.hitPoints <= 0)
        {
            print("Dead");
            if(isServer){
                CmdKillPlayer();
            }
        }
    }



    ///////////  NETWORK //////////
    [Command]
    public void CmdKillPlayer(){
        Destroy(gameObject);
    }


    [Command]
    public void CmdMove(int num)
    {
        RpcMove(num);
    }

    [Command]
    public void CmdMoveFinished(Vector3 position)
    {
        RpcMoveFinished(position);
    }

    [Command]
    public void CmdJumpPressed(Vector3 position)
    {
        RpcJumpPressed(position);
    }

    [Command]
    public void CmdJumpHold(Vector3 position)
    {
        RpcJumpHold(position);
    }

    [Command]
    public void CmdJumpReleased(Vector3 position)
    {
        RpcJumpReleased(position);
    }

    [Command]
    public void CmdShootbullet(Vector3 targetdirection, Vector3 origin, float bulletDamage){
        GameObject bulletObj = Instantiate(bulletPrefab);
        NetworkServer.Spawn(bulletObj);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int layer = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
        bullet.Shoot(targetdirection, origin, bulletDamage,layer);
        bullet.RpcShootBulletForClient(targetdirection, origin, bulletDamage,layer);
    }



    [ClientRpc]
    public void RpcMove(int num)
    {
        if (isLocalPlayer)
        {
            return;
        }
        characterMove.MovePressed(num);
    }

    [ClientRpc]
    public void RpcMoveFinished(Vector3 position)
    {
        if (isLocalPlayer)
        {
            return;
        }
        Debug.Log("RPC MoveFINISHED");
        transform.position = position;
    }

    [ClientRpc]
    public void RpcJumpPressed(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpPressed();
    }

    [ClientRpc]
    public void RpcJumpHold(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpHold();
    }

    [ClientRpc]
    public void RpcJumpReleased(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpReleased();
    }

    [ClientRpc]
    public void RpcShootBullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        if (isServer)
        {
            return;
        }
    }

    [ClientRpc]
    public void RpcTakeAttack(float damage,string buffName)
    {
        if (isServer)
            return;
        TakeAttack(damage, buffName);
    }
}


