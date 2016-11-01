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
    public class ProjectRepositoryTests : TestBase
    {
        [Fact]
        public async void GetSingleProject_Existing_ReturnsProject()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var actualProject = await _ProjectRepository.Get(id);

            Assert.NotNull(actualProject);
            Assert.IsType<Project>(actualProject);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void GetSingleProject_NullOrEmpty_ReturnsProject(Guid id)
        {
            var actualProject = await _ProjectRepository.Get(id);

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
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"),
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
            Assert.NotNull(_ProjectRepository.Get(newProject.Id));
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
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var actualProject = await _ProjectRepository.Get(id);

            actualProject.Name = "NotHisPreviousName";

            actualProject = await _ProjectRepository.Update(actualProject);
            actualProject = await _ProjectRepository.Get(id);

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
                    GetTestFormWithoutElements(4),
                    GetTestFormWithoutElements(5),
                    GetTestFormWithoutElements(6),
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
            Assert.NotNull(_ProjectRepository.Get(newProject.Id));
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
        public ProjectRepositoryTests()// : base()
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
    }
}
