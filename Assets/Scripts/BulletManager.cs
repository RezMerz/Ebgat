using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] bulletObjects;
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

    public void Shoot(Vector2 direction,float gravityAcc, int id,float range,int number,Vector2 startPos)
    {
        Vector2 side;
        if (playerControl.IsServer())
        {
            side = playerControl.charStats.Side;
        }
        else
        {
            side = playerControl.charStatsClient.side;
        }
        GameObject bulletObject = Instantiate(bulletObjects[number], transform.position + (Vector3)startPos, Quaternion.identity);
        bulletObject.layer = gameObject.layer;
        shotBullets.Add(id, bulletObject);
        bulletObject.GetComponent<Bullet>().Shoot(direction,layerMask, gravityAcc,range);
    }
    public void DestroyBullet(int id)
    {
        if (shotBullets.Contains(id))
        {
            GameObject bullet = shotBullets[id] as GameObject;
            shotBullets.Remove(id);
            bullet.GetComponent<Bullet>().DestroyAnimation();
            StartCoroutine(BulletDestroy(bullet));
        }
    }

    private IEnumerator BulletDestroy(GameObject bullet)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bullet);
    }
}
