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
    public class FormRepositoryTests : TestBase
    {
        [Fact]
        public async void GetSingleForm_Existing_ReturnsForm()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var actualForm = await _FormRepository.Get(id);

            Assert.NotNull(actualForm);
            Assert.IsType<Form>(actualForm);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void GetSingleForm_NullOrEmpty_ReturnsForm(Guid id)
        {
            var actualForm = await _FormRepository.Get(id);

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
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            Project actualProject = await _ProjectRepository.Get(id);
            await Assert.ThrowsAsync<ArgumentNullException>(() => _FormRepository.Add(actualProject, null));
        }

        [Fact]
        public async void AddForm_NotNull_ReturnsFormWithId()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            Project actualProject = await _ProjectRepository.Get(id);

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
            Assert.NotNull(_FormRepository.Get(newForm.Id));
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

            var updatedForm = await _FormRepository.Update(newForm);
            Assert.Null(updatedForm);
        }

        [Fact]
        public async void UpdateForm_NotNull_InDB()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var actualForm = await _FormRepository.Get(id);

            actualForm.Name = "NotHisPreviousName";

            actualForm = await _FormRepository.Update(actualForm);
            actualForm = await _FormRepository.Get(id);

            // Checks returned value
            Assert.NotNull(actualForm);
            Assert.Equal("NotHisPreviousName", actualForm.Name);
            Assert.NotEqual(Guid.Empty, actualForm.Id);
        }

        [Fact]
        public async void AddForm_CorrectParts_ReturnsFormWithId()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var actualProject = await _ProjectRepository.Get(id);
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
            Assert.NotNull(_FormRepository.Get(actualForm.Id));
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
            Form formToDelete = GetTestFormWithoutElements(1);

            var deletedForm = await _FormRepository.Delete(formToDelete);

            // Checks returned value
            Assert.NotNull(deletedForm);
            Assert.Equal("TestForm1", deletedForm.Name);
            Assert.NotEqual(Guid.Empty, deletedForm.Id);

            deletedForm = await _FormRepository.Get(formToDelete.Id);
            Assert.Null(deletedForm);
        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public FormRepositoryTests()// : base()
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
    }
}
