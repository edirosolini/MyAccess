// <copyright file="AuthenticateResponse.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Responses
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public class AuthenticateResponse
    {
        public Guid Id { get; set; }

        public string BusinessName { get; set; }

        public string EmailAddress { get; set; }

        public string TokenAccess
        {
            get { return GenerateJwtToken(this); }
        }

        public string TokenType { get; } = "Bearer";

        private static string GenerateJwtToken(AuthenticateResponse model)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SIGNING_KEY")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);

            var claims = new[]
            {
                new Claim("Id", model.Id.ToString()),
                new Claim("BusinessName", $"{model.BusinessName}"),
                new Claim("EmailAddress", $"{model.EmailAddress}"),
            };

            var payload = new JwtPayload(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                notBefore: null,
                expires: DateTime.UtcNow.AddDays(1));

            var token = new JwtSecurityToken(
                    header,
                    payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
