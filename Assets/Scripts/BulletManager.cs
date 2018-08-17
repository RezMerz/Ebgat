using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {
    [SerializeField]
    private Bullet bullet;
    private Hashtable shotBullets = new Hashtable();
    
    public void Shoot(Vector2 direction, int layer, float gravityAcc, float range, int id)
    {
        var bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        shotBullets.Add(id, bullet);
        bullet.Shoot(direction, layer, gravityAcc, range);
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
