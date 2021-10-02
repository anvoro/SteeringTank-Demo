
using UnityEngine;

/// <summary>
/// Attribute that require implementation of the provided interface.
/// </summary>
public class RequiredInterfaceAttribute : PropertyAttribute
{
    // Interface type.
    public System.Type requiredType { get; private set; }

    /// <summary>
    /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
    /// </summary>
    /// <param name="type">Interface type.</param>
    public RequiredInterfaceAttribute(System.Type type)
    {
        this.requiredType = type;
    }
}
