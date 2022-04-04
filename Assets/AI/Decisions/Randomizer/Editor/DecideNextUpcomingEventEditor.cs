using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DecideNextUpcomingEvent))]
public class DecideNextUpcomingEventEditor : Editor
{
    private DecideNextUpcomingEvent script;
    private GameResourceManager grm;

    private void OnEnable()
    {
        script = (DecideNextUpcomingEvent)target;
        grm = FindObjectOfType<GameResourceManager>();
        var grmDebug = FindObjectOfType<GameResourceManagerDebug>();
        grm.SetDebugValues(grmDebug);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.IsPlaying(script)) { return; }
        if (GUILayout.Button("Decide!"))
        {
            script.Decide();
        }
    }
}