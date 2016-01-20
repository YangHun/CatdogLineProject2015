using UnityEngine;
using System.Collections;

public class ObjL1S1TimePlatfromPlant : ObjectPlatformPlant {

    public override void OnStateHealed()
    {
        if(m_PlantState.IsFirstUpdate())
        {
            StartCoroutine(ReturnToUnHealedState());
        }

        base.OnStateHealed();
    }


    private IEnumerator ReturnToUnHealedState()
    {

        yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(3));

        m_PlantState.ChangeState(PlantStates.Deactivating);
        GiveDamage(2);
    }
}
