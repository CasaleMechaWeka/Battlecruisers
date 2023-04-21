using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Utils
{
    public struct AddressableGUID : INetworkSerializable
    {
        public FixedString128Bytes Value;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class AddressableGUIDEqualityComparer : IEqualityComparer<AddressableGUID>
    {
        public int GetHashCode(AddressableGUID addressableGUID)
        {
            return addressableGUID.GetHashCode();
        }

        public bool Equals(AddressableGUID x, AddressableGUID y)
        {
            return x.GetHashCode().Equals(y.GetHashCode());
        }
    }
}
