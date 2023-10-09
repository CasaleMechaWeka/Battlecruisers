#if PLATFORM_IOS
using System.Text;
using UnityEngine;
using Unity.Services.Authentication;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System.Threading.Tasks;
using Unity.Services.Core;
using System;

namespace BattleCruisers.Utils.Network
{
    public class AppleAuthentication : IAppleAuthentication
    {
        public string Token { get; set; }
    }
}
#endif
