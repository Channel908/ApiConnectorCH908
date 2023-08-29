using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiConnectorCH908;

internal class RequestorInfo
{
    public string step { get; set; } = string.Empty;
    public string client_id { get; set; } = string.Empty;
    public string ui_locales { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string objectId { get; set; } = string.Empty;
    public string surname { get; set; } = string.Empty;
    public string displayName { get; set; } = string.Empty;
    public string givenName { get; set; } = string.Empty;

}
