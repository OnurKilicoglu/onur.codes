using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselPortfoy.Core.Entities
{
    public class ProfileInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }             // Örn: Bilgisayar Programcısı ve Full Stack...
        public string Description { get; set; }        // Kısa tanıtım alt başlık
        public string Skills { get; set; }             // Virgülle ayrılmış
        public string Experience { get; set; }         // Kısa deneyim metni
        public string Goals { get; set; }              // Hedefler
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string PhotoUrl { get; set; }
    }
}
