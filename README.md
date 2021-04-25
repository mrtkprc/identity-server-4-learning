# identity-server-4-learning
Identity Server 4 Learning Notes


## IdentityServer and Angular App Notes

[oidc](https://www.npmjs.com/package/oidc-client)

### `GetClients` Configuration

```c#
new Client
        {
            ClientId = "AngularClient",
            ClientName = "Angular Client",
            RequireClientSecret = false,
            AllowedScopes = {
                "Garanti.Write",
                "Garanti.Read",
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Email,
                "Roles"
            },
            RedirectUris = {"http://localhost:4200/callback"},
            AllowedCorsOrigins = {"http://localhost:4200"},
            PostLogoutRedirectUris = {"http://localhost:4200"},
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
        }

```

- `RequireClientSecret` should be `false`
- `RequirePkce` should be `true` 

#### To avoid vulnerabilities, we **should not** use client secret value

### Silent Token

```c#
public static IEnumerable<Client> GetClients() =>
    new List<Client>
    {
        new Client
        {
            ClientId = "AngularClient",
            .
            .
            .
            RedirectUris = {
            "http://localhost:4200/callback", 
            "http://localhost:4200/silent-callback" //Silent endpoint
            },
            AccessTokenLifetime = 70
        }
    };
```


