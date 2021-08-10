using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// Component to identify Characters
/// </summary>
[Serializable]
[GenerateAuthoringComponent]
public struct CharaterData : IComponentData
{
    /// <summary>
    /// Speed of character movement
    /// </summary>
    public float movementSpeed;
    /// <summary>
    /// Speeed of rotation speed
    /// </summary>
    public float rotationSpeed;
    /// <summary>
    /// The current character is Alive
    /// </summary>
    public bool isAlive;
}
