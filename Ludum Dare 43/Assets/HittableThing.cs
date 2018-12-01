using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HittableThing {
	void HitBy(HittableThing instigator);

	float HitAmount { get; }
}
