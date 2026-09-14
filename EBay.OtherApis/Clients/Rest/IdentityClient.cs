// Copyright 2026 eBay Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using EBay.OtherApis.Configuration;
using EBay.OtherApis.Generated.Identity.Models;
using EBay.OtherApis.Generated.Identity;
using Microsoft.Kiota.Abstractions;

namespace EBay.OtherApis.Clients.Rest;

public sealed class IdentityClient
{
    private readonly IdentityApiClient _kiota;
    private readonly OtherApisConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal IdentityClient(IdentityApiClient kiota, OtherApisConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// This method retrieves the account profile information for an authenticated user, which requires a <b>User access token</b>. What is returned is controlled by the <b>scopes</b>.
    /// For a business account you use the default scope <c>commerce.identity.readonly</c>, which returns all the fields in the <b>businessAccount</b> container. These are returned because this is all public information.
    /// For an individual account, the fields returned in the <b>individualAccount</b> container are based on the scope you use. Using the default scope, only public information, such as eBay user ID, are returned. For details about what each scope returns, see the <b>Identity API scopes</b>.
    /// </summary>
    public Task<UserResponse?> GetUserAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.User.EmptyPathSegment.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
