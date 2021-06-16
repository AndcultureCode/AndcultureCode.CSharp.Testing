using Shouldly;
using Xunit;
using Xunit.Abstractions;
using AndcultureCode.CSharp.Testing.Extensions;
using AndcultureCode.CSharp.Testing.Constants;
using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Testing.Factories;
using AndcultureCode.CSharp.Core.Models.Errors;
using AndcultureCode.CSharp.Core.Interfaces.Entity;
using AndcultureCode.CSharp.Testing.Models.Stubs;

namespace AndcultureCode.CSharp.Testing.Tests.Unit.Extensions
{
    public class IResultMatcherExtensionsTest : ProjectUnitTest
    {
        #region Setup

        public IResultMatcherExtensionsTest(ITestOutputHelper output) : base(output)
        {

        }

        #endregion Setup

        #region ShouldBeCreated

        [Fact]
        public void ShouldBeCreated_When_Result_Null_Fails_Assertion()
        {
            // Arrange
            IResult<ICreatable> result = null;

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreated());
        }

        [Fact]
        public void ShouldBeCreated_When_ResultObject_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<UserStub>((e) => e.ResultObject = null);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreated());
        }

        [Fact]
        public void ShouldBeCreated_When_CreatedOn_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<UserStub>((e) => e.CreatedOn = null);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreated());
        }

        [Fact]
        public void ShouldBeCreated_When_CreatedOn_HasValue_Passes_Assertion()
        {
            // Arrange
            var result = BuildResult<UserStub>((e) => e.CreatedOn = Faker.Date.RecentOffset());

            // Act & Assert
            Should.NotThrow(() => result.ShouldBeCreated());
        }

        #endregion ShouldBeCreated

        #region ShouldBeCreatedBy(long createdById)

        [Fact]
        public void ShouldBeCreatedBy_CreatedById_When_Result_Null_Fails_Assertion()
        {
            // Arrange
            IResult<ICreatable> result = null;

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdById: Random.Long()));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedById_When_ResultObject_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<UserStub>((e) => e.ResultObject = null);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdById: Random.Long()));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedById_When_CreatedById_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<UserStub>((e) => e.CreatedById = null);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdById: Random.Long()));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedById_When_CreatedById_Does_Not_Match_Fails_Assertion()
        {
            // Arrange
            var createdById = Random.Long(min: 1, max: 100);
            var unexpected = createdById + 1;
            var result = BuildResult<UserStub>((e) => e.CreatedById = createdById);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdById: unexpected));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedById_When_CreatedById_Matches_Passes_Assertion()
        {
            // Arrange
            var expected = Random.Long();
            var result = BuildResult<UserStub>((e) => e.CreatedById = expected);

            // Act & Assert
            Should.NotThrow(() => result.ShouldBeCreatedBy(createdById: expected));
        }

        #endregion ShouldBeCreatedBy(long createdById)

        #region ShouldBeCreatedBy(IEntity createdBy)

        [Fact]
        public void ShouldBeCreatedBy_CreatedBy_When_Result_Null_Fails_Assertion()
        {
            // Arrange
            IResult<ICreatable> result = null;

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdBy: Build<UserStub>()));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedBy_When_ResultObject_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<UserStub>((e) => e.ResultObject = null);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdBy: Build<UserStub>()));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedBy_When_CreatedById_Null_Fails_Assertion()
        {
            // Arrange
            var unexpected = Build<UserStub>((e) => e.CreatedById = Random.Long());
            var result = BuildResult<UserStub>((e) => e.CreatedById = null);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdBy: unexpected));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedBy_When_CreatedById_Does_Not_Match_Fails_Assertion()
        {
            // Arrange
            var unexpected = Build<UserStub>((e) => e.CreatedById = Random.Long(min: 1, max: 100));
            var result = BuildResult<UserStub>((e) => e.CreatedById = unexpected.Id + 1);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldBeCreatedBy(createdBy: unexpected));
        }

        [Fact]
        public void ShouldBeCreatedBy_CreatedBy_When_CreatedById_Matches_Passes_Assertion()
        {
            // Arrange
            var expected = Build<UserStub>((e) => e.CreatedById = Random.Long());
            var result = BuildResult<UserStub>((e) => e.CreatedById = expected.Id);

            // Act & Assert
            Should.NotThrow(() => result.ShouldBeCreatedBy(createdBy: expected));
        }

        #endregion ShouldBeCreatedBy(long createdById)

        #region ShouldHaveBasicError

        [Fact]
        public void ShouldHaveBasicError_When_Errors_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = null);

            // Act
            var ex = Record.Exception(() => result.ShouldHaveBasicError());

            // Assert
            ex.ShouldNotBeNull();
            ex.Message.ShouldContain(IResultMatcherExtensions.ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
        }

        [Fact]
        public void ShouldHaveBasicError_When_Errors_Empty_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>());

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveBasicError());
        }

        [Fact]
        public void ShouldHaveBasicError_When_Errors_Contains_Other_Keys_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>
            {
                Build<Error>()
            });

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveBasicError());
        }

        [Fact]
        public void ShouldHaveBasicError_When_Errors_Contains_BasicErrorKey_Passes_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>
            {
                Build<Error>(ErrorFactory.BASIC_ERROR)
            });

            // Act & Assert
            Should.NotThrow(() => result.ShouldHaveBasicError());
        }

        #endregion ShouldHaveBasicError

        #region ShouldHaveErrors<T>

        [Fact]
        public void ShouldHaveErrors_T_When_Errors_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = null);

            // Act
            var ex = Record.Exception(() =>
            {
                result.ShouldHaveErrors();
            });

            // Assert
            ex.ShouldNotBeNull();
            ex.Message.ShouldContain(IResultMatcherExtensions.ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
        }

        [Fact]
        public void ShouldHaveErrors_T_When_Errors_Empty_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>());

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveErrors());
        }

        [Fact]
        public void ShouldHaveErrors_T_When_Errors_HasValues_Passes_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>
            {
                Build<Error>()
            });

            // Act & Assert
            Should.NotThrow(() => result.ShouldHaveErrors());
        }

        #endregion ShouldHaveErrors<T>

        #region ShouldHaveErrors<bool>

        [Fact]
        public void ShouldHaveErrors_Bool_When_Errors_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<bool>((e) => e.Errors = null);

            // Act
            var ex = Record.Exception(() => result.ShouldHaveErrors());

            // Assert
            ex.ShouldNotBeNull();
            ex.Message.ShouldContain(IResultMatcherExtensions.ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
        }

        [Fact]
        public void ShouldHaveErrors_Bool_When_Errors_Empty_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<bool>((e) => e.Errors = new List<IError>());

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveErrors());
        }

        [Fact]
        public void ShouldHaveErrors_Bool_When_ResultObject_True_Fails_Assertion()
        {
            // Arrange
            var errors = new List<IError>
            {
                Build<Error>(ErrorFactory.BASIC_ERROR)
            };
            var result = BuildResult<bool>(
                (e) => e.Errors = errors,
                (e) => e.ResultObject = true
            );

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveErrors());
        }

        [Fact]
        public void ShouldHaveErrors_Bool_When_Errors_HasValues_Passes_Assertion()
        {
            // Arrange
            var result = BuildResult<bool>((e) => e.Errors = new List<IError>
            {
                Build<Error>()
            });

            // Act & Assert
            Should.NotThrow(() => result.ShouldHaveErrors());
        }

        #endregion ShouldHaveErrors<bool>

        #region ShouldHaveErrorsFor

        [Fact]
        public void ShouldHaveErrorsFor_When_Errors_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = null);

            // Act
            var ex = Record.Exception(() => result.ShouldHaveErrorsFor(ErrorConstants.BASIC_ERROR_KEY));

            // Assert
            ex.ShouldNotBeNull();
            ex.Message.ShouldContain(IResultMatcherExtensions.ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
        }

        [Fact]
        public void ShouldHaveErrorsFor_When_Errors_Empty_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>());

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveErrorsFor(ErrorConstants.BASIC_ERROR_KEY));
        }

        [Fact]
        public void ShouldHaveErrorsFor_When_Errors_Contains_Other_Keys_Fails_Assertion()
        {
            // Arrange
            var error = Build<Error>();
            var result = BuildResult<object>((e) => e.Errors = new List<IError>
            {
                error
            });

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveErrorsFor($"not-{error.Key}"));
        }

        [Fact]
        public void ShouldHaveErrorsFor_When_Errors_Contains_Key_Passes_Assertion()
        {
            // Arrange
            var error = Build<Error>();
            var result = BuildResult<object>((e) => e.Errors = new List<IError>
            {
                error
            });

            // Act & Assert
            Should.NotThrow(() => result.ShouldHaveErrorsFor(error.Key));
        }

        #endregion ShouldHaveErrorsFor

        #region ShouldHaveResultObject

        [Fact]
        public void ShouldHaveResultObject_When_Result_Null_Fails_Assertion()
        {
            // Arrange
            IResult<object> result = null;

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveResultObject());
        }

        [Fact]
        public void ShouldHaveResultObject_When_ResultObject_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.ResultObject = null);

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveResultObject());
        }

        [Fact]
        public void ShouldHaveResultObject_When_ResultObject_HasValue_Passes_Assertion()
        {
            // Arrange
            var result = BuildResult<UserStub>();

            // Act & Assert
            Should.NotThrow(() => result.ShouldHaveResultObject());
        }

        #endregion ShouldHaveResultObject

        #region ShouldHaveResourceNotFoundError

        [Fact]
        public void ShouldHaveResourceNotFoundError_When_Errors_Null_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = null);

            // Act
            var ex = Record.Exception(() => result.ShouldHaveResourceNotFoundError());

            // Assert
            ex.ShouldNotBeNull();
            ex.Message.ShouldContain(IResultMatcherExtensions.ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
        }

        [Fact]
        public void ShouldHaveResourceNotFoundError_When_Errors_Empty_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>());

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveResourceNotFoundError());
        }

        [Fact]
        public void ShouldHaveResourceNotFoundError_When_Errors_Contains_Other_Keys_Fails_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>
            {
                Build<Error>()
            });

            // Act & Assert
            Should.Throw<ShouldAssertException>(() => result.ShouldHaveResourceNotFoundError());
        }

        [Fact]
        public void ShouldHaveResourceNotFoundError_When_Errors_Contains_ResourceNotFoundKey_Passes_Assertion()
        {
            // Arrange
            var result = BuildResult<object>((e) => e.Errors = new List<IError>
            {
                Build<Error>(ErrorFactory.RESOURCE_NOT_FOUND_ERROR)
            });

            // Act & Assert
            Should.NotThrow(() => result.ShouldHaveResourceNotFoundError());
        }

        #endregion ShouldHaveResourceNotFoundError
    }
}
