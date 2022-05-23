using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

public class BehaviorTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }
    BehaviorTree tree;
    public BehaviorTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);

    }

    internal void PopulateView(BehaviorTree tree)
    {
        this.tree = tree;
        Debug.Log(tree);
        DeleteElements(graphElements);
        tree.nodes.ForEach(n => CreateNodeView(n));
    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();

            foreach(var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}]  {type.Name}", (a)=>CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();

            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}]  {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();

            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}]  {type.Name}", (a) => CreateNode(type));
            }
        }
    }
    void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        CreateNodeView(node);

    }
    void CreateNodeView(Node node)
    {

        NodeView nodeView = new NodeView(node);
        AddElement(nodeView);

    }
}
