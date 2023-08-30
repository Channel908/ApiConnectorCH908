using ApiConnectorCH908.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiConnectorCH908;

internal static class ClassExtensionMethods
{

    internal const string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    internal static Regex _regxEmail = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);

    internal static bool Validate(this RequestorInfo? info, string? clientId)
    {
        if(info is null) return false;

        if(!clientId.IsGuid()) return false;

        if (!info.client_id.IsGuid()) return false;

        if(clientId != info.client_id) return false;

        if (!info.objectId.IsGuid()) return false;

        if(!info.email.IsEmail()) return false;

        return true;
    }

    internal static bool IsEmail(this string? email)
    {
        if (string.IsNullOrEmpty(email)) return false;

        return _regxEmail.IsMatch(email);
    }

    internal static bool IsGuid(this string? guid)
    {
        if(string.IsNullOrEmpty(guid)) return false;

        return Guid.TryParse(guid, out _);
    }
}
