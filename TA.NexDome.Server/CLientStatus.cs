// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System;

    /// <summary>
    ///     Records the connection status of a client.
    /// </summary>
    internal class ClientStatus : IEquatable<ClientStatus>
        {
        /// <summary>
        ///     Gets or sets the client unique identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public Guid ClientId { get; set; }

        /// <summary>
        ///     Gets or sets the client name. Each client provides its name upon first registration and
        ///     typically this will be the name of the executable that owns the client's process.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the is "online". A client is considered to be
        ///     online if it has requested to open the communications channel, and "offline" if it has
        ///     requested to close it.
        /// </summary>
        /// <value><c>true</c> if online; otherwise, <c>false</c>.</value>
        public bool Online { get; set; }

        public bool Equals(ClientStatus other)
            {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ClientId.Equals(other.ClientId);
            }

        public bool Equals(Guid other)
            {
            return ClientId.Equals(other);
            }

        public override bool Equals(object obj)
            {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ClientStatus)) return false;
            return Equals((ClientStatus)obj);
            }

        public override int GetHashCode()
            {
            return ClientId.GetHashCode();
            }

        public static bool operator ==(ClientStatus left, ClientStatus right)
            {
            return Equals(left, right);
            }

        public static bool operator ==(ClientStatus left, Guid right)
            {
            return left?.Equals(right) ?? false;
            }

        public static bool operator !=(ClientStatus left, ClientStatus right)
            {
            return !Equals(left, right);
            }

        public static bool operator !=(ClientStatus left, Guid right)
            {
            return !left?.Equals(right) ?? false;
            }

        public override string ToString()
            {
            return $"{nameof(ClientId)}: {ClientId}, {nameof(Name)}: {Name}, {nameof(Online)}: {Online}";
            }

        public string ToDisplayString()
            {
            string online = Online ? "Online" : "Idle  ";
            return $"{ClientId} {online} {Name}";
            }
        }
    }