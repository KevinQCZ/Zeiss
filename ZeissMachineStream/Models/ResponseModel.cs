namespace ZeissMachineStream.Models
{
    /// <summary>
    /// This is the Model of API Response
    /// Extend the status of API Response if needed
    /// </summary>
    public class ResponseModel 
    {
        /// <summary>
        /// Model used to identify the 5xx series errors
        /// </summary>
        public class ApiServerErrorModel : BaseModel
        {
            /// <summary>
            /// Describe the Server errors of the call
            /// </summary>
            public IEnumerable<string> ServerErrors { get; set; }
        }

        /// <summary>
        /// Model used to identify the 400 errors
        /// </summary>
        public class ApiBadRequestModel : BaseModel
        {
            /// <summary>
            /// Describe the reasons to mark the request as BadRequest
            /// </summary>
            public IEnumerable<string> BadRequestReasons { get; set; }
        }

        /// <summary>
        /// Model used to identify the 404 errors
        /// </summary>
        public class ApiNotFoundModel : BaseModel
        {
            /// <summary>
            /// Describe the NotFound reasons of the call
            /// </summary>
            public IEnumerable<string> NotFoundReasons { get; set; }
        }
    }
}
