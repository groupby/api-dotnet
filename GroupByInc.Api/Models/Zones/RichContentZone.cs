using System;

namespace GroupByInc.Api.Models.Zones
{
    public sealed class RichContentZone : AbstractContentZone<RichContentZone>
    {
        public override Type GeType()
        {
            return Type.Rich_Content;
        }

        public RichContentZone SetId(string id)
        {
            throw new NotImplementedException();
        }

        public RichContentZone SetName(string name)
        {
            throw new NotImplementedException();
        }

        public String GetRichContent()
        {
            return GetContent();
        }

        public RichContentZone SetRichContentZone(string content)
        {
            return SetContent(content);
        }
    }
}