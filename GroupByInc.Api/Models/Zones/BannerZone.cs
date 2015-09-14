namespace GroupByInc.Api.Models.Zones
{
    public sealed class BannerZone : AbstractContentZone<BannerZone>
    {
        public override Type GeType()
        {
            return Type.Banner;
        }

        public BannerZone SetId(string id)
        {
            Id = id;
            return this;
        }

        public BannerZone SetName(string name)
        {
            Name = name;
            return this;
        }

        public string GetBannerUrl()
        {
            return GetContent();
        }

        public BannerZone SetBannerUrl(string bannerUrl)
        {
            return SetContent(bannerUrl);
        }
    }
}