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
using System.Threading.Tasks;
using Xunit;

namespace FaaS.Entities.UnitTests
{
    public class SessionRepositoryTests
    {
        private SessionRepository _SessionRepository;

        [Fact]
        public async void AddSession_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _SessionRepository.Add(null));
        }

        [Fact]
        public async void AddSession_NotNull_ReturnsSessionWithId()
        {
            var newSession = new Session
            {
                Filled = DateTime.Now,
                ElementValues = new List<ElementValue>()
            };
            Session actualSession = await _SessionRepository.Add(newSession);

            // Checks returned value
            Assert.NotNull(actualSession);
            Assert.Equal(newSession.Filled, actualSession.Filled);
            Assert.NotEqual(Guid.Empty, actualSession.Id);
        }

        [Fact]
        public async void UpdateSession_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _SessionRepository.Update(null));
        }

        [Fact]
        public async void UpdateSession_NotNull_NotInDB()
        {
            var newSession = new Session
            {
                Filled = DateTime.Now,
                ElementValues = new List<ElementValue>()
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _SessionRepository.Update(newSession));
        }

        [Fact]
        public async void UpdateSession_NotNull_InDB()
        {
            Session actualSession = new Session
            {
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}")
            };

            actualSession.Filled = new DateTime(2016, 5, 19, 3, 15, 0).AddDays(7);

            actualSession = await _SessionRepository.Update(actualSession);

            // Checks returned value
            Assert.NotNull(actualSession);
            Assert.Equal(new DateTime(2016, 5, 19, 3, 15, 0).AddDays(7), actualSession.Filled);
            Assert.Equal(new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}"), actualSession.Id);
            Assert.NotEqual(Guid.Empty, actualSession.Id);
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public SessionRepositoryTests()
        {
            // Mock awards
            Session testSession1 = GetTestSessionWithoutElementValues(1);
            Session testSession2 = GetTestSessionWithoutElementValues(2);

            var sessionData = new List<Session>
            {
                testSession1,
                testSession2
            };

            // Mock users
            ElementValue testElementValue1 = GetTestElementValueWithoutElementAndSession(1);
            ElementValue testElementValue2 = GetTestElementValueWithoutElementAndSession(1);
            ElementValue testElementValue3 = GetTestElementValueWithoutElementAndSession(1);
            ElementValue testElementValue4 = GetTestElementValueWithoutElementAndSession(1);
            var elementValueData = new List<ElementValue>
            {
                testElementValue1,
                testElementValue2,
                testElementValue3,
                testElementValue4,
            };

            // Mock context
            var sessionsSubstitute = SubstituteQueryable(sessionData);
            var elementValuesSubstitute = SubstituteQueryable(elementValueData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Sessions.Returns(sessionsSubstitute);
            contextSubsitute.ElementValues.Returns(elementValuesSubstitute);

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
        /// Creates new test <see cref="ElementValue"/> object with no <see cref="Element"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        private static ElementValue GetTestElementValueWithoutElementAndSession(int identifier)
        {
            return new ElementValue
            {
                Element = new Element(),
                ElementId = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}"),
                Session = new Session(),
                SessionId = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}"),
                Value = $"TestValue{identifier}"
            };
        }

        /// <summary>
        /// Formates given <paramref name="identifier"/> to (at least) 12 characters long string where all missing characters are replaced with 0.
        /// <see cref="String"/> formatted in this way can be used as last part a GUID.
        /// </summary>
        private static string FormatForLastGuidPart(int identifier) => identifier.ToString().PadLeft(12, '0');
    }
}
