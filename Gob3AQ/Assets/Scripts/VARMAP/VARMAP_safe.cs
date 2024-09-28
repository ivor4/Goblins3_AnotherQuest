using System.Collections.Generic;
using UnityEngine;
using MVerse.VARMAP.Config;

namespace MVerse.VARMAP.Safe
{

    public abstract class VARMAP_Safe : VARMAP
    {
        /// <summary>
        /// Total allocated variables
        /// </summary>
        private static uint usedBinSlots;

        /// <summary>
        /// Transcription from Safe ID to used slot index
        /// </summary>
        private static uint[] id_to_slot;

        /// <summary>
        /// Transcription from Safe ID to last updated tick (to avoid several checks in same variable in same tick)
        /// </summary>
        private static uint[] id_checked_in_tick;

        /// <summary>
        /// FATALERROR of safety (overwritten value)
        /// </summary>
        private static bool FATALERROR;

        /// <summary>
        /// Actual tick random value
        /// </summary>
        private static uint actualTick;

        /// <summary>
        /// Free slots (will be removed and added upon updates)
        /// </summary>
        private static List<uint> free_slots;

        /// <summary>
        /// Call once per program Execution
        /// </summary>
        public static void InitializeSafety()
        {
            FATALERROR = false;
            int totalIds = (int)VARMAP_Config.VARMAP_SAFE_VARIABLES;
  
            Random.InitState(System.DateTime.Now.Millisecond);

            actualTick = (uint)Random.Range(1, int.MaxValue);

            if ((VARMAP_Config.VARMAP_SAFE_RUBISH_BIN_SIZE & (VARMAP_Config.VARMAP_SAFE_RUBISH_BIN_SIZE - 1U)) != 0U)
            {
                throw new System.Exception("Total Bin size is not power of 2");
            }

            if (VARMAP_Config.VARMAP_SAFE_RUBISH_BIN_SIZE > (VARMAP_Config.VARMAP_SAFE_VARIABLES + 1))
            {
                id_to_slot = new uint[totalIds];
                id_checked_in_tick = new uint[totalIds];
                usedBinSlots = 0;
                free_slots = new List<uint>((int)VARMAP_Config.VARMAP_SAFE_RUBISH_BIN_SIZE);
                

                for (uint i = 0; i < totalIds; i++)
                {
                    id_to_slot[i] = 0xFFFFFFFF;
                    id_checked_in_tick[i] = 0;
                }

                for (uint i = 0; i < VARMAP_Config.VARMAP_SAFE_RUBISH_BIN_SIZE; i++)
                {
                    RUBISH_BIN[i] = (uint)Random.Range(int.MinValue, int.MaxValue);
                    free_slots.Add(i);
                }
            }
            else
            {
                throw new System.Exception("Configured Safe bin size is <= Safe variables + 1");
            }


        }

        public static uint RegisterSecureVariable()
        {
            uint candidateSlot;
            int candidateSlotIndex;
            uint newID;

            if (id_to_slot.Length > 0)
            {
                if (usedBinSlots < VARMAP_Config.VARMAP_SAFE_VARIABLES)
                {
                    uint randomnum = (uint)Random.Range(int.MinValue, int.MaxValue);

                    candidateSlotIndex = (int)(randomnum % free_slots.Count);
                    candidateSlot = free_slots[candidateSlotIndex];

                    RUBISH_BIN[candidateSlot] = randomnum;
                    free_slots.RemoveAt(candidateSlotIndex);
                    id_to_slot[usedBinSlots] = candidateSlot;
                    id_checked_in_tick[usedBinSlots] = 0;
                    newID = usedBinSlots++;
                }
                else
                {
                    throw new System.Exception("Exceeded number of registered safe variables");
                }
            }
            else
            {
                throw new System.Exception("Uninitialized Safety VARMAP module before registering variables");
            }

            return newID;
        }

        public static void SecureNewValue(uint id, uint newValue, bool highSecurity)
        {
            uint oldslotu;
            uint candidateSlot;
            uint securedValue;
            uint randomval;
            int newslotindex;


            oldslotu = id_to_slot[id];

            randomval = (uint)Random.Range(int.MinValue, int.MaxValue);

            /* Get a new slot position in memory for new value */
            newslotindex = (int)(randomval % free_slots.Count);
            candidateSlot = free_slots[newslotindex];

            /* Remove from free slots */
            free_slots.RemoveAt(newslotindex);

            /* Get this dirty */
            securedValue = randomval;
            securedValue &= ~0x5A0A5991U;
            securedValue |= 0x5A0A5991U & newValue;

            /* Re-allocate in other place */
            RUBISH_BIN[oldslotu] = randomval^(~RUBISH_BIN[oldslotu]);
            RUBISH_BIN[candidateSlot] = securedValue;
            id_to_slot[id] = candidateSlot;
            id_checked_in_tick[id] = 0;

            /* Add to free slots */
            free_slots.Add(oldslotu);

            /* Change another different value in order to loggers see three involved variables */
            if (highSecurity)
            {
                newslotindex = (int)((randomval^oldslotu) % free_slots.Count);
                candidateSlot = free_slots[newslotindex];
                RUBISH_BIN[candidateSlot] = securedValue^(~RUBISH_BIN[candidateSlot]);
            }
        }

        public static bool CheckSafeValue(uint safeID, uint storedValue)
        {
            uint actualSlot;
            uint securedValue;
            uint maskedOrigin;
            uint maskedDest;
            bool safeOk;

            actualSlot = id_to_slot[safeID];

            securedValue = RUBISH_BIN[(int)actualSlot];

            maskedOrigin = securedValue & 0x5A0A5991U;
            maskedDest = 0x5A0A5991U & storedValue;


            if (maskedOrigin == maskedDest)
            {
                safeOk = true;
                id_checked_in_tick[safeID] = actualTick;
            }
            else
            {
                FATALERROR = true;
                throw new System.Exception("Corrupted safe value");
            }

            return safeOk;
        }

        public static bool IsSafeVariableCheckedInTick(uint safeID)
        {
            bool retVal;

            if(id_checked_in_tick[safeID] == actualTick)
            {
                retVal = true;
            }
            else
            {
                retVal = false;
            }

            return retVal;
        }

        public static void IncrementTick()
        {
            actualTick = (uint)Random.Range(1, int.MaxValue);
        }
    }
}
