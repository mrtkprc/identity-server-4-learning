using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

public class CustomProfileService : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        //Kullanıcı veritabanından çekilir.
        //context.Subject.GetSubjectId(); -> Subject Id değerini getirir.

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, "gncy@gencayyildiz.com"),
            new Claim(JwtRegisteredClaimNames.Website,"https://www.gencayyildiz.com"),
            new Claim("gobekadi", "Ali"),
            new Claim("role", "admin") // for Role based authorization
        };

        context.AddRequestedClaims(claims); //Userinfo Token
        context.IssuedClaims = claims; //JWT
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        //Kullanıcı doğrulanır.
        context.IsActive = true;
    }
}