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

namespace FaaS.Entities.UnitTests
{
    public class TestBase
    {
        protected UserRepository _UserRepository;
        protected ProjectRepository _ProjectRepository;
        protected FormRepository _FormRepository;
        protected ElementRepository _ElementRepository;
        protected ElementValueRepository _ElementValueRepository;
        protected SessionRepository _SessionRepository;

        public TestBase()
        {
            // Mock users
            User testUser1 = GetTestUserWithoutProjects(1);
            User testUser2 = GetTestUserWithoutProjects(2);
            User testUser3 = GetTestUserWithoutProjects(3);

            var usersData = new List<User>
            {
                testUser1,
                testUser2,
                testUser3
            };

            // Mock projects
            Project testProject1 = GetTestProjectWithoutForms(1);
            Project testProject2 = GetTestProjectWithoutForms(2);
            Project testProject3 = GetTestProjectWithoutForms(3);

            var projectsData = new List<Project>
            {
                testProject1,
                testProject2,
                testProject3
            };

            // Mock forms
            Form testForm1 = GetTestFormWithoutElements(1);
            Form testForm2 = GetTestFormWithoutElements(2);
            Form testForm3 = GetTestFormWithoutElements(3);

            var formsData = new List<Form>
            {
                testForm1,
                testForm2,
                testForm3
            };

            Element testElement1 = GetTestElementWithoutElementValuesAndOptions(1, true);
            Element testElement2 = GetTestElementWithoutElementValuesAndOptions(2, false);
            Element testElement3 = GetTestElementWithoutElementValuesAndOptions(3, true);

            var elementsData = new List<Element>
            {
                testElement1,
                testElement2,
                testElement3
            };

            ElementValue testElementValue1 = GetTestElementValueWithoutElementAndSession(1);
            ElementValue testElementValue2 = GetTestElementValueWithoutElementAndSession(2);
            ElementValue testElementValue3 = GetTestElementValueWithoutElementAndSession(3);

            var elementValuesData = new List<ElementValue>
            {
                testElementValue1,
                testElementValue2,
                testElementValue3
            };

            Session testSession1 = GetTestSessionWithoutElementValues(1);
            Session testSession2 = GetTestSessionWithoutElementValues(2);
            Session testSession3 = GetTestSessionWithoutElementValues(3);

            var sessionsData = new List<Session>
            {
                testSession1,
                testSession2,
                testSession3
            };

            // create connections
            testUser1.Projects = new List<Project>
            {
                testProject1
            };
            testUser2.Projects = new List<Project>
            {
                testProject2,
                testProject3
            };
            testProject1.UserId = testUser1.Id;
            testProject2.UserId = testUser2.Id;
            testProject3.UserId = testUser2.Id;

            testProject1.Forms = new List<Form>
            {
                testForm1
            };
            testProject2.Forms = new List<Form>
            {
                testForm2,
                testForm3
            };
            testForm1.ProjectId = testProject1.Id;
            testForm2.ProjectId = testProject2.Id;
            testForm3.ProjectId = testProject2.Id;

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

            testElement1.ElementValues = new List<ElementValue>
            {
                testElementValue1
            };
            testElement2.ElementValues = new List<ElementValue>
            {
                testElementValue2,
                testElementValue3
            };
            testElementValue1.ElementId = testElement1.Id;
            testElementValue2.ElementId = testElement2.Id;
            testElementValue3.ElementId = testElement2.Id;

            testSession1.ElementValues = new List<ElementValue>
            {
                testElementValue1
            };
            testSession2.ElementValues = new List<ElementValue>
            {
                testElementValue2,
                testElementValue3
            };
            testElementValue1.SessionId = testSession1.Id;
            testElementValue2.SessionId = testSession2.Id;
            testElementValue3.SessionId = testSession2.Id;

            // Mock context
            var usersSubstitute = SubstituteQueryable(usersData);
            var projectsSubstitute = SubstituteQueryable(projectsData);
            var formsSubstitute = SubstituteQueryable(formsData);
            var elementsSubstiture = SubstituteQueryable(elementsData);
            var elementValuesSubstitute = SubstituteQueryable(elementValuesData);
            var sessionSubstitute = SubstituteQueryable(sessionsData);

            var contextSubsitute = Substitute.For<FaaSContext>();
            contextSubsitute.Users.Returns(usersSubstitute);
            contextSubsitute.Projects.Returns(projectsSubstitute);
            contextSubsitute.Forms.Returns(formsSubstitute);
            contextSubsitute.Elements.Returns(elementsSubstiture);
            contextSubsitute.ElementValues.Returns(elementValuesSubstitute);
            contextSubsitute.Sessions.Returns(sessionSubstitute);

            _UserRepository = new UserRepository(contextSubsitute);
            _ProjectRepository = new ProjectRepository(contextSubsitute);
            _FormRepository = new FormRepository(contextSubsitute);
            _ElementRepository = new ElementRepository(contextSubsitute);
            _ElementValueRepository = new ElementValueRepository(contextSubsitute);
            _SessionRepository = new SessionRepository(contextSubsitute);
        }

