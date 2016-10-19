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
    public class OptionRepositoryTests : TestBase
    {
        [Fact]
        public async void GetSingleOption_Existing_ReturnsOption()
        {
            var actualOption = await _OptionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            Assert.NotNull(actualOption);
            Assert.IsType<Option>(actualOption);
        }

        [Fact]
        public async void GetAllElementOptions_ReturnsAllOptionInstancesForElement()
        {
            Element actualElement = await _ElementRepository.Get("TestElement1");
            var actualOptions = await _OptionRepository.List(actualElement);

            Assert.NotNull(actualOptions);
            Assert.NotEmpty(actualOptions);
            Assert.Equal(3, actualOptions.Count());
            foreach (var actualOption in actualOptions)
            {
                Assert.IsType<Option>(actualOption);
            }
        }

        [Fact]
        public async void GetAllOptions_ReturnsAllOptionInstances()
        {
            var actualOptions = await _OptionRepository.List();

            Assert.NotNull(actualOptions);
            Assert.NotEmpty(actualOptions);
            Assert.Equal(4, actualOptions.Count());
            foreach (var actualOption in actualOptions)
            {
                Assert.IsType<Option>(actualOption);
            }
        }

        [Fact]
        public async void AddOption_Null_Throws()
        {
            Element actualElement = await _ElementRepository.Get("TestElement1");
            await Assert.ThrowsAsync<ArgumentNullException>(() => _OptionRepository.Add(actualElement, null));
        }

        [Fact]
        public async void AddOption_NullElement_Throws()
        {
            Element actualElement = null;
            Option newOption = new Option
            {
                Label = "TestOptionNotInDb"
            };
            await Assert.ThrowsAsync<ArgumentNullException>(() => _OptionRepository.Add(actualElement, newOption));
        }

        [Fact]
        public async void AddOption_NotNull_ReturnsOptionWithId()
        {
            Element actualElement = await _ElementRepository.Get("TestElement1");

            var newOption = new Option
            {
                Label = "TestOption5"
            };
            Option actualOption = await _OptionRepository.Add(actualElement, newOption);

            // Checks returned value
            Assert.NotNull(actualOption);
            Assert.Equal(newOption.Label, actualOption.Label);
            Assert.NotEqual(Guid.Empty, actualOption.Id);

            // Check storage is persistant
            Assert.NotNull(_OptionRepository.Get(actualOption.Id));
        }

        [Fact]
        public async void UpdateOption_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _OptionRepository.Update(null));
        }

        [Fact]
        public async void UpdateOption_NotNull_NotInDB()
        {
            var newOption = new Option
            {
                Label = "NotInDbOptionLabel"
            };

            var updatedOption = await _OptionRepository.Update(newOption);
            Assert.Null(updatedOption);
        }

        [Fact]
        public async void UpdateOption_NotNull_InDB()
        {
            var actualOption = await _OptionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            actualOption.Label = "NotPreviousLabel";

            actualOption = await _OptionRepository.Update(actualOption);
            actualOption = await _OptionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            // Checks returned value
            Assert.NotNull(actualOption);
            Assert.Equal("NotPreviousLabel", actualOption.Label);
            Assert.NotEqual(Guid.Empty, actualOption.Id);
        }

        [Fact]
        public async void AddOption_CorrectParts_ReturnsOptionWithId()
        {
            var actualElement = await _ElementRepository.Get("TestElement1");

            var newOption = new Option {
                Label = "TestOptionNewName"
            };

            Option actualOption = await _OptionRepository.Add(actualElement, newOption);

            // Checks returned value
            Assert.NotNull(actualOption);
            Assert.Equal(newOption.Label, actualOption.Label);
            Assert.NotEqual(Guid.Empty, actualOption.Id);

            // Checks storage is persistant
            Assert.NotNull(_OptionRepository.Get(actualOption.Id));
        }

        [Fact]
        public async void DeleteOption_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _OptionRepository.Delete(null));
        }

        [Fact]
        public async void DeleteOption_NotNull_NotInDb()
        {
            var newOption = new Option
            {
                Label = "TestElementNotInDb"
            };
            var actualOption = await _OptionRepository.Delete(newOption);
            Assert.Null(actualOption);
        }

        [Fact]
        public async void DeleteOption_NotNull_InDb()
        {
            Option optionToDelete = GetTestOption(1);

            var deletedOption = await _OptionRepository.Delete(optionToDelete);

            // Checks returned value
            Assert.NotNull(deletedOption);
            Assert.Equal("TestOption1", deletedOption.Label);
            Assert.NotEqual(Guid.Empty, deletedOption.Id);

            deletedOption = await _OptionRepository.Get(optionToDelete.Id);
            Assert.Null(deletedOption);
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public OptionRepositoryTests()// : base()
        {
            // Mock options
            Option testOption1 = GetTestOption(1);
            Option testOption2 = GetTestOption(2);
            Option testOption3 = GetTestOption(3);
            Option testOption4 = GetTestOption(4);

            var optionsData = new List<Option>
            {
                testOption1,
                testOption2,
                testOption3,
                testOption4
            };

            // Mock elements
            Element testElement1 = GetTestElementWithoutElementValuesAndOptions(1, true);
            Element testElement2 = GetTestElementWithoutElementValuesAndOptions(2, true);
            Element testElement3 = GetTestElementWithoutElementValuesAndOptions(3, false);
            Element testElement4 = GetTestElementWithoutElementValuesAndOptions(4, false);

            testElement1.Options = new List<Option>
            {
                testOption1,
                testOption2,
                testOption3
            };

            testOption1.Element = testElement1;
            testOption2.Element = testElement1;
            testOption3.Element = testElement1;
            testOption1.ElementId = testElement1.Id;
            testOption2.ElementId = testElement1.Id;
            testOption3.ElementId = testElement1.Id;

            var elementsData = new List<Element>
            {
                testElement1,
                testElement2,
                testElement3,
                testElement4
            };

            // Mock context
            var elementsSubstitute = SubstituteQueryable(elementsData);
            var optionsSubstitute = SubstituteQueryable(optionsData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Elements.Returns(elementsSubstitute);
            contextSubsitute.Options.Returns(optionsSubstitute);

            _OptionRepository = new OptionRepository(contextSubsitute);
            _ElementRepository = new ElementRepository(contextSubsitute);
        }
    }
}
