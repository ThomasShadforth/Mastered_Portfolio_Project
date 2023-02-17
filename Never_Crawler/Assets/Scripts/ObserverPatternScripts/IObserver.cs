using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{

    public void OnNotify(CombatActionEnum actionType, CombatActionEnum diceNum = CombatActionEnum.enemy_Died, CombatActionEnum maxDamage = CombatActionEnum.enemy_Died, CombatActionEnum modifier = CombatActionEnum.enemy_Died);

    public void OnNotify(TutorialEnum tutorialEvent);
}
