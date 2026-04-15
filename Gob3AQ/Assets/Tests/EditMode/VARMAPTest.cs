using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Gob3AQ.VARMAP;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Initialization;
using Gob3AQ.VARMAP.Safe;
using Gob3AQ.VARMAP.Debug;
using Gob3AQ.VARMAP.Enum;
using Gob3AQ.VARMAP.Variable;
using System;

public class VARMAPTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        VARMAP_DataSystem.InitializeVARMAP();
    }

    // A Test behaves as an ordinary method
    [Test]
    [TestCase(VARMAP_Variable_ID.VARMAP_ID_TOTAL)]
    public void CreationTest(int expected_elems)
    {
        Assert.AreEqual(VARMAP_Debug.GetRef().Length,expected_elems, $"VARMAP structure length is not {expected_elems}, it is {VARMAP_Debug.GetRef().Length} instead");
    }

    [Test]
    [TestCase(Room.HIVE1_WC_1)]
    [TestCase(Room.CITY1_SOUTH_STREET_2)]
    public void HackActualRoomValue(Room newValue)
    {
        var VarmapInst = (VARMAP_SafeVariable<Room>)VARMAP_Debug.GetRef()[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM];
        Room newRoom = VarmapInst.GetValue();
        VarmapInst.SetValueIllegal(in newValue);
        VARMAP_Safe.IncrementTick();
        VARMAP_Variable_Indexable.CommitPending();


        Assert.Throws<Exception>(() => newRoom = VarmapInst.GetValue(), $"VARMAP_Safe did allow setting an illegal value");
        Assert.AreNotEqual(newRoom, newValue, $"VARMAP_Safe did not prevent the illegal value change, expected {newValue} but got {newRoom}");
        Assert.True(VARMAP_Safe.IsInFatalError(), "VARMAP_Safe did not enter fatal error state after illegal value change");
    }

    [Test]
    [TestCase(GameEvent.EVENT_FIRST, true)]
    [TestCase(GameEvent.EVENT_CARDS_PICKABLE_TAKEN, false)]
    public void HackActualEventValue(GameEvent game_event, bool occurred)
    {
        var VarmapInst = (VARMAP_SafeArray<MultiBitFieldStruct>)VARMAP_Debug.GetRef()[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED];
        int index = (int)game_event >> 6;

        MultiBitFieldStruct actualValue = VarmapInst.GetListElem(index);
        MultiBitFieldStruct expectedValue = actualValue;
        expectedValue.SetIndividualBool((int)game_event, occurred);
        

        if (!occurred)
        {
            MultiBitFieldStruct preSetValue = expectedValue;
            preSetValue.SetIndividualBool((int)game_event, true);
            VarmapInst.SetListElem(index, in preSetValue);
            VARMAP_Safe.IncrementTick();
            VARMAP_Variable_Indexable.CommitPending();
            actualValue = VarmapInst.GetListElem(index);
        }

        VarmapInst.SetListElemIllegal(index, in expectedValue);
        VARMAP_Safe.IncrementTick();
        VARMAP_Variable_Indexable.CommitPending();
        


        Assert.Throws<Exception>(() => actualValue = VarmapInst.GetListElem(index), $"VARMAP_Safe did allow setting an illegal value");
        Assert.AreNotEqual(actualValue, expectedValue, $"VARMAP_Safe did not prevent the illegal value change, expected {expectedValue} but got {actualValue}");
        Assert.True(VARMAP_Safe.IsInFatalError(), "VARMAP_Safe did not enter fatal error state after illegal value change");
    }

    [TearDown]
    public void TearDown()
    {
        VARMAP_DataSystem.ResetVARMAP();
    }
}
