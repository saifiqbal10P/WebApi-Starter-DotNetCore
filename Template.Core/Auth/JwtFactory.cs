using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Template.Common.Constant;
using Template.Core.Entity;
using static Template.Common.Constant.Constant;

namespace Template.Core.Auth
{
    public class JwtFactory:IJwtFactory
    {
        private readonly JwtIssuerOptions jwtIssuerOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> options)
        {
            this.jwtIssuerOptions = options.Value;
            ThrowIfInvalidOptions(this.jwtIssuerOptions);
        }


        public ClaimsIdentity GenerateClaimsIdentity(ApplicationUser user, string deviceId, string role, string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"), new[] {
                new Claim(Constant.Strings.ClaimIdentifiers.Id, user.Id.ToString()),
                new Claim(Constant.Strings.ClaimIdentifiers.UserRoles, role),
                new Claim(Constant.Strings.ClaimIdentifiers.Email, user.Email),
                new Claim(Constant.Strings.ClaimIdentifiers.DeviceID, deviceId),
                new Claim(Constant.Strings.ClaimIdentifiers.FirstName, user.FirstName),
                new Claim(Constant.Strings.ClaimIdentifiers.LastName, user.LastName),
                new Claim(Constant.Strings.ClaimIdentifiers.UserName, userName)
            });
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, string deviceId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userName),
                new Claim(JwtRegisteredClaimNames.Jti, await this.jwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(this.jwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(Constant.Strings.ClaimIdentifiers.Role),
                identity.FindFirst(Constant.Strings.ClaimIdentifiers.Id),
                identity.FindFirst(Constant.Strings.ClaimIdentifiers.Email),
                identity.FindFirst(Constant.Strings.ClaimIdentifiers.UserRoles),
                identity.FindFirst(Constant.Strings.ClaimIdentifiers.FirstName),
                identity.FindFirst(Constant.Strings.ClaimIdentifiers.LastName),
                new Claim(Constant.Strings.ClaimIdentifiers.DeviceID,deviceId),
                new Claim(Constant.Strings.ClaimIdentifiers.UserName,userName)
            };

            var jwt = new JwtSecurityToken(
                issuer: this.jwtIssuerOptions.Issuer,
                audience: this.jwtIssuerOptions.Audience,
                claims: claims,
                notBefore: this.jwtIssuerOptions.NotBefore,
                expires: this.jwtIssuerOptions.Expiration,
                signingCredentials: this.jwtIssuerOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }



        private static void ThrowIfInvalidOptions(JwtIssuerOptions jwtIssuerOptions)
        {
            if (jwtIssuerOptions == null) throw new ArgumentNullException(nameof(jwtIssuerOptions));

            if (jwtIssuerOptions.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException(ErrorStrings.NonZeroTimeSpan);
            }

            if (jwtIssuerOptions.SigningCredentials == null)
            {
                throw new ArgumentNullException(string.Empty);
            }

            if (jwtIssuerOptions.JtiGenerator == null)
            {
                throw new ArgumentNullException(string.Empty);
            }

        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() -
                    new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}
