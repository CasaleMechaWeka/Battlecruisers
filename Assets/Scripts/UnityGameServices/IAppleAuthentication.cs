using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Network
{
    public interface IAppleAuthentication
    {
        string Token { get; set;}
    }
}
