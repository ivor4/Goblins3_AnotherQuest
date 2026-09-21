using Gob3AQ.ItemMaster;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.GameElement.Extension.RustUnitsGenerator
{
    public class RustUnitsGeneratorClass : MonoBehaviour, IGameElementExtension
    {
        private enum State
        {
            STATE_IDLE,
            STATE_GENERATING,
            STATE_COMPLETING
        }
        private const int MAX_UNITS = 64;
        private GameObject prefab;
        private List<GameObject> unit_pool;
        private List<float> x_delta;
        private State state;
        private int generatedUnits;
        private int completedUnits;
        private ulong lastTimestamp;

        
        private void Awake()
        {
            unit_pool = new List<GameObject>(MAX_UNITS);
            x_delta = new List<float>(MAX_UNITS);
        }

        private void Start()
        {
            prefab = ResourceAtlasClass.GetPrefab(PrefabEnum.PREFAB_RUST_UNIT);
            state = State.STATE_IDLE;

            VARMAP_ItemMaster.REG_GAMESTATUS(ChangedGameStatus);
        }

        private void Update()
        {
            if(state == State.STATE_GENERATING)
            {
                if (generatedUnits < MAX_UNITS)
                {
                    ulong timestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();

                    if ((timestamp - lastTimestamp) > 60)
                    {
                        GameObject unit = unit_pool[generatedUnits];
                        unit.SetActive(true);
                        generatedUnits++;
                        lastTimestamp = timestamp;
                    }
                }
                else
                {
                    state = State.STATE_COMPLETING;
                }
            }

            if(state is State.STATE_GENERATING or State.STATE_COMPLETING)
            {
                for(int i = completedUnits; i < generatedUnits; ++i)
                {
                    GameObject obj = unit_pool[i];

                    if (obj.transform.position.y > -4f)
                    {
                        float addAngle;

                        if ((i & 1) == 0)
                        {
                            addAngle = 1f;
                        }
                        else
                        {
                            addAngle = -1f;
                        }

                        obj.transform.localEulerAngles += new Vector3(0f, 0f, addAngle);

                        obj.transform.position += new Vector3(x_delta[i], -3f, 0f) * Time.deltaTime;
                    }
                    else
                    {
                        ++completedUnits;
                    }
                }

                if(completedUnits == MAX_UNITS)
                {
                    state = State.STATE_IDLE;
                }
            }
        }

        public void OnDespawn()
        {
            for(int i=0; i < unit_pool.Count; ++i)
            {
                GameObject obj = unit_pool[i];
                obj.SetActive(false);
            }
        }

        public void OnExtensionDestroy()
        {
            VARMAP_ItemMaster.UNREG_GAMESTATUS(ChangedGameStatus);
        }

        public void OnSpawn()
        {
            state = State.STATE_GENERATING;
            generatedUnits = 0;
            completedUnits = 0;
            lastTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
        }

        private void ChangedGameStatus(ChangedEventType eventType, in Game_Status oldval, in Game_Status newval)
        {
            _ = oldval;
            _ = eventType;

            if(newval == Game_Status.GAME_STATUS_LOADING)
            {
                for (int i = 0; i < MAX_UNITS; ++i)
                {
                    GameObject unit = Instantiate(prefab, transform.position, Quaternion.Euler(0f, 0f, 180f*((Random.value*2f) - 1f)));
                    unit.name = $"RustUnit_{i}";
                    float scale = (Random.value * 0.75f) + 0.25f;
                    unit.transform.localScale = new Vector3(scale * unit.transform.localScale.x, scale * unit.transform.localScale.y, 1.0f);
                    unit.SetActive(false);
                    unit_pool.Add(unit);
                    x_delta.Add((Random.value*2f - 1f)*0.5f);
                }
            }
        }

        public void OnReceiveExtFn(ItemExtensionFunction extFn, in NumberPack numberPack)
        {
            _ = extFn;
            _ = numberPack;
        }
    }
}
