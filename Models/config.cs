// using System.Collections.Generic;
// using IdentityServer4;
// using IdentityServer4.Models;

// namespace MediaApplication.Models
// {
//     public class Config
//     {

//         public static IEnumerable<IdentityResource> GetIdentityResources()
//         {
//             return new List<IdentityResource>
//             {
//                 new IdentityResources.OpenId(),
//                 new IdentityResources.Profile(),
//             };
//         }

//         // scopes define the API resources in your system
//         public static IEnumerable<ApiResource> GetApiResources()
//         {
//             return new List<ApiResource>
//             {
//                 new ApiResource("api1", "My API")
//             };
//         }

//         // client want to access resources (aka scopes)
//         public static IEnumerable<Client> GetClients()
//         {
//             return new List<Client>
//             {
//                         new Client
//                         {
//                             ClientId = "client",
//                             ClientName = "MVC Client",
//                             AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
//                             //AccessTokenType   :BY default JWT
//                             ClientSecrets =
//                             {
//                                 new Secret("secret".Sha256())
//                             },

//                           RedirectUris           = { "http://localhost:5000/signin-oidc" },
//                           //  PostLogoutRedirectUris = { "http://localhost:5002" },

//                             AllowedScopes =
//                             {
//                                 IdentityServerConstants.StandardScopes.OpenId,
//                                 IdentityServerConstants.StandardScopes.Profile,
//                                 "api1"
//                             },
                           
//                         }
//             };
//         }
//     }
// }