using Makonis.Test.Library.Constants;
using Makonis.Test.Library.Interfaces;
using Makonis.Test.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Makonis.Test.API.Controllers {
    [ApiController]
    [Route("api/")]
    public class PersonController : ControllerBase {
        private readonly ILogger _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger,IPersonService personService) {
            _logger = logger;   
            _personService = personService;
        }
      
        /// <summary>
        /// This API method is used for saving the basic person information to a file.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("persons")]
        public async Task<Response> SavePerson([FromBody]Person person) {

            Response response = new Response() { IsSuccess =false };

            try {
                response = await _personService.SavePerson(person);
            }

            catch (Exception ex) {
                response.ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR;
                response.ReturnMessage = "Invalid server error.";
                _logger.LogError($"Error occured in SavePerson method.Message: {ex.Message}, Parameters => FirstName:{person?.FirstName},LastName:{person?.LastName}");
            }

            return response;
        }
    }
}