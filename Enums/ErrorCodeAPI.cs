using System.ComponentModel;

namespace ApiWithRoles.Enums
{
    public enum ErrorCodeAPI
    {
        #region System Error
        [Description("INTERNAL_ERROR")]
        InternalError = 101,
        [Description("OK")]
        OK = 200,
        [Description("NO_CONTENT")]
        NoContent = 204,
        [Description("REDIRECT")]
        Redirect = 302,
        [Description("BAD_REQUEST")]
        BadRequest = 400,
        [Description("UNAUTHORIZED")]
        Unauthorized = 401,

        [Description("INVALID_PASSWORD")]
        InvalidPassword = 4001,
        [Description("FORBIDDEN")]
        Forbidden = 403,
        [Description("NOT_FOUND")]
        NotFound = 404,
        #endregion

        #region Common Error
        [Description("NOT_OK")]
        NotOk = 501,
        #endregion

        [Description("INVALID_INPUT")]
        InvalidInput = 614,
    }
}
