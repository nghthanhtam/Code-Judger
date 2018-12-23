using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Judge3T
{
    public class KetQuaChamBai
    {
        public KetQuaChamBai()
        {
            Details = null;
        }

        public KetQuaChamBai(StatusResult status, float scores, KieuChamBai type)
        {
            Status = status;
            Scores = scores;
            Type = type;
            Details = null;

        }

        public KetQuaChamBai(StatusResult status, float scores, KieuChamBai type, List<KetQuaChamTest> details) : this(status, scores, type)
        {
            Details = details;
        }

        public StatusResult Status { get; set; }

        public float Scores { get; set; }
        public KieuChamBai Type { get; set; }
         
        public List<KetQuaChamTest> Details { get; set; }
         
    }
}
