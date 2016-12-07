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
    public class ElementValueRepositoryTests : TestBase
    {
        [Fact]
        public async void GetSingleElementValue_Existing_ReturnsElementValue()
        {
            var actualElementValue = await _ElementValueRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            Assert.NotNull(actualElementValue);
            Assert.IsType<DataTransferModels.ElementValue>(actualElementValue);
        }

        [Fact]
        public async void GetAllElementElementValues_ReturnsAllElementValueInstancesForElement()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            DataTransferModels.Element actualElement = await _ElementRepository.Get(id);
            var actualElementValues = await _ElementValueRepository.List(actualElement);

            Assert.NotNull(actualElementValues);
            Assert.NotEmpty(actualElementValues);
            Assert.Equal(3, actualElementValues.Count());
            foreach (var actualElementValue in actualElementValues)
            {
                Assert.IsType<DataTransferModels.ElementValue>(actualElementValue);
            }
        }

        [Fact]
        public async void GetAllElementValues_ReturnsAllElementValueInstances()
        {
            var actualElementValues = await _ElementValueRepository.List();

            Assert.NotNull(actualElementValues);
            Assert.NotEmpty(actualElementValues);
            Assert.Equal(4, actualElementValues.Count());
            foreach (var actualElementValue in actualElementValues)
            {
                Assert.IsType<DataTransferModels.ElementValue>(actualElementValue);
            }
        }

        [Fact]
        public async void AddElementValue_Null_Throws()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            DataTransferModels.Element actualElement = await _ElementRepository.Get(id);
            DataTransferModels.Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Add(actualElement, actualSession, null));
        }

        [Fact]
        public async void AddElementValue_NullElement_Throws()
        {
            DataTransferModels.ElementValue newElementValue = new DataTransferModels.ElementValue
            {
                Value = "NotInDbValue"
            };
            DataTransferModels.Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Add(null, actualSession, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NullSession_Throws()
        {
            DataTransferModels.ElementValue newElementValue = new DataTransferModels.ElementValue
            {
                Value = "NotInDbValue"
            };
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            DataTransferModels.Element actualElement = await _ElementRepository.Get(id);
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Add(actualElement, null, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NotNull_ElementNotInDb()
        {
            DataTransferModels.ElementValue newElementValue = new DataTransferModels.ElementValue
            {
                Value = "NotInDbValue"
            };
            DataTransferModels.Element newElement = new DataTransferModels.Element()
            {
                Type = 2,
                Description = "TestDescriptionNotInDb",
                //ElementValues = new List<ElementValue>(),
                Required = true,
                Options = "TestOption"
            };
            DataTransferModels.Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            await Assert.ThrowsAsync<ArgumentException>(() => _ElementValueRepository.Add(newElement, actualSession, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NotNull_SessionNotInDb()
        {
            DataTransferModels.ElementValue newElementValue = new DataTransferModels.ElementValue
            {
                Value = "NotInDbValue"
            };
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            DataTransferModels.Element actualElement = await _ElementRepository.Get(id);
            DataTransferModels.Session newSession = new DataTransferModels.Session()
            {
                Filled = DateTime.Now,
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _ElementValueRepository.Add(actualElement, newSession, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NotNull_ReturnsElementValueWithId()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            DataTransferModels.Element actualElement = await _ElementRepository.Get(id);

            var newElementValue = new DataTransferModels.ElementValue
            {
                Value = "TestValue5"
            };
            DataTransferModels.Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            DataTransferModels.ElementValue actualElementValue = await _ElementValueRepository.Add(actualElement, actualSession, newElementValue);

            // Checks returned value
            Assert.NotNull(actualElementValue);
            Assert.Equal(newElementValue.Value, actualElementValue.Value);
            Assert.NotEqual(Guid.Empty, actualElementValue.Id);

            // Check storage is persistant
            Assert.NotNull(_ElementValueRepository.Get(actualElementValue.Id));
        }

        [Fact]
        public async void UpdateElementValue_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Update(null));
        }

        [Fact]
        public async void UpdateElementValue_NotNull_NotInDB()
        {
            var newElementValue = new DataTransferModels.ElementValue
            {
                Value = "ValueNotInDb"
            };

            var updatedElement = await _ElementValueRepository.Update(newElementValue);
            Assert.Null(updatedElement);
        }

        [Fact]
        public async void UpdateElementValue_NotNull_InDB()
        {
            var actualElementValue = await _ElementValueRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            actualElementValue.Value = "NotPreviousValue";

            actualElementValue = await _ElementValueRepository.Update(actualElementValue);
            actualElementValue = await _ElementValueRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            // Checks returned value
            Assert.NotNull(actualElementValue);
            Assert.Equal("NotPreviousValue", actualElementValue.Value);
            Assert.NotEqual(Guid.Empty, actualElementValue.Id);
        }

        [Fact]
        public async void AddElementValue_CorrectParts_ReturnsElementValueWithId()
        {
            var id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            DataTransferModels.Element actualElement = await _ElementRepository.Get(id);
            DataTransferModels.Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            var newElementValue = new DataTransferModels.ElementValue
            {
                Value = "TestElementValueNewValue"
            };

            DataTransferModels.ElementValue actualElementValue = await _ElementValueRepository.Add(actualElement, actualSession, newElementValue);

            // Checks returned value
            Assert.NotNull(actualElementValue);
            Assert.Equal(newElementValue.Value, actualElementValue.Value);
            Assert.NotEqual(Guid.Empty, actualElementValue.Id);

            // Checks storage is persistant
            Assert.NotNull(_ElementValueRepository.Get(actualElementValue.Id));
        }

        [Fact]
        public async void DeleteElementValue_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Delete(null));
        }

        [Fact]
        public async void DeleteElementValue_NotNull_NotInDb()
        {
            var newElementValue = new DataTransferModels.ElementValue
            {
                Value = "TestElementValueNotInDb"
            };
            var actualElementValue = await _ElementValueRepository.Delete(newElementValue);
            Assert.Null(actualElementValue);
        }

        [Fact]
        public async void DeleteElementVAlue_NotNull_InDb()
        {
            var elementValueToDelete = new DataTransferModels.ElementValue
            {
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"),
                Value = "test"
            };

            var deletedElement = await _ElementValueRepository.Delete(elementValueToDelete);

            // Checks returned value
            Assert.NotNull(deletedElement);
            Assert.Equal("TestValue1", deletedElement.Value);
            Assert.NotEqual(Guid.Empty, deletedElement.Id);

            deletedElement = await _ElementValueRepository.Get(elementValueToDelete.Id);
            Assert.Null(deletedElement);
        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public ElementValueRepositoryTests()
        {
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

            // Mock element values
            ElementValue testElementValue1 = GetTestElementValueWithoutElementAndSession(1);
            ElementValue testElementValue2 = GetTestElementValueWithoutElementAndSession(2);
            ElementValue testElementValue3 = GetTestElementValueWithoutElementAndSession(3);
            ElementValue testElementValue4 = GetTestElementValueWithoutElementAndSession(4);

            var elementValuesData = new List<ElementValue>
            {
                testElementValue1,
                testElementValue2,
                testElementValue3,
                testElementValue4
            };

            // Mock elements
            Element testElement1 = GetTestElementWithoutElementValuesAndOptions(1, true);
            Element testElement2 = GetTestElementWithoutElementValuesAndOptions(2, true);
            Element testElement3 = GetTestElementWithoutElementValuesAndOptions(3, false);
            Element testElement4 = GetTestElementWithoutElementValuesAndOptions(4, false);

            testElement1.ElementValues = new List<ElementValue>
            {
                testElementValue1,
                testElementValue2,
                testElementValue3
            };

            testElementValue1.Element = testElement1;
            testElementValue2.Element = testElement1;
            testElementValue3.Element = testElement1;
            testElementValue1.ElementId = testElement1.Id;
            testElementValue2.ElementId = testElement1.Id;
            testElementValue3.ElementId = testElement1.Id;

            var elementsData = new List<Element>
            {
                testElement1,
                testElement2,
                testElement3,
                testElement4
            };

            testForm1.Elements = new List<Element>
            {
                testElement1
            };
            testForm2.Elements = new List<Element>
            {
                testElement2,
                testElement3
            };
            testElement1.FormId = testForm1.Id;
            testElement2.FormId = testForm2.Id;
            testElement3.FormId = testForm2.Id;

            // Mock sessions
            Session testSession1 = GetTestSessionWithoutElementValues(1);

            var sessionsData = new List<Session>
            {
                testSession1
            };

            // Mock context
            var formsSubstitute = SubstituteQueryable(formsData);
            var elementsSubstitute = SubstituteQueryable(elementsData);
            var optionsSubstitute = SubstituteQueryable(elementValuesData);
            var sessionsSubstitute = SubstituteQueryable(sessionsData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Forms.Returns(formsSubstitute);
            contextSubsitute.Elements.Returns(elementsSubstitute);
            contextSubsitute.ElementValues.Returns(optionsSubstitute);
            contextSubsitute.Sessions.Returns(sessionsSubstitute);

            _ElementValueRepository = new ElementValueRepository(contextSubsitute);
            _ElementRepository = new ElementRepository(contextSubsitute);
            _SessionRepository = new SessionRepository(contextSubsitute);
        }
    }
}
