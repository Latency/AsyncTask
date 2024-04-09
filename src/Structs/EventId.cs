// ****************************************************************************
// Project:  AsyncTask
// File:     EventId.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

#if !NET5_0_OR_GREATER && !NETSTANDARD2_0_OR_GREATER && !NET461_OR_GREATER

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging;

/// <summary>
///     Initializes an instance of the Microsoft.Extensions.Logging.EventId struct.
///
///     Identifies a logging event.
///
///     The primary identifier is the "Id" property, with the "Name" property
///     providing a short description of this type of event.
/// </summary>
/// <param name="id">The numeric identifier for this event.</param>
/// <param name="name">The name of this event.</param>
public readonly struct EventId(int id, string? name = null)
{
    /// <summary>
    ///     Gets the numeric identifier for this event.
    /// </summary>
    public int Id { get; } = id;

    /// <summary>
    ///     Gets the name of this event.
    /// </summary>
    public string? Name { get; } = name;

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    ///     Two events are equal if they have the same id.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see cref="bool"/> - true if the current object is equal to the other parameter; otherwise, false.</returns>
    public bool Equals(EventId other) => this == other;

    /// <summary>
    ///     Equals
    /// </summary>
    /// <param name="obj"></param>
    /// <returns><see cref="bool"/> - true if the current object is equal to the other parameter; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is not null && this == (EventId)obj;

    /// <summary>
    ///     GetHashCode
    /// </summary>
    /// <returns><see cref="int"/> - gets the hash code.</returns>
    // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    ///     ToString
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public override string ToString() => Name ?? nameof(EventId);

    /// <summary>
    ///     Checks if two specified Microsoft.Extensions.Logging.EventId instances have the same value.
    ///     They are equal if they have the same Id.
    /// </summary>
    /// <param name="left">The first Microsoft.Extensions.Logging.EventId.</param>
    /// <param name="right">The second Microsoft.Extensions.Logging.EventId.</param>
    /// <returns><see cref="bool"/> - true if the objects are equal.</returns>
    public static bool operator ==(EventId left, EventId right) => left.Name == right.Name && left.Id == right.Id;

    /// <summary>
    ///     Checks if two specified Microsoft.Extensions.Logging.EventId instances have different values.
    /// </summary>
    /// <param name="left">The first Microsoft.Extensions.Logging.EventId.</param>
    /// <param name="right">The second Microsoft.Extensions.Logging.EventId.</param>
    /// <returns><see cref="bool"/> - true if the objects are not equal.</returns>
    public static bool operator !=(EventId left, EventId right) => left.Name != right.Name || left.Id != right.Id;

    /// <summary>
    ///     Implicitly creates an EventId from the given <see cref="System.Int32"/>.
    /// </summary>
    /// <param name="i">The <see cref="System.Int32"/> to convert to an EventId.</param>
    public static implicit operator EventId(int i) => new(i, nameof(EventId));
}
#endif