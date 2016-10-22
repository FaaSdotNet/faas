using AutoMapper;
using FaaS.Entities.DataAccessModels;
using FaaS.Services.Configuration;
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
            var dataTransportUserModel = new DataTransferModels.User
            {
                DisplayName = "Display Name",
                UserCodeName = "CodeName",
                GoogleId = "TestGoogleId"
            };
            var expectedDataAccessUserModel = new User
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                GoogleId = "TestGoogleId"
            };

            var actualDataAccessUserModel = GetMapper().Map<DataTransferModels.User, User>(dataTransportUserModel);

            AssertPropertyEquals(expectedDataAccessUserModel, actualDataAccessUserModel);
        }

        [Fact]
        public void Map_DatabaseUser_ServiceUser()
        {
            // Ensures mapping from DAO user to DTO usr is present
            var dataAccessUserModel = new User
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                GoogleId = "TestGoogleId"
            };
            var expectedDataTransportUserModel = new DataTransferModels.User
            {
                DisplayName = "Display Name",
                UserCodeName = "CodeName",
                GoogleId = "TestGoogleId"
            };

            var actualDataTransportUserModel = GetMapper().Map<User, DataTransferModels.User>(dataAccessUserModel);

            AssertPropertyEquals(expectedDataTransportUserModel, actualDataTransportUserModel);
        }

        [Fact]
        public void Map_ServiceProject_DatabaseProject()
        {
            // Ensures mapping from DTO project to DAO project is present
            var dataTransportProjectModel = new DataTransferModels.Project
            {
                DisplayName = "Display Name",
                ProjectCodeName = "CodeName",
                Description = "TestDescription"
            };
            var expectedDataAccessProjectModel = new Project
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Description = "TestDescription"
            };

            var actualDataAccessProjectModel = GetMapper().Map<DataTransferModels.Project, Project>(dataTransportProjectModel);

            AssertPropertyEquals(expectedDataAccessProjectModel, actualDataAccessProjectModel);
        }

        [Fact]
        public void Map_DatabaseProject_ServiceProject()
        {
            // Ensures mapping from DAO project to DTO project is present
            var dataAccessProjectModel = new Project
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Description = "TestDescription"
            };
            var expectedDataTransportProjectModel = new DataTransferModels.Project
            {
                DisplayName = "Display Name",
                ProjectCodeName = "CodeName",
                Description = "TestDescription"
            };

            var actualDataTransportProjectModel = GetMapper().Map<Project, DataTransferModels.Project>(dataAccessProjectModel);

            AssertPropertyEquals(expectedDataTransportProjectModel, actualDataTransportProjectModel);
        }

        [Fact]
        public void Map_ServiceForm_DatabaseForm()
        {
            // Ensures mapping from DTO form to DAO form is present
            var dataTransportFormModel = new DataTransferModels.Form
            {
                DisplayName = "Display Name",
                FormCodeName = "CodeName",
                Description = "TestDescription"
            };
            var expectedDataAccessFormModel = new Form
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Description = "TestDescription"
            };

            var actualDataAccessFormModel = GetMapper().Map<DataTransferModels.Form, Form>(dataTransportFormModel);

            AssertPropertyEquals(expectedDataAccessFormModel, actualDataAccessFormModel);
        }

        [Fact]
        public void Map_DatabaseForm_ServiceForm()
        {
            // Ensures mapping from DAO form to DTO form is present
            var dataAccessFormModel = new Form
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Description = "TestDescription"
            };
            var expectedDataTransportFormModel = new DataTransferModels.Form
            {
                DisplayName = "Display Name",
                FormCodeName = "CodeName",
                Description = "TestDescription"
            };

            var actualDataTransportFormModel = GetMapper().Map<Form, DataTransferModels.Form>(dataAccessFormModel);

            AssertPropertyEquals(expectedDataTransportFormModel, actualDataTransportFormModel);
        }

        [Fact]
        public void Map_ServiceElement_DatabaseElement()
        {
            // Ensures mapping from DTO element to DAO element is present
            var dataTransportElementModel = new DataTransferModels.Element
            {
                DisplayName = "Display Name",
                ElementCodeName = "CodeName",
                Description = "TestDescription"
            };
            var expectedDataAccessElementModel = new Element
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Description = "TestDescription"
            };

            var actualDataAccessElementModel = GetMapper().Map<DataTransferModels.Element, Element>(dataTransportElementModel);

            AssertPropertyEquals(expectedDataAccessElementModel, actualDataAccessElementModel);
        }

        [Fact]
        public void Map_DatabaseElement_ServiceElement()
        {
            // Ensures mapping from DAO element to DTO element is present
            var dataAccessElementModel = new Element
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Description = "TestDescription"
            };
            var expectedDataTransportElementModel = new DataTransferModels.Element
            {
                DisplayName = "Display Name",
                ElementCodeName = "CodeName",
                Description = "TestDescription"
            };

            var actualDataTransportElementModel = GetMapper().Map<Element, DataTransferModels.Element>(dataAccessElementModel);

            AssertPropertyEquals(expectedDataTransportElementModel, actualDataTransportElementModel);
        }

        [Fact]
        public void Map_ServiceElementValue_DatabaseElementValue()
        {
            // Ensures mapping from DTO elementValue to DAO elementValue is present
            var dataTransportElementValueModel = new DataTransferModels.ElementValue
            {
                DisplayName = "Display Name",
                ElementValueCodeName = "CodeName",
                Value = "TestValue"
            };
            var expectedDataAccessElementValueModel = new ElementValue
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Value = "TestValue"
            };

            var actualDataAccessElementValueModel = GetMapper().Map<DataTransferModels.ElementValue, ElementValue>(dataTransportElementValueModel);

            AssertPropertyEquals(expectedDataAccessElementValueModel, actualDataAccessElementValueModel);
        }

        [Fact]
        public void Map_DatabaseElementValue_ServiceElementValue()
        {
            // Ensures mapping from DAO elementValue to DTO elementValue is present
            var dataAccessElementValueModel = new ElementValue
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Value = "TestValue"
            };
            var expectedDataTransportElementValueModel = new DataTransferModels.ElementValue
            {
                DisplayName = "Display Name",
                ElementValueCodeName = "CodeName",
                Value = "TestValue"
            };

            var actualDataTransportElementValueModel = GetMapper().Map<ElementValue, DataTransferModels.ElementValue>(dataAccessElementValueModel);

            AssertPropertyEquals(expectedDataTransportElementValueModel, actualDataTransportElementValueModel);
        }

        [Fact]
        public void Map_ServiceSession_DatabaseSession()
        {
            // Ensures mapping from DTO session to DAO session is present
            var dataTransportSessionModel = new DataTransferModels.Session
            {
                DisplayName = "Display Name",
                SessionCodeName = "CodeName",
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };
            var expectedDataAccessSessionModel = new Session
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };

            var actualDataAccessSessionModel = GetMapper().Map<DataTransferModels.Session, Session>(dataTransportSessionModel);

            AssertPropertyEquals(expectedDataAccessSessionModel, actualDataAccessSessionModel);
        }

        [Fact]
        public void Map_DatabaseSession_ServiceSession()
        {
            // Ensures mapping from DAO session to DTO session is present
            var dataAccessSessionModel = new Session
            {
                DisplayName = "Display Name",
                CodeName = "CodeName",
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };
            var expectedDataTransportSessionModel = new DataTransferModels.Session
            {
                DisplayName = "Display Name",
                SessionCodeName = "CodeName",
                Filled = new DateTime(1992, 5, 19, 3, 15, 0)
            };

            var actualDataTransportSessionModel = GetMapper().Map<Session, DataTransferModels.Session>(dataAccessSessionModel);

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

        private static MapperConfiguration GetMapperConfiguration() => new MapperConfiguration(ServicesMapperConfiguration.InitializeMappings);

        private static IMapper GetMapper() => GetMapperConfiguration().CreateMapper();
    }
}
