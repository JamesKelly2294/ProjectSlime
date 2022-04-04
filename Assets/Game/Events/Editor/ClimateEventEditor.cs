using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ClimateEvent))]
public class ClimateEventEditor : Editor
{
    private GameResourceManager _grm;
    private ClimateEvent _script;

    private void OnEnable()
    {
        _grm = FindObjectOfType<GameResourceManager>();
        _script = (ClimateEvent)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(_grm != null)
        {
            using (var v = new EditorGUILayout.VerticalScope(""))
            {
                EditorGUILayout.LabelField("== Debug ==");
                foreach (var requirement in _script.Requirements)
                {
                    var value = _grm.GetValueForResourceType(requirement.AffectedResource, requirement.Mode, requirement.IsOneShotResourceType());
                    EditorGUILayout.LabelField(requirement.ToString() + " [" + requirement.IsMet(value) + "]");
                }
            }
        }
    }

}
