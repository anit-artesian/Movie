
// using System.Threading.Tasks;
// using IdentityServer4.Validation;
// using MediaApplication.Data;
// namespace MediaApplication.Models
// {

//     public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
//     {
//         private readonly ApplicationDbContext _context;
//         public ResourceOwnerPasswordValidator(ApplicationDbContext context)
//         {
//             _context  =  context;
//         }
//         public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
//         {
//          string username  = context.UserName;
//          string password  =  context.Password;
//          context.Result = new GrantValidationResult("test1","password");
//          return Task.FromResult(0);

//         }
//     }
// }