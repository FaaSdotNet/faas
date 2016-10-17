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
    public class ProjectRepositoryTests
    {
        private readonly ProjectRepository _ProjectRepository;
        private readonly UserRepository _UserRepository;

        [Fact]
        public async void GetSingleProject_Existing_ReturnsProject()
        {
            var actualProject = await _ProjectRepository.Get("TestProject1");

            Assert.NotNull(actualProject);
            Assert.IsType<Project>(actualProject);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void GetSingleProject_NullOrEmpty_ReturnsProject(string projectName)
        {
            var actualProject = await _ProjectRepository.Get(projectName);

            Assert.Null(actualProject);
        }

        [Fact]
        public async void GetAllProjects_ReturnsAllProjectInstances()
        {
            var actualProjects = await _ProjectRepository.List();

            Assert.NotNull(actualProjects);
            Assert.NotEmpty(actualProjects);
            Assert.Equal(2, actualProjects.Count());
            foreach (var actualProject in actualProjects)
            {
                Assert.IsType<Project>(actualProject);
            }
        }

        [Fact]
        public async void AddProject_Null_Throws()
        {
            var user = await _UserRepository.Get("TestGoogleId1");
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ProjectRepository.Add(user, null));
        }

        [Fact]
        public async void AddProject_NullUser_Throws()
        {
            User user = null;
            Project project = new Project
            {
                Name = "TestProjectNotInDb",
                Created = DateTime.Now,
                Description = "TestDescriptionNotInDb"
            };
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ProjectRepository.Add(user, null));
        }

        [Fact]
        public async void AddProject_NotNull_ReturnsProjectWithId()
        {
            var user = await _UserRepository.Get("TestGoogleId1");

            var newProject = new Project
            {
                Name = "TestProject01",
                Created = DateTime.Now,
                Description = "TestDescription01",
                Forms = new List<Form>()
                
            };
            Project actualProject = await _ProjectRepository.Add(user, newProject);

            // Checks returned value
            Assert.NotNull(actualProject);
            Assert.Equal(newProject.Name, actualProject.Name);
            Assert.Equal(newProject.Created, actualProject.Created);
            Assert.Equal(newProject.Description, actualProject.Description);
            Assert.NotEqual(Guid.Empty, actualProject.Id);

            // Check storage is persistant
            Assert.NotNull(_ProjectRepository.Get(newProject.Name));
        }

        [Fact]
        public async void UpdateProject_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ProjectRepository.Update(null));
        }

        [Fact]
        public async void UpdateProject_NotNull_NotInDB()
        {
            var newProject = new Project
            {
               Name = "NotInDbName",
               Description = "NotInDbDescription",
               Created = DateTime.Now
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _ProjectRepository.Update(newProject));
        }

        [Fact]
        public async void UpdateProject_NotNull_InDB()
        {
            var actualProject = await _ProjectRepository.Get("TestProject1");

            actualProject.Name = "NotHisPreviousName";

            actualProject = await _ProjectRepository.Update(actualProject);
            actualProject = await _ProjectRepository.Get("NotHisPreviousName");

            // Checks returned value
            Assert.NotNull(actualProject);
            Assert.Equal("NotHisPreviousName", actualProject.Name);
            Assert.NotEqual(Guid.Empty, actualProject.Id);
        }

        [Fact]
        public async void AddProject_CorrectParts_ReturnsProjectWithId()
        {
            var actualUser = await _UserRepository.Get("TestGoogleId3");

            var forms = new[]
            {
                    GetTestFormWithoutElements(254),
                    GetTestFormWithoutElements(253),
                    GetTestFormWithoutElements(252),
            };

            var newProject = new Project
            {
                Name = "TestProject03",
                Created = DateTime.Now,
                Description = "TestDescription03",
                Forms = forms
            };

            var actualProject = await _ProjectRepository.Add(actualUser, newProject);

            // Checks returned value
            Assert.NotNull(actualProject);
            Assert.Equal(newProject.Name, actualProject.Name);
            Assert.Equal(newProject.Created, actualProject.Created);
            Assert.Equal(newProject.Description, actualProject.Description);
            Assert.NotEqual(Guid.Empty, actualProject.Id);
            Assert.Equal(3, actualProject.Forms.Count);

            // Checks storage is persistant
            Assert.NotNull(_ProjectRepository.Get(newProject.Name));
        }

        [Fact]
        public async void DeleteProject_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ProjectRepository.Delete(null));
        }

        [Fact]
        public async void DeleteProject_NotNull_NotInDb()
        {
            var newProject = new Project
            {
                Name = "TestProjectNotInDb",
                Created = DateTime.Now,
                Description = "TestDescriptionNotInDb",
                Forms = new List<Form>()
            };
            var actualProject = await _ProjectRepository.Delete(newProject);
            Assert.Null(actualProject);
        }

        [Fact]
        public async void DeleteProject_NotNull_InDb()
        {
            var user = await _UserRepository.Get("TestGoogleId1");

            var newProject = new Project
            {
                Name = "TestProject01",
                Created = DateTime.Now,
                Description = "TestDescription01",
                Forms = new List<Form>()
            };

            Project actualProject = await _ProjectRepository.Add(user, newProject);
            //var allProjects = await _ProjectRepository.List();
            //int numProjects = allProjects.Count();
            //var actualProject = await _ProjectRepository.Get("TestProject1");
            var deletedProject = await _ProjectRepository.Delete(actualProject);
            var deletedProject2 = await _ProjectRepository.Delete(actualProject);

            //allProjects = await _ProjectRepository.List();
            //int numProjectsAfter = allProjects.Count();

            //Assert.Equal(numProjects - 1, numProjectsAfter);
            Assert.NotNull(deletedProject);
            Assert.Null(deletedProject2);
            Assert.Equal(actualProject.Name, deletedProject.Name);
            Assert.Equal(actualProject.Created, deletedProject.Created);
            Assert.Equal(actualProject.Description, deletedProject.Description);
        }



        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public ProjectRepositoryTests()
        {
            // Mock projects
            Project testProject1 = GetTestProjectWithoutForms(1);
            Project testProject2 = GetTestProjectWithoutForms(2);

            var projectsData = new List<Project>
            {
                testProject1,
                testProject2
            };

            // Mock users
            User testUser1 = GetTestUserWithoutProjects(1);
            User testUser2 = GetTestUserWithoutProjects(2);
            User testUser3 = GetTestUserWithoutProjects(3);
            User testUser4 = GetTestUserWithoutProjects(4);
            var usersData = new List<User>
            {
                testUser1,
                testUser2,
                testUser3,
                testUser4
            };

            // Mock forms
            Form testForm1 = GetTestFormWithoutElements(1);
            Form testForm2 = GetTestFormWithoutElements(2);
            Form testForm3 = GetTestFormWithoutElements(3);
            Form testForm4 = GetTestFormWithoutElements(4);

            var formsData = new List<Form>
            {
                testForm1,
                testForm2,
                testForm3,
                testForm4
            };

            testProject1.User = testUser1;
            testProject2.User = testUser1;
            testProject1.UserId = testUser1.Id;
            testProject2.UserId = testUser1.Id;

            // Mock context
            var projectsSubstitute = SubstituteQueryable(projectsData);
            var usersSubstitute = SubstituteQueryable(usersData);
            var formsSubstitute = SubstituteQueryable(formsData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Projects.Returns(projectsSubstitute);
            contextSubsitute.Users.Returns(usersSubstitute);
            contextSubsitute.Forms.Returns(formsSubstitute);

            _ProjectRepository = new ProjectRepository(contextSubsitute);
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
        /// Creates new test <see cref="User"/> object with no <see cref="User.Projects"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static User GetTestUserWithoutProjects(int identifier)
        {
            return new User
            {
                GoogleId = $"TestGoogleId{identifier}",
                Registered = DateTime.Now.AddDays(identifier),
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }

        /// <summary>
        /// Creates new test <see cref="Project"/> object with no <see cref="Project.Forms"/>.
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
        /// Creates new test <see cref="Form"/> object with no <see cref="Form.Elements"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static Form GetTestFormWithoutElements(int identifier)
        {
            return new Form
            {
                Name = $"TestForm{identifier}",
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
