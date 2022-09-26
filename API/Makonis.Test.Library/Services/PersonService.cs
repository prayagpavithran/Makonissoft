
using Makonis.Test.Library.Constants;
using Makonis.Test.Library.Interfaces;
using Makonis.Test.Library.Models;
using Microsoft.Extensions.Logging;

namespace Makonis.Test.Library.Services {
    public class PersonService : IPersonService {
        private readonly ILogger _logger;
        private readonly IFileService _fileService;
        public PersonService(ILogger<FileService> logger,IFileService fileService) {
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<Response> SavePerson(Person person) {
            #region Initial Checks

            Response response = new Response() { IsSuccess = false };
            // Validate the input paramters.
            if (person == null || string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName)) {
                response.ReturnCode = ResponseCodes.INVALID_PARAMETERS;
                response.ReturnMessage = "Invalid parameters are passed to the method";
                _logger.LogError($"Invalid parameters are passed to the method . Parameters => FirstName:{person?.FirstName},LastName:{person?.LastName}");
                return response;
            }

            #endregion

            #region Main Logic

            try {
                // Call the file service method for saving the details to the file.
               response = await _fileService.Save(person);
            }
            catch (Exception ex) {
                response.ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR;
                response.ReturnMessage = "Invalid server error.";
                _logger.LogError($"Error occured in SavePerson method.Message: {ex.Message}, Parameters => FirstName:{person?.FirstName},LastName:{person?.LastName}");
                return response;
            }

            return response;

            #endregion
        }
    }
}
