using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    private ObjectPool<Bullet> pool;
    public ObjectPool<Bullet> Pool
    {
        get
        {
            if (pool is null)
                pool = new ObjectPool<Bullet>(OnCreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet);
            return pool;
        }
    }

    [SerializeField]
    private Bullet bulletPrefab;

    Bullet OnCreateBullet()
    {
        Bullet newBullet = Instantiate(bulletPrefab);
        newBullet.BulletPool = this;
        return newBullet;
    }

    void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
