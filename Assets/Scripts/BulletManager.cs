using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {
    [SerializeField]
    private GameObject bulletPrefab;
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
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        shotBullets.Add(id, bullet);
        bulletPrefab.GetComponent<Bullet>().Shoot(direction,layerMask, gravityAcc);
    }
    public void DestroyBullet(int id)
    {
        if (shotBullets.Contains(id))
        {
            GameObject bullet = shotBullets[id] as GameObject;
            Debug.Log(bullet);
            shotBullets.Remove(id);
            Destroy(bullet);
        }
    }
}