        /// <summary>
        /// Creates substitute for a <see cref="DbSet{TEntity}"/> with database replaced with an in-memory structure represented by <paramref name="data"/>.
        /// Can be used for querying and addition, including async operations.
        /// </summary>
        /// <typeparam name="TType">Type of data and <see cref="DbSet{TEntity}"/> to substitute</typeparam>
        /// <param name="data">Initial content of "database"</param>
        /// <returns>Queryable that can be used as <see cref="DbSet{TEntity}"/> substitute.</returns>
        protected static IQueryable<TType> SubstituteQueryable<TType>(ICollection<TType> data)
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

            ((DbSet<TType>)queryableSubstitute).Remove(null).ReturnsForAnyArgs(callInfo => SimulateRemove(callInfo, data));

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
        /// Reads <typeparamref name="TType"/> from <paramref name="callInfo"/> and stores it to the <paramref name="data"/>.
        /// To emulate reald DB, it also sets <see cref="ModelBase.Id"/> with new <see cref="Guid"/> and returns the very
        /// object the method was provided with.
        /// </summary>
        private static TType SimulateRemove<TType>(CallInfo callInfo, ICollection<TType> data)
            where TType : ModelBase
        {
            TType entry = callInfo.Arg<TType>();

            data.Remove(callInfo.Arg<TType>());

            return entry;
        }

        /// <summary>
        /// Formates given <paramref name="identifier"/> to (at least) 12 characters long string where all missing characters are replaced with 0.
        /// <see cref="String"/> formatted in this way can be used as last part a GUID.
        /// </summary>
        protected static string FormatForLastGuidPart(int identifier) => identifier.ToString().PadLeft(12, '0');

        /// <summary>
        /// Creates new test <see cref="User"/> object with no <see cref="User.Projects"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        protected static User GetTestUserWithoutProjects(int identifier)
        {
            return new User
            {
                GoogleToken = $"TestGoogleId{identifier}",
                Registered = new DateTime(2000, 1, identifier, 1, 1, 0),
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }

        /// <summary>
        /// Creates new test <see cref="Project"/> object with no <see cref="Project.Forms"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        protected static Project GetTestProjectWithoutForms(int identifier)
        {
            return new Project
            {
                Name = $"TestProject{identifier}",
                Created = new DateTime(2000, 1, identifier, 1, 1, 0),
                Description = $"TestDescription{identifier}",
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }

        /// <summary>
        /// Creates new test <see cref="Form"/> object with no <see cref="Form.Elements"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        protected static Form GetTestFormWithoutElements(int identifier)
        {
            return new Form
            {
                Name = $"TestForm{identifier}",
                Created = new DateTime(2000, 1, identifier, 1, 1, 0),
                Description = $"TestDescription{identifier}",
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }

        /// <summary>
        /// Creates new test <see cref="Element"/> object with no <see cref="Element.ElementValues"/> and <see cref="Element.Options"/> .
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        protected static Element GetTestElementWithoutElementValuesAndOptions(int identifier, bool mandatory)
        {
            return new Element
            {
                Description = $"TestDescription{identifier}",
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}"),
                Options = $"TestOption{identifier}",
                Type = 1,
                Required = mandatory
            };
        }

        /// <summary>
        /// Creates new test <see cref="Option"/> object/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        protected static ElementValue GetTestElementValueWithoutElementAndSession(int identifier)
        {
            return new ElementValue
            {
                Value = $"TestValue{identifier}",
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }

        /// <summary>
        /// Creates new test <see cref="Session"/> object with no <see cref="Session.ElementValues"/>.
        /// Object name and identifier is accompanied with <paramref name="identifier"/>.
        /// </summary>
        protected static Session GetTestSessionWithoutElementValues(int identifier)
        {
            return new Session
            {
                Filled = new DateTime(2000, 1, identifier, 1, 1, 0),
                Id = new Guid($"{{00000000-1111-0000-0000-{FormatForLastGuidPart(identifier)}}}")
            };
        }
    }
}
