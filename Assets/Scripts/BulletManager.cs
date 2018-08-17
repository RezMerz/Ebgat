using System.Collections;
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

    public void Shoot(Vector2 direction,float gravityAcc, int id)
    {
        var bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        shotBullets.Add(id, bullet);
        bullet.Shoot(direction,layerMask, gravityAcc);
    }
    public void DestroyBullet(int id)
    {
        if (shotBullets.Contains(id))
        {
            GameObject bullet = shotBullets[id] as GameObject;
            shotBullets.Remove(id);
            Destroy(bullet);
        }
    }
}
