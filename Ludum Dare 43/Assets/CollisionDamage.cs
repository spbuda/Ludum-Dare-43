using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType { None, Bullet, Wall, Buff }
public class CollisionDamage : MonoBehaviour
{
	public SoundType sound = SoundType.None;
	public float energyDamage;

}
