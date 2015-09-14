using GroupByInc.Api.Requests;

namespace GroupByInc.Api
{
    public class BaseQuery : AbstractQuery<BaseRequest, BaseQuery>
    {
        protected override BaseRequest GenerateRequest()
        {
            return new BaseRequest();
        }

        protected override RefinementsRequest<BaseRequest> PopulateRefinementRequest()
        {
            return new RefinementsRequest<BaseRequest>();
        }
    }
}
