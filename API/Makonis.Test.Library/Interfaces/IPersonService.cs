
using Makonis.Test.Library.Models;

namespace Makonis.Test.Library.Interfaces {
    public interface IPersonService {
        Task<Response> SavePerson(Person person);
    }
}
