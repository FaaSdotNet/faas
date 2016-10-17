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
    public class FormRepositoryTests
    {
        private readonly FormRepository _FormRepository;
        private readonly ProjectRepository _ProjectRepository;

        [Fact]
        public async void GetSingleForm_Existing_ReturnsForm()
        {
            var actualForm = await _FormRepository.Get("TestForm1");

            Assert.NotNull(actualForm);
            Assert.IsType<Form>(actualForm);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void GetSingleForm_NullOrEmpty_ReturnsForm(string formName)
        {
            var actualForm = await _FormRepository.Get(formName);

            Assert.Null(actualForm);
        }

        [Fact]
        public async void GetAllForms_ReturnsAllFormInstances()
        {
            var actualForms = await _FormRepository.List();

            Assert.NotNull(actualForms);
            Assert.NotEmpty(actualForms);
            Assert.Equal(4, actualForms.Count());
            foreach (var actualForm in actualForms)
            {
                Assert.IsType<Form>(actualForm);
            }
        }

        [Fact]
        public async void AddForm_Null_Throws()
        {
            Project actualProject = await _ProjectRepository.Get("TestProject1");
            await Assert.ThrowsAsync<ArgumentNullException>(() => _FormRepository.Add(actualProject, null));
        }

        [Fact]
        public async void AddForm_NotNull_ReturnsFormWithId()
        {
            Project actualProject = await _ProjectRepository.Get("TestProject1");

            var newForm = new Form
            {
                Name = "NotInDbFormName",
                Created = DateTime.Now,
                Description = "TestDescriptionNew",
                Elements = new List<Element>()
            };
            Form actualForm = await _FormRepository.Add(actualProject, newForm);

            // Checks returned value
            Assert.NotNull(actualForm);
            Assert.Equal(newForm.Name, actualForm.Name);
            Assert.Equal(newForm.Created, actualForm.Created);
            Assert.NotEqual(Guid.Empty, actualForm.Id);

            // Check storage is persistant
            Assert.NotNull(_FormRepository.Get(newForm.Name));
        }

        [Fact]
        public async void UpdateForm_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _FormRepository.Update(null));
        }

        [Fact]
        public async void UpdateForm_NotNull_NotInDB()
        {
            var newForm = new Form
            {
                Name = "NotInDatabaseFormName",
                Created = DateTime.Now,
                Description = "NotInDatabaseDescription",
                Elements = new List<Element>()
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _FormRepository.Update(newForm));
        }

        [Fact]
        public async void UpdateForm_NotNull_InDB()
        {
            var actualForm = await _FormRepository.Get("TestForm1");

            actualForm.Name = "NotHisPreviousName";

            actualForm = await _FormRepository.Update(actualForm);
            actualForm = await _FormRepository.Get("NotHisPreviousName");

            // Checks returned value
            Assert.NotNull(actualForm);
            Assert.Equal("NotHisPreviousName", actualForm.Name);
            Assert.NotEqual(Guid.Empty, actualForm.Id);
        }

        [Fact]
        public async void AddForm_CorrectParts_ReturnsFormWithId()
        {
            var actualProject = await _ProjectRepository.Get("TestProject1");
            var elements = new[]
            {
                    GetTestElementWithoutElementValuesAndOptions(254, true),
                    GetTestElementWithoutElementValuesAndOptions(253, false),
                    GetTestElementWithoutElementValuesAndOptions(252, true),
            };

            var newForm = new Form
            {
                Name = "TestFormNewName",
                Created = DateTime.Now,
                Elements = elements
            };

            Form actualForm = await _FormRepository.Add(actualProject, newForm);

            // Checks returned value
            Assert.NotNull(actualForm);
            Assert.Equal(newForm.Name, actualForm.Name);
            Assert.Equal(newForm.Created, actualForm.Created);
            Assert.NotEqual(Guid.Empty, actualForm.Id);
            Assert.Equal(3, actualForm.Elements.Count);

            // Checks storage is persistant
            Assert.NotNull(_FormRepository.Get(newForm.Name));
        }

        [Fact]
        public async void DeleteForm_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _FormRepository.Delete(null));
        }

        [Fact]
        public async void DeleteForm_NotNull_NotInDb()
        {
            var newForm = new Form
            {
                Name = "TestProjectNotInDb",
                Created = DateTime.Now,
                Description = "TestDescriptionNotInDb",
                Elements = new List<Element>()
            };
            var actualForm = await _FormRepository.Delete(newForm);
            Assert.Null(actualForm);
        }

        [Fact]
        public async void DeleteForm_NotNull_InDb()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public FormRepositoryTests()
        {
            // Mock projects
            Project testProject1 = GetTestProjectWithoutForms(1);
            Project testProject2 = GetTestProjectWithoutForms(2);

            var projectsData = new List<Project>
            {
                testProject1,
                testProject2
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

            // Mock elements
            Element testElement1 = GetTestElementWithoutElementValuesAndOptions(1, true);
            Element testElement2 = GetTestElementWithoutElementValuesAndOptions(2, true);
            Element testElement3 = GetTestElementWithoutElementValuesAndOptions(3, false);
            Element testElement4 = GetTestElementWithoutElementValuesAndOptions(4, false);

            var elementsData = new List<Element>
            {
                testElement1,
                testElement2,
                testElement3,
                testElement4
            };

            // Mock context
            var projectsSubstitute = SubstituteQueryable(projectsData);
            var formsSubstitute = SubstituteQueryable(formsData);
            var elementsSubstitue = SubstituteQueryable(elementsData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Projects.Returns(projectsSubstitute);
            contextSubsitute.Forms.Returns(formsSubstitute);
            contextSubsitute.Elements.Returns(elementsSubstitue);

            _ProjectRepository = new ProjectRepository(contextSubsitute);
            _FormRepository = new FormRepository(contextSubsitute);
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
        /// Creates new test <see cref="Element"/> object with no <see cref="Element.ElementValues"/> and <see cref="Element.Options"/> .
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static Element GetTestElementWithoutElementValuesAndOptions(int identifier, bool mandatory)
        {
            return new Element
            {
                Name = $"TestElement{identifier}",
                Description = $"TestDescription{identifier}",
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}"),
                Type = 1,
                Mandatory = mandatory
            };
        }

        /// <summary>
        /// Formates given <paramref name="identifier"/> to (at least) 12 characters long string where all missing characters are replaced with 0.
        /// <see cref="String"/> formatted in this way can be used as last part a GUID.
        /// </summary>
        private static string FormatForLastGuidPart(int identifier) => identifier.ToString().PadLeft(12, '0');
    }
}
