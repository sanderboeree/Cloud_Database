// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;
// using Technoleon.Api.Infrastructure.EntityFramework;
// using Technoleon.Api.Infrastructure.Identity;
// using Technoleon.Api.Infrastructure.Repositories;
// using Technoleon.Api.Infrastructure.Specifications;
// using Technoleon.Investors.Api.Domain.Tests;
// using Xunit;
//
// namespace Technoleon.Api.Tests.Infrastructure.Repositories
// {
//     public class RepositoryTests
//     {
//         private readonly Repository<TechnoleonUser> _repository;
//
//         public RepositoryTests()
//         {
//             var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("Technoleon").Options;
//             _repository = new Repository<TechnoleonUser>(new ApplicationDbContext(options));
//         }
//
//         [Fact(Skip = "Reason")]
//         public async Task TestSaveAsync()
//         {
//             var input = new TechnoleonUser
//             {
//             };
//
//             await _repository.SaveAsync(input);
//
//             var storedCandidate = await _repository.FindOneAsync(new WithId<TechnoleonUser>(input.Id));
//
//             await _repository.SaveAsync(storedCandidate);
//         }
//     }
// }
