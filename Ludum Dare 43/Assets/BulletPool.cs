using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (menuName = "Scriptables/Bullet Pool")]
public class BulletPool : ScriptableObject {
	private static readonly string ASSETPATH = "Assets/Scriptables/BulletPool.asset";
	private static BulletPool _inst;
	public static BulletPool Instance {
		get {
			if (!_inst)
				_inst = Tools.Teyrutils.GetAsset<BulletPool> (ASSETPATH);
			return _inst;
		}
	}

	[SerializeField]
	private Bullet BulletPrefab;
	[SerializeField]
	private int PoolSize;

	private Queue<Bullet> pool;

	public Bullet Next(float speed, float damage, float lifetime, Vector2 position, Vector2 target) {
		Bullet bullet = null;
		if(pool == null) {
			pool = new Queue<Bullet> (PoolSize);
		}

		if (pool.Count > 0) {
			if(!pool.Peek ().Active || pool.Count >= PoolSize) {
				bullet = pool.Dequeue ();
				if (!bullet.gameObject.activeInHierarchy) {
					bullet.gameObject.SetActive (true);
				}
			} else {
				bullet = Instantiate (BulletPrefab);
				DontDestroyOnLoad (bullet);
			}
		} else {
			bullet = Instantiate (BulletPrefab);
			DontDestroyOnLoad (bullet);
		}

		pool.Enqueue (bullet);
		bullet.Init (speed, damage, lifetime, position, target);
		return bullet;
	}

	public void ResetAll() {
		if(pool != null) {
			foreach(Bullet bullet in pool) {
				bullet.Detonate ();
			}
		}
	}
}
