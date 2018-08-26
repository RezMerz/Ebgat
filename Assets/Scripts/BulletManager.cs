﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {
    [SerializeField]
    private Bullet bullet;
    private Hashtable shotBullets = new Hashtable();
    private int layerMask;
    private PlayerControl playerControl;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerControl.ReadyAction += Initialize;
    }
    private void Initialize()
    {
        if (playerControl.IsServer())
        {
            layerMask = LayerMask.GetMask(playerControl.charStats.enemyTeamName, "Blocks");
        }
        else
        {
            layerMask = LayerMask.GetMask(playerControl.charStatsClient.enemyTeamName, "Blocks");
        }
    }

    public void Shoot(Vector2 direction,float gravityAcc, int id,float range)
    {
        Vector2 side;
        if (playerControl.IsServer())
        {
            side = playerControl.charStats.Side;
        }
        else
        {
            side = playerControl.charStatsClient.Side;
        }
        Bullet bullet = Instantiate(this.bullet, transform.position + (Vector3)side * 2 + Vector3.up * 0.5f, Quaternion.identity);
        shotBullets.Add(id, bullet);
        bullet.Shoot(direction,layerMask, gravityAcc,range);
    }
    public void DestroyBullet(int id)
    {
        if (shotBullets.Contains(id))
        {
            Bullet bullet = shotBullets[id] as Bullet;
            shotBullets.Remove(id);
            StartCoroutine(BulletDestroy(bullet));
        }
    }

    private IEnumerator BulletDestroy(Bullet bullet)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bullet.gameObject);
    }
}