namespace GroupByInc.Api.Models.Zones
{
    public class ContentZone : AbstractContentZone<ContentZone>
    {
        public override Type GeType()
        {
            return Type.Content;
        }

        public ContentZone SetId(string id)
        {
            Id = id;
            return this;
        }

        public ContentZone SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}