

namespace Hana.Api.Common.Models;

public sealed record DefaultResponse(int Rc, string Messages, int ResponseStatus, object Data);

