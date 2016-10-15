using FaaS.Entities.Contexts;
using FaaS.Entities.DataAccessModels;
using FaaS.Entities.Repositories;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Xunit;

namespace FaaS.Entities.UnitTests
{
    public class UserRepositoryTests
    {
        private UserRepository _UserRepository;

        public static IEnumerable<object[]> InvalidAddUserArguments
        {
            get
            {
                yield return new object[] { "104560124403688998123", DateTime.Now, Enumerable.Empty<Project>() };
                yield return new object[] { null, DateTime.Now, Enumerable.Empty<Project>() };
                yield return new object[] { "104560124403688998123", DateTime.Now, (IEnumerable<Project>)null };
            }
        }

        [Fact]
        public async void GetSingleUser_Existing_ReturnsUser() 
        {
            var actualUser = await _UserRepository.GetSingleUser("104560124403688998123");

            Assert.NotNull(actualUser);
            Assert.IsType<User>(actualUser);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void GetSingleUser_NullOrEmpty_ReturnsUser(string googleId)
        {
            var actualUser = await _UserRepository.GetSingleUser(googleId);

            Assert.Null(actualUser);
        }

        [Fact]
        public async void GetAllUsers_ReturnsAllUserInstances()
        {
            var actualUsers = await _UserRepository.GetAllUsers();

            Assert.NotNull(actualUsers);
            Assert.NotEmpty(actualUsers);
            Assert.Equal(4, actualUsers.Count());
            foreach (var actualUser in actualUsers)
            {
                Assert.IsType<User>(actualUser);
            }
        }

        [Fact]
        public async void AddUser_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _UserRepository.AddUser(null));
        }

        [Fact]
        public async void AddUser_NotNull_ReturnsUserWithId()
        {
            var newUser = new User
            {
                GoogleId = "104560124403688998123",
                Registered = DateTime.Now,
                Projects = new List<Project>()
            };
            User actualUser = await _UserRepository.AddUser(newUser);

            // Checks returned value
            Assert.NotNull(actualUser);
            Assert.Equal(newUser.GoogleId, actualUser.GoogleId);
            Assert.Equal(newUser.Registered, actualUser.Registered);
            Assert.NotEqual(Guid.Empty, actualUser.Id);

            // Check storage is persistant
            Assert.NotNull(_UserRepository.GetSingleUser(newUser.GoogleId));
        }

        [Theory]
        [MemberData(nameof(InvalidAddUserArguments))]
        public async void AddUser_NullGameOrDeveloperOrAwards_Throws(string googleId, DateTime registered, IEnumerable<Project> projects)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _UserRepository.AddUser(googleId, registered, projects));
        }

        [Fact]
        public async void AddUser_CorrectParts_ReturnsUserWithId()
        {
            var newUser = new User
            {
                GoogleId = "104560124403688998123",
                Registered = DateTime.Now,
            };
            var projects = new[]
            {
                    GetTestProjectWithoutForms(254),
                    GetTestProjectWithoutForms(253),
                    GetTestProjectWithoutForms(252),
            };

            User actualUser = await _UserRepository.AddUser(newUser.GoogleId, newUser.Registered, projects);

            // Checks returned value
            Assert.NotNull(actualUser);
            Assert.Equal(newUser.GoogleId, actualUser.GoogleId);
            Assert.Equal(newUser.Registered, actualUser.Registered);
            Assert.NotEqual(Guid.Empty, actualUser.Id);

            // Checks storage is persistant
            Assert.NotNull(_UserRepository.GetSingleUser(newUser.GoogleId));
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public UserRepositoryTests()
        {
            // Mock awards
            Project testProject1 = GetTestProjectWithoutForms(1);
            Project testProject2 = GetTestProjectWithoutForms(2);

            var projectsData = new List<Project>
            {
                testProject1,
                testProject2
            };

            // Mock users
            var usersData = new List<User>
            {
                new User { GoogleId = "104560124403688998121", Registered = DateTime.Now.AddDays(1)},
                new User { GoogleId = "104560124403688998122", Registered = DateTime.Now.AddDays(2)},
                new User { GoogleId = "104560124403688998123", Registered = DateTime.Now.AddDays(3)},
                new User { GoogleId = "104560124403688998124", Registered = DateTime.Now.AddDays(3)},
            };

            // Mock context
            var projectsSubstitute = SubstituteQueryable(projectsData);
            var usersSubstitute = SubstituteQueryable(usersData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Projects.Returns(projectsSubstitute);
            contextSubsitute.Users.Returns(usersSubstitute);

            _UserRepository = new UserRepository(contextSubsitute);
        }

        /// <summary>
        /// Creates substitute for a <see cref="DbSet{TEntity}"/> with database replaced with an in-memory structure represented by <paramref name="data"/>.
        /// Can be used for querying and addition, including async operations.
        /// </summary>
        /// <typeparam name="TType">Type of data and <see cref="DbSet{TEntity}"/> to substitute</typeparam>
        /// <param name="data">Initial content of "database"</param>
        /// <returns>Queryable that can be used as <see cref="DbSet{TEntity}"/> substitute.</returns>
        private static IQueryable<TType> SubstituteQueryable<TType>(ICollection<TType> data)
            where TType : ModelBase
        {
            var queryableData = data.AsQueryable();
            var queryableSubstitute = Substitute.For<IQueryable<TType>, IDbAsyncEnumerable<TType>, DbSet<TType>>();

            // Mock queryable
            queryableSubstitute.Provider.Returns(new TestDbAsyncQueryProvider<TType>(queryableData.Provider));
            queryableSubstitute.Expression.Returns(queryableData.Expression);
            queryableSubstitute.ElementType.Returns(queryableData.ElementType);
            queryableSubstitute.GetEnumerator().Returns(queryableData.GetEnumerator());

            // Mock addition
            ((DbSet<TType>)queryableSubstitute).Add(null).ReturnsForAnyArgs(callInfo => SimulateAddition(callInfo, data));

            // Mock async
            ((IDbAsyncEnumerable<TType>)queryableSubstitute).GetAsyncEnumerator().Returns(new TestDbAsyncEnumerator<TType>(data.GetEnumerator()));

            return queryableSubstitute;
        }

        /// <summary>
        /// Reads <typeparamref name="TType"/> from <paramref name="callInfo"/> and stores it to the <paramref name="data"/>.
        /// To emulate reald DB, it also sets <see cref="ModelBase.Id"/> with new <see cref="Guid"/> and returns the very
        /// object the method was provided with.
        /// </summary>
        private static TType SimulateAddition<TType>(CallInfo callInfo, ICollection<TType> data)
            where TType : ModelBase
        {
            TType entry = callInfo.Arg<TType>();

            entry.Id = Guid.NewGuid();
            data.Add(callInfo.Arg<TType>());

            return entry;
        }

        /// <summary>
        /// Creates new test <see cref="Project"/> object with no <see cref="Award.Games"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static Project GetTestProjectWithoutForms(int identifier)
        {
            return new Project
            {
                Name = $"TestProject{identifier}",
                Created = DateTime.Now,
                Description = $"TestDescription{identifier}",
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }

        /// <summary>
        /// Formates given <paramref name="identifier"/> to (at least) 12 characters long string where all missing characters are replaced with 0.
        /// <see cref="String"/> formatted in this way can be used as last part a GUID.
        /// </summary>
        private static string FormatForLastGuidPart(int identifier) => identifier.ToString().PadLeft(12, '0');
    }
}
