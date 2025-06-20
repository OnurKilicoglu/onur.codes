using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselPortfoy.Core.Entities
{
    public class HomePageInfo
    {
        [Key]
        public int Id { get; set; }
        public string WelcomeTitle { get; set; }    // Örn: "Hoşgeldin!"
        public string WelcomeText { get; set; }     // Açıklama metni
        public string ButtonText { get; set; }      // Buton yazısı ("Projeleri İncele")
        public string ButtonUrl { get; set; }       // Buton linki ("/Home/Projects")
        public string? ImageUrl { get; set; }        // Sağdaki görsel
    }

}
