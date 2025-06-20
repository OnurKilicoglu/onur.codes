namespace KisiselPortfoy.Core.Entities
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } // Başlangıçta düz metin, sonra hash'li yapacağız.
    }
}
