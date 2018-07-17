using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    public CharacterAttributes charStats { get; private set; }
    public Moveable characterMove { get; private set; }
    public PlayerJump jump { get; private set; }
    public Attack attack { get; private set; }

    public GameObject bulletPrefab;

    // Use this for initialization
    void Awake()
    {
        charStats = GetComponent<CharacterAttributes>();
        characterMove = GetComponent<Moveable>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<Attack>();
        Camera.main.GetComponent<Camera_Follow>().player_ = gameObject;
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
        charStats.hitPoints -= damage;
        if (charStats.hitPoints <= 0)
        {
            //Death
            print("Dead");
        }
    }



    ///////////  NETWORK //////////

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
    public void CmdJump(Vector3 position)
    {
        RpcJump(position);
    }

    [Command]
    public void CmdShootbullet(Vector3 targetdirection, Vector3 origin, float bulletDamage){
        GameObject bullet = Instantiate(bulletPrefab);
        NetworkServer.Spawn(bullet);
        bullet.GetComponent<Bullet>().Shoot(targetdirection, origin, bulletDamage);
        //RpcShootBullet(targetdirection, origin, bulletDamage);
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

        transform.position = position;
    }

    [ClientRpc]
    public void RpcJump(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpPressed();
    }

    [ClientRpc]
    public void RpcShootBullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        if (isServer)
        {
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<Bullet>().Shoot(targetdirection, origin, bulletDamage);
    }

    /*[ClientRpc]
    public void RpcBulletHit(Bullet bullet){
        
    }*/
 
}


