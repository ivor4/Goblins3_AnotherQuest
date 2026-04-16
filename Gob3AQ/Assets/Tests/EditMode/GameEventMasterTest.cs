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

public class GameEventMasterTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        VARMAP_DataSystem.InitializeVARMAP();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void LoadingTest()
    {
        
    }

    [TearDown]
    public void TearDown()
    {
        VARMAP_DataSystem.ResetVARMAP();
    }
}
