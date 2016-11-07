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
    public class SessionRepositoryTests : TestBase
    {
        [Fact]
        public async void AddSession_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _SessionRepository.Add(null));
        }

        [Fact]
        public async void AddSession_NotNull_ReturnsSessionWithId()
        {
            var newSession = new DataTransferModels.Session
            {
                Filled = DateTime.Now
            };
            var actualSession = await _SessionRepository.Add(newSession);

            // Checks returned value
            Assert.NotNull(actualSession);
            Assert.Equal(newSession.Filled, actualSession.Filled);
            Assert.NotEqual(Guid.Empty, actualSession.Id);
        }

        [Fact]
        public async void GetSession_NotNull_ReturnsSessionWithId()
        {
            var guid = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var actualSession = await _SessionRepository.Get(guid);
        
            Assert.NotNull(actualSession);
            Assert.Equal(guid, actualSession.Id);
        }

        [Fact]
        public async void GetSession_Null_ReturnsNullSessionForWrongGuid()
        {
            var guid = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(100)}}}");
            var actualSession = await _SessionRepository.Get(guid);
            Assert.Null(actualSession);
        }

        [Fact]
        public async void GetAllSessions_ReturnsAllSessionInstances()
        {
            var actualSessions = await _SessionRepository.List();

            Assert.NotNull(actualSessions);
            Assert.NotEmpty(actualSessions);
            Assert.Equal(3, actualSessions.Count());
            foreach (var actualSession in actualSessions)
            {
                Assert.IsType<DataTransferModels.Session>(actualSession);
            }
        }

        [Fact]
        public async void UpdateSession_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _SessionRepository.Update(null));
        }

        [Fact]
        public async void UpdateSession_NotNull_NotInDB()
        {
            var newSession = new DataTransferModels.Session
            {
                Filled = DateTime.Now
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _SessionRepository.Update(newSession));
        }

        [Fact]
        public async void UpdateSession_NotNull_InDB()
        {
            var guid = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var initialDate = new DateTime(2016, 5, 19, 3, 15, 0);

            var actualSession = await _SessionRepository.Get(guid);

            actualSession.Filled = initialDate.AddDays(7);

            actualSession = await _SessionRepository.Update(actualSession);

            // Checks returned value
            Assert.NotNull(actualSession);
            Assert.Equal(initialDate.AddDays(7), actualSession.Filled);
            Assert.Equal(guid, actualSession.Id);
            Assert.NotEqual(Guid.Empty, actualSession.Id);
        }

        [Fact]
        public async void DeleteSession_Null_Throws()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _SessionRepository.Delete(null));
        }

        [Fact]
        public async void DeleteSession_NotNull_NotInDB()
        {
            var newSession = new DataTransferModels.Session
            {
                Filled = DateTime.Now
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _SessionRepository.Delete(newSession));
        }

        [Fact]
        public async void DeleteSession_NotNull_InDB()
        {
            var guid = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(1)}}}");
            var actualSession = await _SessionRepository.Get(guid);

            actualSession = await _SessionRepository.Delete(actualSession);

            // Checks returned value
            Assert.NotNull(actualSession);
            Assert.Equal(guid, actualSession.Id);
            Assert.NotEqual(Guid.Empty, actualSession.Id);

            actualSession = await _SessionRepository.Get(guid);
            Assert.Null(actualSession);

            //var actualUsers = await _UserRepository.List();
            //Assert.Equal(2, actualUsers.Count());
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx
        /// List has to be used in order for addition to be possible.
        /// </summary>
        public SessionRepositoryTests()// : base()
        {
            // Mock sessions
            var testSession1 = GetTestSessionWithoutElementValues(1);
            var testSession2 = GetTestSessionWithoutElementValues(2);
            var testSession3 = GetTestSessionWithoutElementValues(3);

            var sessionData = new List<Session>
            {
                testSession1,
                testSession2,
                testSession3
            };

            // Mock users
            var testElementValue1 = GetTestElementValueWithoutElementAndSession(1);
            var testElementValue2 = GetTestElementValueWithoutElementAndSession(2);
            var testElementValue3 = GetTestElementValueWithoutElementAndSession(3);
            var elementValueData = new List<ElementValue>
            {
                testElementValue1,
                testElementValue2,
                testElementValue3
            };

            // Mock context
            var sessionsSubstitute = SubstituteQueryable(sessionData);
            var elementValuesSubstitute = SubstituteQueryable(elementValueData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Sessions.Returns(sessionsSubstitute);
            contextSubsitute.ElementValues.Returns(elementValuesSubstitute);

            _SessionRepository = new SessionRepository(contextSubsitute);
        }
    }
}
