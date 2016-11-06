using AutoMapper;
using FaaS.Entities.Configuration;
using FaaS.Entities.DataAccessModels;
using System;
using System.Reflection;
using Xunit;

namespace FaaS.Services.UnitTests
{
    public class MapperConfigurationTests
    {
        [Fact]
        public void AssertMapperConfiguration()
        {
            // Ensures no properties are missed when mapping from narrow to wide type
            GetMapperConfiguration().AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_ServiceUser_DatabaseUser()
        {
            // Ensures mapping from DTO user to DAO user is present
            var dataTransportUserModel = new FaaS.DataTransferModels.User
            {
                UserName = "Display Name",
                GoogleId = "TestGoogleId"
            };
            var expectedDataAccessUserModel = new User
            {
                Name = "Display Name",
                GoogleId = "TestGoogleId"
            };

            var actualDataAccessUserModel = GetMapper().Map<FaaS.DataTransferModels.User, User>(dataTransportUserModel);

            AssertPropertyEquals(expectedDataAccessUserModel, actualDataAccessUserModel);
        }

        [Fact]
        public void Map_DatabaseUser_ServiceUser()
        {
            // Ensures mapping from DAO user to DTO usr is present
            var dataAccessUserModel = new User
            {
                Name = "Display Name",
                GoogleId = "TestGoogleId"
            };
            var expectedDataTransportUserModel = new FaaS.DataTransferModels.User
            {
                UserName = "Display Name",
                GoogleId = "TestGoogleId"
            };

            var actualDataTransportUserModel = GetMapper().Map<User, FaaS.DataTransferModels.User>(dataAccessUserModel);

            AssertPropertyEquals(expectedDataTransportUserModel, actualDataTransportUserModel);
        }

        [Fact]
        public void Map_ServiceProject_DatabaseProject()
        {
            // Ensures mapping from DTO project to DAO project is present
            var dataTransportProjectModel = new FaaS.DataTransferModels.Project
            {
                ProjectName = "Display Name",
                Description = "TestDescription"
            };
            var expectedDataAccessProjectModel = new Project
            {
                Name = "Display Name",
                Description = "TestDescription"
            };

            var actualDataAccessProjectModel = GetMapper().Map<FaaS.DataTransferModels.Project, Project>(dataTransportProjectModel);

            AssertPropertyEquals(expectedDataAccessProjectModel, actualDataAccessProjectModel);
        }

        [Fact]
        public void Map_DatabaseProject_ServiceProject()
        {
            // Ensures mapping from DAO project to DTO project is present
            var dataAccessProjectModel = new Project
            {
                Name = "Display Name",
                Description = "TestDescription"
            };
            var expectedDataTransportProjectModel = new FaaS.DataTransferModels.Project
            {
                ProjectName = "Display Name",
                Description = "TestDescription"
            };

            var actualDataTransportProjectModel = GetMapper().Map<Project, FaaS.DataTransferModels.Project>(dataAccessProjectModel);

            AssertPropertyEquals(expectedDataTransportProjectModel, actualDataTransportProjectModel);
        }

        [Fact]
        public void Map_ServiceForm_DatabaseForm()
        {
            // Ensures mapping from DTO form to DAO form is present
            var dataTransportFormModel = new FaaS.DataTransferModels.Form
            {
                FormName = "Display Name",
                Description = "TestDescription"
            };
            var expectedDataAccessFormModel = new Form
            {
                Name = "Display Name",
                Description = "TestDescription"
            };

            var actualDataAccessFormModel = GetMapper().Map<FaaS.DataTransferModels.Form, Form>(dataTransportFormModel);

            AssertPropertyEquals(expectedDataAccessFormModel, actualDataAccessFormModel);
        }

        [Fact]
        public void Map_DatabaseForm_ServiceForm()
        {
            // Ensures mapping from DAO form to DTO form is present
            var dataAccessFormModel = new Form
            {
                Name = "Display Name",
                Description = "TestDescription"
            };
            var expectedDataTransportFormModel = new FaaS.DataTransferModels.Form
            {
                FormName = "Display Name",
                Description = "TestDescription"
            };

            var actualDataTransportFormModel = GetMapper().Map<Form, FaaS.DataTransferModels.Form>(dataAccessFormModel);

            AssertPropertyEquals(expectedDataTransportFormModel, actualDataTransportFormModel);
        }

        [Fact]
        public void Map_ServiceElement_DatabaseElement()
        {
            // Ensures mapping from DTO element to DAO element is present
            var dataTransportElementModel = new FaaS.DataTransferModels.Element
            {
                Description = "TestDescription"
            };
            var expectedDataAccessElementModel = new Element
            {
                Description = "TestDescription"
            };

            var actualDataAccessElementModel = GetMapper().Map<FaaS.DataTransferModels.Element, Element>(dataTransportElementModel);

            AssertPropertyEquals(expectedDataAccessElementModel, actualDataAccessElementModel);
        }

        [Fact]
        public void Map_DatabaseElement_ServiceElement()
        {
            // Ensures mapping from DAO element to DTO element is present
            var dataAccessElementModel = new Element
            {
                Description = "TestDescription"
            };
            var expectedDataTransportElementModel = new FaaS.DataTransferModels.Element
            {
                Description = "TestDescription"
            };

            var actualDataTransportElementModel = GetMapper().Map<Element, FaaS.DataTransferModels.Element>(dataAccessElementModel);

            AssertPropertyEquals(expectedDataTransportElementModel, actualDataTransportElementModel);
        }

        [Fact]
        public void Map_ServiceElementValue_DatabaseElementValue()
        {
            // Ensures mapping from DTO elementValue to DAO elementValue is present
            var dataTransportElementValueModel = new FaaS.DataTransferModels.ElementValue
            {
                Value = "TestValue"
            };
            var expectedDataAccessElementValueModel = new ElementValue
            {
                Value = "TestValue"
            };

            var actualDataAccessElementValueModel = GetMapper().Map<FaaS.DataTransferModels.ElementValue, ElementValue>(dataTransportElementValueModel);

            AssertPropertyEquals(expectedDataAccessElementValueModel, actualDataAccessElementValueModel);
        }

        [Fact]
        public void Map_DatabaseElementValue_ServiceElementValue()
        {
            // Ensures mapping from DAO elementValue to DTO elementValue is present
            var dataAccessElementValueModel = new ElementValue
            {
                Value = "TestValue"
            };
            var expectedDataTransportElementValueModel = new FaaS.DataTransferModels.ElementValue
            {
                Value = "TestValue"
            };

            var actualDataTransportElementValueModel = GetMapper().Map<ElementValue, FaaS.DataTransferModels.ElementValue>(dataAccessElementValueModel);

            AssertPropertyEquals(expectedDataTransportElementValueModel, actualDataTransportElementValueModel);
        }

        [Fact]
        public void Map_ServiceSession_DatabaseSession()
        {
            // Ensures mapping from DTO session to DAO session is present
            var dataTransportSessionModel = new FaaS.DataTransferModels.Session
            {
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };
            var expectedDataAccessSessionModel = new Session
            {
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };

            var actualDataAccessSessionModel = GetMapper().Map<FaaS.DataTransferModels.Session, Session>(dataTransportSessionModel);

            AssertPropertyEquals(expectedDataAccessSessionModel, actualDataAccessSessionModel);
        }

        [Fact]
        public void Map_DatabaseSession_ServiceSession()
        {
            // Ensures mapping from DAO session to DTO session is present
            var dataAccessSessionModel = new Session
            {
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };
            var expectedDataTransportSessionModel = new FaaS.DataTransferModels.Session
            {
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };

            var actualDataTransportSessionModel = GetMapper().Map<Session, FaaS.DataTransferModels.Session>(dataAccessSessionModel);

            AssertPropertyEquals(expectedDataTransportSessionModel, actualDataTransportSessionModel);
        }

        /// <summary>
        /// Expects equality of (presumably scaler) values of each public property of <typeparamref name="TType"/>.
        /// </summary>
        private static void AssertPropertyEquals<TType>(TType expectedDataAccessModel, TType actualDataAccessModel)
        {
            foreach (var property in typeof(TType).GetProperties(BindingFlags.Public | BindingFlags.GetProperty))
            {
                Assert.Equal(property.GetValue(expectedDataAccessModel), property.GetValue(actualDataAccessModel));
            }
        }

        private static MapperConfiguration GetMapperConfiguration() => new MapperConfiguration(EntitiesMapperConfiguration.InitializeMappings);

        private static IMapper GetMapper() => GetMapperConfiguration().CreateMapper();
    }
}
