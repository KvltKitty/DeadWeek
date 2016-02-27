using UnityEngine;
using System.Collections;
[System.Serializable]
public class Ability {
	public enum AbilityType{
		throwBall,
		setRadio
	};
	public AbilityType abilityType;
}
