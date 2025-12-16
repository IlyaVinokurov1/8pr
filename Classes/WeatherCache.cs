using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Vinokurov.Classes
{
    [Table("pogoda_kesh")]
    public class WeatherCache
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("gorod")]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [Column("shirota")]
        public float Latitude { get; set; }

        [Required]
        [Column("dolgota")]
        public float Longitude { get; set; }

        [Required]
        [Column("pogoda_json", TypeName = "LONGTEXT")]
        public string WeatherJson { get; set; }

        [Required]
        [Column("poslednee_obnovlenie")]
        public DateTime LastUpdated { get; set; }

        [Required]
        [Column("deistvitelno_do")]
        public DateTime ValidUntil { get; set; }
    }

    [Table("logi_zaprosov")]
    public class RequestLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("data_zaprosa")]
        public DateTime RequestDate { get; set; }

        [Required]
        [Column("kolichestvo_zaprosov")]
        public int RequestCount { get; set; } = 0;

        [Required]
        [Column("vremya_poslednego_zaprosa")]
        public DateTime LastRequestTime { get; set; }
    }
}