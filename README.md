# identity-server-4-learning
Identity Server 4 Learning Notes


## Discovery Endpoint 

To determine usable endpoints in IdentityServer, `/.well-known/openid-configuration` can be used.

To retrieve acces token: 

`"token_endpoint": "https://localhost:5000/connect/token"`

`/.well-known/openid-configuration` shows supported scopes.
"scopes_supported": [
        "Garanti.Write",
        "Garanti.Read",
        "HalkBank.Write",
        "HalkBank.Read",
        "offline_access"
],

## Introspect Endpoint

Endpoint `/connect/introspect` can be used to see token status and scopes.
