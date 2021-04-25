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


## IdentityServer and Angular App Notes

Angular uses npm package [oidc](https://www.npmjs.com/package/oidc-client)

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

## Resource Owner Credential (Lesson-22)

If you prefer SPA such as React, you can develop Auth API Service between client and auth server.

## Persitable Data Storage (Lesson-23)

### Adjustment of services.
```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
 
        services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer("Server=.;Database=AuthServerDB;Trusted_Connection=True;",
                        sqlOptions => sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer("Server=.;Database=AuthServerDB;Trusted_Connection=True;",
                        sqlOptions => sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
            })
            .AddDeveloperSigningCredential();
            .
            .
            .
    }
    .
    .
    .
}
```

### Seed Configuration

```c#
public static class SeedData
{
    public static void Seed(ConfigurationDbContext context)
    {
        if (!context.Clients.Any())
            foreach (var client in Config.GetClients())
                context.Clients.Add(client.ToEntity());
 
        if (!context.ApiResources.Any())
            foreach (var apiResource in Config.GetApiResources())
                context.ApiResources.Add(apiResource.ToEntity());
 
        if (!context.ApiScopes.Any())
            foreach (var scope in Config.GetApiScopes())
                context.ApiScopes.Add(scope.ToEntity());
 
        if (!context.IdentityResources.Any())
            foreach (var identityResource in Config.GetIdentityResources())
                context.IdentityResources.Add(identityResource.ToEntity());
 
        context.SaveChanges();
    }
}
```

### Inject data

```c#
public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        using var serviceScope = host.Services.CreateScope();
        var services = serviceScope.ServiceProvider;
        var context = services.GetRequiredService<ConfigurationDbContext>();
        SeedData.Seed(context);
        host.Run();
    }
 
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
```

### Apply migration and creating database

`dotnet ef migrations add mig_1 -c ConfigurationDbContext`

`dotnet ef database update -c ConfigurationDbContext` 
