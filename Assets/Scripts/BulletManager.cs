using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] bulletObjects;
    private Hashtable shotBullets = new Hashtable();
    private int layerMask;
    private PlayerControlClientside playerControl;

    private void Start()
    {
        playerControl = GetComponent<PlayerControlClientside>();
        playerControl.ReadyAction += Initialize;
    }
    private void Initialize()
    {
    }

    public void Shoot(Vector2 direction, float gravityAcc, int id, float range, int number, Vector2 startPos,float changeRange)
    {
        layerMask = LayerMask.GetMask(playerControl.charStatsClient.enemyTeamName, "Blocks");
        if(number == 2)
        {
            layerMask = LayerMask.GetMask(playerControl.charStatsClient.enemyTeamName);
        }
        GameObject bulletObject = Instantiate(bulletObjects[number], transform.position + (Vector3)startPos, Quaternion.identity);
        bulletObject.layer = gameObject.layer;
        shotBullets.Add(id, bulletObject);
        bulletObject.GetComponent<Bullet>().Shoot(direction, layerMask, gravityAcc, range,changeRange);
    }

    public void DestroyBullet(int id)
    {
        if (shotBullets.Contains(id))
        {
            GameObject bullet = shotBullets[id] as GameObject;
            shotBullets.Remove(id);
           // bullet.GetComponent<Bullet>().DestroyAnimation();
            StartCoroutine(BulletDestroy(bullet));
        }
    }

    private IEnumerator BulletDestroy(GameObject bullet)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bullet);
    }
}
