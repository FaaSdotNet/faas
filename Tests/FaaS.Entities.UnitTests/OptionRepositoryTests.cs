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
    public class OptionRepositoryTests
    {
        private readonly OptionRepository _OptionRepository;
        private readonly ElementRepository _ElementRepository;

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

            await Assert.ThrowsAsync<ArgumentException>(() => _OptionRepository.Update(newOption));
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
            throw new NotImplementedException();
        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public OptionRepositoryTests()
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
        /// Creates new test <see cref="Option"/> object/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static Option GetTestOption(int identifier)
        {
            return new Option
            {
                Label = $"TestOption{identifier}",
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
