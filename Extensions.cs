using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ThunderRoad;
using ThunderRoad.Pools;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static ThunderRoad.FileManager;
using Action = System.Action;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Type = System.Type;

internal static class Methods {
    /// <summary>
    /// Gets a entity's ragdoll part (selectable).
    /// </summary>
    /// <param name="creature">The creature to get the ragdoll part from.</param>
    /// <param name="ragdollPartType">The type of ragdoll part to get.</param>
    /// <returns>The requested ragdoll part.</returns>
    public static RagdollPart GetRagdollPart(this Creature creature, RagdollPart.Type ragdollPartType) {
        return creature?.ragdoll?.GetPart(ragdollPartType);
    }

    /// <summary>
    /// Gets a entity's head ragdoll.
    /// </summary>
    /// <param name="creature">The creature to get the head ragdoll part from.</param>
    /// <returns>The head ragdoll part.</returns>
    public static RagdollPart Head(this Creature creature) {
        return creature?.GetRagdollPart(RagdollPart.Type.Head);
    }

    /// <summary>
    /// Get or add a component to a game object.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    /// <param name="gameObject">The game object to get or add the component to.</param>
    /// <returns>The component of type T.</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        return gameObject?.GetComponent<T>() ?? gameObject?.AddComponent<T>();
    }
}