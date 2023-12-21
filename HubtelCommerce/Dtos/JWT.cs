namespace HubtelCommerce.Dtos
{
    public class JWT
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public int TokenExpiry { get; set; } = 8;
    }
}
