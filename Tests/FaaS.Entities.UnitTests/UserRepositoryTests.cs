using FaaS.Entities.Contexts;
using FaaS.Entities.DataAccessModels;
using FaaS.Entities.Repositories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FaaS.Entities.UnitTests
{
    public class UserRepositoryTests : TestBase
    {
        public static IEnumerable<object[]> InvalidAddUserArguments
        {
            get
            {
                yield return new object[] { null, new DateTime(2016, 5, 19, 3, 15, 0), Enumerable.Empty<Project>() };
                yield return new object[] { "104560124403688998123", new DateTime(2016, 5, 19, 3, 15, 0), (IEnumerable<Project>)null };
            }
        }

        [Fact]
        public async void GetSingleUser_Existing_ReturnsUser() 
        {
            var actualUser = await _UserRepository.Get("TestGoogleId1");

            Assert.NotNull(actualUser);
            Assert.IsType<User>(actualUser);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void GetSingleUser_NullOrEmpty_ReturnsUser(string googleId)
        {
            var actualUser = await _UserRepository.Get(googleId);

            Assert.Null(actualUser);
        }

        [Fact]
        public async void GetAllUsers_ReturnsAllUserInstances()
        {
            var actualUsers = await _UserRepository.List();

            Assert.NotNull(actualUsers);
            Assert.NotEmpty(actualUsers);
            Assert.Equal(3, actualUsers.Count());
            foreach (var actualUser in actualUsers)
            {
                Assert.IsType<User>(actualUser);
            }
        }

        [Fact]
        public async void AddUser_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _UserRepository.Add(null));
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
            User actualUser = await _UserRepository.Add(newUser);

            // Checks returned value
            Assert.NotNull(actualUser);
            Assert.Equal(newUser.GoogleId, actualUser.GoogleId);
            Assert.Equal(newUser.Registered, actualUser.Registered);
            Assert.NotEqual(Guid.Empty, actualUser.Id);

            // Check storage is persistant
            Assert.NotNull(_UserRepository.Get(newUser.GoogleId));
        }

        [Fact]
        public async void UpdateUser_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _UserRepository.Update(null));
        }

        [Fact]
        public async void UpdateUser_NotNull_NotInDB()
        {
            var newUser = new User
            {
                GoogleId = "NotInDatabaseGoogleId",
                Registered = DateTime.Now,
                Projects = new List<Project>()
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _UserRepository.Update(newUser));
        }

        [Fact]
        public async void UpdateUser_NotNull_InDB()
        {
            var actualUser = await _UserRepository.Get("TestGoogleId1");

            actualUser.GoogleId = "NotHisPreviousGoogleId";

            actualUser = await _UserRepository.Update(actualUser);
            actualUser = await _UserRepository.Get("NotHisPreviousGoogleId");

            // Checks returned value
            Assert.NotNull(actualUser);
            Assert.Equal("NotHisPreviousGoogleId", actualUser.GoogleId);
            Assert.NotEqual(Guid.Empty, actualUser.Id);
        }

        [Fact]
        public async void DeleteUser_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _UserRepository.Delete(null));
        }

        [Fact]
        public async void DeleteUser_NotNull_NotInDB()
        {
            var newUser = new User
            {
                GoogleId = "NotInDatabaseGoogleId",
                Registered = DateTime.Now,
                Projects = new List<Project>()
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _UserRepository.Delete(newUser));
        }

        [Fact]
        public async void DeleteUser_NotNull_InDB()
        {
            var actualUser = await _UserRepository.Get("TestGoogleId1");

            actualUser = await _UserRepository.Delete(actualUser);
           
            // Checks returned value
            Assert.NotNull(actualUser);
            Assert.Equal("TestGoogleId1", actualUser.GoogleId);
            Assert.NotEqual(Guid.Empty, actualUser.Id);

            actualUser = await _UserRepository.Get("TestGoogleId1");
            Assert.Null(actualUser);

            //var actualUsers = await _UserRepository.List();
            //Assert.Equal(2, actualUsers.Count());
        }

        [Theory]
        [MemberData(nameof(InvalidAddUserArguments))]
        public async void AddUser_NullGoogleIdOrRegisteredOrProjects_Throws(string googleId, DateTime registered, IEnumerable<Project> projects)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _UserRepository.Add(googleId, registered, projects));
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
                    GetTestProjectWithoutForms(4),
                    GetTestProjectWithoutForms(5),
                    GetTestProjectWithoutForms(6),
            };

            User actualUser = await _UserRepository.Add(newUser.GoogleId, newUser.Registered, projects);

            // Checks returned value
            Assert.NotNull(actualUser);
            Assert.Equal(newUser.GoogleId, actualUser.GoogleId);
            Assert.Equal(newUser.Registered, actualUser.Registered);
            Assert.NotEqual(Guid.Empty, actualUser.Id);

            // Checks storage is persistant
            Assert.NotNull(_UserRepository.Get(newUser.GoogleId));
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public UserRepositoryTests()// : base()
        {

            var usersData = new List<User>
            {
                testUser1,
                testUser2,
                testUser3
            };

            var projectsData = new List<Project>
            {
                testProject1,
                testProject2
            };

            // Mock context
            var projectsSubstitute = SubstituteQueryable(projectsData);
            var usersSubstitute = SubstituteQueryable(usersData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Projects.Returns(projectsSubstitute);
            contextSubsitute.Users.Returns(usersSubstitute);

            _UserRepository = new UserRepository(contextSubsitute);
        }
    }
}
