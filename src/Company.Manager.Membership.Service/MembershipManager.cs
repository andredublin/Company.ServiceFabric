﻿using Company.Common.Data;
using Company.Engine.Registration.Interface;
using Company.Manager.Membership.Interface;
using Company.ServiceFabric.Client;
using Company.ServiceFabric.Server;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Serilog;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Zametek.Utility.Logging;

namespace Company.Manager.Membership.Service
{
    internal sealed class MembershipManager
        : StatelessService, IMembershipManager
    {
        private IMembershipManager _Impl;
        private readonly ILogger _Logger;

        public MembershipManager(
            StatelessServiceContext context,
            ILogger logger)
            : base(context)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var registrationEngine = TrackingProxy.ForComponent<IRegistrationEngine>(this);
            _Impl = LogProxy.Create<IMembershipManager>(new Impl.MembershipManager(registrationEngine, logger), logger, LogType.All);
            _Logger.Information("Constructed");
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(
                    (context) => new FabricTransportServiceRemotingListener(
                        context,
                        new TrackingServiceRemotingDispatcher(context, this),
                        new FabricTransportRemotingListenerSettings
                        {
                            EndpointResourceName = typeof(IMembershipManager).Name
                        }),
                    typeof(IMembershipManager).Name),
            };
        }

        protected override Task OnCloseAsync(CancellationToken cancellationToken)
        {
            _Logger.Information($"{nameof(OnCloseAsync)} Invoked");
            return base.OnCloseAsync(cancellationToken);
        }

        protected override void OnAbort()
        {
            _Logger.Information($"{nameof(OnAbort)} Invoked");
            base.OnAbort();
        }

        public Task<string> RegisterMemberAsync(RegisterRequest request)
        {
            return _Impl.RegisterMemberAsync(request);
        }
    }
}
