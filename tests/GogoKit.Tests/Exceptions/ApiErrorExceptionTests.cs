using System;
using System.Collections.Generic;
using System.Linq;
using GogoKit.Exceptions;
using GogoKit.Models.Response;
using HalKit.Http;
using NUnit.Framework;

namespace GogoKit.Tests.Exceptions
{
    [TestFixture]
    public class ApiErrorExceptionTests
    {
        public static IEnumerable<Type> ApiErrorExceptionTypes
        {
            get
            {
                return typeof(ApiErrorException)
                        .Assembly
                        .GetTypes()
                        .Where(t => typeof(ApiErrorException).IsAssignableFrom(t) && t != typeof(ResourceNotFoundException))
                        .OrderBy(t => t.Name);
            }
        }

        [Test, TestCaseSource(nameof(ApiErrorExceptionTypes))]
        public void ApiErrorExceptions_ShouldHaveAPrettyErrorMessage(Type apiErrorExceptionType)
        {
            var apiResponse = new ApiResponse<ApiError> { Body = "{\"message\":\"pretty message\"}" };
            var expectedMessage = $"An error occurred with this API request: {apiResponse.Body}";
            var ctor = apiErrorExceptionType.GetConstructor(new[] { typeof(IApiResponse), typeof(ApiError) });

            var actualException = ctor.Invoke(new object[] { apiResponse, new ApiError() }) as ApiErrorException;

            Assert.AreEqual(expectedMessage, actualException.Message);
        }
    }
}
