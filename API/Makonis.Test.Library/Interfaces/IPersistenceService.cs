using Makonis.Test.Library.Models;

namespace Makonis.Test.Library.Interfaces {
    public interface IPersistenceService {
        Task<Response> Save(Person personInfo);
    }
}
