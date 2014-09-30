﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Thinktecture.IdentityManager.Resources;

namespace Thinktecture.IdentityManager.Configuration.Hosting.LocalAuthenticationMiddleware
{
    public class LocalAuthenticationHandler : AuthenticationHandler<LocalAuthenticationOptions>
    {
        protected override Task<Microsoft.Owin.Security.AuthenticationTicket> AuthenticateCoreAsync()
        {
            var ctx = this.Context;
            var localAddresses = new string[] { "127.0.0.1", "::1", ctx.Request.LocalIpAddress };
            if (localAddresses.Contains(ctx.Request.RemoteIpAddress))
            {
                var id = new ClaimsIdentity(Constants.LocalAuthenticationType, Constants.ClaimTypes.Name, Constants.ClaimTypes.Role);
                id.AddClaim(new Claim(Constants.ClaimTypes.Name, Messages.LocalUsername));
                id.AddClaim(new Claim(Constants.ClaimTypes.Role, this.Options.RoleToAssign));

                var ticket = new AuthenticationTicket(id, new AuthenticationProperties());
                return Task.FromResult(ticket);
            }

            return Task.FromResult<AuthenticationTicket>(null);
        }
    }
}
