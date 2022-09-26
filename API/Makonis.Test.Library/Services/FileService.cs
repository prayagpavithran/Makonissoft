
using System.Text;
using System.Text.Json;
using Makonis.Test.Library.Constants;
using Makonis.Test.Library.Interfaces;
using Makonis.Test.Library.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Makonis.Test.Library.Services {
    public class FileService : IFileService {
        private readonly ILogger _logger;
        protected readonly IConfiguration _configuration;

        private static object lockerFile = new Object();

        public FileService(ILogger<FileService> logger,IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// This method is used for saving the person details to the file .
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<Response> Save(Person person) {
            #region Initial Checks

            Response response = new Response() { IsSuccess = false };

            // Validate the input values.
            if (person == null || string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName)) {
                response.ReturnCode = ResponseCodes.INVALID_PARAMETERS;
                response.ReturnMessage = "Invalid parameters are passed to the method";
                _logger.LogError($"Invalid parameters are passed to the method . Parameters => FirstName:{person?.FirstName},LastName:{person?.LastName}");
                return response;
            }

            #endregion

            #region Main Logic

            try {

                string? directoryPath = _configuration?.GetSection(GeneralConstants.FILE_PATH_KEY)?.Value;

                // Check if the directory path exists . If not create the directory.
                if (!Directory.Exists(directoryPath)) {
                    Directory.CreateDirectory(directoryPath ?? String.Empty);
                }

                List<Person>? persons = null;
                string filePath = $"{directoryPath}/{GeneralConstants.FILE_NAME}";

                // Check if file exists , if so retrieve the existing entries from the file.
                if (File.Exists(filePath)) {
                    persons = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(filePath));
                }

                if (persons == null) {
                    persons = new List<Person>();
                }

                persons.Add(person);

                // Serialize the person list to a Json object.
                string personInfoes = JsonSerializer.Serialize(persons);

                Boolean resFileWrite = await Task.Run<bool>(() => { return WriteToFile(filePath,personInfoes); });

                response = new Response() {
                    IsSuccess = resFileWrite,
                    ReturnCode = ResponseCodes.SUCCESS,
                    ReturnMessage = ""
                };

            }
            catch (Exception ex) {
                response.ReturnCode = ResponseCodes.INTERNAL_SERVER_ERROR;
                response.ReturnMessage = "Invalid server error.";
                _logger.LogError($"Error occured in Save method. Message: {ex.Message}, Parameters => FirstName:{person?.FirstName},LastName:{person?.LastName}");
            }

            return response;

            #endregion
        }

        /// <summary>
        /// This method contains the logic for writing json contents to the file .
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private bool WriteToFile(string filePath,string contents) {
            bool isSuccess = false;
            try {
                lock (lockerFile) {
                    using (FileStream file = new FileStream(filePath,FileMode.Create,FileAccess.Write,FileShare.Read)) {
                        using (StreamWriter fWriter = new StreamWriter(file,Encoding.Unicode)) {
                            fWriter.Write(contents);
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex) {
                isSuccess = false;
                _logger.LogError($"Error occured in WriteToFile method. Message: {ex.Message}, Parameters => filePath:{filePath},contents:{contents}");
            }
            return isSuccess;

        }
    }
}
