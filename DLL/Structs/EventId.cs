#if !NET5_0_OR_GREATER
#nullable enable


// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
    //
    // Summary:
    //     Identifies a logging event. The primary identifier is the "Id" property, with
    //     the "Name" property providing a short description of this type of event.
    public readonly struct EventId
    {
        //
        // Summary:
        //     Initializes an instance of the Microsoft.Extensions.Logging.EventId struct.
        //
        // Parameters:
        //   id:
        //     The numeric identifier for this event.
        //
        //   name:
        //     The name of this event.
        public EventId(int id, string? name = null)
        {
            Id   = id;
            Name = name;
        }

        //
        // Summary:
        //     Gets the numeric identifier for this event.
        public int Id { get; }
        //
        // Summary:
        //     Gets the name of this event.
        public string? Name { get; }

        //
        // Summary:
        //     Indicates whether the current object is equal to another object of the same type.
        //     Two events are equal if they have the same id.
        //
        // Parameters:
        //   other:
        //     An object to compare with this object.
        //
        // Returns:
        //     true if the current object is equal to the other parameter; otherwise, false.
        public bool Equals(EventId other) => this == other;
        //
        // Parameters:
        //   obj:
        public override bool   Equals(object? obj) => obj is not null && this == (EventId) obj;
        public override int    GetHashCode()       => base.GetHashCode();
        public override string ToString()          => Name ?? nameof(EventId);

        //
        // Summary:
        //     Checks if two specified Microsoft.Extensions.Logging.EventId instances have the
        //     same value. They are equal if they have the same Id.
        //
        // Parameters:
        //   left:
        //     The first Microsoft.Extensions.Logging.EventId.
        //
        //   right:
        //     The second Microsoft.Extensions.Logging.EventId.
        //
        // Returns:
        //     true if the objects are equal.
        public static bool operator ==(EventId left, EventId right) => left.Name == right.Name && left.Id == right.Id;
        //
        // Summary:
        //     Checks if two specified Microsoft.Extensions.Logging.EventId instances have different
        //     values.
        //
        // Parameters:
        //   left:
        //     The first Microsoft.Extensions.Logging.EventId.
        //
        //   right:
        //     The second Microsoft.Extensions.Logging.EventId.
        //
        // Returns:
        //     true if the objects are not equal.
        public static bool operator !=(EventId left, EventId right) => left.Name != right.Name || left.Id != right.Id;

        //
        // Summary:
        //     Implicitly creates an EventId from the given System.Int32.
        //
        // Parameters:
        //   i:
        //     The System.Int32 to convert to an EventId.
        public static implicit operator EventId(int i) => new(i, nameof(EventId));
    }
}
#endif