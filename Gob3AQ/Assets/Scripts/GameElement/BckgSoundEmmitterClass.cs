using UnityEngine;
using Gob3AQ.VARMAP.GameEventMaster;
using Gob3AQ.VARMAP.Types;

[System.Serializable]
public class BckgSoundEmmitterClass : MonoBehaviour
{
    [System.Serializable]
    private struct GameEventCombi_prv
    {
        public GameEvent ev;
        public bool not;
    }

    [SerializeField]
    private GameSound soundItem;

    [SerializeField]
    private bool loop;

    [SerializeField]
    private MomentType dayMoment;

    [SerializeField]
    private GameEventCombi_prv[] neededEvents;

    private GameEventCombi[] baked_neededEvents;

    private void Awake()
    {
        baked_neededEvents = new GameEventCombi[neededEvents.Length];
        for(int i = 0; i < neededEvents.Length; i++)
        {
            baked_neededEvents[i] = new GameEventCombi(neededEvents[i].ev, neededEvents[i].not);
        }
    }

    private void Start()
    {
        VARMAP_GameEventMaster.REG_GAMESTATUS(OnGameStatusChanged);
    }



    private void OnDestroy()
    {
        VARMAP_GameEventMaster.UNREG_GAMESTATUS(OnGameStatusChanged);
    }

    private void OnGameStatusChanged(ChangedEventType eventType, in Game_Status oldval, in Game_Status newval)
    {
        if((newval == Game_Status.GAME_STATUS_PLAY)&&(oldval == Game_Status.GAME_STATUS_LOADING))
        {
            if(soundItem != GameSound.SOUND_NONE)
            {
                MomentType currentMoment = VARMAP_GameEventMaster.GET_DAY_MOMENT();
                VARMAP_GameEventMaster.IS_EVENT_COMBI_OCCURRED(baked_neededEvents, out bool eventsOk);

                if (eventsOk && ((dayMoment == MomentType.MOMENT_ANY) || (dayMoment == currentMoment)))
                {
                    VARMAP_GameEventMaster.PLAY_SOUND(soundItem, null, loop);
                }
            }
        }
    }
}
