namespace WebApplication1.Models
{
    public class Salarie
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string telFixe { get; set; }
        public string telMobile { get; set; }
        public string email { get; set; }

        public int idSite { get; set; }

        public int idService { get; set; }
    }
}
