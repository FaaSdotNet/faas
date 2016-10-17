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
    public class ElementValueRepositoryTests
    {
        private readonly ElementValueRepository _ElementValueRepository;
        private readonly ElementRepository _ElementRepository;
        private readonly SessionRepository _SessionRepository;

        [Fact]
        public async void GetSingleElementValue_Existing_ReturnsElementValue()
        {
            var actualElementValue = await _ElementValueRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            Assert.NotNull(actualElementValue);
            Assert.IsType<ElementValue>(actualElementValue);
        }

        [Fact]
        public async void GetAllElementElementValues_ReturnsAllElementValueInstancesForElement()
        {
            Element actualElement = await _ElementRepository.Get("TestElement1");
            var actualElementValues = await _ElementValueRepository.List(actualElement);

            Assert.NotNull(actualElementValues);
            Assert.NotEmpty(actualElementValues);
            Assert.Equal(3, actualElementValues.Count());
            foreach (var actualElementValue in actualElementValues)
            {
                Assert.IsType<ElementValue>(actualElementValue);
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
                Assert.IsType<ElementValue>(actualElementValue);
            }
        }

        [Fact]
        public async void AddElementValue_Null_Throws()
        {
            Element actualElement = await _ElementRepository.Get("TestElement1");
            Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Add(actualElement, actualSession, null));
        }

        [Fact]
        public async void AddElementValue_NullElement_Throws()
        {
            ElementValue newElementValue = new ElementValue
            {
                Value = "NotInDbValue"
            };
            Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Add(null, actualSession, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NullSession_Throws()
        {
            ElementValue newElementValue = new ElementValue
            {
                Value = "NotInDbValue"
            };
            Element actualElement = await _ElementRepository.Get("TestElement1");
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ElementValueRepository.Add(actualElement, null, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NotNull_ElementNotInDb()
        {
            ElementValue newElementValue = new ElementValue
            {
                Value = "NotInDbValue"
            };
            Element newElement = new Element()
            {
                Name = "TestElementNotInDb",
                Type = 2,
                Description = "TestDescriptionNotInDb",
                ElementValues = new List<ElementValue>(),
                Mandatory = true,
                Options = new List<Option>()
            };
            Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));

            await Assert.ThrowsAsync<ArgumentException>(() => _ElementValueRepository.Add(newElement, actualSession, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NotNull_SessionNotInDb()
        {
            ElementValue newElementValue = new ElementValue
            {
                Value = "NotInDbValue"
            };
            Element actualElement = await _ElementRepository.Get("TestElement1");
            Session newSession = new Session()
            {
                Filled = DateTime.Now,
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _ElementValueRepository.Add(actualElement, newSession, newElementValue));
        }

        [Fact]
        public async void AddElementValue_NotNull_ReturnsElementValueWithId()
        {
            Element actualElement = await _ElementRepository.Get("TestElement1");

            var newElementValue = new ElementValue
            {
                Value = "TestValue5"
            };
            Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            ElementValue actualElementValue = await _ElementValueRepository.Add(actualElement, actualSession, newElementValue);

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
            var newElementValue = new ElementValue
            {
                Value = "ValueNotInDb"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _ElementValueRepository.Update(newElementValue));
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
            Element actualElement = await _ElementRepository.Get("TestElement1");
            Session actualSession = await _SessionRepository.Get(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"));
            var newElementValue = new ElementValue
            {
                Value = "TestElementValueNewValue"
            };

            ElementValue actualElementValue = await _ElementValueRepository.Add(actualElement, actualSession, newElementValue);

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
            var newElementValue = new ElementValue
            {
                Value = "TestElementValueNotInDb"
            };
            var actualElementValue = await _ElementValueRepository.Delete(newElementValue);
            Assert.Null(actualElementValue);
        }

        [Fact]
        public async void DeleteElementVAlue_NotNull_InDb()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public ElementValueRepositoryTests()
        {
            // Mock element values
            ElementValue testElementValue1 = GetTestElementValue(1);
            ElementValue testElementValue2 = GetTestElementValue(2);
            ElementValue testElementValue3 = GetTestElementValue(3);
            ElementValue testElementValue4 = GetTestElementValue(4);

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

            // Mock sessions
            Session testSession1 = GetTestSessionWithoutElementValues(1);

            var sessionsData = new List<Session>
            {
                testSession1
            };

            // Mock context
            var elementsSubstitute = SubstituteQueryable(elementsData);
            var optionsSubstitute = SubstituteQueryable(elementValuesData);
            var sessionsSubstitute = SubstituteQueryable(sessionsData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Elements.Returns(elementsSubstitute);
            contextSubsitute.ElementValues.Returns(optionsSubstitute);
            contextSubsitute.Sessions.Returns(sessionsSubstitute);

            _ElementValueRepository = new ElementValueRepository(contextSubsitute);
            _ElementRepository = new ElementRepository(contextSubsitute);
            _SessionRepository = new SessionRepository(contextSubsitute);
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
        /// Creates new test <see cref="Session"/> object with no <see cref="Session.ElementValues"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static Session GetTestSessionWithoutElementValues(int identifier)
        {
            return new Session
            {
                Filled = new DateTime(2016, 5, 19, 3, 15, 0).AddDays(identifier),
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }

        /// <summary>
        /// Creates new test <see cref="Option"/> object/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static ElementValue GetTestElementValue(int identifier)
        {
            return new ElementValue
            {
                Value = $"TestValue{identifier}",
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
