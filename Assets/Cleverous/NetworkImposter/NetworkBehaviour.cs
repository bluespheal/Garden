// (c) Copyright Cleverous 2022. All rights reserved.

#if !MIRROR && !FISHNET
using UnityEngine;

namespace Cleverous.NetworkImposter
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkBehaviour : MonoBehaviour
    {
        public bool isServer = true;
        public bool isClient = true;
        public bool hasAuthority = true;
        public bool isLocalPlayer = true;
        public NetworkIdentity netIdentity;
        public int netId => netIdentity.netId;
        public NetworkConnection connectionToClient;

        protected virtual void Start()
        {
            OnStartServer();
            OnStartLocalPlayer();
            OnStartClient();
        }

        public virtual void OnStartClient()
        {
        }

        public virtual void OnStopClient()
        {
        }

        public virtual void OnStartServer()
        {
        }

        public virtual void OnStopServer()
        {
        }

        public virtual void OnStartLocalPlayer()
        {
        }
    }
}
#endif