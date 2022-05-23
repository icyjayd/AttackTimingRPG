using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;
 
    // Start is called before the first frame update
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviorTree>();
        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message = "Hello world 1";

        var pause1 = ScriptableObject.CreateInstance<WaitNode>();

        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message = "Hello world 2";

        var pause2 = ScriptableObject.CreateInstance<WaitNode>();

        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message = "Hello world 3";

        var pause3 = ScriptableObject.CreateInstance<WaitNode>();

        var sequence = ScriptableObject.CreateInstance<SequencerNode>();
        sequence.children.Add(log1);
        sequence.children.Add(pause1);
        sequence.children.Add(log2);
        sequence.children.Add(pause2);
        sequence.children.Add(log3);
        sequence.children.Add(pause3);
        var loop = ScriptableObject.CreateInstance<RepeatNode>();

        tree.rootNode = loop;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
