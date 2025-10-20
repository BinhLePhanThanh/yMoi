namespace yMoi.Dto
{
    public class JsonResponse
    {
        public static JsonResponseModel Success(object data, object paging = null)
        {
            return new JsonResponseModel(Constants.Contants.SUCCESS, Constants.Contants.SUCCESS_CODE, Constants.Contants.MESSAGE_SUCCESS, data, paging);
        }


        public static JsonResponseModel Error(int code, string message)
        {
            return new JsonResponseModel(Constants.Contants.ERROR, code, message, "");
        }
    }
}